Shader "UIEffects/HologramScanline"
{
    // 홀로그램 스캔라인: 단색 홀로그램 톤 + 흐르는 스캔라인 + 위로 지나가는 밝은 띠 + 깜빡임
    // _Reveal 0→1 로 아래에서 위로 스캔되며 나타남
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Hologram)]
        [HDR] _HoloColor ("Hologram Color", Color) = (0.3, 0.9, 1, 1)
        _LineCount ("Scanline Count", Float) = 120
        _LineSpeed ("Scanline Speed", Float) = 2
        _LineStrength ("Scanline Strength", Range(0, 1)) = 0.5
        _BandSpeed ("Band Speed", Float) = 0.35
        _FlickerSpeed ("Flicker Speed", Float) = 12
        _RimStrength ("Rim Strength", Range(0, 2)) = 0.6
        _Reveal ("Reveal", Range(0, 1)) = 1

        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"

            CGPROGRAM
            #pragma vertex UIVert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UIEffectsCommon.cginc"

            fixed4 _HoloColor;
            float _LineCount;
            float _LineSpeed;
            float _LineStrength;
            float _BandSpeed;
            float _FlickerSpeed;
            float _RimStrength;
            float _Reveal;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 src = SampleUI(uv) * i.color;

                // 원본 밝기를 홀로그램 단색으로 치환
                float3 col = _HoloColor.rgb * (0.35 + Gray(src.rgb) * 1.2);

                // 흘러내리는 스캔라인
                float lines = sin((uv.y * _LineCount - _Time.y * _LineSpeed) * 6.2831853) * 0.5 + 0.5;
                float scan = lerp(1.0, lines, _LineStrength);

                // 아래→위로 지나가는 밝은 띠
                float band = exp(-pow((uv.y - frac(_Time.y * _BandSpeed)) * 10.0, 2.0));

                // 불안정한 깜빡임
                float flicker = 0.85 + 0.15 * ValueNoise(float2(_Time.y * _FlickerSpeed, 3.7));

                // 사각형 가장자리 림
                float2 e = abs(uv - 0.5) * 2.0;
                float rim = pow(max(e.x, e.y), 8.0) * _RimStrength;

                // 아래에서 위로 스캔되며 나타남 (스캔 라인 위치에 밝은 선)
                float revealLine = _Reveal * 1.1;
                float reveal = 1.0 - smoothstep(revealLine - 0.04, revealLine, uv.y);
                float revealEdge = exp(-pow((uv.y - revealLine) * 50.0, 2.0)) * saturate(_Reveal * 20.0);

                col = col * scan * flicker + _HoloColor.rgb * (band * 0.4 + rim);
                col += _HoloColor.rgb * revealEdge * 2.0;

                float a = src.a * (0.55 + 0.45 * scan) * flicker * reveal + src.a * revealEdge;
                return UIFinalize(fixed4(col, saturate(a)), i);
            }
            ENDCG
        }
    }
}

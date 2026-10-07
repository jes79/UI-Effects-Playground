Shader "UIEffects/UnlockReveal"
{
    // 언락 리빌: 잠긴 상태(흑백 + 어둡게)에서 가운데부터 원형으로 컬러가 퍼지며 해금됨
    // _Reveal 0 = 잠김, 1 = 완전히 해금. 퍼지는 경계에 빛나는 링
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Locked)]
        _LockedSaturation ("Locked Saturation", Range(0, 1)) = 0
        _LockedBrightness ("Locked Brightness", Range(0, 1)) = 0.35

        [Header(Reveal)]
        _Reveal ("Reveal", Range(0, 1)) = 0
        [HDR] _RingColor ("Ring Color", Color) = (1, 0.85, 0.4, 1)
        _RingWidth ("Ring Width", Range(0.005, 0.3)) = 0.035
        _Aspect ("Aspect (width / height, set by script)", Float) = 1

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

            float _LockedSaturation;
            float _LockedBrightness;
            float _Reveal;
            fixed4 _RingColor;
            float _RingWidth;
            float _Aspect;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 c = SampleUI(uv) * i.color;

                float3 locked = lerp(Gray(c.rgb).xxx, c.rgb, _LockedSaturation) * _LockedBrightness;

                // 가로세로 비율을 보정한 중심 거리 (찌그러지지 않은 원)
                float2 p = (uv - 0.5) * float2(_Aspect, 1.0);
                float d = length(p);
                float maxRadius = length(float2(_Aspect, 1.0)) * 0.5;
                float r = _Reveal * (maxRadius + _RingWidth);

                float inside = 1.0 - smoothstep(r - _RingWidth * 0.3, r, d);
                float ring = exp(-pow((d - r) / _RingWidth, 2.0)) * saturate(_Reveal * 30.0);

                c.rgb = lerp(locked, c.rgb, inside) + _RingColor.rgb * ring * 0.9;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

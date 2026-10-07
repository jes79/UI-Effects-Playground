Shader "UIEffects/Glitch"
{
    // 글리치: 가로 줄/블록 단위로 이미지가 랜덤하게 밀리고, RGB 가 갈라지고, 일부 블록은 색이 반전됨
    // _Intensity 0 = 정상, 1 = 최대 글리치
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Glitch)]
        _Intensity ("Intensity", Range(0, 1)) = 0.1
        _BlockCount ("Row Count", Float) = 24
        _RGBSplit ("RGB Split", Range(0, 0.1)) = 0.03
        _Speed ("Change Rate (per sec)", Float) = 16

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

            float _Intensity;
            float _BlockCount;
            float _RGBSplit;
            float _Speed;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                float t = floor(_Time.y * _Speed); // 연속이 아니라 "툭툭" 끊기게 시간 양자화

                // 가로 줄 단위로 랜덤하게 밀림
                float row = floor(uv.y * _BlockCount);
                float rowRand = Hash21(float2(row, t));
                float rowActive = step(1.0 - _Intensity * 0.6, rowRand);
                float shift = (Hash21(float2(t, row + 13.1)) - 0.5) * 0.25 * _Intensity * rowActive;

                // 큰 블록 단위 노이즈
                float2 block = floor(uv * float2(8.0, _BlockCount * 0.5));
                float blockRand = Hash21(block + t);
                float blockActive = step(1.0 - _Intensity * 0.25, blockRand);
                shift += (blockRand - 0.5) * 0.1 * blockActive;

                float2 guv = uv + float2(shift, 0.0);
                float split = _RGBSplit * _Intensity * (0.5 + rowRand);
                float2 so = float2(split, 0.0);

                fixed4 r = SampleUI(guv + so) * InsideUV(guv + so);
                fixed4 g = SampleUI(guv) * InsideUV(guv);
                fixed4 b = SampleUI(guv - so) * InsideUV(guv - so);

                fixed4 c;
                c.a = max(max(r.a, g.a), b.a);
                c.rgb = float3(r.r * r.a, g.g * g.a, b.b * b.a) / max(c.a, 1e-4);

                // 일부 블록 색 반전
                float invert = blockActive * step(0.5, Hash21(block * 1.7 + t + 3.3)) * _Intensity;
                c.rgb = lerp(c.rgb, 1.0 - c.rgb, invert);

                // 흐르는 스캔라인 노이즈
                c.rgb *= 1.0 - 0.15 * _Intensity * step(0.5, frac(uv.y * 120.0 + _Time.y * 30.0));

                c *= i.color;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

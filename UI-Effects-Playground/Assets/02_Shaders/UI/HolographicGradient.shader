Shader "UIEffects/HolographicGradient"
{
    // 무지개/홀로그램 그라디언트: 대각선으로 흐르는 무지개 + 스트라이프 + 반짝이
    // _Tilt(x,y) 를 스크립트에서 넣어주면 카드를 기울이는 방향에 따라 색과 글레어가 움직임
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Holo)]
        _HoloStrength ("Holo Strength", Range(0, 1)) = 0.45
        _HoloScale ("Rainbow Scale", Float) = 1.2
        _HoloSpeed ("Rainbow Speed", Float) = 0.15
        _StripeStrength ("Stripe Strength", Range(0, 1)) = 0.25
        _Tilt ("Tilt (xy, set by script)", Vector) = (0,0,0,0)

        [Header(Glare)]
        _GlareStrength ("Glare Strength", Range(0, 2)) = 0.6
        _GlareWidth ("Glare Width", Range(0.01, 1)) = 0.12

        [Header(Sparkle)]
        _SparkleDensity ("Sparkle Density", Float) = 28
        _SparkleStrength ("Sparkle Strength", Range(0, 2)) = 0.8

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

            float _HoloStrength;
            float _HoloScale;
            float _HoloSpeed;
            float _StripeStrength;
            float4 _Tilt;
            float _GlareStrength;
            float _GlareWidth;
            float _SparkleDensity;
            float _SparkleStrength;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 c = SampleUI(uv) * i.color;

                // 대각선으로 흐르는 무지개 (기울기에 따라 위상이 바뀜)
                float phase = (uv.x + uv.y) * _HoloScale + _Time.y * _HoloSpeed + (_Tilt.x + _Tilt.y) * 0.8;
                float3 rainbow = HueToRGB(frac(phase));

                // Screen 블렌드: 밝은 부분일수록 무지개가 자연스럽게 얹힘
                float3 holo = 1.0 - (1.0 - c.rgb) * (1.0 - rainbow * _HoloStrength);

                // 홀로그램 특유의 미세 스트라이프
                float stripes = sin((uv.x - uv.y) * 60.0 + _Tilt.x * 6.0) * 0.5 + 0.5;
                holo += rainbow * stripes * _StripeStrength * _HoloStrength;

                // 기울기에 따라 이동하는 대각선 글레어
                float g = dot(uv - 0.5, float2(0.7071, 0.7071)) + (_Tilt.x + _Tilt.y) * 0.35;
                float glare = exp(-pow(g / _GlareWidth, 2.0)) * _GlareStrength;

                // 격자 셀마다 랜덤하게 반짝이는 점 (셀 중심에서 멀어질수록 어두운 동그란 점)
                float2 cell = floor(uv * _SparkleDensity);
                float rnd = Hash21(cell);
                float dotShape = saturate(1.0 - length(frac(uv * _SparkleDensity) - 0.5) * 3.0);
                float twinkle = step(0.92, rnd) * dotShape * dotShape
                              * pow(saturate(sin(_Time.y * 3.0 + rnd * 40.0 + _Tilt.x * 5.0)), 8.0);

                c.rgb = holo + glare + twinkle * _SparkleStrength;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

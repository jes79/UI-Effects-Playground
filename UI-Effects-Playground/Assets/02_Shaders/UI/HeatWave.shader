Shader "UIEffects/HeatWave"
{
    // 히트 웨이브: 위로 흘러가는 노이즈로 uv 를 일렁이게 해서 아지랑이처럼 보이게 함
    // 아래쪽(열원)일수록 강하게 일렁이고 살짝 붉은 톤이 섞임
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Heat)]
        _Strength ("Strength", Range(0, 0.1)) = 0.025
        _Scale ("Noise Scale", Float) = 5
        _Speed ("Rise Speed", Float) = 1.2
        _RiseFalloff ("Height Falloff", Range(0, 4)) = 1.2
        _HeatTint ("Heat Tint (a = strength)", Color) = (1, 0.6, 0.3, 0.25)

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

            float _Strength;
            float _Scale;
            float _Speed;
            float _RiseFalloff;
            fixed4 _HeatTint;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                float t = _Time.y * _Speed;

                // y 방향으로 길쭉하고 위로 흘러가는 노이즈 두 개 → x/y 오프셋
                float n1 = Fbm(float2(uv.x * _Scale, uv.y * _Scale * 1.6 - t));
                float n2 = Fbm(float2(uv.x * _Scale + 5.2, uv.y * _Scale * 1.6 - t * 1.3 + 1.7));

                float heightMask = pow(saturate(1.0 - uv.y), _RiseFalloff);
                float2 offset = (float2(n1, n2) - 0.5) * _Strength * heightMask;

                fixed4 c = SampleUI(uv + offset) * i.color;

                // 열기 톤 (강도가 0 이면 원본 그대로)
                float tint = heightMask * _HeatTint.a * saturate(_Strength * 40.0);
                c.rgb = lerp(c.rgb, c.rgb * _HeatTint.rgb * 1.4, tint);

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

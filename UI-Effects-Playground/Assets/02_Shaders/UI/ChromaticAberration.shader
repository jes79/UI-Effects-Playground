Shader "UIEffects/ChromaticAberration"
{
    // 크로매틱 애버레이션: R/G/B 채널을 서로 다른 방향으로 밀어서 색이 갈라지는 연출
    // _Amount 를 스크립트에서 순간적으로 키웠다 줄이면 "충격" 느낌
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Chromatic)]
        _Amount ("Amount (UV)", Range(0, 0.1)) = 0.004
        _Angle ("Angle", Range(0, 360)) = 0
        _JitterSpeed ("Jitter Speed", Float) = 8
        _JitterAmount ("Jitter Amount", Range(0, 0.05)) = 0.003

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

            float _Amount;
            float _Angle;
            float _JitterSpeed;
            float _JitterAmount;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;

                // 시간에 따라 조금씩 흔들리는 오프셋 (크기와 방향 모두)
                float n = ValueNoise(float2(_Time.y * _JitterSpeed, 0.0)) - 0.5;
                float2 dir = AngleToDir(_Angle + n * 90.0);
                float2 off = dir * (_Amount + n * 2.0 * _JitterAmount);

                fixed4 r = SampleUI(uv + off) * InsideUV(uv + off);
                fixed4 g = SampleUI(uv);
                fixed4 b = SampleUI(uv - off) * InsideUV(uv - off);

                // 채널마다 자기 알파로 곱한 뒤 합침 → 갈라진 부분은 단색으로 보임
                fixed4 c;
                c.a = max(max(r.a, g.a), b.a);
                c.rgb = float3(r.r * r.a, g.g * g.a, b.b * b.a) / max(c.a, 1e-4);
                c *= i.color;

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

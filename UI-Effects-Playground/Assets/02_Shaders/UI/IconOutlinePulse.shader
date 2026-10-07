Shader "UIEffects/IconOutlinePulse"
{
    // 아이콘 아웃라인 펄스: 스프라이트 알파 외곽에 아웃라인을 그리고, 두께와 밝기가 맥박처럼 변함
    // _OutlineAlpha 를 스크립트에서 0↔1 트윈해서 선택/해제 표현
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Outline)]
        [HDR] _OutlineColor ("Outline Color", Color) = (1, 0.85, 0.3, 1)
        _OutlineWidth ("Outline Width (UV)", Range(0, 0.1)) = 0.025
        _OutlineAlpha ("Outline Alpha", Range(0, 1)) = 1

        [Header(Pulse)]
        _PulseAmount ("Width Pulse Amount", Range(0, 2)) = 0.8
        _PulseSpeed ("Pulse Speed", Float) = 5

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

            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineAlpha;
            float _PulseAmount;
            float _PulseSpeed;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 tex = SampleUI(uv);
                fixed4 c = tex * i.color;

                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                float width = _OutlineWidth * (1.0 + pulse * _PulseAmount);

                // 16방향으로 width 만큼 떨어진 곳의 최대 알파 = 팽창(dilate)된 실루엣
                float maxA = 0.0;
                [unroll]
                for (int k = 0; k < 16; k++)
                {
                    float a = k * 6.2831853 / 16.0;
                    float2 suv = uv + float2(cos(a), sin(a)) * width;
                    maxA = max(maxA, tex2D(_MainTex, suv).a * InsideUV(suv));
                }

                // 팽창 실루엣 - 원본 = 외곽선
                float outline = saturate(maxA - tex.a);
                fixed4 o = fixed4(_OutlineColor.rgb, outline * _OutlineAlpha * lerp(0.6, 1.0, pulse) * i.color.a);

                // 아이콘 자체도 살짝 밝아짐
                c.rgb += _OutlineColor.rgb * _OutlineAlpha * pulse * 0.15;

                return UIFinalize(BlendOver(c, o), i);
            }
            ENDCG
        }
    }
}

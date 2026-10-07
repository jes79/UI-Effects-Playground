Shader "UIEffects/GlowPulse"
{
    // 글로우 펄스: 스프라이트 알파 외곽을 따라 바깥쪽 글로우를 만들고, 시간에 따라 밝기가 숨 쉬듯 변함
    // 스프라이트 주변에 글로우가 퍼질 투명 여백이 있어야 함
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Glow)]
        _GlowColor ("Glow Color", Color) = (0.4, 0.8, 1, 1)
        _GlowSize ("Glow Size (UV)", Range(0, 0.2)) = 0.06
        _GlowIntensity ("Glow Intensity", Range(0, 3)) = 1.5
        _InnerGlow ("Inner Glow", Range(0, 1)) = 0.25

        [Header(Pulse)]
        _PulseSpeed ("Pulse Speed", Float) = 3
        _PulseMin ("Pulse Min", Range(0, 1)) = 0.35

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

            fixed4 _GlowColor;
            float _GlowSize;
            float _GlowIntensity;
            float _InnerGlow;
            float _PulseSpeed;
            float _PulseMin;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                fixed4 c = SampleUI(uv) * i.color;

                // 16방향 x 2링으로 주변 알파를 모아서 "번진" 알파를 만든다
                float sum = 0.0;
                [unroll]
                for (int k = 0; k < 16; k++)
                {
                    float a = k * 6.2831853 / 16.0;
                    float2 d = float2(cos(a), sin(a)) * _GlowSize;
                    sum += tex2D(_MainTex, uv + d).a * InsideUV(uv + d);
                    sum += tex2D(_MainTex, uv + d * 0.5).a * InsideUV(uv + d * 0.5);
                }
                float spread = saturate(sum / 32.0 * 1.6);

                float pulse = lerp(_PulseMin, 1.0, sin(_Time.y * _PulseSpeed) * 0.5 + 0.5);

                // 바깥 글로우 (원본 뒤에 깔림)
                fixed4 glow = fixed4(_GlowColor.rgb, saturate(spread * pulse * _GlowIntensity) * _GlowColor.a * i.color.a);

                // 원본 안쪽도 살짝 빛나게
                c.rgb += _GlowColor.rgb * _InnerGlow * pulse;

                return UIFinalize(BlendOver(c, glow), i);
            }
            ENDCG
        }
    }
}

Shader "UIEffects/WaveDistortion"
{
    // 물결 왜곡: 사인파로 uv 를 흔들어 일렁이게 + 클릭 지점에서 원형 파동(리플)이 퍼짐
    // _RippleCenter / _RippleProgress 는 스크립트에서 클릭할 때마다 설정
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Wave)]
        _Amplitude ("Amplitude", Range(0, 0.05)) = 0.006
        _Frequency ("Frequency", Float) = 18
        _Speed ("Speed", Float) = 2

        [Header(Ripple)]
        _RippleCenter ("Ripple Center (uv)", Vector) = (0.5, 0.5, 0, 0)
        _RippleProgress ("Ripple Progress", Range(0, 1)) = 1
        _RippleStrength ("Ripple Strength", Range(0, 0.1)) = 0.03
        _RippleWidth ("Ripple Width", Range(0.01, 0.5)) = 0.08
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

            float _Amplitude;
            float _Frequency;
            float _Speed;
            float4 _RippleCenter;
            float _RippleProgress;
            float _RippleStrength;
            float _RippleWidth;
            float _Aspect;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                float t = _Time.y * _Speed;

                // 기본 일렁임: x 는 y 에 따라, y 는 x 에 따라 흔들림
                float2 offset;
                offset.x = sin(uv.y * _Frequency + t) * _Amplitude;
                offset.y = cos(uv.x * _Frequency * 0.8 + t * 1.3) * _Amplitude * 0.6;

                // 클릭 리플: 반지름이 progress 에 따라 커지는 링 모양 파동
                float2 rp = (uv - _RippleCenter.xy) * float2(_Aspect, 1.0);
                float d = length(rp);
                float radius = _RippleProgress * 1.2;
                float fade = 1.0 - _RippleProgress;
                float wave = sin((d - radius) * 60.0) * exp(-pow((d - radius) / _RippleWidth, 2.0)) * fade;
                float2 dir = rp / max(d, 1e-4);
                offset += dir / float2(_Aspect, 1.0) * wave * _RippleStrength;

                fixed4 c = SampleUI(uv + offset) * i.color;

                // 파동 마루에 살짝 하이라이트
                c.rgb += saturate(wave) * _RippleStrength * 3.0;

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

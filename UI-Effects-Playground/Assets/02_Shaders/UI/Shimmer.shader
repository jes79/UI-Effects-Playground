Shader "UIEffects/Shimmer"
{
    // 셔이머: 사선 빛줄기가 이미지 위를 훑고 지나가는 연출
    // _ShineLocation(0→1)을 스크립트에서 트윈해서 움직임
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Shine)]
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineLocation ("Shine Location", Range(0, 1)) = 0
        _ShineWidth ("Shine Width", Range(0.01, 1)) = 0.15
        _ShineSoftness ("Shine Softness", Range(0, 1)) = 0.7
        _ShineAngle ("Shine Angle", Range(0, 180)) = 30
        _ShineIntensity ("Shine Intensity", Range(0, 3)) = 1

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

            fixed4 _ShineColor;
            float _ShineLocation;
            float _ShineWidth;
            float _ShineSoftness;
            float _ShineAngle;
            float _ShineIntensity;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                fixed4 c = SampleUI(i.texcoord) * i.color;

                // 빛줄기 방향으로 uv 를 투영 → 0(시작)~1(끝)
                float pos = ProjectUV(i.texcoord, AngleToDir(_ShineAngle));

                // 빛줄기 폭만큼 바깥에서 시작/끝나야 깔끔하게 들어오고 빠져나감
                float center = lerp(-_ShineWidth, 1.0 + _ShineWidth, _ShineLocation);
                float band = 1.0 - smoothstep(_ShineWidth * (1.0 - _ShineSoftness), _ShineWidth, abs(pos - center));

                // 알파는 그대로 두고 색만 더해서, 이미지가 있는 곳에만 빛이 보임
                c.rgb += _ShineColor.rgb * _ShineColor.a * band * _ShineIntensity;

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

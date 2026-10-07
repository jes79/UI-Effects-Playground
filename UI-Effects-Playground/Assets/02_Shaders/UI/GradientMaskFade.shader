Shader "UIEffects/GradientMaskFade"
{
    // 그라디언트 마스크 페이드: 지정한 방향으로 부드러운 그라디언트 경계가 쓸고 지나가며 나타남/사라짐
    // _Progress 0 = 완전히 숨김, 1 = 완전히 보임
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Mask)]
        _Progress ("Progress", Range(0, 1)) = 1
        _Softness ("Softness", Range(0.01, 1)) = 0.3
        _Angle ("Angle", Range(0, 360)) = 0

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

            float _Progress;
            float _Softness;
            float _Angle;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                fixed4 c = SampleUI(i.texcoord) * i.color;

                // 진행 방향으로 0(시작)~1(끝)
                float t = ProjectUV(i.texcoord, AngleToDir(_Angle));

                // 경계를 softness 만큼 늘려서 0 → 전부 숨김, 1 → 전부 보임이 정확히 맞도록
                float mask = saturate((_Progress * (1.0 + _Softness) - t) / _Softness);
                c.a *= mask;

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

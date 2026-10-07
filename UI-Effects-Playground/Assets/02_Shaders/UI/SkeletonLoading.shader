Shader "UIEffects/SkeletonLoading"
{
    // 스켈레톤 로딩: 회색 자리표시자 위를 밝은 띠가 계속 훑고 지나감
    // 캔버스 좌표(x) 기준으로 띠 위치를 계산 → 같은 머티리얼을 쓰는 모든 자리표시자가 한 줄로 이어져 보임
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Skeleton)]
        _BaseColor ("Base Color", Color) = (0.22, 0.24, 0.3, 1)
        _HighlightColor ("Highlight Color (a = strength)", Color) = (0.38, 0.41, 0.5, 1)
        _BandWidth ("Band Width (canvas units)", Float) = 220
        _Speed ("Sweeps per second", Float) = 0.7
        _Angle ("Band Slant (deg)", Range(-60, 60)) = 20
        _SweepMin ("Sweep Start X (canvas)", Float) = -1200
        _SweepMax ("Sweep End X (canvas)", Float) = 1200

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

            fixed4 _BaseColor;
            fixed4 _HighlightColor;
            float _BandWidth;
            float _Speed;
            float _Angle;
            float _SweepMin;
            float _SweepMax;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                fixed4 tex = SampleUI(i.texcoord);

                // 캔버스 좌표 기준 x (y 에 따라 살짝 기울여 사선 띠)
                float x = i.worldPosition.x + i.worldPosition.y * tan(radians(_Angle));
                float center = lerp(_SweepMin, _SweepMax, frac(_Time.y * _Speed));
                float band = exp(-pow((x - center) / _BandWidth, 2.0));

                float3 col = lerp(_BaseColor.rgb, _HighlightColor.rgb, band * _HighlightColor.a);
                fixed4 c = fixed4(col, tex.a * _BaseColor.a) * i.color;

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

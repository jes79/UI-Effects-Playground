Shader "UIEffects/PixelateTransition"
{
    // 픽셀화 전환: _Progress 가 커질수록 픽셀 블록이 커짐 → 최대일 때 이미지를 교체하고 다시 선명하게
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Pixelate)]
        _Progress ("Progress", Range(0, 1)) = 0
        _MinCells ("Min Cells (most pixelated)", Float) = 6
        _MaxCells ("Max Cells (sharp)", Float) = 300
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

            float _Progress;
            float _MinCells;
            float _MaxCells;
            float _Aspect;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                // 초반에 빠르게 거칠어지도록 sqrt 커브 사용
                float cells = lerp(_MaxCells, _MinCells, sqrt(_Progress));
                float2 cellCount = float2(cells * _Aspect, cells);

                // uv 를 셀 중심으로 스냅
                float2 pixelUV = (floor(i.texcoord * cellCount) + 0.5) / cellCount;
                float2 uv = lerp(i.texcoord, pixelUV, step(0.001, _Progress));

                fixed4 c = SampleUI(uv) * i.color;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

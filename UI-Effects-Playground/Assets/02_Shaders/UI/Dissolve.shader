Shader "UIEffects/Dissolve"
{
    // 디졸브: 노이즈 값이 임계값보다 낮은 부분부터 타들어가듯 사라짐
    // _DissolveAmount 0 = 전부 보임, 1 = 전부 사라짐. 경계에는 타는 듯한 엣지 색
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Dissolve)]
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _NoiseScale ("Noise Scale", Float) = 6
        _EdgeWidth ("Edge Width", Range(0, 0.3)) = 0.08
        [HDR] _EdgeColor ("Edge Color", Color) = (1, 0.5, 0.1, 1)
        _EdgeIntensity ("Edge Intensity", Range(0, 4)) = 1.6

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

            float _DissolveAmount;
            float _NoiseScale;
            float _EdgeWidth;
            fixed4 _EdgeColor;
            float _EdgeIntensity;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                fixed4 c = SampleUI(i.texcoord) * i.color;

                float n = Fbm(i.texcoord * _NoiseScale);

                // 엣지 폭만큼 범위를 늘려서 1 일 때 엣지까지 완전히 사라지게 함
                float threshold = _DissolveAmount * (1.0 + _EdgeWidth);

                // 임계값보다 노이즈가 작은 곳은 사라짐
                float visible = smoothstep(threshold - 0.01, threshold + 0.01, n);

                // 임계값 바로 위쪽 띠 = 타는 엣지
                float edge = (1.0 - smoothstep(threshold, threshold + _EdgeWidth, n)) * saturate(_DissolveAmount * 20.0);

                c.rgb = lerp(c.rgb, _EdgeColor.rgb * _EdgeIntensity, edge);
                c.a *= visible;

                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

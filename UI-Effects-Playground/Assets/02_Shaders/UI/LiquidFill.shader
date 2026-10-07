Shader "UIEffects/LiquidFill"
{
    // 액체 채움: 원형 용기 안에 물결치는 액체가 _Fill 만큼 차오름 (앞/뒤 두 겹의 물결)
    // 스프라이트 없이(흰색) Image 에 적용 → uv 0~1 기준으로 원을 그림
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Liquid)]
        _Fill ("Fill", Range(0, 1)) = 0.5
        _LiquidColor ("Liquid Top Color", Color) = (0.35, 0.8, 1, 1)
        _LiquidDeepColor ("Liquid Deep Color", Color) = (0.1, 0.35, 0.9, 1)
        _BackWaveColor ("Back Wave Color", Color) = (0.15, 0.45, 0.8, 1)
        _EmptyColor ("Empty Color", Color) = (0.12, 0.14, 0.2, 1)

        [Header(Wave)]
        _WaveAmp ("Wave Amplitude", Range(0, 0.1)) = 0.02
        _WaveFreq ("Wave Frequency", Float) = 10
        _WaveSpeed ("Wave Speed", Float) = 2.5

        [Header(Container)]
        _BorderWidth ("Border Width", Range(0, 0.1)) = 0.035
        _BorderColor ("Border Color", Color) = (0.9, 0.95, 1, 1)

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

            float _Fill;
            fixed4 _LiquidColor;
            fixed4 _LiquidDeepColor;
            fixed4 _BackWaveColor;
            fixed4 _EmptyColor;
            float _WaveAmp;
            float _WaveFreq;
            float _WaveSpeed;
            float _BorderWidth;
            fixed4 _BorderColor;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;
                float2 p = uv - 0.5;
                float d = length(p);
                float aa = fwidth(d) * 1.5;

                // 원형 용기 / 안쪽 영역
                float circle = 1.0 - smoothstep(0.5 - aa, 0.5, d);
                float inner = 1.0 - smoothstep(0.5 - _BorderWidth - aa, 0.5 - _BorderWidth, d);

                // fill 0~1 을 용기 안쪽 높이에 맞춤
                float level = lerp(_BorderWidth, 1.0 - _BorderWidth, _Fill);
                float t = _Time.y * _WaveSpeed;

                // 앞 물결(두 사인파 합성) / 뒤 물결(위상이 다른 사인파)
                float front = level + sin(uv.x * _WaveFreq + t) * _WaveAmp + sin(uv.x * _WaveFreq * 2.3 - t * 1.3) * _WaveAmp * 0.4;
                float back = level + sin(uv.x * _WaveFreq * 0.8 - t * 1.1 + 1.7) * _WaveAmp * 1.2;

                float edgeAA = 0.004;
                float frontMask = (1.0 - smoothstep(front - edgeAA, front + edgeAA, uv.y)) * inner;
                float backMask = (1.0 - smoothstep(back - edgeAA, back + edgeAA, uv.y)) * inner;

                float3 liquid = lerp(_LiquidDeepColor.rgb, _LiquidColor.rgb, saturate(uv.y / max(level, 0.01)));

                float3 col = _EmptyColor.rgb;
                col = lerp(col, _BackWaveColor.rgb, backMask);
                col = lerp(col, liquid, frontMask);

                // 수면 하이라이트
                float surface = exp(-pow((uv.y - front) / 0.01, 2.0)) * inner;
                col += surface * 0.35;

                // 유리 반사 (왼쪽 위 호)
                float arc = exp(-pow((d - 0.36) / 0.015, 2.0)) * saturate((-p.x + p.y) * 4.0) * inner;
                col += arc * 0.35;

                // 테두리
                float border = circle - inner;
                col = lerp(col, _BorderColor.rgb, border);

                fixed4 c = fixed4(col, circle) * i.color;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

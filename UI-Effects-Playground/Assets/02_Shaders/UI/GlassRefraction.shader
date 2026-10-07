Shader "UIEffects/GlassRefraction"
{
    // 유리/굴절: 패널 뒤의 배경 텍스처를 노이즈 + 렌즈(가장자리 볼록) 로 휘어서 블러와 함께 다시 그림
    // URP 의 UI 는 GrabPass 를 쓸 수 없으므로 배경 텍스처와 배경 Rect(캔버스 좌표)를 직접 넘겨받는 방식
    // 패널 모양은 둥근 사각형 SDF 로 셰이더에서 직접 그림 (스프라이트 없이 Image 에 적용)
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Background)]
        _BackgroundTex ("Background Texture", 2D) = "black" {}
        _BgRect ("Background Rect (canvas xy, wh - set by script)", Vector) = (-960, -540, 1920, 1080)

        [Header(Glass)]
        _GlassTint ("Glass Tint (a = strength)", Color) = (0.9, 0.95, 1, 0.15)
        _Distortion ("Noise Distortion", Range(0, 0.05)) = 0.012
        _NoiseScale ("Noise Scale", Float) = 4
        _EdgeBulge ("Edge Lens Bulge", Range(0, 0.1)) = 0.03
        _BlurSize ("Blur Size", Range(0, 0.02)) = 0.004
        _HighlightStrength ("Highlight Strength", Range(0, 1)) = 0.25
        _BorderGlow ("Border Glow", Range(0, 2)) = 0.6

        [Header(Shape)]
        _CornerRadius ("Corner Radius", Range(0, 0.5)) = 0.12
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

            sampler2D _BackgroundTex;
            float4 _BgRect;
            fixed4 _GlassTint;
            float _Distortion;
            float _NoiseScale;
            float _EdgeBulge;
            float _BlurSize;
            float _HighlightStrength;
            float _BorderGlow;
            float _CornerRadius;
            float _Aspect;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                float2 uv = i.texcoord;

                // 둥근 사각형 SDF (안쪽이 음수)
                float2 p = (uv - 0.5) * float2(_Aspect, 1.0);
                float2 halfSize = float2(_Aspect, 1.0) * 0.5;
                float2 q = abs(p) - halfSize + _CornerRadius;
                float sd = length(max(q, 0.0)) + min(max(q.x, q.y), 0.0) - _CornerRadius;
                float shape = 1.0 - smoothstep(-fwidth(sd), 0.0, sd);

                // 이 픽셀 뒤에 있는 배경의 uv (캔버스 좌표 → 배경 Rect 기준 0~1)
                float2 bgUV = (i.worldPosition.xy - _BgRect.xy) / _BgRect.zw;

                // 굴절 1: 천천히 움직이는 노이즈로 울퉁불퉁한 유리 표면
                float n1 = ValueNoise(uv * _NoiseScale + _Time.y * 0.05);
                float n2 = ValueNoise(uv * _NoiseScale + 17.3 - _Time.y * 0.04);
                float2 refractOffset = (float2(n1, n2) - 0.5) * _Distortion;

                // 굴절 2: 가장자리로 갈수록 렌즈처럼 안쪽으로 휨
                float edgeFactor = saturate(1.0 + sd / 0.08);
                refractOffset -= normalize(p + 1e-5) * edgeFactor * edgeFactor * _EdgeBulge;

                // 9탭 블러 (반투명 유리 느낌)
                float2 rUV = bgUV + refractOffset;
                float3 col = 0;
                [unroll]
                for (int x = -1; x <= 1; x++)
                {
                    [unroll]
                    for (int y = -1; y <= 1; y++)
                        col += tex2D(_BackgroundTex, rUV + float2(x, y) * _BlurSize).rgb;
                }
                col /= 9.0;

                col = lerp(col, _GlassTint.rgb, _GlassTint.a);

                // 왼쪽 위 대각선 하이라이트
                float hl = exp(-pow((uv.x * 0.6 + (1.0 - uv.y) - 0.35) / 0.08, 2.0)) * _HighlightStrength;
                col += hl;

                // 테두리 빛
                float rim = exp(-pow(sd / 0.008, 2.0)) * _BorderGlow;
                col += rim;

                fixed4 c = fixed4(col, shape) * i.color;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

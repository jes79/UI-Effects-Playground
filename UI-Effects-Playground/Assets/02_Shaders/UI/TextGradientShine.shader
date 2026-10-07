Shader "UIEffects/TextGradientShine"
{
    // 텍스트 그라디언트 + 샤인: TextMeshPro(SDF 폰트 아틀라스)용 셰이더
    // - 아틀라스 알파(거리값)를 0.5 기준으로 잘라 글자 모양을 만들고, fwidth 로 안티에일리어싱
    // - 글자 전체에 걸친 3색 그라디언트가 흐르고, 그 위로 사선 빛줄기가 지나감
    // - _TextRect(캔버스 좌표의 텍스트 영역)는 스크립트에서 넣어줌 → 글자 단위가 아니라 문장 전체 기준 그라디언트
    Properties
    {
        _MainTex ("Font Atlas", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Gradient)]
        _ColorA ("Color A", Color) = (1, 0.45, 0.75, 1)
        _ColorB ("Color B", Color) = (0.45, 0.75, 1, 1)
        _ColorC ("Color C", Color) = (1, 0.9, 0.4, 1)
        _GradientSpeed ("Gradient Scroll Speed", Float) = 0.25
        _TextRect ("Text Rect (canvas xy, wh - set by script)", Vector) = (-500, -50, 1000, 100)

        [Header(Shine)]
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineLocation ("Shine Location", Range(0, 1)) = 0
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.06

        [Header(SDF)]
        _Softness ("Edge Softness", Range(0, 1)) = 0
        _Dilate ("Dilate (bold)", Range(-0.2, 0.2)) = 0

        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
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

            #include "UIEffectsCommon.cginc"

            fixed4 _ColorA;
            fixed4 _ColorB;
            fixed4 _ColorC;
            float _GradientSpeed;
            float4 _TextRect;
            fixed4 _ShineColor;
            float _ShineLocation;
            float _ShineWidth;
            float _Softness;
            float _Dilate;

            // 0~3 구간을 A → B → C → A 로 순환하는 3색 그라디언트
            float3 Gradient3(float g)
            {
                float3 ab = lerp(_ColorA.rgb, _ColorB.rgb, saturate(g));
                float3 bc = lerp(_ColorB.rgb, _ColorC.rgb, saturate(g - 1.0));
                float3 ca = lerp(_ColorC.rgb, _ColorA.rgb, saturate(g - 2.0));
                return g < 1.0 ? ab : (g < 2.0 ? bc : ca);
            }

            fixed4 frag(v2f_ui i) : SV_Target
            {
                // SDF: 아틀라스 알파 0.5 가 글자 외곽선
                float d = tex2D(_MainTex, i.texcoord).a + _Dilate;
                float w = max(fwidth(d), 1e-4) * (0.75 + _Softness * 4.0);
                float alpha = smoothstep(0.5 - w, 0.5 + w, d);

                // 텍스트 영역 기준 0~1 위치
                float tx = saturate((i.worldPosition.x - _TextRect.x) / max(_TextRect.z, 1e-3));
                float ty = saturate((i.worldPosition.y - _TextRect.y) / max(_TextRect.w, 1e-3));

                // 흐르는 그라디언트
                float g = frac(tx * 0.5 + ty * 0.15 - _Time.y * _GradientSpeed) * 3.0;
                float3 col = Gradient3(g);

                // 사선 빛줄기
                float pos = tx + (ty - 0.5) * 0.3;
                float center = lerp(-_ShineWidth * 3.0, 1.0 + _ShineWidth * 3.0, _ShineLocation);
                float shine = exp(-pow((pos - center) / _ShineWidth, 2.0));
                col += _ShineColor.rgb * _ShineColor.a * shine;

                fixed4 c = fixed4(col, alpha) * i.color;
                return UIFinalize(c, i);
            }
            ENDCG
        }
    }
}

#ifndef UI_EFFECTS_COMMON_INCLUDED
#define UI_EFFECTS_COMMON_INCLUDED

// ─────────────────────────────────────────────────────────────
// 커스텀 UI 셰이더 공통 베이스
// - 기본 UI/Default 셰이더와 같은 방식으로 버텍스를 처리해서
//   Image / RawImage / TMP 에 그대로 쓸 수 있고, Mask(Stencil) / RectMask2D 클리핑도 지원
// - 각 셰이더는 이 파일을 include 하고 frag 함수만 작성하면 됨
//
// ※ UI 버텍스는 Canvas 가 배칭하면서 "캔버스 기준 좌표"로 바뀌어 들어옴
//    → worldPosition 은 캔버스 로컬 좌표 (RectMask2D 의 _ClipRect 도 같은 좌표계)
// ─────────────────────────────────────────────────────────────

#include "UnityCG.cginc"
#include "UnityUI.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
float4 _MainTex_TexelSize;
fixed4 _Color;
fixed4 _TextureSampleAdd;
float4 _ClipRect;
float _UIMaskSoftnessX;
float _UIMaskSoftnessY;

struct appdata_ui
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f_ui
{
    float4 vertex        : SV_POSITION;
    fixed4 color         : COLOR;
    float2 texcoord      : TEXCOORD0;
    float4 worldPosition : TEXCOORD1; // 캔버스 로컬 좌표
    float4 mask          : TEXCOORD2;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f_ui UIVert(appdata_ui v)
{
    v2f_ui o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    float4 vPosition = UnityObjectToClipPos(v.vertex);
    o.worldPosition = v.vertex;
    o.vertex = vPosition;

    // RectMask2D 소프트니스 계산 (UI/Default 와 동일)
    float2 pixelSize = vPosition.w;
    pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
    float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
    o.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw,
                    0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    o.color = v.color * _Color;
    return o;
}

// 모든 frag 마지막에 호출: RectMask2D 클리핑 + 알파 클립
fixed4 UIFinalize(fixed4 color, v2f_ui i)
{
    #ifdef UNITY_UI_CLIP_RECT
    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(i.mask.xy)) * i.mask.zw);
    color.a *= m.x * m.y;
    #endif

    #ifdef UNITY_UI_ALPHACLIP
    clip(color.a - 0.001);
    #endif

    return color;
}

// ───────────────────────── 샘플링 헬퍼 ─────────────────────────

fixed4 SampleUI(float2 uv)
{
    return tex2D(_MainTex, uv) + _TextureSampleAdd;
}

// uv 가 0~1 안이면 1, 밖이면 0 (텍스처 Clamp 로 가장자리가 늘어나 보이는 것 방지)
float InsideUV(float2 uv)
{
    float2 s = step(0.0, uv) * step(uv, 1.0);
    return s.x * s.y;
}

// ───────────────────────── 수학 헬퍼 ─────────────────────────

float Hash21(float2 p)
{
    p = frac(p * float2(123.34, 456.21));
    p += dot(p, p + 45.32);
    return frac(p.x * p.y);
}

float ValueNoise(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);
    float a = Hash21(i);
    float b = Hash21(i + float2(1, 0));
    float c = Hash21(i + float2(0, 1));
    float d = Hash21(i + float2(1, 1));
    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
}

// 4옥타브 프랙탈 노이즈 (0~1)
float Fbm(float2 p)
{
    float v = 0.0;
    float amp = 0.5;
    for (int k = 0; k < 4; k++)
    {
        v += amp * ValueNoise(p);
        p *= 2.02;
        amp *= 0.5;
    }
    return v / 0.9375;
}

// 색상환(Hue 0~1) → RGB
float3 HueToRGB(float h)
{
    return saturate(abs(frac(h + float3(0.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 1.0);
}

float Gray(float3 c)
{
    return dot(c, float3(0.299, 0.587, 0.114));
}

// 각도(도)를 방향 벡터로
float2 AngleToDir(float angleDeg)
{
    float r = radians(angleDeg);
    return float2(cos(r), sin(r));
}

// uv(0~1)를 dir 방향으로 투영해서 사각형 전체 기준 0~1 로 정규화
float ProjectUV(float2 uv, float2 dir)
{
    float extent = (abs(dir.x) + abs(dir.y)) * 0.5;
    return dot(uv - 0.5, dir) / extent * 0.5 + 0.5;
}

// A 위에 B 를 알파 합성 (straight alpha)
fixed4 BlendOver(fixed4 top, fixed4 bottom)
{
    float a = top.a + bottom.a * (1.0 - top.a);
    float3 rgb = (top.rgb * top.a + bottom.rgb * bottom.a * (1.0 - top.a)) / max(a, 1e-4);
    return fixed4(rgb, a);
}

#endif

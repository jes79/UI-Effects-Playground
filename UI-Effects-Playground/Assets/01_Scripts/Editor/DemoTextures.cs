using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

// 셰이더 데모에 쓰는 샘플 이미지를 코드로 그려서 PNG 로 저장 (외부 에셋 없이 데모를 구성하기 위함)
// 이미 만들어진 파일이 있으면 그대로 불러옴
public static class DemoTextures
{
    public static Sprite Landscape => Load("Landscape") ?? CreateSprite("Landscape", 512, 320, (u, v) => DrawLandscape(u, v, false));
    public static Sprite NightLandscape => Load("NightLandscape") ?? CreateSprite("NightLandscape", 512, 320, (u, v) => DrawLandscape(u, v, true));

    // 글로우/아웃라인이 퍼질 수 있도록 주변에 투명 여백이 있는 아이콘들
    public static Sprite StarIcon => Load("StarIcon") ?? CreateSprite("StarIcon", 256, 256, (u, v) => DrawIcon(u, v, StarShape, new Color(1f, 0.85f, 0.3f), new Color(1f, 0.55f, 0.15f)));
    public static Sprite HeartIcon => Load("HeartIcon") ?? CreateSprite("HeartIcon", 256, 256, (u, v) => DrawIcon(u, v, HeartShape, new Color(1f, 0.45f, 0.55f), new Color(0.85f, 0.15f, 0.3f)));
    public static Sprite DiamondIcon => Load("DiamondIcon") ?? CreateSprite("DiamondIcon", 256, 256, (u, v) => DrawIcon(u, v, DiamondShape, new Color(0.55f, 0.95f, 1f), new Color(0.2f, 0.55f, 0.95f)));

    public static Sprite HoloCard => Load("HoloCard") ?? CreateSprite("HoloCard", 320, 448, DrawHoloCard);
    public static Sprite GlassBackground => Load("GlassBackground") ?? CreateSprite("GlassBackground", 960, 540, DrawGlassBackground);

    private static Sprite Load(string name) => AssetDatabase.LoadAssetAtPath<Sprite>($"{TextureFolder}/{name}.png");

    // ───────────────────────── 풍경 ─────────────────────────

    private static Color DrawLandscape(float u, float v, bool night)
    {
        Color skyTop = night ? new Color(0.03f, 0.04f, 0.12f) : new Color(0.25f, 0.45f, 0.85f);
        Color skyHorizon = night ? new Color(0.15f, 0.12f, 0.3f) : new Color(1f, 0.75f, 0.55f);
        Color col = Color.Lerp(skyHorizon, skyTop, Mathf.Pow(Mathf.Clamp01((v - 0.3f) / 0.7f), 0.8f));

        // 해 / 달
        Vector2 sunPos = night ? new Vector2(0.25f, 0.72f) : new Vector2(0.7f, 0.6f);
        float aspect = 512f / 320f;
        float sd = new Vector2((u - sunPos.x) * aspect, v - sunPos.y).magnitude;
        Color sunColor = night ? new Color(0.95f, 0.95f, 1f) : new Color(1f, 0.95f, 0.7f);
        col = Color.Lerp(col, sunColor, Mathf.Clamp01((0.11f - sd) * 200f));
        col += sunColor * Mathf.Exp(-sd * 9f) * 0.25f;

        // 밤하늘 별
        if (night)
        {
            float h = Hash(Mathf.Floor(u * 120f), Mathf.Floor(v * 75f));
            if (h > 0.985f && v > 0.45f) col = Color.Lerp(col, Color.white, 0.8f);
        }

        // 먼 산 / 가까운 산 / 땅
        float far = 0.45f + 0.10f * Mathf.Sin(u * 7f + 1f) + 0.04f * Mathf.Sin(u * 19f);
        float near = 0.33f + 0.07f * Mathf.Sin(u * 11f + 2f) + 0.03f * Mathf.Sin(u * 29f + 0.5f);
        if (v < far) col = night ? new Color(0.12f, 0.12f, 0.25f) : new Color(0.42f, 0.42f, 0.68f);
        if (v < near) col = night ? new Color(0.07f, 0.09f, 0.16f) : new Color(0.22f, 0.32f, 0.45f);
        if (v < 0.18f)
        {
            Color ground = night ? new Color(0.05f, 0.12f, 0.1f) : new Color(0.25f, 0.55f, 0.3f);
            col = Color.Lerp(ground * 0.6f, ground, v / 0.18f);
        }

        col.a = 1f;
        return col;
    }

    // ───────────────────────── 아이콘 ─────────────────────────

    // shape: 중심 기준 좌표(-0.5~0.5) → 부호 있는 거리 근사값 (안쪽이 음수)
    private static Color DrawIcon(float u, float v, System.Func<Vector2, float> shape, Color light, Color dark)
    {
        Vector2 p = new Vector2(u - 0.5f, v - 0.5f);
        float d = shape(p);
        float alpha = Mathf.Clamp01(0.5f - d * 256f); // 1px 안티에일리어싱
        Color col = Color.Lerp(dark, light, Mathf.Clamp01(v * 1.2f - 0.1f));
        col = Color.Lerp(col, Color.white, Mathf.Clamp01(-d * 12f - 0.6f) * 0.35f); // 안쪽 하이라이트
        col.a = alpha;
        return col;
    }

    // 오각 별 SDF (Inigo Quilez 의 sdStar5) - r: 바깥 반지름, rf: 안쪽 꼭짓점 비율
    private static float StarShape(Vector2 p)
    {
        const float r = 0.32f, rf = 0.5f;
        Vector2 k1 = new Vector2(0.809016994375f, -0.587785252292f);
        Vector2 k2 = new Vector2(-k1.x, k1.y);
        p.x = Mathf.Abs(p.x);
        p -= 2f * Mathf.Max(Vector2.Dot(k1, p), 0f) * k1;
        p -= 2f * Mathf.Max(Vector2.Dot(k2, p), 0f) * k2;
        p.x = Mathf.Abs(p.x);
        p.y -= r;
        Vector2 ba = rf * new Vector2(-k1.y, k1.x) - new Vector2(0f, 1f);
        float h = Mathf.Clamp(Vector2.Dot(p, ba) / Vector2.Dot(ba, ba), 0f, r);
        return (p - ba * h).magnitude * Mathf.Sign(p.y * ba.x - p.x * ba.y);
    }

    private static float HeartShape(Vector2 p)
    {
        // 하트 방정식 (x² + y² - 1)³ - x²y³ ≤ 0 을 f / |∇f| 로 나눠 근사 거리로 사용
        const float e = 0.001f;
        float f = HeartImplicit(p);
        float gx = (HeartImplicit(p + new Vector2(e, 0f)) - f) / e;
        float gy = (HeartImplicit(p + new Vector2(0f, e)) - f) / e;
        return f / Mathf.Max(Mathf.Sqrt(gx * gx + gy * gy), 1e-4f);
    }

    private static float HeartImplicit(Vector2 p)
    {
        float x = p.x / 0.27f;
        float y = p.y / 0.27f + 0.15f;
        float a = x * x + y * y - 1f;
        return a * a * a - x * x * y * y * y;
    }

    private static float DiamondShape(Vector2 p)
    {
        return (Mathf.Abs(p.x) / 0.24f + Mathf.Abs(p.y) / 0.32f - 1f) * 0.2f;
    }

    // ───────────────────────── 홀로 카드 ─────────────────────────

    private static Color DrawHoloCard(float u, float v)
    {
        Vector2 p = new Vector2((u - 0.5f) * 320f, (v - 0.5f) * 448f);
        float d = RoundedBoxSdf(p, new Vector2(158f, 222f), 24f);
        float alpha = Mathf.Clamp01(0.5f - d);

        Color col = Color.Lerp(new Color(0.18f, 0.12f, 0.4f), new Color(0.35f, 0.25f, 0.65f), v);

        // 테두리
        float inner = RoundedBoxSdf(p, new Vector2(142f, 206f), 16f);
        if (inner > 0f) col = new Color(0.85f, 0.85f, 0.95f);

        // 가운데 별
        float star = StarShape(new Vector2(u - 0.5f, (v - 0.55f) * 1.4f) * 1.3f);
        if (star < 0f && inner <= 0f) col = Color.Lerp(new Color(1f, 0.9f, 0.5f), Color.white, Mathf.Clamp01(-star * 8f));

        // 아래 이름칸
        if (inner <= 0f && v > 0.12f && v < 0.22f && u > 0.15f && u < 0.85f) col = new Color(0.1f, 0.08f, 0.22f);

        col.a = alpha;
        return col;
    }

    // ───────────────────────── 유리 배경 ─────────────────────────

    private static Color DrawGlassBackground(float u, float v)
    {
        Color col = Color.Lerp(new Color(0.06f, 0.07f, 0.12f), new Color(0.1f, 0.08f, 0.18f), v);

        col += Blob(u, v, 0.25f, 0.65f, 0.16f, new Color(1f, 0.35f, 0.5f));
        col += Blob(u, v, 0.7f, 0.35f, 0.2f, new Color(0.3f, 0.6f, 1f));
        col += Blob(u, v, 0.55f, 0.8f, 0.12f, new Color(1f, 0.8f, 0.3f));
        col += Blob(u, v, 0.85f, 0.75f, 0.1f, new Color(0.4f, 1f, 0.7f));
        col += Blob(u, v, 0.12f, 0.2f, 0.12f, new Color(0.7f, 0.4f, 1f));

        // 굴절이 잘 보이도록 얇은 사선 줄무늬
        float stripe = Mathf.Sin((u * 16f + v * 9f) * Mathf.PI);
        col += Color.white * (stripe > 0.92f ? 0.12f : 0f);

        col.a = 1f;
        return col;
    }

    private static Color Blob(float u, float v, float cx, float cy, float radius, Color color)
    {
        float dx = (u - cx) * (960f / 540f);
        float dy = v - cy;
        float d = Mathf.Sqrt(dx * dx + dy * dy);
        return color * Mathf.Clamp01(1f - d / radius) * 0.9f;
    }

    private static float Hash(float x, float y)
    {
        float h = Mathf.Sin(x * 127.1f + y * 311.7f) * 43758.5453f;
        return h - Mathf.Floor(h);
    }
}

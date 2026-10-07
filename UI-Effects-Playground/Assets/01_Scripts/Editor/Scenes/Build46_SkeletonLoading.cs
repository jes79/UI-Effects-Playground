using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build46_SkeletonLoading
{
    [DemoScene("46_SkeletonLoading")]
    [MenuItem("Tools/UI Effects/Build Scene/46_SkeletonLoading")]
    public static void Build()
    {
        var canvas = NewScene("Skeleton Loading", "Placeholders share one shimmer material (canvas-space band), then real content slides in");
        var root = canvas.transform;

        var mat = CreateMaterial("SkeletonLoading", "UIEffects/SkeletonLoading");

        var card = CreateImage("Card", root, new Vector2(0f, 0f), new Vector2(900f, 560f), PanelColor, RoundedSprite, true);

        // ── 스켈레톤 (자리표시자) ──
        var skeleton = CreateRect("Skeleton", card.transform, Vector2.zero, Vector2.zero);
        Stretch(skeleton);
        var skeletonGroup = skeleton.gameObject.AddComponent<CanvasGroup>();

        Placeholder("Avatar", skeleton, new Vector2(-340f, 190f), new Vector2(110f, 110f), KnobSprite, mat);
        Placeholder("TitleBar", skeleton, new Vector2(-30f, 210f), new Vector2(500f, 36f), RoundedSprite, mat);
        Placeholder("SubtitleBar", skeleton, new Vector2(-120f, 160f), new Vector2(320f, 28f), RoundedSprite, mat);
        Placeholder("ImageBlock", skeleton, new Vector2(0f, -20f), new Vector2(800f, 240f), RoundedSprite, mat);
        Placeholder("Line1", skeleton, new Vector2(0f, -185f), new Vector2(800f, 26f), RoundedSprite, mat);
        Placeholder("Line2", skeleton, new Vector2(-100f, -230f), new Vector2(600f, 26f), RoundedSprite, mat);

        // ── 실제 콘텐츠 ──
        var content = CreateRect("Content", card.transform, Vector2.zero, Vector2.zero);
        Stretch(content);
        var contentGroup = content.gameObject.AddComponent<CanvasGroup>();
        contentGroup.alpha = 0f;

        var avatar = CreateImage("Avatar", content, new Vector2(-340f, 190f), new Vector2(110f, 110f), new Color32(255, 140, 120, 255), KnobSprite);
        var title = CreateText("Title", content, new Vector2(-30f, 210f), new Vector2(500f, 50f), "Unity UI Effects", 36f, TextColor, TMPro.TextAlignmentOptions.Left);
        title.fontStyle = TMPro.FontStyles.Bold;
        var subtitle = CreateText("Subtitle", content, new Vector2(-30f, 160f), new Vector2(500f, 40f), "posted 2 minutes ago", 26f, SubTextColor, TMPro.TextAlignmentOptions.Left);
        var picture = CreateImage("Picture", content, new Vector2(0f, -20f), new Vector2(800f, 240f), Color.white, DemoTextures.Landscape);
        var body = CreateText("Body", content, new Vector2(0f, -205f), new Vector2(800f, 80f),
            "Skeleton screens keep the layout stable while data loads,\nso the page never jumps when the real content arrives.", 26f, SubTextColor, TMPro.TextAlignmentOptions.TopLeft);

        RectTransform[] items = { avatar.rectTransform, title.rectTransform, subtitle.rectTransform, picture.rectTransform, body.rectTransform };
        foreach (var item in items) item.gameObject.AddComponent<CanvasGroup>();

        var loading = card.gameObject.AddComponent<SkeletonLoading>();
        Set(loading, "skeletonGroup", skeletonGroup);
        Set(loading, "contentGroup", contentGroup);
        SetArray(loading, "contentItems", items);

        var button = CreateButtonRow(root, -380f, "Reload")[0];
        OnClick(button, loading.Reload);

        SaveScene("46_SkeletonLoading");
    }

    private static void Placeholder(string name, Transform parent, Vector2 pos, Vector2 size, Sprite sprite, Material mat)
    {
        var img = CreateImage(name, parent, pos, size, Color.white, sprite, sprite == RoundedSprite);
        img.material = mat;
        img.raycastTarget = false;
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build23_PageSwipe
{
    private static readonly Color32[] PageColors =
    {
        new Color32(88, 140, 255, 255),
        new Color32(255, 120, 140, 255),
        new Color32(80, 200, 160, 255),
        new Color32(255, 180, 70, 255),
    };

    private const float PageWidth = 1000f;

    [DemoScene("23_PageSwipe")]
    [MenuItem("Tools/UI Effects/Build Scene/23_PageSwipe")]
    public static void Build()
    {
        var canvas = NewScene("Page Swipe", "Drag left / right (or flick) to change pages - it snaps with OutCubic");
        var root = canvas.transform;

        // 뷰포트: 드래그를 받는 영역 + 바깥 페이지를 잘라내는 마스크
        var viewport = CreateImage("Viewport", root, new Vector2(0f, 10f), new Vector2(PageWidth, 560f), new Color(1f, 1f, 1f, 0.02f));
        viewport.gameObject.AddComponent<RectMask2D>();

        var content = CreateRect("Content", viewport.transform, Vector2.zero, new Vector2(PageWidth * PageColors.Length, 560f));
        content.anchorMin = content.anchorMax = new Vector2(0f, 0.5f);
        content.pivot = new Vector2(0f, 0.5f);
        content.anchoredPosition = Vector2.zero;

        for (int i = 0; i < PageColors.Length; i++)
        {
            var page = CreateImage($"Page{i + 1}", content, Vector2.zero, new Vector2(PageWidth - 100f, 500f), PageColors[i], RoundedSprite, true);
            page.rectTransform.anchorMin = page.rectTransform.anchorMax = new Vector2(0f, 0.5f);
            page.rectTransform.anchoredPosition = new Vector2(i * PageWidth + PageWidth * 0.5f, 0f);
            page.raycastTarget = false;

            var title = CreateText("Title", page.transform, new Vector2(0f, 40f), new Vector2(800f, 140f), $"Page {i + 1}", 110f, Color.white);
            title.fontStyle = TMPro.FontStyles.Bold;
            CreateText("Hint", page.transform, new Vector2(0f, -80f), new Vector2(800f, 60f), "Swipe left / right", 34f, new Color(1f, 1f, 1f, 0.8f));
        }

        // 페이지 인디케이터
        var dots = new Image[PageColors.Length];
        for (int i = 0; i < dots.Length; i++)
        {
            float x = (i - (dots.Length - 1) * 0.5f) * 44f;
            dots[i] = CreateImage($"Dot{i + 1}", root, new Vector2(x, -320f), new Vector2(20f, 20f), Color.white, KnobSprite);
            dots[i].raycastTarget = false;
        }

        var swipe = viewport.gameObject.AddComponent<PageSwipe>();
        Set(swipe, "content", content);
        SetArray(swipe, "dots", dots);
        SetFloat(swipe, "pageWidth", PageWidth);

        var prev = CreateButton("PrevButton", root, new Vector2(-640f, 10f), new Vector2(120f, 120f), "<", PanelColor, 56f);
        var next = CreateButton("NextButton", root, new Vector2(640f, 10f), new Vector2(120f, 120f), ">", PanelColor, 56f);
        OnClick(prev, swipe.PrevPage);
        OnClick(next, swipe.NextPage);

        SaveScene("23_PageSwipe");
    }
}

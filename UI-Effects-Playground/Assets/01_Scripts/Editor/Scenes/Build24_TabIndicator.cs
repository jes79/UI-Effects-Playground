using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build24_TabIndicator
{
    private static readonly string[] TabNames = { "Home", "Shop", "Quest", "Profile" };

    private static readonly Color32[] PageColors =
    {
        new Color32(60, 70, 100, 255),
        new Color32(95, 60, 90, 255),
        new Color32(55, 90, 80, 255),
        new Color32(100, 80, 55, 255),
    };

    [DemoScene("24_TabIndicator")]
    [MenuItem("Tools/UI Effects/Build Scene/24_TabIndicator")]
    public static void Build()
    {
        var canvas = NewScene("Tab Indicator", "The underline stretches while moving to the selected tab, content slides in from that side");
        var root = canvas.transform;

        const float tabWidth = 300f;
        var bar = CreateImage("TabBar", root, new Vector2(0f, 230f), new Vector2(tabWidth * TabNames.Length, 110f), PanelColor, RoundedSprite, true);

        var tabs = new RectTransform[TabNames.Length];
        var labels = new TMPro.TMP_Text[TabNames.Length];
        var buttons = new Button[TabNames.Length];
        for (int i = 0; i < TabNames.Length; i++)
        {
            float x = (i - (TabNames.Length - 1) * 0.5f) * tabWidth;
            var tab = CreateImage($"Tab_{TabNames[i]}", bar.transform, new Vector2(x, 0f), new Vector2(tabWidth, 110f), new Color(1f, 1f, 1f, 0f));
            buttons[i] = tab.gameObject.AddComponent<Button>();
            buttons[i].transition = Selectable.Transition.None;
            tabs[i] = tab.rectTransform;

            labels[i] = CreateText("Label", tab.transform, Vector2.zero, Vector2.zero, TabNames[i], 36f, Color.white);
            Stretch(labels[i].rectTransform);
            labels[i].fontStyle = TMPro.FontStyles.Bold;
        }

        var indicator = CreateImage("Indicator", bar.transform, new Vector2(tabs[0].anchoredPosition.x, -44f), new Vector2(tabWidth * 0.6f, 8f), AccentColor, WhiteSprite);
        indicator.raycastTarget = false;

        // 탭별 내용
        var pageArea = CreateRect("Pages", root, new Vector2(0f, -110f), new Vector2(1200f, 480f));
        var pages = new CanvasGroup[TabNames.Length];
        for (int i = 0; i < TabNames.Length; i++)
        {
            var page = CreateImage($"Page_{TabNames[i]}", pageArea, Vector2.zero, new Vector2(1200f, 480f), PageColors[i], RoundedSprite, true);
            pages[i] = page.gameObject.AddComponent<CanvasGroup>();
            pages[i].alpha = i == 0 ? 1f : 0f; // 에디터에서는 첫 페이지만 보이게 (런타임에선 스크립트가 처리)
            var title = CreateText("Title", page.transform, new Vector2(0f, 40f), new Vector2(1000f, 120f), TabNames[i], 90f, Color.white);
            title.fontStyle = TMPro.FontStyles.Bold;
            CreateText("Body", page.transform, new Vector2(0f, -70f), new Vector2(1000f, 60f), $"{TabNames[i]} tab content", 34f, new Color(1f, 1f, 1f, 0.7f));
        }

        var tabIndicator = bar.gameObject.AddComponent<TabIndicator>();
        Set(tabIndicator, "indicator", indicator.rectTransform);
        SetArray(tabIndicator, "tabs", tabs);
        SetArray(tabIndicator, "tabLabels", labels);
        SetArray(tabIndicator, "pages", pages);

        for (int i = 0; i < buttons.Length; i++)
            OnClick(buttons[i], tabIndicator.SelectTab, i);

        SaveScene("24_TabIndicator");
    }
}

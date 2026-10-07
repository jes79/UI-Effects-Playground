using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build22_AccordionExpand
{
    private static readonly string[] Titles =
    {
        "What is DOTween?",
        "How does the accordion work?",
        "Why RectMask2D?",
        "Can several items stay open?",
    };

    private static readonly string[] Bodies =
    {
        "A fast and efficient tween engine for Unity.\nIt animates almost anything with a single line of code.",
        "Each item tweens its LayoutElement.preferredHeight.\nThe VerticalLayoutGroup re-arranges the list every frame.",
        "The content area clips its text while the height is animating,\nso nothing spills outside during the transition.",
        "Turn off 'Close Others' on AccordionItem\nto let multiple items stay expanded at the same time.",
    };

    private const float ExpandedHeight = 150f;

    [DemoScene("22_AccordionExpand")]
    [MenuItem("Tools/UI Effects/Build Scene/22_AccordionExpand")]
    public static void Build()
    {
        var canvas = NewScene("Accordion Expand", "Click a header to expand it - the list below is pushed down smoothly");
        var root = canvas.transform;

        var list = CreateRect("AccordionList", root, new Vector2(0f, 330f), new Vector2(1000f, 0f));
        list.pivot = new Vector2(0.5f, 1f);
        list.anchoredPosition = new Vector2(0f, 330f);
        var listLayout = list.gameObject.AddComponent<VerticalLayoutGroup>();
        listLayout.spacing = 14f;
        listLayout.childControlWidth = true;
        listLayout.childControlHeight = true;
        listLayout.childForceExpandWidth = true;
        listLayout.childForceExpandHeight = false;

        // 리스트 높이 = 항목들의 선호 높이 합 (고정 높이면 공간이 부족할 때 항목이 최소 높이로 찌그러짐)
        var fitter = list.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        for (int i = 0; i < Titles.Length; i++)
            CreateItem(list, i, i == 0);

        SaveScene("22_AccordionExpand");
    }

    private static void CreateItem(Transform list, int index, bool startExpanded)
    {
        var item = CreateImage($"Item{index + 1}", list, Vector2.zero, new Vector2(1000f, 90f), PanelColor, RoundedSprite, true);
        var itemLayout = item.gameObject.AddComponent<VerticalLayoutGroup>();
        itemLayout.childControlWidth = true;
        itemLayout.childControlHeight = true;
        itemLayout.childForceExpandWidth = true;
        itemLayout.childForceExpandHeight = false;

        // 헤더 (클릭 영역)
        var header = CreateImage("Header", item.transform, Vector2.zero, new Vector2(1000f, 90f), new Color(1f, 1f, 1f, 0f));
        header.gameObject.AddComponent<LayoutElement>().preferredHeight = 90f;
        var headerButton = header.gameObject.AddComponent<Button>();
        headerButton.transition = Selectable.Transition.None;

        var title = CreateText("Title", header.transform, Vector2.zero, Vector2.zero, Titles[index], 34f, TextColor, TMPro.TextAlignmentOptions.Left);
        Stretch(title.rectTransform);
        title.rectTransform.offsetMin = new Vector2(36f, 0f);
        title.rectTransform.offsetMax = new Vector2(-100f, 0f);
        title.fontStyle = TMPro.FontStyles.Bold;

        var icon = CreateText("Icon", header.transform, Vector2.zero, new Vector2(70f, 70f), "+", 60f, AccentColor);
        icon.rectTransform.anchorMin = icon.rectTransform.anchorMax = new Vector2(1f, 0.5f);
        icon.rectTransform.anchoredPosition = new Vector2(-50f, 0f);

        // 본문 (높이가 0 ↔ expandedHeight 로 트윈됨)
        var content = CreateRect("Content", item.transform, Vector2.zero, new Vector2(1000f, 0f));
        var contentLayout = content.gameObject.AddComponent<LayoutElement>();
        contentLayout.preferredHeight = startExpanded ? ExpandedHeight : 0f;
        content.gameObject.AddComponent<RectMask2D>();
        var contentGroup = content.gameObject.AddComponent<CanvasGroup>();
        contentGroup.alpha = startExpanded ? 1f : 0f;

        var body = CreateText("Body", content, Vector2.zero, Vector2.zero, Bodies[index], 28f, SubTextColor, TMPro.TextAlignmentOptions.TopLeft);
        var bodyRect = body.rectTransform;
        bodyRect.anchorMin = new Vector2(0f, 1f);
        bodyRect.anchorMax = new Vector2(1f, 1f);
        bodyRect.pivot = new Vector2(0.5f, 1f);
        bodyRect.offsetMin = new Vector2(36f, -ExpandedHeight + 20f);
        bodyRect.offsetMax = new Vector2(-36f, 0f);

        var accordion = item.gameObject.AddComponent<AccordionItem>();
        Set(accordion, "contentLayout", contentLayout);
        Set(accordion, "contentGroup", contentGroup);
        Set(accordion, "icon", icon.rectTransform);
        SetFloat(accordion, "expandedHeight", ExpandedHeight);
        SetBool(accordion, "startExpanded", startExpanded);

        OnClick(headerButton, accordion.Toggle);
    }
}

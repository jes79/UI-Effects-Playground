using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build21_PopupOpenClose
{
    [DemoScene("21_PopupOpenClose")]
    [MenuItem("Tools/UI Effects/Build Scene/21_PopupOpenClose")]
    public static void Build()
    {
        var canvas = NewScene("Popup Open / Close", "Dim fades in while the panel pops with OutBack, closes with InBack");
        var root = canvas.transform;

        var openButton = CreateButton("OpenButton", root, new Vector2(0f, -40f), new Vector2(340f, 100f), "Open Popup");

        // 팝업 루트 (딤 + 패널)
        var popupRoot = CreateRect("Popup", root, Vector2.zero, Vector2.zero);
        Stretch(popupRoot);

        var dim = CreateImage("Dim", popupRoot, Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.65f));
        Stretch(dim.rectTransform);
        var dimGroup = dim.gameObject.AddComponent<CanvasGroup>();
        var dimButton = dim.gameObject.AddComponent<Button>();
        dimButton.transition = Selectable.Transition.None;

        var panel = CreateImage("Panel", popupRoot, Vector2.zero, new Vector2(720f, 460f), PanelColor, RoundedSprite, true);
        var panelGroup = panel.gameObject.AddComponent<CanvasGroup>();

        var title = CreateText("Title", panel.transform, new Vector2(0f, 150f), new Vector2(640f, 70f), "Reward Get!", 48f, TextColor);
        title.fontStyle = TMPro.FontStyles.Bold;
        CreateText("Body", panel.transform, new Vector2(0f, 30f), new Vector2(640f, 140f),
            "You received <color=#FFD54A>100 Gems</color>.\nTap outside or press Close.", 32f, SubTextColor);
        var closeButton = CreateButton("CloseButton", panel.transform, new Vector2(0f, -150f), new Vector2(260f, 84f), "Close");

        var controller = CreateRect("PopupController", root, Vector2.zero, Vector2.zero);
        var popup = controller.gameObject.AddComponent<PopupOpenClose>();
        Set(popup, "popupRoot", popupRoot.gameObject);
        Set(popup, "dimGroup", dimGroup);
        Set(popup, "panel", panel.rectTransform);
        Set(popup, "panelGroup", panelGroup);

        OnClick(openButton, popup.Open);
        OnClick(closeButton, popup.Close);
        OnClick(dimButton, popup.Close); // 바깥(딤) 클릭 시 닫기

        SaveScene("21_PopupOpenClose");
    }
}

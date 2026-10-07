using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build28_TutorialSpotlight
{
    [DemoScene("28_TutorialSpotlight")]
    [MenuItem("Tools/UI Effects/Build Scene/28_TutorialSpotlight")]
    public static void Build()
    {
        var canvas = NewScene("Tutorial Spotlight", "Darken the screen and move a hole between UI elements step by step");
        var root = canvas.transform;

        // ── 강조할 가짜 게임 UI ──
        var profile = CreateImage("ProfileIcon", root, new Vector2(-760f, 330f), new Vector2(140f, 140f), new Color32(255, 140, 120, 255), KnobSprite);
        var profileLabel = CreateText("Label", profile.transform, Vector2.zero, Vector2.zero, "ME", 44f, Color.white);
        Stretch(profileLabel.rectTransform);
        profileLabel.fontStyle = TMPro.FontStyles.Bold;

        var gold = CreateImage("GoldPanel", root, new Vector2(700f, 330f), new Vector2(320f, 90f), PanelColor, RoundedSprite, true);
        CreateText("Label", gold.transform, Vector2.zero, new Vector2(300f, 80f), "<color=#FFD54A>G</color>  1,250", 40f, Color.white).fontStyle = TMPro.FontStyles.Bold;

        var shop = CreateButton("ShopButton", root, new Vector2(0f, 60f), new Vector2(360f, 110f), "Shop", new Color32(150, 100, 230, 255), 42f);
        var battle = CreateButton("BattleButton", root, new Vector2(620f, -130f), new Vector2(340f, 120f), "Battle", new Color32(232, 72, 85, 255), 44f);
        var start = CreateButton("StartTutorialButton", root, new Vector2(-620f, -330f), new Vector2(360f, 100f), "Start Tutorial");

        // ── 오버레이 ──
        var overlay = CreateImage("TutorialOverlay", root, Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0f));
        Stretch(overlay.rectTransform);
        var overlayGroup = overlay.gameObject.AddComponent<CanvasGroup>();
        overlayGroup.alpha = 0f;
        overlayGroup.blocksRaycasts = false;
        var overlayButton = overlay.gameObject.AddComponent<Button>();
        overlayButton.transition = Selectable.Transition.None;

        Color dim = new Color(0f, 0f, 0f, 0.78f);

        // 가운데가 부드럽게 뚫린 원형 텍스처
        var holeSprite = CreateSprite("SpotlightHole", 256, 256, (u, v) =>
        {
            float d = Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f));
            float a = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.40f, 0.5f, d));
            return new Color(1f, 1f, 1f, a);
        });

        var spotlight = CreateImage("Spotlight", overlay.transform, Vector2.zero, new Vector2(400f, 200f), dim, holeSprite);
        spotlight.raycastTarget = false;

        // 구멍 바깥을 덮는 어두운 사각형 4개 (spotlight 의 자식이라 같이 움직이고 같이 늘어남)
        CreateDimPiece("Top", spotlight.transform, dim, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0f), new Vector2(10000f, 6000f));
        CreateDimPiece("Bottom", spotlight.transform, dim, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(10000f, 6000f));
        CreateDimPiece("Left", spotlight.transform, dim, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 0.5f), new Vector2(6000f, 0f));
        CreateDimPiece("Right", spotlight.transform, dim, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0.5f), new Vector2(6000f, 0f));

        // 안내 문구 박스 (구멍 아래에 따라다님)
        var messageBox = CreateImage("MessageBox", spotlight.transform, Vector2.zero, new Vector2(660f, 150f), PanelColor, RoundedSprite, true);
        messageBox.rectTransform.anchorMin = messageBox.rectTransform.anchorMax = new Vector2(0.5f, 0f);
        messageBox.rectTransform.pivot = new Vector2(0.5f, 1f);
        messageBox.rectTransform.anchoredPosition = new Vector2(0f, -30f);
        messageBox.raycastTarget = false;
        var messageGroup = messageBox.gameObject.AddComponent<CanvasGroup>();
        var messageText = CreateText("Message", messageBox.transform, Vector2.zero, Vector2.zero, "", 32f, Color.white);
        Stretch(messageText.rectTransform, 20f);

        var tutorial = overlay.gameObject.AddComponent<TutorialSpotlight>();
        Set(tutorial, "overlayGroup", overlayGroup);
        Set(tutorial, "spotlight", spotlight.rectTransform);
        Set(tutorial, "messageGroup", messageGroup);
        Set(tutorial, "messageText", messageText);
        SetArray(tutorial, "targets", profile.rectTransform, gold.rectTransform, shop.transform, battle.transform);
        SetStrings(tutorial, "messages",
            "This is your profile.",
            "Your gold is shown here.",
            "Visit the shop to buy new items.",
            "Press Battle to start a match!");

        OnClick(start, tutorial.StartTutorial);
        OnClick(overlayButton, tutorial.Next);

        SaveScene("28_TutorialSpotlight");
    }

    private static void CreateDimPiece(string name, Transform parent, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 size)
    {
        var piece = CreateImage(name, parent, Vector2.zero, size, color);
        var rt = piece.rectTransform;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = size;
        piece.raycastTarget = false;
    }
}

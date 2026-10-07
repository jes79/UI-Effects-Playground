using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build27_CardFlip
{
    private static readonly string[] CardNames = { "FIRE", "WATER", "LEAF" };

    private static readonly Color32[] FrontColors =
    {
        new Color32(240, 100, 80, 255),
        new Color32(80, 160, 240, 255),
        new Color32(90, 200, 120, 255),
    };

    [DemoScene("27_CardFlip")]
    [MenuItem("Tools/UI Effects/Build Scene/27_CardFlip")]
    public static void Build()
    {
        var canvas = NewScene("Card Flip", "Click a card - it rotates to 90 deg, swaps faces, then rotates back");
        var root = canvas.transform;

        for (int i = 0; i < CardNames.Length; i++)
        {
            float x = (i - 1) * 420f;

            // 카드 루트: 투명 이미지로 클릭만 받고, 앞/뒷면은 자식으로 둠
            var card = CreateImage($"Card_{CardNames[i]}", root, new Vector2(x, -30f), new Vector2(300f, 440f), new Color(1f, 1f, 1f, 0f));
            var button = card.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;

            var back = CreateImage("Back", card.transform, Vector2.zero, Vector2.zero, new Color32(40, 46, 80, 255), RoundedSprite, true);
            Stretch(back.rectTransform);
            back.raycastTarget = false;
            var pattern = CreateImage("Pattern", back.transform, Vector2.zero, Vector2.zero, new Color32(70, 80, 130, 255), RoundedSprite, true);
            Stretch(pattern.rectTransform, 22f);
            pattern.raycastTarget = false;
            CreateText("Mark", back.transform, Vector2.zero, new Vector2(200f, 200f), "?", 160f, new Color(1f, 1f, 1f, 0.85f)).fontStyle = TMPro.FontStyles.Bold;

            var front = CreateImage("Front", card.transform, Vector2.zero, Vector2.zero, FrontColors[i], RoundedSprite, true);
            Stretch(front.rectTransform);
            front.raycastTarget = false;
            var icon = CreateImage("Icon", front.transform, new Vector2(0f, 50f), new Vector2(170f, 170f), new Color(1f, 1f, 1f, 0.9f), KnobSprite);
            icon.raycastTarget = false;
            CreateText("Name", front.transform, new Vector2(0f, -140f), new Vector2(280f, 70f), CardNames[i], 44f, Color.white).fontStyle = TMPro.FontStyles.Bold;

            var flip = card.gameObject.AddComponent<CardFlip>();
            Set(flip, "card", card.rectTransform);
            Set(flip, "frontFace", front.gameObject);
            Set(flip, "backFace", back.gameObject);
            front.gameObject.SetActive(false);

            OnClick(button, flip.Flip);
        }

        SaveScene("27_CardFlip");
    }
}

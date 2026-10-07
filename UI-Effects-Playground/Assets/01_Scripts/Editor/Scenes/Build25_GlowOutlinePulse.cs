using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build25_GlowOutlinePulse
{
    [DemoScene("25_GlowOutlinePulse")]
    [MenuItem("Tools/UI Effects/Build Scene/25_GlowOutlinePulse")]
    public static void Build()
    {
        var canvas = NewScene("Glow / Outline Pulse", "A soft glow behind the button and its Outline breathe in a Yoyo loop");
        var root = canvas.transform;

        // 9-slice 로 늘려 쓰는 부드러운 둥근 사각형 글로우 (텍스처 가장자리 48px 지점이 버튼 테두리와 맞닿음)
        var softGlow = CreateSprite("SoftGlow", 128, 128, (u, v) =>
        {
            Vector2 p = new Vector2(u * 128f - 64f, v * 128f - 64f);
            float d = RoundedBoxSdf(p, new Vector2(16f, 16f), 12f);
            float a = d <= 0f ? 1f : Mathf.Pow(Mathf.Clamp01(1f - d / 46f), 2.2f);
            return new Color(1f, 1f, 1f, a);
        }, new Vector4(60f, 60f, 60f, 60f));

        Vector2 buttonSize = new Vector2(440f, 130f);
        var glow = CreateImage("Glow", root, new Vector2(0f, 30f), buttonSize + Vector2.one * 96f, new Color(0.45f, 0.75f, 1f, 0.25f), softGlow, true);
        glow.raycastTarget = false;

        var button = CreateButton("PlayButton", root, new Vector2(0f, 30f), buttonSize, "PLAY", AccentColor, 52f);
        var outline = button.gameObject.AddComponent<Outline>();
        outline.effectColor = new Color(0.75f, 0.9f, 1f, 0.9f);
        outline.effectDistance = new Vector2(1f, -1f);

        var pulse = button.gameObject.AddComponent<GlowOutlinePulse>();
        Set(pulse, "glowImage", glow);
        Set(pulse, "outline", outline);

        var toggle = CreateButtonRow(root, -280f, "Toggle Pulse")[0];
        toggle.GetComponent<Image>().color = PanelColor;
        OnClick(toggle, pulse.TogglePulse);

        SaveScene("25_GlowOutlinePulse");
    }
}

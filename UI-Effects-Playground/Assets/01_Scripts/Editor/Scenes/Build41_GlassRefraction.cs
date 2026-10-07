using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build41_GlassRefraction
{
    [DemoScene("41_GlassRefraction")]
    [MenuItem("Tools/UI Effects/Build Scene/41_GlassRefraction")]
    public static void Build()
    {
        var canvas = NewScene("Glass Refraction", "Drag the panel - the background behind it is refracted, blurred and tinted");
        var root = canvas.transform;

        var backgroundSprite = DemoTextures.GlassBackground;

        // 화면 전체 배경 (타이틀보다 뒤에 오도록 기본 배경 바로 위에 배치)
        var background = CreateImage("GlassBackground", root, Vector2.zero, Vector2.zero, Color.white, backgroundSprite);
        Stretch(background.rectTransform);
        background.raycastTarget = false;
        background.transform.SetSiblingIndex(1);

        var mat = CreateMaterial("GlassRefraction", "UIEffects/GlassRefraction", m => m.SetTexture("_BackgroundTex", backgroundSprite.texture));

        var panel = CreateEffectImage("GlassPanel", root, new Vector2(0f, -30f), new Vector2(640f, 400f), null, mat);
        panel.raycastTarget = true; // 드래그를 받아야 함

        var title = CreateText("Title", panel.transform, new Vector2(0f, 40f), new Vector2(560f, 80f), "Glass Panel", 56f, Color.white);
        title.fontStyle = TMPro.FontStyles.Bold;
        CreateText("Hint", panel.transform, new Vector2(0f, -40f), new Vector2(560f, 50f), "Drag me around", 32f, new Color(1f, 1f, 1f, 0.75f));

        var glass = panel.gameObject.AddComponent<GlassPanel>();
        Set(glass, "targetGraphic", panel);
        Set(glass, "background", background.rectTransform);

        SaveScene("41_GlassRefraction");
    }
}

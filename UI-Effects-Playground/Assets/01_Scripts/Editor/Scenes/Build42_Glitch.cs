using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build42_Glitch
{
    [DemoScene("42_Glitch")]
    [MenuItem("Tools/UI Effects/Build Scene/42_Glitch")]
    public static void Build()
    {
        var canvas = NewScene("Glitch", "Rows and blocks shift randomly with RGB split and inverted blocks");
        var root = canvas.transform;

        var mat = CreateMaterial("Glitch", "UIEffects/Glitch");

        var picture = CreateEffectImage("Picture", root, new Vector2(-330f, 30f), new Vector2(580f, 362f), DemoTextures.Landscape, mat);
        var card = CreateEffectImage("HoloCard", root, new Vector2(440f, 30f), new Vector2(280f, 392f), DemoTextures.HoloCard, mat);

        var pictureGlitch = picture.gameObject.AddComponent<GlitchEffect>();
        Set(pictureGlitch, "targetGraphic", picture);
        var cardGlitch = card.gameObject.AddComponent<GlitchEffect>();
        Set(cardGlitch, "targetGraphic", card);

        var buttons = CreateButtonRow(root, -330f, "Glitch!", "Toggle Auto");
        OnClick(buttons[0], pictureGlitch.Burst);
        OnClick(buttons[0], cardGlitch.Burst);
        OnClick(buttons[1], pictureGlitch.ToggleAutoBurst);
        OnClick(buttons[1], cardGlitch.ToggleAutoBurst);

        SaveScene("42_Glitch");
    }
}

using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build34_Dissolve
{
    [DemoScene("34_Dissolve")]
    [MenuItem("Tools/UI Effects/Build Scene/34_Dissolve")]
    public static void Build()
    {
        var canvas = NewScene("Dissolve", "Noise below the threshold burns away with a glowing edge (_DissolveAmount 0 -> 1)");
        var root = canvas.transform;

        var mat = CreateMaterial("Dissolve", "UIEffects/Dissolve");

        var picture = CreateEffectImage("Picture", root, new Vector2(-330f, 30f), new Vector2(580f, 362f), DemoTextures.Landscape, mat);
        var card = CreateEffectImage("HoloCard", root, new Vector2(440f, 30f), new Vector2(280f, 392f), DemoTextures.HoloCard, mat);

        var pictureDissolve = picture.gameObject.AddComponent<DissolveEffect>();
        Set(pictureDissolve, "targetGraphic", picture);
        var cardDissolve = card.gameObject.AddComponent<DissolveEffect>();
        Set(cardDissolve, "targetGraphic", card);

        var buttons = CreateButtonRow(root, -330f, "Dissolve", "Appear");
        OnClick(buttons[0], pictureDissolve.Dissolve);
        OnClick(buttons[0], cardDissolve.Dissolve);
        OnClick(buttons[1], pictureDissolve.Appear);
        OnClick(buttons[1], cardDissolve.Appear);

        SaveScene("34_Dissolve");
    }
}

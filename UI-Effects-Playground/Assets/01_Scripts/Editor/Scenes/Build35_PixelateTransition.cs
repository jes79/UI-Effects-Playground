using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build35_PixelateTransition
{
    [DemoScene("35_PixelateTransition")]
    [MenuItem("Tools/UI Effects/Build Scene/35_PixelateTransition")]
    public static void Build()
    {
        var canvas = NewScene("Pixelate Transition", "Pixelate out -> swap the image at the most pixelated moment -> pixelate back in");
        var root = canvas.transform;

        var mat = CreateMaterial("PixelateTransition", "UIEffects/PixelateTransition");

        var picture = CreateEffectImage("Picture", root, new Vector2(0f, 30f), new Vector2(720f, 450f), DemoTextures.Landscape, mat);
        var pixelate = picture.gameObject.AddComponent<PixelateTransition>();
        Set(pixelate, "targetGraphic", picture);
        SetArray(pixelate, "sprites", DemoTextures.Landscape, DemoTextures.NightLandscape);

        var button = CreateButtonRow(root, -330f, "Next Image")[0];
        OnClick(button, pixelate.Next);

        SaveScene("35_PixelateTransition");
    }
}

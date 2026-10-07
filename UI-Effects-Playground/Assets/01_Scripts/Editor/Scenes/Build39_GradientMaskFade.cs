using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build39_GradientMaskFade
{
    [DemoScene("39_GradientMaskFade")]
    [MenuItem("Tools/UI Effects/Build Scene/39_GradientMaskFade")]
    public static void Build()
    {
        var canvas = NewScene("Gradient Mask Fade", "A soft gradient edge sweeps across to show / hide the image in any direction");
        var root = canvas.transform;

        var mat = CreateMaterial("GradientMaskFade", "UIEffects/GradientMaskFade");

        var picture = CreateEffectImage("Picture", root, new Vector2(0f, 30f), new Vector2(720f, 450f), DemoTextures.Landscape, mat);
        var fade = picture.gameObject.AddComponent<GradientMaskFade>();
        Set(fade, "targetGraphic", picture);

        var buttons = CreateButtonRow(root, -330f, "Show / Hide", "Left > Right", "Bottom > Top", "Diagonal");
        OnClick(buttons[0], fade.Toggle);
        OnClick(buttons[1], fade.SetAngle, 0f);
        OnClick(buttons[2], fade.SetAngle, 90f);
        OnClick(buttons[3], fade.SetAngle, 315f);

        SaveScene("39_GradientMaskFade");
    }
}

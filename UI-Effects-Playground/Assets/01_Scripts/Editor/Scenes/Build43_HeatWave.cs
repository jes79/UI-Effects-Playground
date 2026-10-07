using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build43_HeatWave
{
    [DemoScene("43_HeatWave")]
    [MenuItem("Tools/UI Effects/Build Scene/43_HeatWave")]
    public static void Build()
    {
        var canvas = NewScene("Heat Wave", "Rising noise shimmers the image like hot air - stronger near the bottom");
        var root = canvas.transform;

        var mat = CreateMaterial("HeatWave", "UIEffects/HeatWave");

        var picture = CreateEffectImage("Picture", root, new Vector2(0f, 30f), new Vector2(760f, 475f), DemoTextures.Landscape, mat);
        var heat = picture.gameObject.AddComponent<HeatWaveEffect>();
        Set(heat, "targetGraphic", picture);

        var button = CreateButtonRow(root, -340f, "Toggle Heat")[0];
        OnClick(button, heat.ToggleHeat);

        SaveScene("43_HeatWave");
    }
}

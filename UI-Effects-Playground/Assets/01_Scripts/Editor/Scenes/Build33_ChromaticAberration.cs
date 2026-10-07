using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build33_ChromaticAberration
{
    [DemoScene("33_ChromaticAberration")]
    [MenuItem("Tools/UI Effects/Build Scene/33_ChromaticAberration")]
    public static void Build()
    {
        var canvas = NewScene("Chromatic Aberration", "R / G / B channels are offset in different directions - spike it on hit");
        var root = canvas.transform;

        var mat = CreateMaterial("ChromaticAberration", "UIEffects/ChromaticAberration");

        var picture = CreateEffectImage("Picture", root, new Vector2(-340f, 30f), new Vector2(560f, 350f), DemoTextures.Landscape, mat);
        var star = CreateEffectImage("StarIcon", root, new Vector2(420f, 30f), new Vector2(340f, 340f), DemoTextures.StarIcon, mat);

        var pictureHit = picture.gameObject.AddComponent<ChromaticHitEffect>();
        Set(pictureHit, "targetGraphic", picture);
        var starHit = star.gameObject.AddComponent<ChromaticHitEffect>();
        Set(starHit, "targetGraphic", star);

        var button = CreateButtonRow(root, -330f, "Hit!")[0];
        OnClick(button, pictureHit.Hit);
        OnClick(button, starHit.Hit);

        SaveScene("33_ChromaticAberration");
    }
}

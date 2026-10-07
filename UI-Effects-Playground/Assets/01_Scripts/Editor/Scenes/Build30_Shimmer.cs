using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build30_Shimmer
{
    [DemoScene("30_Shimmer")]
    [MenuItem("Tools/UI Effects/Build Scene/30_Shimmer")]
    public static void Build()
    {
        var canvas = NewScene("Shimmer / Shine", "A slanted light band sweeps across the image (_ShineLocation 0 -> 1)");
        var root = canvas.transform;

        var mat = CreateMaterial("Shimmer", "UIEffects/Shimmer", m =>
        {
            m.SetColor("_ShineColor", new Color(1f, 1f, 1f, 0.9f));
            m.SetFloat("_ShineWidth", 0.08f);
            m.SetFloat("_ShineIntensity", 0.7f);
        });

        // 자동 반복
        var picture = CreateEffectImage("Picture", root, new Vector2(-380f, 30f), new Vector2(560f, 350f), DemoTextures.Landscape, mat);
        var loopShimmer = picture.gameObject.AddComponent<ShimmerEffect>();
        Set(loopShimmer, "targetGraphic", picture);
        CreateCaption(root, new Vector2(-380f, -180f), "Auto loop");

        // 버튼으로 한 번씩
        var star = CreateEffectImage("StarIcon", root, new Vector2(420f, 30f), new Vector2(340f, 340f), DemoTextures.StarIcon, mat);
        var onceShimmer = star.gameObject.AddComponent<ShimmerEffect>();
        Set(onceShimmer, "targetGraphic", star);
        SetBool(onceShimmer, "autoLoop", false);
        CreateCaption(root, new Vector2(420f, -180f), "PlayOnce()");

        var button = CreateButtonRow(root, -330f, "Shine Star")[0];
        OnClick(button, onceShimmer.PlayOnce);

        SaveScene("30_Shimmer");
    }
}

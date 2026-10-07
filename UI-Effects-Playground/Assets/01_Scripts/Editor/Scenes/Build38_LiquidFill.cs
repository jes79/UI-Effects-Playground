using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build38_LiquidFill
{
    [DemoScene("38_LiquidFill")]
    [MenuItem("Tools/UI Effects/Build Scene/38_LiquidFill")]
    public static void Build()
    {
        var canvas = NewScene("Liquid Fill", "Two layers of sine waves inside a round container, sloshing whenever the level changes");
        var root = canvas.transform;

        var mat = CreateMaterial("LiquidFill", "UIEffects/LiquidFill");

        // 스프라이트 없이 흰색 Image 에 적용 (원형 용기는 셰이더가 그림)
        var orb = CreateEffectImage("LiquidOrb", root, new Vector2(0f, 30f), new Vector2(440f, 440f), null, mat);

        var percent = CreateText("PercentText", orb.transform, Vector2.zero, new Vector2(300f, 120f), "40%", 80f, Color.white);
        percent.fontStyle = TMPro.FontStyles.Bold;

        var liquid = orb.gameObject.AddComponent<LiquidFillEffect>();
        Set(liquid, "targetGraphic", orb);
        Set(liquid, "percentText", percent);

        var buttons = CreateButtonRow(root, -330f, "-10%", "+10%", "Random");
        OnClick(buttons[0], liquid.AddFill, -0.1f);
        OnClick(buttons[1], liquid.AddFill, 0.1f);
        OnClick(buttons[2], liquid.RandomFill);

        SaveScene("38_LiquidFill");
    }
}

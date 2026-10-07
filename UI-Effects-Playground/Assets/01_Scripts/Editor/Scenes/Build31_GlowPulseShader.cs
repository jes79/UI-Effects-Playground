using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build31_GlowPulseShader
{
    [DemoScene("31_GlowPulseShader")]
    [MenuItem("Tools/UI Effects/Build Scene/31_GlowPulseShader")]
    public static void Build()
    {
        var canvas = NewScene("Glow Pulse (Shader)", "Outer glow built from the sprite alpha, pulsing over time");
        var root = canvas.transform;

        var goldGlow = CreateMaterial("GlowPulse_Gold", "UIEffects/GlowPulse", m => m.SetColor("_GlowColor", new Color(1f, 0.8f, 0.3f, 1f)));
        var pinkGlow = CreateMaterial("GlowPulse_Pink", "UIEffects/GlowPulse", m => m.SetColor("_GlowColor", new Color(1f, 0.4f, 0.6f, 1f)));

        var star = CreateEffectImage("StarIcon", root, new Vector2(-280f, 30f), new Vector2(380f, 380f), DemoTextures.StarIcon, goldGlow);
        var heart = CreateEffectImage("HeartIcon", root, new Vector2(280f, 30f), new Vector2(380f, 380f), DemoTextures.HeartIcon, pinkGlow);

        var starGlow = star.gameObject.AddComponent<GlowPulseShaderEffect>();
        Set(starGlow, "targetGraphic", star);
        var heartGlow = heart.gameObject.AddComponent<GlowPulseShaderEffect>();
        Set(heartGlow, "targetGraphic", heart);

        var buttons = CreateButtonRow(root, -330f, "Boost", "Toggle Glow");
        OnClick(buttons[0], starGlow.Boost);
        OnClick(buttons[0], heartGlow.Boost);
        OnClick(buttons[1], starGlow.ToggleGlow);
        OnClick(buttons[1], heartGlow.ToggleGlow);

        SaveScene("31_GlowPulseShader");
    }
}

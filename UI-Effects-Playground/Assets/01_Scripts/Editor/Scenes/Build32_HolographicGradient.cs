using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build32_HolographicGradient
{
    [DemoScene("32_HolographicGradient")]
    [MenuItem("Tools/UI Effects/Build Scene/32_HolographicGradient")]
    public static void Build()
    {
        var canvas = NewScene("Holographic Gradient", "Move the mouse over the card - the tilt drives the rainbow, glare and sparkles");
        var root = canvas.transform;

        var mat = CreateMaterial("HolographicGradient", "UIEffects/HolographicGradient");

        var card = CreateEffectImage("HoloCard", root, new Vector2(0f, -30f), new Vector2(400f, 560f), DemoTextures.HoloCard, mat);
        card.raycastTarget = true;

        var holo = card.gameObject.AddComponent<HolographicCard>();
        Set(holo, "targetGraphic", card);

        SaveScene("32_HolographicGradient");
    }
}

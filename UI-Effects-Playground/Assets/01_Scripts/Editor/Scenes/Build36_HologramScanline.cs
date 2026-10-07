using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build36_HologramScanline
{
    [DemoScene("36_HologramScanline")]
    [MenuItem("Tools/UI Effects/Build Scene/36_HologramScanline")]
    public static void Build()
    {
        var canvas = NewScene("Hologram Scanline", "Mono-tone hologram with scanlines, a moving band and flicker - powers on from the bottom");
        var root = canvas.transform;

        var mat = CreateMaterial("HologramScanline", "UIEffects/HologramScanline");

        // 홀로그램 투사기 받침 (납작한 원)
        var projector = CreateImage("Projector", root, new Vector2(0f, -260f), new Vector2(420f, 70f), new Color(0.3f, 0.9f, 1f, 0.35f), KnobSprite);
        projector.raycastTarget = false;

        var card = CreateEffectImage("HoloCard", root, new Vector2(0f, 10f), new Vector2(320f, 448f), DemoTextures.HoloCard, mat);
        var hologram = card.gameObject.AddComponent<HologramEffect>();
        Set(hologram, "targetGraphic", card);

        var button = CreateButtonRow(root, -380f, "Toggle Power")[0];
        OnClick(button, hologram.Toggle);

        SaveScene("36_HologramScanline");
    }
}

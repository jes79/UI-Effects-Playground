using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build37_UnlockReveal
{
    [DemoScene("37_UnlockReveal")]
    [MenuItem("Tools/UI Effects/Build Scene/37_UnlockReveal")]
    public static void Build()
    {
        var canvas = NewScene("Unlock Reveal", "The lock shakes and pops, then color spreads from the center with a glowing ring");
        var root = canvas.transform;

        var mat = CreateMaterial("UnlockReveal", "UIEffects/UnlockReveal");

        var picture = CreateEffectImage("SecretArea", root, new Vector2(0f, 30f), new Vector2(640f, 400f), DemoTextures.Landscape, mat);

        var lockBadge = CreateImage("LockBadge", picture.transform, Vector2.zero, new Vector2(190f, 190f), PanelColor, KnobSprite);
        lockBadge.raycastTarget = false;
        var lockGroup = lockBadge.gameObject.AddComponent<CanvasGroup>();
        var lockText = CreateText("Label", lockBadge.transform, Vector2.zero, Vector2.zero, "LOCKED", 32f, new Color(1f, 0.85f, 0.4f));
        Stretch(lockText.rectTransform);
        lockText.fontStyle = TMPro.FontStyles.Bold;

        var unlock = picture.gameObject.AddComponent<UnlockReveal>();
        Set(unlock, "targetGraphic", picture);
        Set(unlock, "lockIcon", lockBadge.rectTransform);
        Set(unlock, "lockGroup", lockGroup);

        var buttons = CreateButtonRow(root, -330f, "Unlock", "Reset");
        OnClick(buttons[0], unlock.Unlock);
        OnClick(buttons[1], unlock.ResetLock);

        SaveScene("37_UnlockReveal");
    }
}

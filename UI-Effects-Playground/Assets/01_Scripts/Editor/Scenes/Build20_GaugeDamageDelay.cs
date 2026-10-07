using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build20_GaugeDamageDelay
{
    [DemoScene("20_GaugeDamageDelay")]
    [MenuItem("Tools/UI Effects/Build Scene/20_GaugeDamageDelay")]
    public static void Build()
    {
        var canvas = NewScene("Gauge Damage Delay", "The front bar drops instantly, the delayed bar catches up after a short pause");
        var root = canvas.transform;

        var gauge = CreateRect("Gauge", root, new Vector2(0f, 40f), new Vector2(900f, 64f));

        var frame = CreateImage("Frame", gauge, Vector2.zero, Vector2.zero, new Color32(12, 13, 17, 255), RoundedSprite, true);
        Stretch(frame.rectTransform, -8f);

        var fillBg = CreateImage("FillBackground", gauge, Vector2.zero, Vector2.zero, new Color32(55, 30, 36, 255), WhiteSprite);
        Stretch(fillBg.rectTransform);

        var delayFill = CreateFilled("DelayFill", gauge, new Color(1f, 0.85f, 0.3f));
        var frontFill = CreateFilled("FrontFill", gauge, new Color32(232, 72, 85, 255));

        var flash = CreateImage("Flash", gauge, Vector2.zero, Vector2.zero, new Color(1f, 1f, 1f, 0f), WhiteSprite);
        Stretch(flash.rectTransform);
        flash.raycastTarget = false;

        var hpText = CreateText("HpText", gauge, Vector2.zero, Vector2.zero, "100 / 100", 34f, Color.white);
        Stretch(hpText.rectTransform);
        hpText.fontStyle = TMPro.FontStyles.Bold;

        var label = CreateText("HpLabel", gauge, new Vector2(-400f, 70f), new Vector2(200f, 50f), "HP", 36f, TextColor, TMPro.TextAlignmentOptions.Left);
        label.fontStyle = TMPro.FontStyles.Bold;

        var gaugeEffect = gauge.gameObject.AddComponent<GaugeDamageDelay>();
        Set(gaugeEffect, "gaugeRoot", gauge);
        Set(gaugeEffect, "frontFill", frontFill);
        Set(gaugeEffect, "delayFill", delayFill);
        Set(gaugeEffect, "flashImage", flash);
        Set(gaugeEffect, "hpText", hpText);

        var buttons = CreateButtonRow(root, -260f, "Damage", "Heal");
        OnClick(buttons[0], gaugeEffect.TestDamage);
        OnClick(buttons[1], gaugeEffect.TestHeal);

        SaveScene("20_GaugeDamageDelay");
    }

    private static Image CreateFilled(string name, Transform parent, Color color)
    {
        var img = CreateImage(name, parent, Vector2.zero, Vector2.zero, color, WhiteSprite);
        Stretch(img.rectTransform);
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Horizontal;
        img.fillOrigin = (int)Image.OriginHorizontal.Left;
        img.raycastTarget = false;
        return img;
    }
}

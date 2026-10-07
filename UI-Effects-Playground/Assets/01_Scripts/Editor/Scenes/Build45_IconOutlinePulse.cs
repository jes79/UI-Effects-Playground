using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build45_IconOutlinePulse
{
    [DemoScene("45_IconOutlinePulse")]
    [MenuItem("Tools/UI Effects/Build Scene/45_IconOutlinePulse")]
    public static void Build()
    {
        var canvas = NewScene("Icon Outline Pulse", "Click an icon to toggle a pulsing outline traced from its alpha");
        var root = canvas.transform;

        var mat = CreateMaterial("IconOutlinePulse", "UIEffects/IconOutlinePulse");

        Sprite[] icons = { DemoTextures.StarIcon, DemoTextures.HeartIcon, DemoTextures.DiamondIcon };
        string[] names = { "StarIcon", "HeartIcon", "DiamondIcon" };

        for (int i = 0; i < icons.Length; i++)
        {
            var icon = CreateEffectImage(names[i], root, new Vector2((i - 1) * 400f, 20f), new Vector2(300f, 300f), icons[i], mat);
            icon.raycastTarget = true;
            var button = icon.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;

            var outline = icon.gameObject.AddComponent<IconOutlinePulse>();
            Set(outline, "targetGraphic", icon);
            SetBool(outline, "startSelected", i == 0);

            OnClick(button, outline.ToggleSelect);
        }

        SaveScene("45_IconOutlinePulse");
    }
}

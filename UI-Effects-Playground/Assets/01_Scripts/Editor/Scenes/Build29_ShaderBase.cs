using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DemoSceneKit;

public static class Build29_ShaderBase
{
    [DemoScene("29_ShaderBase")]
    [MenuItem("Tools/UI Effects/Build Scene/29_ShaderBase")]
    public static void Build()
    {
        var canvas = NewScene("Custom UI Shader Base", "UIEffects/UIBase = UI/Default + Saturation / Brightness, works with RectMask2D and Mask");
        var root = canvas.transform;

        var picture = DemoTextures.Landscape;
        var baseMat = CreateMaterial("UIBase", "UIEffects/UIBase");
        Vector2 size = new Vector2(480f, 300f);

        // 1. 기본 UI/Default
        CreateImage("Default", root, new Vector2(-560f, 30f), size, Color.white, picture);
        CreateCaption(root, new Vector2(-560f, -150f), "UI/Default");

        // 2. 커스텀 베이스 셰이더 + 채도 루프
        var saturation = CreateEffectImage("UIBase_Saturation", root, new Vector2(0f, 30f), size, picture, baseMat);
        var saturationTween = saturation.gameObject.AddComponent<UIShaderPropertyTween>();
        Set(saturationTween, "targetGraphic", saturation);
        SetString(saturationTween, "propertyName", "_Saturation");
        SetFloat(saturationTween, "from", 0f);
        SetFloat(saturationTween, "to", 1.4f);
        SetFloat(saturationTween, "duration", 1.5f);
        SetBool(saturationTween, "loop", true);
        CreateCaption(root, new Vector2(0f, -150f), "UIEffects/UIBase  (_Saturation loop)");

        // 3. RectMask2D 안에서도 클리핑이 잘 되는지 확인 + 밝기 토글
        var maskFrame = CreateImage("MaskFrame", root, new Vector2(560f, 30f), new Vector2(380f, 240f), PanelColor);
        maskFrame.gameObject.AddComponent<RectMask2D>();
        var masked = CreateEffectImage("UIBase_Masked", maskFrame.transform, new Vector2(60f, 30f), size, picture, baseMat);
        var brightnessTween = masked.gameObject.AddComponent<UIShaderPropertyTween>();
        Set(brightnessTween, "targetGraphic", masked);
        SetString(brightnessTween, "propertyName", "_Brightness");
        SetFloat(brightnessTween, "from", 1f);
        SetFloat(brightnessTween, "to", 0.35f);
        SetFloat(brightnessTween, "duration", 0.5f);
        CreateCaption(root, new Vector2(560f, -150f), "Inside RectMask2D  (_Brightness)");

        var button = CreateButtonRow(root, -330f, "Toggle Brightness")[0];
        OnClick(button, brightnessTween.Toggle);

        SaveScene("29_ShaderBase");
    }
}

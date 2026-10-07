using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build44_TextGradientShine
{
    [DemoScene("44_TextGradientShine")]
    [MenuItem("Tools/UI Effects/Build Scene/44_TextGradientShine")]
    public static void Build()
    {
        var canvas = NewScene("Text Gradient Shine", "Custom SDF shader on TextMeshPro: a flowing 3-color gradient + a sweeping shine");
        var root = canvas.transform;

        // TMP 는 머티리얼의 _MainTex 가 폰트 아틀라스와 같아야 해당 머티리얼을 그대로 사용함
        var font = DefaultFont;
        var mat = CreateMaterial("TextGradientShine", "UIEffects/TextGradientShine", m =>
        {
            m.SetTexture("_MainTex", font.atlasTexture);
            m.SetFloat("_Dilate", 0.04f);
        });

        var text = CreateText("GradientText", root, new Vector2(0f, 40f), new Vector2(1600f, 220f), "LEVEL UP!", 180f, Color.white);
        text.fontSharedMaterial = mat;

        var shine = text.gameObject.AddComponent<TextGradientShine>();
        Set(shine, "targetGraphic", text);
        SetStrings(shine, "messages", "LEVEL UP!", "VICTORY", "NEW RECORD", "LEGENDARY");

        var button = CreateButtonRow(root, -330f, "Change Text")[0];
        OnClick(button, shine.ChangeText);

        SaveScene("44_TextGradientShine");
    }
}

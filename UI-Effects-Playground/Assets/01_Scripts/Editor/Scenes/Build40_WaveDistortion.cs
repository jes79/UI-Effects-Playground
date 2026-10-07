using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build40_WaveDistortion
{
    [DemoScene("40_WaveDistortion")]
    [MenuItem("Tools/UI Effects/Build Scene/40_WaveDistortion")]
    public static void Build()
    {
        var canvas = NewScene("Wave Distortion", "Sine-wave UV offset + click the image to spawn a ripple from that point");
        var root = canvas.transform;

        var mat = CreateMaterial("WaveDistortion", "UIEffects/WaveDistortion");

        var picture = CreateEffectImage("Picture", root, new Vector2(0f, 30f), new Vector2(760f, 475f), DemoTextures.Landscape, mat);
        picture.raycastTarget = true; // 클릭을 받아야 함

        var wave = picture.gameObject.AddComponent<WaveRippleEffect>();
        Set(wave, "targetGraphic", picture);

        var button = CreateButtonRow(root, -340f, "Toggle Wave")[0];
        OnClick(button, wave.ToggleWave);

        SaveScene("40_WaveDistortion");
    }
}

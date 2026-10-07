using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build26_UIParticle
{
    private const string ParticleMaterialPath = "Packages/com.unity.render-pipelines.universal/Runtime/Materials/ParticlesUnlit.mat";

    [DemoScene("26_UIParticle")]
    [MenuItem("Tools/UI Effects/Build Scene/26_UIParticle")]
    public static void Build()
    {
        // 파티클을 UI 위에 그리기 위해 Screen Space - Camera 캔버스 사용
        var canvas = NewScene("UI Particle", "Screen Space - Camera canvas + ParticleSystem with a higher Order in Layer", cameraCanvas: true);
        var root = canvas.transform;

        var button = CreateButton("CollectButton", root, new Vector2(0f, -20f), new Vector2(400f, 130f), "Collect!", new Color32(255, 170, 40, 255), 50f);

        var particleGo = new GameObject("BurstParticle");
        var ps = particleGo.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1f;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1.1f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 7f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.15f, 0.35f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.9f, 0.35f), new Color(1f, 0.45f, 0.2f));
        main.gravityModifier = 0.8f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 300;

        var emission = ps.emission;
        emission.rateOverTime = 0f; // 스크립트에서 Emit() 으로만 방출

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        var fade = new Gradient();
        fade.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.6f), new GradientAlphaKey(0f, 1f) });
        colorOverLifetime.color = fade;

        var renderer = particleGo.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(ParticleMaterialPath);
        renderer.sortingOrder = 10; // 캔버스(Order 0)보다 위에 그림

        var burst = button.gameObject.AddComponent<UIParticleBurst>();
        Set(burst, "burstParticle", ps);
        Set(burst, "target", button.transform);
        OnClick(button, burst.Play);

        SaveScene("26_UIParticle");
    }
}

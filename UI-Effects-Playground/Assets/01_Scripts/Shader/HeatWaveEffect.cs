using UnityEngine;
using DG.Tweening;

public class HeatWaveEffect : UIShaderEffect
{
    private const string Strength = "_Strength";

    [SerializeField] private float heatStrength = 0.025f;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private bool startHot = true;

    private bool isHot;

    private void Start()
    {
        isHot = startHot;
        material.SetFloat(Strength, isHot ? heatStrength : 0f);
    }

    // 열기가 서서히 올라오고 / 서서히 가라앉음
    public void ToggleHeat()
    {
        isHot = !isHot;
        material.DOKill();
        material.DOFloat(isHot ? heatStrength : 0f, Strength, duration).SetEase(Ease.InOutSine);
    }
}

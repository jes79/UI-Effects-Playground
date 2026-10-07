using UnityEngine;
using DG.Tweening;

// 맥박(펄스) 자체는 셰이더가 _Time 으로 계속 돌리고,
// 스크립트는 글로우 켜기/끄기와 순간 강조(Boost)만 담당
public class GlowPulseShaderEffect : UIShaderEffect
{
    private const string GlowIntensity = "_GlowIntensity";

    [SerializeField] private float glowIntensity = 1.5f;
    [SerializeField] private float boostIntensity = 3.5f;
    [SerializeField] private float fadeDuration = 0.4f;

    private bool isOn = true;

    private void Start()
    {
        material.SetFloat(GlowIntensity, glowIntensity);
    }

    public void ToggleGlow()
    {
        isOn = !isOn;
        material.DOKill();
        material.DOFloat(isOn ? glowIntensity : 0f, GlowIntensity, fadeDuration);
    }

    // 획득/레벨업 같은 순간: 글로우가 확 밝아졌다가 원래대로
    public void Boost()
    {
        isOn = true;
        material.DOKill();
        material.SetFloat(GlowIntensity, boostIntensity);
        material.DOFloat(glowIntensity, GlowIntensity, 0.6f).SetEase(Ease.OutQuad);

        TargetRect.DOKill(true);
        TargetRect.DOPunchScale(Vector3.one * 0.15f, 0.4f, 6, 0.8f);
    }
}

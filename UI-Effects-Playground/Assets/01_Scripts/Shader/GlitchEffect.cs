using UnityEngine;
using DG.Tweening;

public class GlitchEffect : UIShaderEffect
{
    private const string Intensity = "_Intensity";

    [SerializeField] private float idleIntensity = 0.05f;
    [SerializeField] private float burstIntensity = 1f;
    [SerializeField] private float burstDuration = 0.35f;
    [SerializeField] private Vector2 autoBurstInterval = new Vector2(1.5f, 3.5f);
    [SerializeField] private bool autoBurst = true;   // 랜덤한 간격으로 저절로 글리치

    private Tween autoBurstTimer;

    private void Start()
    {
        material.SetFloat(Intensity, idleIntensity);
        if (autoBurst) ScheduleAutoBurst();
    }

    public void Burst()
    {
        // 순간적으로 최대치 → 툭툭 끊기며 원래대로
        material.DOKill();
        material.SetFloat(Intensity, burstIntensity);
        material.DOFloat(idleIntensity, Intensity, burstDuration).SetEase(Ease.InExpo);

        TargetRect.DOKill(true);
        TargetRect.DOShakeAnchorPos(burstDuration * 0.6f, new Vector2(18f, 0f), 40, 0f, true, true);
    }

    public void ToggleAutoBurst()
    {
        autoBurst = !autoBurst;
        autoBurstTimer?.Kill();
        if (autoBurst) ScheduleAutoBurst();
    }

    private void ScheduleAutoBurst()
    {
        autoBurstTimer = DOVirtual.DelayedCall(Random.Range(autoBurstInterval.x, autoBurstInterval.y), () =>
        {
            Burst();
            ScheduleAutoBurst();
        });
    }

    protected override void OnDestroy()
    {
        autoBurstTimer?.Kill();
        base.OnDestroy();
    }
}

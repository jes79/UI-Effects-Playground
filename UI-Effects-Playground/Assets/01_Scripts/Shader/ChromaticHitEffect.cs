using UnityEngine;
using DG.Tweening;

public class ChromaticHitEffect : UIShaderEffect
{
    private const string Amount = "_Amount";

    [SerializeField] private float idleAmount = 0.004f;  // 평소에도 살짝 갈라져 있는 정도
    [SerializeField] private float hitAmount = 0.05f;    // 피격 순간 최대치
    [SerializeField] private float hitDuration = 0.5f;
    [SerializeField] private float shakeStrength = 20f;

    private void Start()
    {
        material.SetFloat(Amount, idleAmount);
    }

    public void Hit()
    {
        // 순간적으로 색이 크게 갈라졌다가 서서히 원래대로 + 흔들림
        material.DOKill();
        material.SetFloat(Amount, hitAmount);
        material.DOFloat(idleAmount, Amount, hitDuration).SetEase(Ease.OutQuad);

        TargetRect.DOKill(true);
        TargetRect.DOShakeAnchorPos(0.3f, shakeStrength, 25, 90f, false, true);
    }
}

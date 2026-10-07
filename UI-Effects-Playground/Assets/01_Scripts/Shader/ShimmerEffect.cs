using UnityEngine;
using DG.Tweening;

public class ShimmerEffect : UIShaderEffect
{
    private const string ShineLocation = "_ShineLocation";

    [SerializeField] private float duration = 0.8f;   // 빛줄기가 한 번 지나가는 시간
    [SerializeField] private float interval = 1.5f;   // 반복 사이 대기 시간
    [SerializeField] private bool autoLoop = true;

    private Sequence loopSequence;

    private void Start()
    {
        material.SetFloat(ShineLocation, 0f);
        if (autoLoop) StartLoop();
    }

    public void StartLoop()
    {
        loopSequence?.Kill();
        material.DOKill();

        // 0 으로 되돌림 → 0→1 로 훑고 지나감 → 대기 → 반복
        loopSequence = DOTween.Sequence()
            .AppendCallback(() => material.SetFloat(ShineLocation, 0f))
            .Append(material.DOFloat(1f, ShineLocation, duration).SetEase(Ease.InOutSine))
            .AppendInterval(interval)
            .SetLoops(-1);
    }

    // 버튼 클릭 등에 맞춰 한 번만 반짝
    public void PlayOnce()
    {
        loopSequence?.Kill();
        material.DOKill();
        material.SetFloat(ShineLocation, 0f);
        material.DOFloat(1f, ShineLocation, duration).SetEase(Ease.OutQuad);
    }

    protected override void OnDestroy()
    {
        loopSequence?.Kill();
        base.OnDestroy();
    }
}

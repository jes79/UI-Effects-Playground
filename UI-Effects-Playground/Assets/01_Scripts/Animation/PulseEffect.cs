using UnityEngine;
using DG.Tweening;

public class PulseEffect : MonoBehaviour
{
    [SerializeField] private float maxScale = 1.15f;
    [SerializeField] private float duration = 0.6f;

    private Tween pulseTween;

    private void OnEnable()
    {
        StartPulse();
    }

    private void OnDisable()
    {
        pulseTween?.Kill();
    }

    private void StartPulse()
    {
        transform.localScale = Vector3.one;

        pulseTween = transform.DOScale(maxScale, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // -1: 무한 반복, Yoyo: 커졌다 작아졌다 반복
    }

    // 필요하면 외부에서 펄스를 멈추고 싶을 때 호출
    public void StopPulse()
    {
        pulseTween?.Kill();
        transform.DOScale(Vector3.one, 0.2f);
    }
}
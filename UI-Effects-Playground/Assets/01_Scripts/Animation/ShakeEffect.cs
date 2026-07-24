using UnityEngine;
using DG.Tweening;

public class ShakeEffect : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float strength = 15f;   // 흔들리는 강도 (픽셀)
    [SerializeField] private int vibrato = 10;        // 흔들리는 횟수
    [SerializeField] private float randomness = 90f;  // 흔들림의 무작위성 (각도)

    public void Shake()
    {
        target.DOKill();
        target.anchoredPosition = target.anchoredPosition; // 현재 위치 기준

        target.DOShakeAnchorPos(
            duration,
            new Vector2(strength, 0f), // 좌우로만 흔들리게 (Y값 0)
            vibrato,
            randomness,
            false,  // snapping
            true    // fadeOut: 갈수록 흔들림이 줄어듦
        );
    }
}
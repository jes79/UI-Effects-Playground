using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressBarFill : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text percentText; // 선택사항, 없으면 null 체크로 스킵
    [SerializeField] private float duration = 0.8f;

    private Tween fillTween;

    private void Awake()
    {
        progressSlider.value = 0f;
        UpdatePercentText(0f);
    }

    public void FillTo(float targetValue01) // 0~1 사이 값
    {
        fillTween?.Kill();

        fillTween = progressSlider.DOValue(targetValue01, duration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() => UpdatePercentText(progressSlider.value));
    }

    private void UpdatePercentText(float value01)
    {
        if (percentText != null)
            percentText.text = Mathf.RoundToInt(value01 * 100f) + "%";
    }

    // 테스트용: 버튼 클릭 시 랜덤 진행률로 채움
    public void TestRandomFill()
    {
        float randomTarget = Random.Range(0.2f, 1f);
        FillTo(randomTarget);
    }
}
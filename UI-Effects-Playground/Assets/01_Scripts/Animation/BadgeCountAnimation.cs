using UnityEngine;
using TMPro;
using DG.Tweening;

public class BadgeCountAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text badgeText;
    [SerializeField] private RectTransform badgeTransform;
    [SerializeField] private float punchScale = 0.4f;
    [SerializeField] private float duration = 0.3f;

    private int currentCount = 0;

    public void Increase()
    {
        currentCount++;
        UpdateBadge(currentCount);
    }

    private void UpdateBadge(int newValue)
    {
        badgeText.text = newValue.ToString();

        badgeTransform.DOKill();
        badgeTransform.localScale = Vector3.one; // 혹시 이전 트윈이 덜 끝났을 때 대비해 초기화

        // 살짝 커졌다가 원래 크기로 되돌아오는 펀치 효과
        badgeTransform.DOPunchScale(Vector3.one * punchScale, duration, vibrato: 1, elasticity: 0.5f);
    }
}
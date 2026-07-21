using UnityEngine;
using DG.Tweening;

public class ScalePopup : MonoBehaviour
{
    [SerializeField] private RectTransform targetPanel;
    [SerializeField] private CanvasGroup targetGroup;
    [SerializeField] private float duration = 0.4f;

    private void Awake()
    {
        // 시작할 때 안 보이는 상태로 세팅
        targetPanel.localScale = Vector3.zero;
        targetGroup.alpha = 0f;
        targetGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        targetPanel.DOKill();
        targetGroup.DOKill();

        targetGroup.blocksRaycasts = true;
        targetGroup.DOFade(1f, duration * 0.6f); // 살짝 빠르게 페이드
        targetPanel.DOScale(1f, duration)
            .SetEase(Ease.OutBack); // 통통 튀는 느낌의 핵심
    }

    public void Close()
    {
        targetPanel.DOKill();
        targetGroup.DOKill();

        targetGroup.DOFade(0f, duration * 0.6f);
        targetPanel.DOScale(0f, duration)
            .SetEase(Ease.InBack)
            .OnComplete(() => targetGroup.blocksRaycasts = false);
    }
    //test
}
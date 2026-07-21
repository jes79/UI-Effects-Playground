using UnityEngine;
using DG.Tweening;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup targetGroup;
    [SerializeField] private float duration = 0.5f;

    public void FadeIn()
    {
        targetGroup.DOKill(); // 진행 중인 트윈 정리
        targetGroup.DOFade(1f, duration)
            .SetEase(Ease.OutQuad);
        targetGroup.blocksRaycasts = true; // 페이드 인 후 클릭 가능하게
    }

    public void FadeOut()
    {
        targetGroup.DOKill();
        targetGroup.DOFade(0f, duration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => targetGroup.blocksRaycasts = false); // 페이드 아웃 후 클릭 막기
    }
    
}
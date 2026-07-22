using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private float pressedScale = 0.9f;
    [SerializeField] private float duration = 0.1f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale * pressedScale, duration)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutBack); // 놓을 때 살짝 튕기며 복귀
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 누른 상태에서 버튼 영역 밖으로 손가락/마우스가 벗어나도 원래 크기로 복귀
        transform.DOKill();
        transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutQuad);
    }
}
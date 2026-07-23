using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonRipple : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform rippleCircle; // RippleCircle의 RectTransform
    [SerializeField] private Image rippleImage;           // RippleCircle의 Image
    [SerializeField] private float maxScale = 15f;        // 최대로 퍼지는 배율
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Color rippleColor = new Color(1f, 1f, 1f, 0.4f); // 반투명 흰색

    public void OnPointerDown(PointerEventData eventData)
    {
        // 클릭한 화면 좌표를 로컬 좌표로 변환해서 리플 시작 위치로 설정
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rippleCircle.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        rippleCircle.anchoredPosition = localPoint;
        rippleCircle.localScale = Vector3.zero;

        rippleImage.color = rippleColor;

        rippleCircle.DOKill();
        rippleImage.DOKill();

        // 원이 커지면서 퍼짐 + 동시에 서서히 투명해짐
        rippleCircle.DOScale(maxScale, duration)
            .SetEase(Ease.OutQuad);

        rippleImage.DOFade(0f, duration)
            .SetEase(Ease.OutQuad);
    }
}
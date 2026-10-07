using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// 마우스 위치에 따라 카드를 기울이고, 같은 기울기 값을 셰이더(_Tilt)에 넘겨서 무지개/글레어가 함께 움직이게 함
public class HolographicCard : UIShaderEffect, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float hoverScale = 1.05f;
    [SerializeField] private bool idleSway = true;   // 마우스가 없을 때 천천히 혼자 흔들림

    private Vector2 tilt;
    private Vector2 targetTilt;
    private bool isHovering;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        TargetRect.DOKill();
        TargetRect.DOScale(hoverScale, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        targetTilt = Vector2.zero;
        TargetRect.DOKill();
        TargetRect.DOScale(1f, 0.25f).SetEase(Ease.OutQuad);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(TargetRect, eventData.position, eventData.enterEventCamera, out Vector2 local))
            return;

        // 카드 안의 위치를 -1 ~ 1 로
        Vector2 normalized = Rect.PointToNormalized(TargetRect.rect, local);
        targetTilt = normalized * 2f - Vector2.one;
    }

    private void Update()
    {
        if (!isHovering && idleSway)
            targetTilt = new Vector2(Mathf.Sin(Time.time * 0.8f), Mathf.Cos(Time.time * 0.6f)) * 0.35f;

        tilt = Vector2.Lerp(tilt, targetTilt, 1f - Mathf.Exp(-followSpeed * Time.deltaTime));

        material.SetVector("_Tilt", tilt);
        // 마우스가 있는 쪽이 뒤로 눌리듯 기울어짐
        TargetRect.localRotation = Quaternion.Euler(tilt.y * maxTiltAngle, -tilt.x * maxTiltAngle, 0f);
    }
}

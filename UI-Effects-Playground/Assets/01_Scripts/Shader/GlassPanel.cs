using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// 유리 패널: 뒤에 있는 배경 이미지의 위치(캔버스 좌표)를 매 프레임 셰이더에 넘겨서
// 패널을 드래그해도 그 자리 뒤의 배경이 굴절되어 보이게 함
public class GlassPanel : UIShaderEffect, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform background;  // 배경 텍스처를 그리는 RawImage/Image (셰이더 _BackgroundTex 와 같은 이미지)

    private Canvas rootCanvas;
    private readonly Vector3[] corners = new Vector3[4];

    protected override void Awake()
    {
        base.Awake();
        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
    }

    private void Start()
    {
        ApplyAspect();
    }

    private void LateUpdate()
    {
        // UI 버텍스는 루트 캔버스 기준 좌표로 셰이더에 들어오므로 배경 Rect 도 같은 좌표계로 변환
        background.GetWorldCorners(corners);
        Transform canvasTransform = rootCanvas.transform;
        Vector3 min = canvasTransform.InverseTransformPoint(corners[0]);
        Vector3 max = canvasTransform.InverseTransformPoint(corners[2]);
        material.SetVector("_BgRect", new Vector4(min.x, min.y, max.x - min.x, max.y - min.y));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TargetRect.DOKill();
        TargetRect.DOScale(1.04f, 0.15f).SetEase(Ease.OutQuad);
    }

    public void OnDrag(PointerEventData eventData)
    {
        TargetRect.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TargetRect.DOKill();
        TargetRect.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonHoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(1f, 0.85f, 0.4f); // 살짝 노란빛 강조색
    [SerializeField] private float hoverScale = 1.08f;
    [SerializeField] private float duration = 0.2f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        if (targetImage != null)
            targetImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, duration)
            .SetEase(Ease.OutQuad);

        if (targetImage != null)
        {
            targetImage.DOKill();
            targetImage.DOColor(hoverColor, duration);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutQuad);

        if (targetImage != null)
        {
            targetImage.DOKill();
            targetImage.DOColor(normalColor, duration);
        }
    }
}
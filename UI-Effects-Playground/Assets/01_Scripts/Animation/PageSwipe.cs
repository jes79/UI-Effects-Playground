using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

// 뷰포트(RectMask2D)에 붙여서 사용. content 아래의 자식들을 한 페이지씩 가로로 나열해 둔 구조
public class PageSwipe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform content;    // 페이지들이 가로로 나열된 부모
    [SerializeField] private Image[] dots;             // 페이지 인디케이터 (선택사항)
    [SerializeField] private float pageWidth = 1000f;
    [SerializeField] private float snapDuration = 0.35f;
    [SerializeField] private float swipeThreshold = 0.2f;  // 페이지 폭 대비 이 비율 이상 끌면 다음 페이지로
    [SerializeField] private float flickSpeed = 1500f;     // 이 속도 이상으로 튕기면 짧게 끌어도 넘어감
    [SerializeField] private float edgeResistance = 0.35f; // 첫/마지막 페이지 바깥으로 끌 때 저항
    [SerializeField] private Color dotActiveColor = Color.white;
    [SerializeField] private Color dotInactiveColor = new Color(1f, 1f, 1f, 0.3f);

    private Canvas canvas;
    private int pageCount;
    private int currentPage;
    private float dragStartX;
    private float dragOffset;
    private float dragStartTime;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        pageCount = content.childCount;
        UpdateDots(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        content.DOKill();
        dragStartX = content.anchoredPosition.x;
        dragOffset = 0f;
        dragStartTime = Time.unscaledTime;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 화면 픽셀 이동량을 캔버스 단위로 변환
        dragOffset += eventData.delta.x / canvas.scaleFactor;

        float x = dragStartX + dragOffset;
        float minX = -(pageCount - 1) * pageWidth;
        if (x > 0f) x *= edgeResistance;
        else if (x < minX) x = minX + (x - minX) * edgeResistance;

        content.anchoredPosition = new Vector2(x, content.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float elapsed = Mathf.Max(0.01f, Time.unscaledTime - dragStartTime);
        float velocity = dragOffset / elapsed;

        int target = currentPage;
        if (dragOffset < -pageWidth * swipeThreshold || velocity < -flickSpeed) target++;
        else if (dragOffset > pageWidth * swipeThreshold || velocity > flickSpeed) target--;

        GoToPage(target);
    }

    public void GoToPage(int index)
    {
        currentPage = Mathf.Clamp(index, 0, pageCount - 1);

        content.DOKill();
        content.DOAnchorPosX(-currentPage * pageWidth, snapDuration).SetEase(Ease.OutCubic);
        UpdateDots(false);
    }

    public void NextPage() => GoToPage(currentPage + 1);
    public void PrevPage() => GoToPage(currentPage - 1);

    private void UpdateDots(bool instant)
    {
        if (dots == null) return;

        for (int i = 0; i < dots.Length; i++)
        {
            bool active = i == currentPage;
            Color color = active ? dotActiveColor : dotInactiveColor;
            float scale = active ? 1.3f : 1f;

            dots[i].DOKill();
            dots[i].rectTransform.DOKill();
            if (instant)
            {
                dots[i].color = color;
                dots[i].rectTransform.localScale = Vector3.one * scale;
            }
            else
            {
                dots[i].DOColor(color, 0.2f);
                dots[i].rectTransform.DOScale(scale, 0.2f).SetEase(Ease.OutBack);
            }
        }
    }
}

using UnityEngine;
using TMPro;
using DG.Tweening;

public class TabIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform indicator;  // 탭 아래를 따라다니는 밑줄 (탭들과 같은 부모 아래에 둠)
    [SerializeField] private RectTransform[] tabs;
    [SerializeField] private TMP_Text[] tabLabels;
    [SerializeField] private CanvasGroup[] pages;      // 탭별 내용 (선택사항)

    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float widthRatio = 0.6f;   // 탭 폭 대비 밑줄 폭
    [SerializeField] private float stretchRatio = 1.8f; // 이동 중 밑줄이 늘어나는 정도
    [SerializeField] private float pageSlideDistance = 60f;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.45f);

    private int currentIndex = -1;
    private Vector2[] pageOriginalPos;

    private void Awake()
    {
        if (pages != null)
        {
            pageOriginalPos = new Vector2[pages.Length];
            for (int i = 0; i < pages.Length; i++)
            {
                pageOriginalPos[i] = ((RectTransform)pages[i].transform).anchoredPosition;
                pages[i].alpha = 0f;
                pages[i].blocksRaycasts = false;
            }
        }

        Select(0, true);
    }

    public void SelectTab(int index) => Select(index, false);

    private void Select(int index, bool instant)
    {
        if (index == currentIndex) return;
        int prevIndex = currentIndex;
        currentIndex = index;

        float height = indicator.sizeDelta.y;
        float targetWidth = tabs[index].rect.width * widthRatio;
        Vector2 targetPos = new Vector2(tabs[index].anchoredPosition.x, indicator.anchoredPosition.y);

        indicator.DOKill();
        if (instant)
        {
            indicator.anchoredPosition = targetPos;
            indicator.sizeDelta = new Vector2(targetWidth, height);
        }
        else
        {
            // 이동하는 동안 밑줄이 길게 늘어났다가 도착하면서 원래 폭으로 돌아오는 '젤리' 느낌
            float distance = Mathf.Abs(targetPos.x - indicator.anchoredPosition.x);
            float stretchedWidth = Mathf.Max(targetWidth, Mathf.Min(targetWidth * stretchRatio, targetWidth + distance));

            DOTween.Sequence()
                .SetTarget(indicator)
                .Append(indicator.DOAnchorPos(targetPos, duration).SetEase(Ease.OutCubic))
                .Join(indicator.DOSizeDelta(new Vector2(stretchedWidth, height), duration * 0.4f).SetEase(Ease.OutQuad))
                .Insert(duration * 0.4f, indicator.DOSizeDelta(new Vector2(targetWidth, height), duration * 0.6f).SetEase(Ease.OutBack));
        }

        for (int i = 0; i < tabLabels.Length; i++)
        {
            Color color = i == index ? activeColor : inactiveColor;
            tabLabels[i].DOKill();
            if (instant) tabLabels[i].color = color;
            else tabLabels[i].DOColor(color, duration * 0.5f);
        }

        SwitchPage(prevIndex, index, instant);
    }

    private void SwitchPage(int prevIndex, int index, bool instant)
    {
        if (pages == null || pages.Length == 0) return;

        // 오른쪽 탭으로 가면 내용이 오른쪽에서, 왼쪽 탭으로 가면 왼쪽에서 들어옴
        float direction = prevIndex < index ? 1f : -1f;

        if (prevIndex >= 0)
        {
            CanvasGroup prev = pages[prevIndex];
            prev.DOKill();
            prev.blocksRaycasts = false;
            prev.DOFade(0f, duration * 0.5f);
            ((RectTransform)prev.transform).DOAnchorPosX(pageOriginalPos[prevIndex].x - pageSlideDistance * direction, duration * 0.5f);
        }

        CanvasGroup next = pages[index];
        var nextRect = (RectTransform)next.transform;
        next.DOKill();
        nextRect.DOKill();
        next.blocksRaycasts = true;

        if (instant)
        {
            next.alpha = 1f;
            nextRect.anchoredPosition = pageOriginalPos[index];
            return;
        }

        next.alpha = 0f;
        nextRect.anchoredPosition = pageOriginalPos[index] + new Vector2(pageSlideDistance * direction, 0f);
        next.DOFade(1f, duration).SetDelay(duration * 0.2f);
        nextRect.DOAnchorPos(pageOriginalPos[index], duration).SetDelay(duration * 0.2f).SetEase(Ease.OutCubic);
    }
}

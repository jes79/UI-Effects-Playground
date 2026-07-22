using UnityEngine;
using DG.Tweening;

public class SlideInOut : MonoBehaviour
{
    [SerializeField] private RectTransform targetPanel;
    [SerializeField] private CanvasGroup targetGroup;
    [SerializeField] private float duration = 0.5f;

    private Vector2 onScreenPos;   // 화면에 보일 때 위치
    private Vector2 offScreenPos;  // 화면 밖 위치 (오른쪽으로 숨김)

    private void Awake()
    {
        onScreenPos = targetPanel.anchoredPosition;
        // 패널 너비만큼 오른쪽으로 벗어난 위치를 오프스크린 좌표로 설정
        offScreenPos = onScreenPos + new Vector2(targetPanel.rect.width, 0f);

        // 시작할 땐 화면 밖에 숨겨둠
        targetPanel.anchoredPosition = offScreenPos;
        targetGroup.blocksRaycasts = false;
    }

    public void SlideIn()
    {
        targetPanel.DOKill();
        targetGroup.blocksRaycasts = true;

        targetPanel.DOAnchorPos(onScreenPos, duration)
            .SetEase(Ease.OutCubic);
    }

    public void SlideOut()
    {
        targetPanel.DOKill();

        targetPanel.DOAnchorPos(offScreenPos, duration)
            .SetEase(Ease.InCubic)
            .OnComplete(() => targetGroup.blocksRaycasts = false);
    }
}
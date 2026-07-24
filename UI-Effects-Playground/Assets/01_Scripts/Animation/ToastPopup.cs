using UnityEngine;
using TMPro;
using DG.Tweening;

public class ToastPopup : MonoBehaviour
{
    [SerializeField] private RectTransform toastPanel;
    [SerializeField] private CanvasGroup toastGroup;
    [SerializeField] private TMP_Text toastText;

    [SerializeField] private float slideDistance = 40f; // 아래에서 위로 슬라이드되는 거리
    [SerializeField] private float showDuration = 0.3f;
    [SerializeField] private float stayDuration = 1.5f;  // 화면에 머무는 시간
    [SerializeField] private float hideDuration = 0.3f;

    private Vector2 originalPos;
    private Sequence currentSequence;

    private void Awake()
    {
        originalPos = toastPanel.anchoredPosition;
        toastGroup.alpha = 0f;
    }

    public void Show(string message)
    {
        toastText.text = message;

        // 진행 중인 토스트가 있으면 죽이고 새로 시작
        currentSequence?.Kill();

        toastPanel.anchoredPosition = originalPos - new Vector2(0f, slideDistance);
        toastGroup.alpha = 0f;

        currentSequence = DOTween.Sequence();
        currentSequence.Append(toastPanel.DOAnchorPos(originalPos, showDuration).SetEase(Ease.OutQuad));
        currentSequence.Join(toastGroup.DOFade(1f, showDuration));
        currentSequence.AppendInterval(stayDuration);
        currentSequence.Append(toastGroup.DOFade(0f, hideDuration));
    }
}
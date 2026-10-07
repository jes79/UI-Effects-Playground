using UnityEngine;
using DG.Tweening;

public class PopupOpenClose : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot;    // 딤 + 패널을 포함한 루트 (닫히면 비활성화)
    [SerializeField] private CanvasGroup dimGroup;    // 뒤 배경을 어둡게 가리는 딤
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup panelGroup;

    [SerializeField] private float openDuration = 0.35f;
    [SerializeField] private float closeDuration = 0.2f;
    [SerializeField] private float startScale = 0.7f;

    private Sequence currentSequence;
    private bool isOpen;

    private void Awake()
    {
        popupRoot.SetActive(false);
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        currentSequence?.Kill();
        popupRoot.SetActive(true);

        dimGroup.alpha = 0f;
        panelGroup.alpha = 0f;
        panelGroup.interactable = true;
        panel.localScale = Vector3.one * startScale;

        currentSequence = DOTween.Sequence();
        currentSequence.Append(dimGroup.DOFade(1f, openDuration * 0.6f));
        currentSequence.Join(panel.DOScale(1f, openDuration).SetEase(Ease.OutBack)); // 살짝 튀어나오듯 열림
        currentSequence.Join(panelGroup.DOFade(1f, openDuration * 0.5f));
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        currentSequence?.Kill();
        panelGroup.interactable = false; // 닫히는 중 중복 클릭 방지

        currentSequence = DOTween.Sequence();
        currentSequence.Append(panel.DOScale(startScale, closeDuration).SetEase(Ease.InBack)); // 살짝 빨려 들어가듯 닫힘
        currentSequence.Join(panelGroup.DOFade(0f, closeDuration));
        currentSequence.Join(dimGroup.DOFade(0f, closeDuration));
        currentSequence.OnComplete(() => popupRoot.SetActive(false));
    }
}

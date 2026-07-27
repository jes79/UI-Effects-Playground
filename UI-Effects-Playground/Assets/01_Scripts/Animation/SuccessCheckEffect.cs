using UnityEngine;
using DG.Tweening;

public class SuccessCheckEffect : MonoBehaviour
{
    [SerializeField] private RectTransform circleTransform;
    [SerializeField] private CanvasGroup circleGroup;
    [SerializeField] private RectTransform checkMarkTransform;
    [SerializeField] private CanvasGroup checkMarkGroup;

    [SerializeField] private float circleDuration = 0.35f;
    [SerializeField] private float checkDelay = 0.2f;   // 원이 어느 정도 나온 뒤 체크마크 등장
    [SerializeField] private float checkDuration = 0.3f;

    private void Awake()
    {
        // 시작할 땐 안 보이는 상태
        circleTransform.localScale = Vector3.zero;
        circleGroup.alpha = 0f;
        checkMarkTransform.localScale = Vector3.zero;
        checkMarkGroup.alpha = 0f;
    }

    public void PlaySuccess()
    {
        // 초기화 (여러 번 테스트할 때를 위해)
        circleTransform.DOKill();
        circleGroup.DOKill();
        checkMarkTransform.DOKill();
        checkMarkGroup.DOKill();

        circleTransform.localScale = Vector3.zero;
        circleGroup.alpha = 0f;
        checkMarkTransform.localScale = Vector3.zero;
        checkMarkGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        // 1. 원이 통통 튀며 등장
        seq.Append(circleTransform.DOScale(1f, circleDuration).SetEase(Ease.OutBack));
        seq.Join(circleGroup.DOFade(1f, circleDuration * 0.6f));

        // 2. 살짝 딜레이 후 체크마크가 튀어나오듯 등장
        seq.Insert(checkDelay, checkMarkTransform.DOScale(1f, checkDuration).SetEase(Ease.OutBack));
        seq.Insert(checkDelay, checkMarkGroup.DOFade(1f, checkDuration * 0.6f));
    }
}
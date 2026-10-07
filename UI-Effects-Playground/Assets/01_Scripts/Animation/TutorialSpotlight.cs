using UnityEngine;
using TMPro;
using DG.Tweening;

// 화면을 어둡게 덮고 특정 UI만 구멍처럼 밝게 보여주는 튜토리얼 스포트라이트
// spotlight 는 가운데가 뚫린 원형 이미지이고, 그 바깥은 자식으로 붙은 어두운 사각형 4개가 덮음 → spotlight 만 움직이면 전체가 따라감
public class TutorialSpotlight : MonoBehaviour
{
    [SerializeField] private CanvasGroup overlayGroup;    // 오버레이 전체 (클릭하면 다음 단계)
    [SerializeField] private RectTransform spotlight;
    [SerializeField] private CanvasGroup messageGroup;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private RectTransform[] targets;     // 순서대로 강조할 UI
    [SerializeField] private string[] messages;           // 단계별 안내 문구

    [SerializeField] private float padding = 40f;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float startSize = 2600f;     // 시작할 때 화면 전체를 덮을 만큼 큰 구멍에서 줄어듦

    private int step = -1;
    private Tween breatheTween;

    private void Awake()
    {
        overlayGroup.alpha = 0f;
        overlayGroup.blocksRaycasts = false;
    }

    public void StartTutorial()
    {
        if (targets.Length == 0) return;

        step = -1;
        overlayGroup.DOKill();
        overlayGroup.blocksRaycasts = true;
        overlayGroup.DOFade(1f, 0.3f);

        spotlight.DOKill();
        spotlight.position = targets[0].position;
        spotlight.sizeDelta = Vector2.one * startSize;

        Next();
    }

    public void Next()
    {
        step++;
        if (step >= targets.Length)
        {
            EndTutorial();
            return;
        }

        MoveTo(targets[step], step < messages.Length ? messages[step] : "");
    }

    private void MoveTo(RectTransform target, string message)
    {
        breatheTween?.Kill();
        spotlight.DOKill();

        Vector2 size = target.rect.size + Vector2.one * padding * 2f;

        // 구멍이 다음 타겟으로 이동하면서 크기도 맞춰짐
        DOTween.Sequence()
            .SetTarget(spotlight)
            .Append(spotlight.DOMove(target.position, moveDuration).SetEase(Ease.InOutCubic))
            .Join(spotlight.DOSizeDelta(size, moveDuration).SetEase(Ease.InOutCubic))
            .OnComplete(() =>
            {
                // 도착 후 숨 쉬듯 살짝 커졌다 작아졌다 하며 시선을 유도
                breatheTween = spotlight.DOSizeDelta(size * 1.06f, 0.8f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });

        // 안내 문구는 잠깐 사라졌다가 새 문구로 등장
        messageGroup.DOKill();
        messageGroup.alpha = 0f;
        messageText.text = $"{message}\n<size=70%><alpha=#88>({step + 1}/{targets.Length})  Click to continue";
        messageGroup.DOFade(1f, 0.3f).SetDelay(moveDuration * 0.8f);
    }

    private void EndTutorial()
    {
        breatheTween?.Kill();
        overlayGroup.DOKill();
        overlayGroup.blocksRaycasts = false;
        overlayGroup.DOFade(0f, 0.3f);
    }
}

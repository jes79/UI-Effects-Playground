using UnityEngine;
using DG.Tweening;

// 스켈레톤(자리표시자)이 셔이머 셰이더로 반짝이는 동안 로딩을 흉내 내고,
// 로딩이 끝나면 스켈레톤은 사라지고 실제 콘텐츠가 순서대로 올라오며 등장
// (반짝임 자체는 셰이더가 _Time 으로 처리하므로 공유 머티리얼 그대로 사용)
public class SkeletonLoading : MonoBehaviour
{
    [SerializeField] private CanvasGroup skeletonGroup;
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private RectTransform[] contentItems;  // 순서대로 등장할 실제 콘텐츠
    [SerializeField] private float loadDuration = 2.5f;     // 가짜 로딩 시간
    [SerializeField] private float itemStagger = 0.08f;
    [SerializeField] private float itemSlideDistance = 30f;

    private Vector2[] itemOriginalPos;
    private Sequence sequence;

    private void Awake()
    {
        itemOriginalPos = new Vector2[contentItems.Length];
        for (int i = 0; i < contentItems.Length; i++)
            itemOriginalPos[i] = contentItems[i].anchoredPosition;
    }

    private void Start()
    {
        Reload();
    }

    public void Reload()
    {
        sequence?.Kill();
        skeletonGroup.DOKill();
        contentGroup.DOKill();

        skeletonGroup.alpha = 1f;
        contentGroup.alpha = 0f;

        sequence = DOTween.Sequence();
        sequence.AppendInterval(loadDuration);
        sequence.AppendCallback(ShowContent);
    }

    private void ShowContent()
    {
        skeletonGroup.DOFade(0f, 0.3f);
        contentGroup.alpha = 1f;

        for (int i = 0; i < contentItems.Length; i++)
        {
            RectTransform item = contentItems[i];
            CanvasGroup itemGroup = item.GetComponent<CanvasGroup>();

            item.DOKill();
            item.anchoredPosition = itemOriginalPos[i] - new Vector2(0f, itemSlideDistance);
            item.DOAnchorPos(itemOriginalPos[i], 0.4f).SetDelay(i * itemStagger).SetEase(Ease.OutCubic);

            if (itemGroup != null)
            {
                itemGroup.DOKill();
                itemGroup.alpha = 0f;
                itemGroup.DOFade(1f, 0.4f).SetDelay(i * itemStagger);
            }
        }
    }

    private void OnDestroy()
    {
        sequence?.Kill();
    }
}

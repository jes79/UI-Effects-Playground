using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AccordionItem : MonoBehaviour
{
    [SerializeField] private LayoutElement contentLayout; // 높이를 조절할 본문 영역 (RectMask2D 로 넘치는 부분을 잘라냄)
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private RectTransform icon;          // 펼침 상태를 보여주는 아이콘 (+ → ×)
    [SerializeField] private float expandedHeight = 200f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private bool closeOthers = true;     // 펼칠 때 같은 부모 아래의 다른 항목은 접기
    [SerializeField] private bool startExpanded = false;

    private bool isExpanded;
    private Tween heightTween;

    private void Awake()
    {
        isExpanded = startExpanded;
        contentLayout.preferredHeight = isExpanded ? expandedHeight : 0f;
        contentGroup.alpha = isExpanded ? 1f : 0f;
        icon.localEulerAngles = new Vector3(0f, 0f, isExpanded ? 45f : 0f);
    }

    public void Toggle()
    {
        if (isExpanded) Collapse();
        else Expand();
    }

    public void Expand()
    {
        if (isExpanded) return;

        if (closeOthers)
        {
            foreach (Transform sibling in transform.parent)
            {
                var other = sibling.GetComponent<AccordionItem>();
                if (other != null && other != this) other.Collapse();
            }
        }

        Animate(true);
    }

    public void Collapse()
    {
        if (!isExpanded) return;
        Animate(false);
    }

    private void Animate(bool expand)
    {
        isExpanded = expand;

        // LayoutElement.preferredHeight 를 트윈하면 부모의 VerticalLayoutGroup 이 매 프레임 다시 배치해서
        // 아래 항목들이 자연스럽게 밀려나고 당겨짐
        heightTween?.Kill();
        heightTween = DOTween.To(
                () => contentLayout.preferredHeight,
                h => contentLayout.preferredHeight = h,
                expand ? expandedHeight : 0f,
                duration)
            .SetEase(expand ? Ease.OutCubic : Ease.InOutCubic);

        contentGroup.DOKill();
        contentGroup.DOFade(expand ? 1f : 0f, expand ? duration : duration * 0.6f);

        icon.DOKill();
        icon.DOLocalRotate(new Vector3(0f, 0f, expand ? 45f : 0f), duration).SetEase(Ease.OutBack);
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GlowOutlinePulse : MonoBehaviour
{
    [SerializeField] private Image glowImage;    // 버튼 뒤에 깔린 부드러운 글로우 이미지
    [SerializeField] private Outline outline;    // 버튼에 붙은 Outline 컴포넌트 (선택사항)

    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float glowMinAlpha = 0.25f;
    [SerializeField] private float glowMaxAlpha = 0.9f;
    [SerializeField] private float glowMaxScale = 1.08f;
    [SerializeField] private Vector2 outlineMinDistance = new Vector2(1f, -1f);
    [SerializeField] private Vector2 outlineMaxDistance = new Vector2(4f, -4f);
    [SerializeField] private bool playOnEnable = true;

    private Sequence pulseSequence;

    private void OnEnable()
    {
        if (playOnEnable) StartPulse();
    }

    private void OnDisable()
    {
        pulseSequence?.Kill();
    }

    public void StartPulse()
    {
        pulseSequence?.Kill();
        glowImage.DOKill();
        glowImage.rectTransform.DOKill();

        SetGlowAlpha(glowMinAlpha);
        glowImage.rectTransform.localScale = Vector3.one;
        if (outline != null) outline.effectDistance = outlineMinDistance;

        // 글로우 밝기 + 크기 + 외곽선 두께를 함께 키웠다 줄였다 반복
        pulseSequence = DOTween.Sequence();
        pulseSequence.Append(glowImage.DOFade(glowMaxAlpha, duration));
        pulseSequence.Join(glowImage.rectTransform.DOScale(glowMaxScale, duration));
        if (outline != null)
            pulseSequence.Join(DOTween.To(() => outline.effectDistance, v => outline.effectDistance = v, outlineMaxDistance, duration));

        pulseSequence.SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopPulse()
    {
        pulseSequence?.Kill();
        pulseSequence = null;

        glowImage.DOFade(0f, 0.25f);
        glowImage.rectTransform.DOScale(1f, 0.25f);
        if (outline != null)
            DOTween.To(() => outline.effectDistance, v => outline.effectDistance = v, Vector2.zero, 0.25f);
    }

    public void TogglePulse()
    {
        if (pulseSequence != null && pulseSequence.IsActive()) StopPulse();
        else StartPulse();
    }

    private void SetGlowAlpha(float alpha)
    {
        Color c = glowImage.color;
        c.a = alpha;
        glowImage.color = c;
    }
}

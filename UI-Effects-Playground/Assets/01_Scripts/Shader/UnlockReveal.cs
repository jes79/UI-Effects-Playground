using UnityEngine;
using DG.Tweening;

public class UnlockReveal : UIShaderEffect
{
    private const string Reveal = "_Reveal";

    [SerializeField] private RectTransform lockIcon;   // 자물쇠 배지
    [SerializeField] private CanvasGroup lockGroup;
    [SerializeField] private float revealDuration = 1.2f;

    private bool isUnlocked;
    private Sequence sequence;

    private void Start()
    {
        ApplyAspect();
        material.SetFloat(Reveal, 0f);
    }

    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;

        sequence?.Kill();
        sequence = DOTween.Sequence();

        // 1. 자물쇠가 덜컹거림
        sequence.Append(lockIcon.DOShakeRotation(0.4f, new Vector3(0f, 0f, 25f), 15, 0f));
        // 2. 자물쇠가 커지면서 사라짐
        sequence.Append(lockIcon.DOScale(1.6f, 0.3f).SetEase(Ease.OutQuad));
        sequence.Join(lockGroup.DOFade(0f, 0.3f));
        // 3. 가운데부터 컬러가 원형으로 퍼짐
        sequence.Append(material.DOFloat(1f, Reveal, revealDuration).SetEase(Ease.OutCubic));
        // 4. 다 퍼지면 통 튀는 느낌
        sequence.Append(TargetRect.DOPunchScale(Vector3.one * 0.08f, 0.35f, 6, 0.8f));
    }

    public void ResetLock()
    {
        isUnlocked = false;
        sequence?.Kill();
        material.DOKill();
        material.SetFloat(Reveal, 0f);

        lockIcon.DOKill();
        lockIcon.localScale = Vector3.one;
        lockIcon.localRotation = Quaternion.identity;
        lockGroup.alpha = 1f;
        TargetRect.localScale = Vector3.one;
    }

    protected override void OnDestroy()
    {
        sequence?.Kill();
        base.OnDestroy();
    }
}

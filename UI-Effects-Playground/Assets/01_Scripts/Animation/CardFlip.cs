using UnityEngine;
using DG.Tweening;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private RectTransform card;
    [SerializeField] private GameObject frontFace;
    [SerializeField] private GameObject backFace;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float liftScale = 1.1f;    // 뒤집는 동안 살짝 들어 올리는 느낌
    [SerializeField] private bool startFront = false;   // false 면 뒷면부터 시작

    private bool isFront;
    private bool isFlipping;

    private void Awake()
    {
        isFront = startFront;
        ApplyFace();
    }

    public void Flip()
    {
        if (isFlipping) return;
        isFlipping = true;

        float half = duration * 0.5f;
        Sequence seq = DOTween.Sequence();

        // 1. 90도까지 돌려서 카드가 옆면(안 보이는 상태)이 되게 함
        seq.Append(card.DOLocalRotate(new Vector3(0f, 90f, 0f), half).SetEase(Ease.InQuad));
        seq.Join(card.DOScale(liftScale, half).SetEase(Ease.OutQuad));

        // 2. 안 보이는 순간에 앞/뒷면 교체 후 반대편(-90도)에서 이어서 회전
        seq.AppendCallback(() =>
        {
            isFront = !isFront;
            ApplyFace();
            card.localEulerAngles = new Vector3(0f, -90f, 0f);
        });

        // 3. 0도로 돌아오며 새 면이 보임
        seq.Append(card.DOLocalRotate(Vector3.zero, half).SetEase(Ease.OutQuad));
        seq.Join(card.DOScale(1f, half).SetEase(Ease.InQuad));
        seq.OnComplete(() => isFlipping = false);
    }

    private void ApplyFace()
    {
        frontFace.SetActive(isFront);
        backFace.SetActive(!isFront);
    }
}

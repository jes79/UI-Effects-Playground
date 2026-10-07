using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 이미지가 점점 픽셀화 → 가장 거칠어진 순간 다음 이미지로 교체 → 다시 선명해짐
public class PixelateTransition : UIShaderEffect
{
    private const string Progress = "_Progress";

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float duration = 0.8f;

    private int currentIndex;
    private bool isTransitioning;

    private void Start()
    {
        ApplyAspect();
        material.SetFloat(Progress, 0f);
    }

    public void Next()
    {
        if (isTransitioning || sprites.Length == 0) return;
        isTransitioning = true;

        float half = duration * 0.5f;
        var image = (Image)targetGraphic;

        DOTween.Sequence()
            .Append(material.DOFloat(1f, Progress, half).SetEase(Ease.InQuad))
            .AppendCallback(() =>
            {
                currentIndex = (currentIndex + 1) % sprites.Length;
                image.sprite = sprites[currentIndex];
            })
            .Append(material.DOFloat(0f, Progress, half).SetEase(Ease.OutQuad))
            .OnComplete(() => isTransitioning = false);
    }
}

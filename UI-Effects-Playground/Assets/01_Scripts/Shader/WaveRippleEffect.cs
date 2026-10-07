using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// 이미지를 클릭하면 클릭한 지점에서 물결(리플)이 퍼져나감 + 기본 일렁임 세기 토글
public class WaveRippleEffect : UIShaderEffect, IPointerClickHandler
{
    private const string RippleCenter = "_RippleCenter";
    private const string RippleProgress = "_RippleProgress";
    private const string Amplitude = "_Amplitude";

    [SerializeField] private float rippleDuration = 1.2f;
    [SerializeField] private float waveAmplitude = 0.006f;

    private bool isWaving = true;
    private Tween rippleTween;
    private Tween waveTween;

    private void Start()
    {
        ApplyAspect();
        material.SetFloat(RippleProgress, 1f); // 1 = 리플 없음
        material.SetFloat(Amplitude, waveAmplitude);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(TargetRect, eventData.position, eventData.pressEventCamera, out Vector2 local))
            return;

        // 클릭 위치를 이미지 uv(0~1)로 변환해서 리플 중심으로
        Vector2 uv = Rect.PointToNormalized(TargetRect.rect, local);
        material.SetVector(RippleCenter, uv);

        // 리플과 일렁임은 같은 머티리얼의 다른 프로퍼티라 material.DOKill() 대신 트윈을 각각 관리
        rippleTween?.Kill();
        material.SetFloat(RippleProgress, 0f);
        rippleTween = material.DOFloat(1f, RippleProgress, rippleDuration).SetEase(Ease.OutSine);
    }

    public void ToggleWave()
    {
        isWaving = !isWaving;
        waveTween?.Kill();
        waveTween = material.DOFloat(isWaving ? waveAmplitude : 0f, Amplitude, 0.5f);
    }
}

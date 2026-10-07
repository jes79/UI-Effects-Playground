using UnityEngine;
using TMPro;
using DG.Tweening;

public class LiquidFillEffect : UIShaderEffect
{
    private const string Fill = "_Fill";
    private const string WaveAmp = "_WaveAmp";

    [SerializeField] private TMP_Text percentText;     // 선택사항
    [SerializeField] private float startFill = 0.4f;
    [SerializeField] private float fillDuration = 1.0f;
    [SerializeField] private float baseWaveAmp = 0.015f;
    [SerializeField] private float sloshWaveAmp = 0.06f;  // 채워질 때 출렁이는 정도

    private float currentFill;
    private float targetFill;
    private Tween fillTween;
    private Sequence sloshSequence;

    private void Start()
    {
        currentFill = targetFill = startFill;
        material.SetFloat(Fill, currentFill);
        material.SetFloat(WaveAmp, baseWaveAmp);
        UpdateText();
    }

    public void SetFill(float value01)
    {
        targetFill = Mathf.Clamp01(value01);

        // 1. 수위가 목표치까지 차오름 (퍼센트 텍스트도 같이)
        fillTween?.Kill();
        fillTween = DOTween.To(() => currentFill, x =>
            {
                currentFill = x;
                material.SetFloat(Fill, x);
                UpdateText();
            }, targetFill, fillDuration)
            .SetEase(Ease.OutCubic);

        // 2. 물결이 크게 출렁였다가 잔잔해짐
        sloshSequence?.Kill();
        sloshSequence = DOTween.Sequence()
            .Append(material.DOFloat(sloshWaveAmp, WaveAmp, 0.15f))
            .Append(material.DOFloat(baseWaveAmp, WaveAmp, 1.4f).SetEase(Ease.OutQuad));
    }

    public void AddFill(float delta) => SetFill(targetFill + delta);

    public void RandomFill() => SetFill(Random.Range(0.1f, 0.95f));

    private void UpdateText()
    {
        if (percentText != null)
            percentText.text = Mathf.RoundToInt(currentFill * 100f) + "%";
    }

    protected override void OnDestroy()
    {
        fillTween?.Kill();
        sloshSequence?.Kill();
        base.OnDestroy();
    }
}

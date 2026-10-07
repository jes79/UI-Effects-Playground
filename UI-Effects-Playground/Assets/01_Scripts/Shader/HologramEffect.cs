using UnityEngine;
using DG.Tweening;

public class HologramEffect : UIShaderEffect
{
    private const string Reveal = "_Reveal";

    [SerializeField] private float powerOnDuration = 1.0f;
    [SerializeField] private float powerOffDuration = 0.4f;
    [SerializeField] private bool startOn = true;

    private bool isOn;

    private void Start()
    {
        isOn = startOn;
        material.SetFloat(Reveal, isOn ? 1f : 0f);
    }

    // 아래에서 위로 스캔되며 나타나고, 도착 순간 살짝 흔들림
    public void PowerOn()
    {
        isOn = true;
        material.DOKill();
        material.SetFloat(Reveal, 0f);
        material.DOFloat(1f, Reveal, powerOnDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                TargetRect.DOKill(true);
                TargetRect.DOShakeAnchorPos(0.2f, new Vector2(10f, 0f), 30, 0f, false, true);
            });
    }

    public void PowerOff()
    {
        isOn = false;
        material.DOKill();
        material.DOFloat(0f, Reveal, powerOffDuration).SetEase(Ease.InQuad);
    }

    public void Toggle()
    {
        if (isOn) PowerOff();
        else PowerOn();
    }
}

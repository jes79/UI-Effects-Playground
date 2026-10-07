using UnityEngine;
using DG.Tweening;

public class GradientMaskFade : UIShaderEffect
{
    private const string Progress = "_Progress";
    private const string Angle = "_Angle";

    [SerializeField] private float duration = 0.9f;
    [SerializeField] private Ease ease = Ease.InOutSine;

    private bool isVisible = true;

    private void Start()
    {
        material.SetFloat(Progress, 1f);
    }

    public void Show()
    {
        isVisible = true;
        material.DOKill();
        material.DOFloat(1f, Progress, duration).SetEase(ease);
    }

    public void Hide()
    {
        isVisible = false;
        material.DOKill();
        material.DOFloat(0f, Progress, duration).SetEase(ease);
    }

    public void Toggle()
    {
        if (isVisible) Hide();
        else Show();
    }

    // 쓸고 지나가는 방향 변경 (0 = 왼→오, 90 = 아래→위, 315 = 왼위→오아래)
    public void SetAngle(float angle)
    {
        material.SetFloat(Angle, angle);
    }
}

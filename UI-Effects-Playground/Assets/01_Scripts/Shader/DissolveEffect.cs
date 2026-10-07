using UnityEngine;
using DG.Tweening;

public class DissolveEffect : UIShaderEffect
{
    private const string DissolveAmount = "_DissolveAmount";

    [SerializeField] private float duration = 1.2f;
    [SerializeField] private Ease ease = Ease.InOutSine;

    private bool isDissolved;

    private void Start()
    {
        material.SetFloat(DissolveAmount, 0f);
    }

    public void Dissolve()
    {
        isDissolved = true;
        material.DOKill();
        material.DOFloat(1f, DissolveAmount, duration).SetEase(ease);
    }

    public void Appear()
    {
        isDissolved = false;
        material.DOKill();
        material.DOFloat(0f, DissolveAmount, duration).SetEase(ease);
    }

    public void Toggle()
    {
        if (isDissolved) Appear();
        else Dissolve();
    }
}

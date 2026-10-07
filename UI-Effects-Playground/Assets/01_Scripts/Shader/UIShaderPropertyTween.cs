using UnityEngine;
using DG.Tweening;

// 셰이더 프로퍼티(float) 하나를 from ↔ to 로 트윈하는 범용 스크립트
// 새로운 셰이더를 만들었을 때 인스펙터에서 프로퍼티 이름만 적어 바로 테스트해볼 수 있음
public class UIShaderPropertyTween : UIShaderEffect
{
    [SerializeField] private string propertyName = "_Saturation";
    [SerializeField] private float from = 0f;
    [SerializeField] private float to = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.InOutSine;
    [SerializeField] private bool loop = false; // true 면 Yoyo 무한 반복

    private bool isAtTo;

    private void Start()
    {
        material.SetFloat(propertyName, from);

        if (loop)
        {
            material.DOFloat(to, propertyName, duration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void Toggle()
    {
        isAtTo = !isAtTo;
        material.DOKill();
        material.DOFloat(isAtTo ? to : from, propertyName, duration).SetEase(ease);
    }
}

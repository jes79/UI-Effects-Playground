using UnityEngine;
using DG.Tweening;

// 아이콘 선택 시 아웃라인 펄스가 켜지고, 다시 누르면 꺼짐
public class IconOutlinePulse : UIShaderEffect
{
    private const string OutlineAlpha = "_OutlineAlpha";

    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private bool startSelected = false;

    private bool isSelected;

    private void Start()
    {
        isSelected = startSelected;
        material.SetFloat(OutlineAlpha, isSelected ? 1f : 0f);
    }

    public void ToggleSelect()
    {
        isSelected = !isSelected;

        material.DOKill();
        material.DOFloat(isSelected ? 1f : 0f, OutlineAlpha, fadeDuration);

        TargetRect.DOKill(true);
        if (isSelected) TargetRect.DOPunchScale(Vector3.one * 0.15f, 0.3f, 6, 0.8f);
    }
}

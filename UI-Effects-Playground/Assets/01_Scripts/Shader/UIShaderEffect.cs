using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 커스텀 UI 셰이더 연출 스크립트들의 공통 베이스
// - Graphic(Image / RawImage / TMP)에 연결된 머티리얼을 인스턴스로 복제해서 사용
//   → 같은 머티리얼 에셋을 쓰는 다른 UI 에 영향을 주지 않고, 에셋 파일도 플레이 중에 바뀌지 않음
// - 머티리얼 프로퍼티는 DOTween 의 material.DOFloat / DOColor / DOVector 로 트윈
public abstract class UIShaderEffect : MonoBehaviour
{
    [SerializeField] protected Graphic targetGraphic;

    protected Material material;

    protected RectTransform TargetRect => targetGraphic.rectTransform;

    protected virtual void Awake()
    {
        if (targetGraphic == null) targetGraphic = GetComponent<Graphic>();
        material = CreateMaterialInstance();
    }

    protected virtual Material CreateMaterialInstance()
    {
        Material source = targetGraphic.material;
        var instance = new Material(source) { name = source.name + " (Instance)" };
        targetGraphic.material = instance;
        return instance;
    }

    protected virtual void OnDestroy()
    {
        if (material == null) return;
        material.DOKill();
        Destroy(material);
    }

    // 원형 마스크처럼 가로세로 비율 보정이 필요한 셰이더용 (_Aspect = width / height)
    protected void ApplyAspect(string property = "_Aspect")
    {
        Rect rect = TargetRect.rect;
        if (rect.height > 0f) material.SetFloat(property, rect.width / rect.height);
    }
}

using UnityEngine;
using TMPro;
using DG.Tweening;

// TMP 텍스트에 TextGradientShine 셰이더를 적용하고, 텍스트 영역(_TextRect)과 빛줄기(_ShineLocation)를 제어
public class TextGradientShine : UIShaderEffect
{
    private const string ShineLocation = "_ShineLocation";

    [SerializeField] private float shineDuration = 0.9f;
    [SerializeField] private float shineInterval = 1.2f;
    [SerializeField] private string[] messages;     // ChangeText 로 순환할 문구 (선택사항)

    private TMP_Text text;
    private Canvas rootCanvas;
    private Sequence shineLoop;
    private int messageIndex;
    private readonly Vector3[] worldCorners = new Vector3[2];

    // TMP 는 Graphic.material 이 아니라 fontMaterial 로 인스턴스를 만들어야 함
    protected override Material CreateMaterialInstance()
    {
        text = (TMP_Text)targetGraphic;
        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
        return text.fontMaterial;
    }

    private void Start()
    {
        StartShineLoop();
    }

    private void LateUpdate()
    {
        // 텍스트가 바뀌거나 움직여도 그라디언트가 문장 전체에 맞도록 매 프레임 갱신
        Bounds bounds = text.textBounds;
        if (bounds.size.x <= 0f) return;

        Transform canvasTransform = rootCanvas.transform;
        worldCorners[0] = canvasTransform.InverseTransformPoint(text.transform.TransformPoint(bounds.min));
        worldCorners[1] = canvasTransform.InverseTransformPoint(text.transform.TransformPoint(bounds.max));
        Vector3 min = worldCorners[0];
        Vector3 max = worldCorners[1];
        material.SetVector("_TextRect", new Vector4(min.x, min.y, max.x - min.x, max.y - min.y));
    }

    public void StartShineLoop()
    {
        shineLoop?.Kill();
        shineLoop = DOTween.Sequence()
            .AppendCallback(() => material.SetFloat(ShineLocation, 0f))
            .Append(material.DOFloat(1f, ShineLocation, shineDuration).SetEase(Ease.InOutSine))
            .AppendInterval(shineInterval)
            .SetLoops(-1);
    }

    public void ChangeText()
    {
        if (messages == null || messages.Length == 0) return;

        messageIndex = (messageIndex + 1) % messages.Length;
        text.text = messages[messageIndex];

        // 글자가 바뀌는 순간 톡 튀어나오는 느낌 + 바로 빛줄기
        text.rectTransform.DOKill(true);
        text.rectTransform.DOPunchScale(Vector3.one * 0.12f, 0.35f, 6, 0.8f);
        StartShineLoop();
    }

    protected override void OnDestroy()
    {
        // fontMaterial 인스턴스는 TMP 가 관리하므로 트윈만 정리
        shineLoop?.Kill();
        if (material != null) material.DOKill();
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GaugeDamageDelay : MonoBehaviour
{
    [SerializeField] private RectTransform gaugeRoot;  // 피격 시 흔들릴 게이지 전체
    [SerializeField] private Image frontFill;          // 즉시 줄어드는 메인 게이지
    [SerializeField] private Image delayFill;          // 뒤늦게 따라오는 잔상 게이지
    [SerializeField] private Image flashImage;         // 피격 시 번쩍이는 오버레이 (선택사항)
    [SerializeField] private TMP_Text hpText;          // 선택사항

    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float frontDuration = 0.15f;
    [SerializeField] private float delayTime = 0.4f;     // 잔상 게이지가 따라오기 전 대기 시간
    [SerializeField] private float delayDuration = 0.5f;
    [SerializeField] private float shakeStrength = 12f;
    [SerializeField] private Color damageColor = new Color(1f, 0.85f, 0.3f); // 데미지 잔상 색
    [SerializeField] private Color healColor = new Color(0.45f, 1f, 0.55f);  // 회복 미리보기 색

    private float currentHp;
    private Vector2 originalPos;

    private void Awake()
    {
        currentHp = maxHp;
        originalPos = gaugeRoot.anchoredPosition;
        frontFill.fillAmount = 1f;
        delayFill.fillAmount = 1f;

        if (flashImage != null)
            flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0f);

        UpdateText();
    }

    public void Damage(float amount)
    {
        if (currentHp <= 0f) return;

        currentHp = Mathf.Max(0f, currentHp - amount);
        float target = currentHp / maxHp;

        frontFill.DOKill();
        delayFill.DOKill();

        // 회복 연출 도중이었다면 잔상 게이지를 메인 게이지 위치로 맞춘 뒤 시작
        if (delayFill.fillAmount < frontFill.fillAmount)
            delayFill.fillAmount = frontFill.fillAmount;
        delayFill.color = damageColor;

        // 1. 메인 게이지는 즉시 줄어듦
        frontFill.DOFillAmount(target, frontDuration).SetEase(Ease.OutQuad);

        // 2. 잔상 게이지는 잠깐 머물렀다가 천천히 따라옴 → 얼마나 깎였는지 눈에 보임
        delayFill.DOFillAmount(target, delayDuration)
            .SetDelay(delayTime)
            .SetEase(Ease.InOutQuad);

        // 3. 게이지 흔들림
        gaugeRoot.DOKill();
        gaugeRoot.anchoredPosition = originalPos;
        gaugeRoot.DOShakeAnchorPos(0.3f, new Vector2(shakeStrength, shakeStrength * 0.5f), 20, 90f, false, true)
            .OnComplete(() => gaugeRoot.anchoredPosition = originalPos);

        // 4. 번쩍임
        if (flashImage != null)
        {
            flashImage.DOKill();
            flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 0.7f);
            flashImage.DOFade(0f, 0.25f);
        }

        UpdateText();
    }

    public void Heal(float amount)
    {
        currentHp = Mathf.Min(maxHp, currentHp + amount);
        float target = currentHp / maxHp;

        frontFill.DOKill();
        delayFill.DOKill();

        // 회복은 반대로: 잔상 게이지(초록)가 먼저 목표치까지 늘어나고, 메인 게이지가 뒤따라 채워짐
        delayFill.color = healColor;
        delayFill.fillAmount = target;

        frontFill.DOFillAmount(target, delayDuration)
            .SetDelay(delayTime * 0.5f)
            .SetEase(Ease.OutQuad);

        UpdateText();
    }

    private void UpdateText()
    {
        if (hpText != null)
            hpText.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
    }

    // 테스트용
    public void TestDamage() => Damage(Random.Range(8f, 25f));
    public void TestHeal() => Heal(20f);
}

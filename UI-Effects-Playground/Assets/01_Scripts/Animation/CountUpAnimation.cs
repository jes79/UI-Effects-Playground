using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountUpAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    [SerializeField] private float duration = 1f;

    private int currentValue = 0;
    private Tween countTween;

    public void CountTo(int targetValue)
    {
        countTween?.Kill();

        int startValue = currentValue;

        // DOTween.To를 이용해 임의의 값(int)을 원하는 시간 동안 보간
        countTween = DOTween.To(
            () => currentValue,           // getter: 현재 값을 어떻게 가져올지
            value => {                     // setter: 값이 갱신될 때마다 실행
                currentValue = value;
                countText.text = currentValue.ToString();
            },
            targetValue,                   // 목표 값
            duration
        ).SetEase(Ease.OutQuad);
    }

    // 테스트용: 버튼 클릭 시 랜덤 목표값으로 카운트업
    public void TestRandomCount()
    {
        int randomTarget = Random.Range(currentValue + 50, currentValue + 500);
        CountTo(randomTarget);
    }
}
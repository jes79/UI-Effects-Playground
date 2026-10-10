# 20. 게이지 흔들림/강조 (Gauge Damage Delay)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `20_GaugeDamageDelay` | [`GaugeDamageDelay.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/GaugeDamageDelay.cs) | [`b4909b9`](https://github.com/jes79/UI-Effects-Playground/commit/b4909b91a96b4afc1a8a5617d27425d27824e66d) |

[← 위키 홈](../README.md)

## 무엇을 하나요
피격 시 HP 게이지가 즉시 줄어들고, 그 뒤의 **잔상 게이지**가 잠깐 머물렀다 천천히 따라옵니다. 게이지가 흔들리고 번쩍여서 "얼마나 맞았는지"가 한눈에 보여요. 회복은 반대로 초록 미리보기가 먼저 차고 메인 게이지가 뒤따라요.

## 조작
- **Damage** : 8~25 랜덤 데미지
- **Heal** : 20 회복

## 구성
```
Gauge                (GaugeDamageDelay)
├─ Frame             테두리
├─ FillBackground    빈 게이지
├─ DelayFill         잔상 게이지 (Image Filled / Horizontal)
├─ FrontFill         메인 게이지 (Image Filled / Horizontal)
├─ Flash             피격 번쩍임 (alpha 0)
└─ HpText
```

## 핵심 원리
- 게이지 이미지를 **두 겹**으로 쌓고, 앞은 즉시 / 뒤는 `SetDelay()` 후 천천히 `DOFillAmount()`
- 피격 순간 `DOShakeAnchorPos` + 흰색 오버레이 `DOFade(0)` 로 타격감 추가
- Filled 타입 Image 는 **스프라이트가 있어야** 동작함

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `frontDuration` | 0.15 | 메인 게이지가 줄어드는 시간 |
| `delayTime` | 0.4 | 잔상이 따라오기 전 대기 시간 |
| `delayDuration` | 0.5 | 잔상이 따라오는 시간 |
| `shakeStrength` | 12 | 흔들림 세기(px) |

## 관련 항목
[13. 흔들림](../../UI-Effects-Playground/Assets/01_Scripts/Animation/ShakeEffect.cs) · [19. 프로그레스바 채움](../../UI-Effects-Playground/Assets/01_Scripts/Animation/ProgressBarFill.cs)

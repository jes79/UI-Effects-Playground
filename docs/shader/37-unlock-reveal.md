# 37. 언락 리빌 (Unlock Reveal)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `37_UnlockReveal` | [`UnlockReveal.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/UnlockReveal.shader) | [`UnlockReveal.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/UnlockReveal.cs) | [`3d3a353`](https://github.com/jes79/UI-Effects-Playground/commit/3d3a35364af4967ebe48117b36849a1e57083e62) |

[← 위키 홈](../README.md)

## 무엇을 하나요
잠긴 콘텐츠(흑백 + 어둡게) 위의 자물쇠가 덜컹거리다 커지며 사라지고, 가운데부터 빛나는 링과 함께 컬러가 원형으로 퍼지며 해금됩니다.

## 조작
- **Unlock** : 해금 연출
- **Reset** : 다시 잠금

## 핵심 원리
- 잠긴 상태 = `Gray()` 흑백 × 어둡게, 원 안쪽만 원래 색
- 원 반지름을 `_Reveal` 로 키우고, 경계에 가우시안 링
- `_Aspect` 로 가로로 긴 이미지에서도 찌그러지지 않은 원, 최대 반지름은 모서리까지
- **애니메이션 → 셰이더 → 애니메이션** 을 하나의 Sequence 로 연결하는 예제
  1. `DOShakeRotation` 자물쇠 덜컹
  2. `DOScale` + `DOFade` 자물쇠 사라짐
  3. `material.DOFloat("_Reveal")` 컬러 퍼짐
  4. `DOPunchScale` 마무리

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_LockedSaturation` / `_LockedBrightness` | 0 / 0.35 | 잠긴 상태 채도 / 밝기 |
| `_Reveal` | 0 | 0 = 잠김, 1 = 해금 (스크립트가 트윈) |
| `_RingColor` / `_RingWidth` | 금색 / 0.035 | 링 색 / 두께 |
| `_Aspect` | 1 | 가로/세로 비율 (스크립트) |

## 스크립트
| 필드/메서드 | 설명 |
|---|---|
| `lockIcon` / `lockGroup` | 자물쇠 배지 RectTransform / CanvasGroup |
| `revealDuration` | 컬러 퍼지는 시간 (1.2) |
| `Unlock()` / `ResetLock()` | 해금 / 초기화 |

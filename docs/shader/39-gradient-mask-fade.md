# 39. 그라디언트 마스크 페이드 (Gradient Mask Fade)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `39_GradientMaskFade` | [`GradientMaskFade.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/GradientMaskFade.shader) | [`GradientMaskFade.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/GradientMaskFade.cs) | [`8f86450`](https://github.com/jes79/UI-Effects-Playground/commit/8f8645033e1cee99df14090155f38c795c238439) |

[← 위키 홈](../README.md)

## 무엇을 하나요
부드러운 그라디언트 경계가 지정한 방향으로 쓸고 지나가며 이미지를 보여주거나 숨깁니다. ([7번 페이드](../../UI-Effects-Playground/Assets/01_Scripts/Animation/FadeInOut.cs)는 전체 알파를 한 번에, 이번엔 위치마다 다른 알파)

## 조작
- **Show / Hide** : 보이기 / 숨기기
- **Left > Right**, **Bottom > Top**, **Diagonal** : 방향 변경

## 핵심 원리
- `ProjectUV(uv, AngleToDir(angle))` 로 진행 방향 기준 0~1 위치 `t`
- `mask = saturate((progress * (1 + softness) - t) / softness)` → 부드러운 경계 폭까지 포함해서 0 이면 완전히 숨고 1 이면 완전히 보임
- `_Angle` 하나로 방향 자유롭게

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Progress` | 1 | 0 = 숨김, 1 = 보임 (스크립트가 트윈) |
| `_Softness` | 0.3 | 경계 부드러움 |
| `_Angle` | 0 | 0 = 왼→오, 90 = 아래→위, 315 = 왼위→오아래 |

## 스크립트 API
| 메서드 | 설명 |
|---|---|
| `Show()` / `Hide()` / `Toggle()` | `_Progress` 트윈 (0.9초, InOutSine) |
| `SetAngle(float)` | 방향 변경 |

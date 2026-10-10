# 32. 무지개/홀로그램 그라디언트 (Holographic Gradient)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `32_HolographicGradient` | [`HolographicGradient.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/HolographicGradient.shader) | [`HolographicCard.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/HolographicCard.cs) | [`8da549c`](https://github.com/jes79/UI-Effects-Playground/commit/8da549c115436340cea16a6c57e84e6787363245) |

[← 위키 홈](../README.md)

## 무엇을 하나요
홀로그램 카드처럼 대각선으로 흐르는 무지개 + 미세 스트라이프 + 반짝이 + 글레어를 그립니다. 마우스를 카드 위에서 움직이면 카드가 기울고, 그 기울기에 맞춰 색과 빛이 함께 움직여요.

## 조작
- 카드 위에서 마우스 이동 : 기울기 + 색/글레어 변화
- 마우스가 없으면 천천히 혼자 흔들림

## 핵심 원리
- `HueToRGB(uv.x + uv.y + time)` → 흐르는 무지개
- Screen 블렌드 `1 - (1-a)(1-b)` 로 원본 그림을 살리면서 무지개를 얹음
- 반짝이: `floor(uv * 밀도)` 격자 셀마다 `Hash21` 랜덤으로 일부만 켜고, 셀 중심 거리로 동그란 점 모양
- 스크립트가 마우스 위치(-1~1)를 `_Tilt` 로 넘기고, 같은 값으로 카드 `localRotation` 도 회전
- 매 프레임 바뀌는 마우스 값은 `Update()` 에서 `Lerp` 로 따라가고, 진입/이탈(스케일)만 DOTween

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_HoloStrength` | 0.45 | 무지개 세기 |
| `_HoloScale` / `_HoloSpeed` | 1.2 / 0.15 | 무지개 반복 수 / 흐르는 속도 |
| `_StripeStrength` | 0.25 | 스트라이프 |
| `_Tilt` | (0,0) | 기울기 (스크립트) |
| `_GlareStrength` / `_GlareWidth` | 0.6 / 0.12 | 글레어 |
| `_SparkleDensity` / `_SparkleStrength` | 28 / 0.8 | 반짝이 |

## 스크립트 필드
| 필드 | 기본값 | 설명 |
|---|---|---|
| `maxTiltAngle` | 15 | 최대 기울기(도) |
| `followSpeed` | 8 | 마우스 추적 속도 |
| `hoverScale` | 1.05 | 마우스 올렸을 때 크기 |
| `idleSway` | true | 대기 중 흔들림 |

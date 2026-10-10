# 30. 셔이머 (Shimmer / Shine)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `30_Shimmer` | [`Shimmer.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/Shimmer.shader) | [`ShimmerEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/ShimmerEffect.cs) | [`09c4e96`](https://github.com/jes79/UI-Effects-Playground/commit/09c4e96866137e93219b5bef1304fb19aed0b4b6) |

[← 위키 홈](../README.md)

## 무엇을 하나요
사선 빛줄기가 이미지 위를 훑고 지나갑니다. 보상 · 희귀 아이템 · 구매 버튼 강조에 많이 쓰여요.

## 조작
- 왼쪽 풍경 : 자동 반복
- **Shine Star** : 별 아이콘에 한 번만 반짝

## 핵심 원리
- `ProjectUV()` 로 uv 를 빛줄기 진행 방향으로 투영 → 각도와 무관하게 "시작 0 ~ 끝 1" 위치를 얻음
- 빛줄기는 `_ShineLocation` 위치의 띠로 계산하고, **알파는 그대로 두고 rgb 에만 더해서** 투명한 곳엔 빛이 안 보임
- 셰이더는 위치만 받고, 언제/얼마나 빠르게/몇 번 지나갈지는 DOTween 이 결정 (`AppendInterval` 로 반복 간격)

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_ShineLocation` | 0 | 빛줄기 위치 (스크립트가 0→1 트윈) |
| `_ShineWidth` | 0.15 (데모 0.08) | 빛줄기 폭 |
| `_ShineSoftness` | 0.7 | 가장자리 부드러움 |
| `_ShineAngle` | 30 | 각도 |
| `_ShineIntensity` | 1 (데모 0.7) | 밝기 |

## 스크립트 API
| 메서드 | 설명 |
|---|---|
| `StartLoop()` | 0→1 지나감 → `interval` 대기 → 반복 (`autoLoop` 이면 시작 시 자동) |
| `PlayOnce()` | 한 번만 반짝 |

# 46. 스켈레톤 로딩 (Skeleton Loading)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `46_SkeletonLoading` | [`SkeletonLoading.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/SkeletonLoading.shader) | [`SkeletonLoading.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/SkeletonLoading.cs) | [`99fb679`](https://github.com/jes79/UI-Effects-Playground/commit/99fb6794d545d7f3a430b0aea081cdc31f21900b) |

[← 위키 홈](../README.md)

## 무엇을 하나요
데이터를 불러오는 동안 회색 자리표시자(스켈레톤) 위로 밝은 띠가 한 줄로 이어져 훑고 지나가고, 로딩이 끝나면 실제 콘텐츠가 하나씩 올라오며 등장합니다. (유튜브/페이스북 로딩 화면)

## 조작
- **Reload** : 로딩 연출 다시 보기 (2.5초 후 콘텐츠 등장)

## 핵심 원리
- [30번 셔이머](30-shimmer.md)는 uv(이미지 기준), 스켈레톤은 **캔버스 좌표(`worldPosition`) 기준**으로 띠 위치 계산
- 그래서 여러 자리표시자가 **머티리얼 하나를 공유**해도 빛 띠가 화면 전체를 한 줄로 이어 지나감
- 시간 기반이라 머티리얼 인스턴스가 필요 없음 (스크립트는 `UIShaderEffect` 를 상속하지 않음)
- 스프라이트는 **모양(알파)** 만 사용 (둥근 사각형 Sliced / 원 Knob)
- 실제 콘텐츠는 `SetDelay(i * stagger)` 로 시간차 등장 (스태거)
- 스켈레톤과 실제 콘텐츠를 같은 위치·크기로 배치해야 로딩 후 화면이 덜컹거리지 않음

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_BaseColor` / `_HighlightColor` | 회색 / 밝은 회색 | 바탕 / 띠 색 |
| `_BandWidth` | 220 | 띠 폭 (캔버스 단위) |
| `_Speed` | 0.7 | 초당 지나가는 횟수 |
| `_Angle` | 20 | 띠 기울기 |
| `_SweepMin` / `_SweepMax` | -1200 / 1200 | 띠 이동 범위 (캔버스 x) |

## 스크립트 필드
| 필드 | 기본값 | 설명 |
|---|---|---|
| `skeletonGroup` / `contentGroup` | - | 스켈레톤 / 실제 콘텐츠 CanvasGroup |
| `contentItems` | - | 순서대로 등장할 콘텐츠 |
| `loadDuration` | 2.5 | 가짜 로딩 시간 |
| `itemStagger` / `itemSlideDistance` | 0.08 / 30 | 시간차 / 올라오는 거리 |

# 35. 픽셀화 전환 (Pixelate Transition)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `35_PixelateTransition` | [`PixelateTransition.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/PixelateTransition.shader) | [`PixelateTransition.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/PixelateTransition.cs) | [`d257ce8`](https://github.com/jes79/UI-Effects-Playground/commit/d257ce8e3f7945a17a4a0940a69e5d71c44c9463) |

[← 위키 홈](../README.md)

## 무엇을 하나요
이미지가 점점 모자이크처럼 거칠어지다가, 가장 거친 순간 다음 이미지로 바뀌고 다시 선명해집니다. (낮 ↔ 밤 풍경)

## 조작
- **Next Image** : 다음 이미지로 전환

## 핵심 원리
- uv 를 격자 셀 단위로 `floor` → 한 셀 안의 픽셀이 같은 색을 샘플링
- 셀 개수를 300 → 6 으로 줄이면 거칠어짐 (`sqrt` 커브로 초반에 빠르게)
- `_Aspect` 를 곱해 셀을 정사각형으로 (스크립트가 `ApplyAspect()` 로 자동 설정)
- 27번 카드 플립과 같은 원리: **가장 안 보이는 순간** `AppendCallback` 으로 스프라이트 교체

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Progress` | 0 | 0 = 선명, 1 = 가장 거침 (스크립트가 트윈) |
| `_MinCells` / `_MaxCells` | 6 / 300 | 가장 거칠 때 / 선명할 때 셀 수 |
| `_Aspect` | 1 | 가로/세로 비율 (스크립트) |

## 스크립트
| 필드/메서드 | 설명 |
|---|---|
| `sprites` | 순환할 이미지 목록 |
| `duration` | 전환 전체 시간 (0.8) |
| `Next()` | 픽셀화 → 교체 → 복원 |

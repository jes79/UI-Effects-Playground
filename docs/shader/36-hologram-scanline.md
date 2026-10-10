# 36. 홀로그램 스캔라인 (Hologram Scanline)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `36_HologramScanline` | [`HologramScanline.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/HologramScanline.shader) | [`HologramEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/HologramEffect.cs) | [`bd514f8`](https://github.com/jes79/UI-Effects-Playground/commit/bd514f8fa5ad1d03ad1457edca972cc78729368f) |

[← 위키 홈](../README.md)

## 무엇을 하나요
이미지를 하늘색 단색 홀로그램으로 바꾸고, 흘러내리는 스캔라인 · 위로 지나가는 밝은 띠 · 불안정한 깜빡임을 더합니다. 전원을 켜면 아래에서 위로 스캔되며 나타나요.

## 조작
- **Toggle Power** : 끄기 / 아래에서 위로 스캔되며 켜기

## 핵심 원리
- 홀로그램 = **단색 치환 + 스캔라인 + 깜빡임 + 반투명** 조합 (원본의 밝기 `Gray()` 만 남기고 색은 `_HoloColor`)
- 스캔라인 `sin(uv.y * 줄 수 - time)`, 깜빡임은 `ValueNoise(time)`
- `_Reveal` 위치 아래만 보이게 하고, 경계에 밝은 선을 그려 "스캔되며 켜지는" 등장 연출
- 켜짐 완료 시 `DOShakeAnchorPos` 로 지직 흔들림

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_HoloColor` | 하늘색 | 홀로그램 색 |
| `_LineCount` / `_LineSpeed` / `_LineStrength` | 120 / 2 / 0.5 | 스캔라인 |
| `_BandSpeed` | 0.35 | 밝은 띠 속도 |
| `_FlickerSpeed` | 12 | 깜빡임 속도 |
| `_RimStrength` | 0.6 | 가장자리 밝기 |
| `_Reveal` | 1 | 0 = 꺼짐, 1 = 켜짐 (스크립트가 트윈) |

## 스크립트 API
| 메서드 | 설명 |
|---|---|
| `PowerOn()` / `PowerOff()` / `Toggle()` | `_Reveal` 트윈 (켜기 1.0초 OutCubic, 끄기 0.4초 InQuad) |

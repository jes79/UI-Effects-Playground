# 40. 물결 왜곡 (Wave Distortion)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `40_WaveDistortion` | [`WaveDistortion.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/WaveDistortion.shader) | [`WaveRippleEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/WaveRippleEffect.cs) | [`17f66b2`](https://github.com/jes79/UI-Effects-Playground/commit/17f66b2294cec98f8bd97bb0287a34ed23ec6cb0) |

[← 위키 홈](../README.md)

## 무엇을 하나요
이미지가 물에 비친 듯 잔잔하게 일렁이고, 클릭한 지점에서 원형 물결(리플)이 퍼져나갑니다.

## 조작
- 이미지 클릭 : 그 지점에서 리플
- **Toggle Wave** : 기본 일렁임 켜기/끄기

## 핵심 원리
- 왜곡 셰이더의 기본: 색을 바꾸지 않고 **샘플링할 uv 를 살짝 밀어서** 그림이 휘어 보이게 함
- 기본 일렁임: `sin(uv.y)` 로 x 를, `cos(uv.x)` 로 y 를 밀기
- 클릭 리플: 클릭 지점과의 거리가 반지름 근처일 때만 바깥 방향으로 밀어서 링 모양 파동
- 클릭 좌표 → `ScreenPointToLocalPointInRectangle` → `Rect.PointToNormalized` 로 **0~1 uv** 로 변환해 `_RippleCenter` 에 전달 ([12번 리플](../../UI-Effects-Playground/Assets/01_Scripts/Animation/ButtonRipple.cs)과 같은 좌표 변환)

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Amplitude` / `_Frequency` / `_Speed` | 0.006 / 18 / 2 | 기본 일렁임 |
| `_RippleCenter` | (0.5, 0.5) | 리플 중심 uv (스크립트) |
| `_RippleProgress` | 1 | 0→1 로 퍼짐, 1 = 리플 없음 (스크립트가 트윈) |
| `_RippleStrength` / `_RippleWidth` | 0.03 / 0.08 | 리플 세기 / 폭 |
| `_Aspect` | 1 | 가로/세로 비율 (스크립트) |

## 스크립트
| 필드/메서드 | 설명 |
|---|---|
| `OnPointerClick` | 클릭 위치에서 리플 시작 (`rippleDuration` 1.2) |
| `ToggleWave()` | 기본 일렁임 진폭 0 ↔ `waveAmplitude` |

> 이미지의 **Raycast Target** 이 켜져 있어야 클릭을 받아요.

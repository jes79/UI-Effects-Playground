# 43. 히트 웨이브 (Heat Wave)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `43_HeatWave` | [`HeatWave.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/HeatWave.shader) | [`HeatWaveEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/HeatWaveEffect.cs) | [`a086d62`](https://github.com/jes79/UI-Effects-Playground/commit/a086d6252c4979ebfe5367a0304a942055f8b10f) |

[← 위키 홈](../README.md)

## 무엇을 하나요
뜨거운 공기처럼 이미지가 아지랑이로 일렁입니다. 아래쪽(열원)일수록 강하게 일렁이고 살짝 붉은 톤이 섞여요.

## 조작
- **Toggle Heat** : 열기가 서서히 가라앉음 / 다시 올라옴

## 핵심 원리
- [40번 물결](40-wave-distortion.md)은 규칙적인 `sin` 으로 uv 를 밀었다면, 히트 웨이브는 **불규칙한 `Fbm` 노이즈**로 밀어서 아지랑이처럼 보임
- 노이즈 y 좌표에서 시간을 빼면(`- t`) 무늬가 **위로 올라감**
- `heightMask = pow(1 - uv.y, falloff)` 로 아래쪽일수록 강하게

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Strength` | 0.025 | 일렁임 세기 (스크립트가 트윈) |
| `_Scale` | 5 | 노이즈 크기 |
| `_Speed` | 1.2 | 올라가는 속도 |
| `_RiseFalloff` | 1.2 | 높이에 따른 감쇠 |
| `_HeatTint` | 주황, a 0.25 | 열기 톤 |

## 스크립트
| 필드/메서드 | 설명 |
|---|---|
| `heatStrength` | 켰을 때 세기 (0.025) |
| `ToggleHeat()` | `_Strength` 0 ↔ `heatStrength` (1초, InOutSine) |

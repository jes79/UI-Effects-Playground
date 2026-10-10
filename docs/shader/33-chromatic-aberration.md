# 33. 크로매틱 애버레이션 (Chromatic Aberration)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `33_ChromaticAberration` | [`ChromaticAberration.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/ChromaticAberration.shader) | [`ChromaticHitEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/ChromaticHitEffect.cs) | [`f867d61`](https://github.com/jes79/UI-Effects-Playground/commit/f867d61fdb15393700b3af77295a79020ce586a6) |

[← 위키 홈](../README.md)

## 무엇을 하나요
R / G / B 채널을 서로 다른 방향으로 밀어서 색이 갈라져 보이게 합니다. 평소엔 살짝 떨리고, 피격 순간 크게 갈라졌다가 돌아와요.

## 조작
- **Hit!** : 피격 연출 (색 분리 스파이크 + 흔들림)

## 핵심 원리
- 텍스처를 **세 번** 샘플링: R 은 `uv + offset`, G 는 `uv`, B 는 `uv - offset`
- 최종 알파는 세 채널 알파의 **최대값** → 갈라진 색이 이미지 바깥으로 삐져나와 보임
- `InsideUV()` 로 범위 밖 샘플을 0 처리 (Clamp 로 가장자리가 늘어나 보이는 것 방지)
- "최대치로 즉시 세팅 → 원래 값으로 트윈" 은 피격/충격 연출의 기본 패턴

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Amount` | 0.004 | 분리 거리 (스크립트가 트윈) |
| `_Angle` | 0 | 분리 방향 |
| `_JitterSpeed` / `_JitterAmount` | 8 / 0.003 | 떨림 속도 / 크기 |

## 스크립트 필드
| 필드 | 기본값 | 설명 |
|---|---|---|
| `idleAmount` | 0.004 | 평소 분리량 |
| `hitAmount` | 0.05 | 피격 순간 분리량 |
| `hitDuration` | 0.5 | 원래대로 돌아오는 시간 |
| `shakeStrength` | 20 | 흔들림 세기 |

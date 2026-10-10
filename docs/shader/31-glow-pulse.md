# 31. 글로우 펄스 (Glow Pulse Shader)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `31_GlowPulseShader` | [`GlowPulse.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/GlowPulse.shader) | [`GlowPulseShaderEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/GlowPulseShaderEffect.cs) | [`912e3f2`](https://github.com/jes79/UI-Effects-Playground/commit/912e3f2f9db59a62a25931d782248f251089665a) |

[← 위키 홈](../README.md)

## 무엇을 하나요
스프라이트의 **알파 모양을 따라** 바깥 글로우를 만들고, 시간에 따라 숨 쉬듯 밝아졌다 어두워집니다. ([25번](../animation/25-glow-outline-pulse.md)은 글로우 이미지를 따로 깔았다면, 이번엔 별·하트 등 어떤 모양이든 셰이더가 알아서 둘러싸요.)

## 조작
- **Boost** : 순간적으로 확 밝아지며 톡 튐
- **Toggle Glow** : 글로우 켜기/끄기

## 핵심 원리
- 16방향 × 2링으로 주변 알파를 샘플링해서 **평균** → 번진 실루엣
- 번진 실루엣을 원본 뒤에 `BlendOver()` 로 깔면 바깥 글로우
- 맥박은 `sin(_Time.y)` 로 셰이더가 처리 → 반복은 셰이더, 이벤트(Boost)는 DOTween 으로 역할 분담
- 글로우가 퍼질 **투명 여백**이 스프라이트 안에 있어야 함 (Mesh Type = Full Rect)

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_GlowColor` | 하늘색 (데모: 금색 / 분홍) | 글로우 색 |
| `_GlowSize` | 0.06 | 퍼지는 거리 (uv) |
| `_GlowIntensity` | 1.5 | 세기 (스크립트가 트윈) |
| `_InnerGlow` | 0.25 | 원본 안쪽 밝아짐 |
| `_PulseSpeed` / `_PulseMin` | 3 / 0.35 | 맥박 속도 / 최소 밝기 |

## 스크립트 API
| 메서드 | 설명 |
|---|---|
| `Boost()` | `boostIntensity` 로 올렸다가 원래대로 + `DOPunchScale` |
| `ToggleGlow()` | 글로우 페이드 인/아웃 |

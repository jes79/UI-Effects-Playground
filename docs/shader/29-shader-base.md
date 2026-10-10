# 29. 커스텀 UI 셰이더 베이스 (Shader Base)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `29_ShaderBase` | [`UIEffectsCommon.cginc`](../../UI-Effects-Playground/Assets/02_Shaders/UI/UIEffectsCommon.cginc) · [`UIBase.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/UIBase.shader) | [`UIShaderEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/UIShaderEffect.cs) · [`UIShaderPropertyTween.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/UIShaderPropertyTween.cs) | [`71741ca`](https://github.com/jes79/UI-Effects-Playground/commit/71741ca620681d7c1f2957ad43fcfa8ffdea6ccd) |

[← 위키 홈](../README.md) · 자세한 구조: [커스텀 UI 셰이더 구조](../shader-architecture.md)

## 무엇을 하나요
이후 30~46번 셰이더가 모두 공유하는 **뼈대**를 만듭니다. `UI/Default` 와 똑같이 동작(Mask / RectMask2D 지원)하면서 채도·밝기만 조절하는 `UIEffects/UIBase` 셰이더와, 셰이더 값을 DOTween 으로 트윈하는 제어 스크립트 베이스를 셋업해요.

## 데모 씬
| 이미지 | 내용 |
|---|---|
| 왼쪽 | 기본 `UI/Default` (비교용) |
| 가운데 | `UIBase` + `_Saturation` 0 ↔ 1.4 무한 루프 |
| 오른쪽 | `RectMask2D` 안에서 잘리는지 확인 + **Toggle Brightness** 버튼 |

## 핵심 원리
- 공통 include 의 `UIVert` 가 UI/Default 와 같은 버텍스 처리를 하고, `UIFinalize()` 가 RectMask2D 클리핑을 적용
- `UIShaderEffect` 는 `Awake()` 에서 머티리얼을 `new Material()` 로 복제 → 같은 에셋을 쓰는 다른 UI · 에셋 파일이 같이 바뀌지 않음
- `material.DOFloat(end, "_Prop", time)` 은 DOTween 기본 기능이라 Ease / Loop / Sequence 를 그대로 사용 가능

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Saturation` | 1 | 채도 (0 = 흑백) |
| `_Brightness` | 1 | 밝기 |

## UIShaderPropertyTween
| 필드 | 설명 |
|---|---|
| `propertyName` | 트윈할 셰이더 프로퍼티 이름 |
| `from` / `to` / `duration` / `ease` | 트윈 범위와 커브 |
| `loop` | 켜면 Yoyo 무한 반복, 끄면 `Toggle()` 로 왕복 |

> 새 셰이더를 만들었을 때 스크립트를 짜기 전에 이 컴포넌트로 바로 값을 움직여 볼 수 있어요.

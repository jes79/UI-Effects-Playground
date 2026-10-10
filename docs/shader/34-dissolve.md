# 34. 디졸브 (Dissolve)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `34_Dissolve` | [`Dissolve.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/Dissolve.shader) | [`DissolveEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/DissolveEffect.cs) | [`fba3ba5`](https://github.com/jes79/UI-Effects-Playground/commit/fba3ba5dc9275b46782dd45db4a63ca917516068) |

[← 위키 홈](../README.md)

## 무엇을 하나요
이미지가 노이즈 모양으로 타들어가며 사라지고, 경계에는 주황색으로 빛나는 불꽃 엣지가 생깁니다.

## 조작
- **Dissolve** : 타들어가며 사라짐
- **Appear** : 역순으로 다시 나타남

## 핵심 원리
- 픽셀마다 노이즈(0~1)를 정하고 **노이즈가 임계값보다 낮은 곳부터 지움** → 임계값을 0→1 로 올리면 점점 타들어감
- 임계값 바로 위 얇은 띠에 엣지 색 → 불타는 경계
- 노이즈 텍스처 없이 공통 include 의 `Fbm()` 으로 코드에서 생성
- 임계값 범위를 `1 + edgeWidth` 로 늘려서 1 일 때 엣지까지 완전히 사라지게 함

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_DissolveAmount` | 0 | 0 = 보임, 1 = 사라짐 (스크립트가 트윈) |
| `_NoiseScale` | 6 | 노이즈 크기 (클수록 잘게) |
| `_EdgeWidth` | 0.08 | 엣지 폭 |
| `_EdgeColor` / `_EdgeIntensity` | 주황 / 1.6 | 엣지 색 / 밝기 |

## 스크립트 API
| 메서드 | 설명 |
|---|---|
| `Dissolve()` / `Appear()` / `Toggle()` | `_DissolveAmount` 를 1 / 0 으로 트윈 (`duration` 1.2, `InOutSine`) |

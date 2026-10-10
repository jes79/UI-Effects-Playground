# 44. 텍스트 그라디언트 애니메이션 (Text Gradient Shine)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `44_TextGradientShine` | [`TextGradientShine.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/TextGradientShine.shader) | [`TextGradientShine.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/TextGradientShine.cs) | [`733a22d`](https://github.com/jes79/UI-Effects-Playground/commit/733a22dded8dfcd73d57f35231cb7a01d8ec442b) |

[← 위키 홈](../README.md)

## 무엇을 하나요
TextMeshPro 텍스트 전체에 3색 그라디언트가 흐르고, 주기적으로 사선 빛줄기가 지나갑니다. (LEVEL UP!, VICTORY 같은 타이틀 연출)

## 조작
- **Change Text** : 문구 변경 (톡 튀며 바뀌고 그라디언트가 새 길이에 맞춰짐)

## 왜 템플릿과 다른가요
TMP 는 글자를 **SDF(거리장) 폰트 아틀라스**로 그려서, 29번 템플릿을 그대로 쓰면 글자가 네모 박스로 보여요. 그래서 이 셰이더는 SDF 를 직접 해석합니다.

## 핵심 원리
- SDF 아틀라스의 알파 = 글자 외곽선까지의 거리 → **0.5 를 기준으로 잘라** 글자 모양, `fwidth()` 로 안티에일리어싱
- uv 로 그라디언트를 계산하면 글자마다 따로 칠해짐 → **캔버스 좌표(`worldPosition`)를 텍스트 영역(`_TextRect`)으로 나눠 문장 전체 기준**으로 칠함
- 스크립트가 `textBounds`(로컬) → 캔버스 좌표로 변환해 매 프레임 `_TextRect` 갱신
- `UIShaderEffect.CreateMaterialInstance()` 를 오버라이드해서 TMP 전용 인스턴스 `fontMaterial` 사용

> ⚠️ 머티리얼의 `_MainTex` 에 **폰트 아틀라스 텍스처**를 꼭 지정하세요. TMP 는 아틀라스가 다르면 머티리얼을 무시하고 기본 머티리얼로 되돌려요.
> TMP 의 Bold 는 이 셰이더에서 적용되지 않아요. 굵게 하려면 `_Dilate` 를 올리세요.

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_MainTex` | - | 폰트 아틀라스 |
| `_ColorA` / `_ColorB` / `_ColorC` | 분홍 / 하늘 / 노랑 | 그라디언트 3색 |
| `_GradientSpeed` | 0.25 | 흐르는 속도 |
| `_TextRect` | - | 텍스트 영역 (스크립트) |
| `_ShineColor` / `_ShineLocation` / `_ShineWidth` | 흰색 / 0 / 0.06 | 빛줄기 |
| `_Softness` / `_Dilate` | 0 / 0 (데모 0.04) | 외곽 부드러움 / 두께 |

## 스크립트
| 필드/메서드 | 설명 |
|---|---|
| `shineDuration` / `shineInterval` | 빛줄기 시간 / 반복 간격 |
| `messages` | `ChangeText()` 로 순환할 문구 |
| `StartShineLoop()` / `ChangeText()` | 빛줄기 반복 / 문구 변경 |

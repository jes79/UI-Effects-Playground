# 커스텀 UI 셰이더 구조

29~46번 셰이더 연출이 공통으로 쓰는 구조입니다. 새 셰이더를 만들 때는 이 문서의 템플릿에서 **세 군데만** 바꾸면 됩니다.

[← 위키 홈](README.md)

## 한눈에 보기

```
UIEffectsCommon.cginc ──include──▶ XXX.shader (Properties + frag)
                                        │
                                        ▼
                                   XXX.mat  ──▶  Image / RawImage / TMP
                                                    │
UIShaderEffect (머티리얼 인스턴스 복제) ◀──상속── XXXEffect.cs
                                                    │
                                     material.DOFloat("_Prop", ...)  ← DOTween
```

| 구성 | 파일 | 역할 |
|---|---|---|
| 공통 include | [`UIEffectsCommon.cginc`](../UI-Effects-Playground/Assets/02_Shaders/UI/UIEffectsCommon.cginc) | UI/Default 와 같은 버텍스 처리, RectMask2D 클리핑(`UIFinalize`), 노이즈 · 색상 · uv 헬퍼 |
| 템플릿 셰이더 | [`UIBase.shader`](../UI-Effects-Playground/Assets/02_Shaders/UI/UIBase.shader) | 모든 셰이더의 뼈대 (Stencil / Mask 지원 포함) |
| 제어 스크립트 베이스 | [`UIShaderEffect.cs`](../UI-Effects-Playground/Assets/01_Scripts/Shader/UIShaderEffect.cs) | Graphic 의 머티리얼을 인스턴스로 복제해서 사용 |
| 범용 테스트 | [`UIShaderPropertyTween.cs`](../UI-Effects-Playground/Assets/01_Scripts/Shader/UIShaderPropertyTween.cs) | 프로퍼티 이름만 적으면 from ↔ to 트윈 |

## 새 셰이더 만드는 법

1. `UIBase.shader` 를 복사해서 이름 변경
2. 아래 세 군데만 교체

```c
// ① 셰이더 이름
Shader "UIEffects/MyEffect"

// ② Properties 의 [Header(...)] 부분
        [Header(My Effect)]
        _Amount ("Amount", Range(0, 1)) = 0

// ③ #include "UIEffectsCommon.cginc" 아래 (변수 + frag)
            float _Amount;

            fixed4 frag(v2f_ui i) : SV_Target
            {
                fixed4 c = SampleUI(i.texcoord) * i.color;
                // ... 효과 ...
                return UIFinalize(c, i);   // 마지막에 항상 호출 (클리핑)
            }
```

3. 제어 스크립트는 `UIShaderEffect` 를 상속하고 `material.DOFloat()` 로 트윈

```csharp
public class MyEffect : UIShaderEffect
{
    public void Play() => material.DOFloat(1f, "_Amount", 0.5f);
}
```

## 꼭 지켜야 할 것

| 규칙 | 이유 |
|---|---|
| `_Color` 프로퍼티 유지 | 없으면 기본값 0 이 들어가서 UI 가 투명해짐 |
| Stencil 블록 + 숨김 프로퍼티 유지 | `Mask` 컴포넌트 지원 |
| `UNITY_UI_CLIP_RECT` + `UIFinalize()` | `RectMask2D` 지원 |
| 머티리얼은 인스턴스로 복제해서 사용 | 같은 에셋을 쓰는 다른 UI 와 에셋 파일이 같이 바뀌는 것 방지 |
| 이미지는 Image Type = **Simple** | 대부분의 효과가 `texcoord` 를 이미지 기준 0~1 로 가정 |
| 스프라이트 Mesh Type = **Full Rect** | Tight 이면 투명 여백이 잘려 글로우/아웃라인이 그려질 공간이 사라짐 |

## 좌표계 정리

| 값 | 의미 | 쓰는 곳 |
|---|---|---|
| `i.texcoord` | 이미지 기준 uv (0~1) | 대부분의 효과 |
| `i.worldPosition` | **루트 캔버스 기준 좌표** (UI 버텍스는 배칭 시 캔버스 좌표로 변환됨) | 41 유리(배경 위치), 44 텍스트(문장 전체 그라디언트), 46 스켈레톤(여러 오브젝트에 걸친 띠) |
| `_Time.y` | 경과 시간(초) | 반복 애니메이션(맥박, 스캔라인, 물결 …) |

## 공통 include 헬퍼

| 함수 | 설명 |
|---|---|
| `SampleUI(uv)` | `_MainTex` 샘플 + `_TextureSampleAdd` |
| `InsideUV(uv)` | uv 가 0~1 안이면 1 (오프셋 샘플링 시 가장자리 늘어짐 방지) |
| `Hash21`, `ValueNoise`, `Fbm` | 랜덤 / 값 노이즈 / 프랙탈 노이즈 (노이즈 텍스처 불필요) |
| `HueToRGB(h)` | 색상환 → RGB (무지개) |
| `Gray(c)` | 밝기 |
| `AngleToDir(deg)`, `ProjectUV(uv, dir)` | 방향 벡터 / 방향 기준 0~1 위치 |
| `BlendOver(top, bottom)` | 알파 합성 (글로우·아웃라인을 원본 뒤에 깔 때) |

## 역할 분담

- **시간에 따라 계속 반복되는 것** → 셰이더가 `_Time` 으로 처리 (스크립트 불필요)
- **이벤트로 바뀌는 것** (클릭, 등장, 피격) → DOTween 이 셰이더 프로퍼티를 트윈
- **매 프레임 바뀌는 외부 값** (마우스 위치, 오브젝트 위치) → `Update`/`LateUpdate` 에서 `SetFloat` / `SetVector`

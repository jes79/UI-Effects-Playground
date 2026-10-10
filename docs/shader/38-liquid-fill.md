# 38. 액체 채움 (Liquid Fill)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `38_LiquidFill` | [`LiquidFill.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/LiquidFill.shader) | [`LiquidFillEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/LiquidFillEffect.cs) | [`e5a387b`](https://github.com/jes79/UI-Effects-Playground/commit/e5a387ba07f3121f6dfd08c36831121800a804dc) |

[← 위키 홈](../README.md)

## 무엇을 하나요
둥근 유리 용기 안에 물결치는 액체가 차오릅니다. 수위가 바뀔 때마다 물결이 크게 출렁였다가 잔잔해지고, 가운데 퍼센트 숫자도 같이 올라가요. (HP/MP 오브, 경험치 게이지 등)

## 조작
- **-10%** / **+10%** / **Random**

## 핵심 원리
- 스프라이트 없이 `length(uv - 0.5)` 로 **원형 용기를 셰이더가 직접 그림** (Image 의 Source Image 비움)
- 수면 높이 = `_Fill` + `sin(x)` 물결. 위상이 다른 물결을 **앞/뒤 두 겹**으로 그려 입체감
- `fwidth()` 로 원 테두리를 크기와 무관하게 부드럽게
- 수위(`_Fill`)와 물결 진폭(`_WaveAmp`)은 같은 머티리얼의 다른 프로퍼티라, `material.DOKill()` 대신 **트윈 변수를 따로** 관리
- `DOTween.To()` setter 안에서 셰이더 값과 퍼센트 텍스트를 함께 갱신

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Fill` | 0.5 | 수위 (스크립트가 트윈) |
| `_LiquidColor` / `_LiquidDeepColor` / `_BackWaveColor` / `_EmptyColor` | - | 수면 / 바닥 / 뒤 물결 / 빈 공간 색 |
| `_WaveAmp` / `_WaveFreq` / `_WaveSpeed` | 0.02 / 10 / 2.5 | 물결 |
| `_BorderWidth` / `_BorderColor` | 0.035 / 흰색 | 용기 테두리 |

## 스크립트 API
| 메서드 | 설명 |
|---|---|
| `SetFill(float)` | 목표 수위로 트윈 + 출렁임 |
| `AddFill(float)` | 현재 목표에서 더하기 (버튼에 ±0.1 연결) |
| `RandomFill()` | 랜덤 수위 |

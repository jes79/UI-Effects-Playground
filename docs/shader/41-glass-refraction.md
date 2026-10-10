# 41. 유리/굴절 효과 (Glass Refraction)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `41_GlassRefraction` | [`GlassRefraction.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/GlassRefraction.shader) | [`GlassPanel.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/GlassPanel.cs) | [`536d7d7`](https://github.com/jes79/UI-Effects-Playground/commit/536d7d763df6c71356d8ba76305c20eb9ebc7167) |

[← 위키 홈](../README.md)

## 무엇을 하나요
글래스모피즘 패널입니다. 패널 뒤의 배경이 흐릿하게 휘어 보이고, 가장자리는 렌즈처럼 굴절되며 테두리와 대각선 하이라이트가 빛나요. 패널을 드래그하면 새 위치 뒤의 배경이 굴절됩니다.

## 조작
- 패널 드래그

## 왜 이런 방식인가요
URP 에서는 UI 셰이더가 **GrabPass(화면 뒤 내용 가져오기)를 쓸 수 없어요.** 그래서 "뒤에 있는 배경 텍스처"와 "그 배경의 캔버스 Rect"를 셰이더에 직접 넘기고, 패널 위치에 해당하는 배경 부분을 다시 그리는 방식으로 굴절을 흉내 냅니다.

## 핵심 원리
- `i.worldPosition`(루트 캔버스 좌표)을 배경 Rect 로 나눠 **이 픽셀 뒤 배경의 uv** 계산
- 굴절: 천천히 움직이는 노이즈 + 가장자리로 갈수록 안쪽으로 휘는 렌즈 오프셋
- 블러: 주변 9칸 평균
- 패널 모양은 스프라이트 대신 **둥근 사각형 SDF** → 테두리 빛도 SDF 값으로 쉽게
- 스크립트는 `LateUpdate()` 마다 배경의 캔버스 Rect 를 `_BgRect` 로 갱신

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_BackgroundTex` | - | 뒤 배경과 **같은 텍스처** (머티리얼에 지정) |
| `_BgRect` | - | 배경의 캔버스 Rect (스크립트) |
| `_GlassTint` | 흰 푸른색, a 0.15 | 유리 색 |
| `_Distortion` / `_NoiseScale` | 0.012 / 4 | 표면 울퉁불퉁함 |
| `_EdgeBulge` | 0.03 | 가장자리 렌즈 굴절 |
| `_BlurSize` | 0.004 | 블러 |
| `_HighlightStrength` / `_BorderGlow` | 0.25 / 0.6 | 하이라이트 / 테두리 빛 |
| `_CornerRadius` / `_Aspect` | 0.12 / (스크립트) | 패널 모양 |

## 구성
```
GlassBackground   Image (배경, Stretch, 타이틀보다 뒤)
GlassPanel        Image (Source Image 없음) + GlassPanel  ← background 필드에 GlassBackground 연결
```

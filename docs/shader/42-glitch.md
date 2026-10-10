# 42. 글리치 (Glitch)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `42_Glitch` | [`Glitch.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/Glitch.shader) | [`GlitchEffect.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/GlitchEffect.cs) | [`24a2fae`](https://github.com/jes79/UI-Effects-Playground/commit/24a2fae8f4540e19fe5c452aa1f182ddb64b5536) |

[← 위키 홈](../README.md)

## 무엇을 하나요
디지털 오류처럼 가로 줄과 블록이 랜덤하게 밀리고, RGB 가 갈라지고, 일부 블록은 색이 반전됩니다. 가끔 저절로 지직거리고, 버튼으로 강하게 깨뜨릴 수 있어요.

## 조작
- **Glitch!** : 강한 글리치 버스트
- **Toggle Auto** : 랜덤 간격 자동 글리치 켜기/끄기

## 핵심 원리
- **시간 양자화** `floor(_Time.y * speed)` → 부드럽게 변하지 않고 "툭툭" 끊겨 디지털 오류처럼 보임
- 줄/블록마다 `Hash21` 랜덤으로 일부만 골라 옆으로 밀기 + [33번](33-chromatic-aberration.md) RGB 분리 + 색 반전 + 스캔라인 노이즈
- `_Intensity` 하나로 모든 효과의 강도를 함께 제어
- 자동 버스트: `DOVirtual.DelayedCall` 콜백 안에서 다시 예약 → 코루틴 없이 랜덤 간격 반복
- 흔들림 `snapping = true` 로 픽셀 단위로 딱딱 끊기게

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_Intensity` | 0.1 | 0 = 정상, 1 = 최대 (스크립트가 트윈) |
| `_BlockCount` | 24 | 가로 줄 개수 |
| `_RGBSplit` | 0.03 | 색 분리 |
| `_Speed` | 16 | 초당 변화 횟수 |

## 스크립트 필드
| 필드 | 기본값 | 설명 |
|---|---|---|
| `idleIntensity` / `burstIntensity` | 0.05 / 1 | 평소 / 버스트 강도 |
| `burstDuration` | 0.35 | 버스트 후 복귀 시간 |
| `autoBurstInterval` | (1.5, 3.5) | 자동 버스트 간격 범위 |
| `autoBurst` | true | 자동 버스트 |

# 26. 파티클 연동 (UI Particle)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `26_UIParticle` | [`UIParticleBurst.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/UIParticleBurst.cs) | [`a725527`](https://github.com/jes79/UI-Effects-Playground/commit/a72552710145b8a63b385c2232870ef585f28a85) |

[← 위키 홈](../README.md)

## 무엇을 하나요
버튼을 누르면 버튼 위치에서 파티클이 터지고 버튼이 톡 튑니다. 파티클이 **UI 위에** 그려지도록 캔버스와 렌더러를 설정하는 것이 핵심이에요.

## 설정 포인트
| 대상 | 설정 | 이유 |
|---|---|---|
| Canvas | **Screen Space - Camera**, Plane Distance 5 | Overlay 는 모든 것 위에 그려져서 파티클이 UI 뒤로 숨음 |
| Main Camera | Orthographic, Size 5.4 | |
| ParticleSystem Renderer | **Order in Layer 10** (캔버스 0 보다 높게), Material `ParticlesUnlit`(URP) | UI 위에 그리기 |
| ParticleSystem | Looping / Play On Awake 해제, Emission Rate 0, Simulation Space World | 스크립트에서 `Emit()` 으로만 방출 |

## 핵심 원리
- Camera 모드 캔버스의 UI 는 실제 월드 좌표를 가지므로 `target.position` 을 그대로 파티클 위치로 사용
- `ParticleSystem.Emit(count)` 로 원하는 개수를 즉시 방출
- 버튼은 `DOPunchScale` 로 "획득" 손맛 추가

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `burstCount` | 40 | 방출 개수 |
| `punchScale` | 0.2 | 버튼 펀치 크기 |
| `zOffset` | -1 | 캔버스보다 카메라 쪽으로 띄우는 거리 |

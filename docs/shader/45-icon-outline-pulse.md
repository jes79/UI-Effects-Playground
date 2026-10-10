# 45. 아이콘 아웃라인 펄스 (Icon Outline Pulse)

| 씬 | 셰이더 | 스크립트 | 커밋 |
|---|---|---|---|
| `45_IconOutlinePulse` | [`IconOutlinePulse.shader`](../../UI-Effects-Playground/Assets/02_Shaders/UI/IconOutlinePulse.shader) | [`IconOutlinePulse.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Shader/IconOutlinePulse.cs) | [`3861aed`](https://github.com/jes79/UI-Effects-Playground/commit/3861aeda99a21b8302bbe1d8a907b54ff824b788) |

[← 위키 홈](../README.md)

## 무엇을 하나요
아이콘을 선택하면 아이콘 모양을 따라 금색 아웃라인이 생기고, 맥박치듯 두꺼워졌다 얇아집니다. (선택된 아이템, 새 알림 아이콘 강조)

## 조작
- 아이콘(별 / 하트 / 다이아) 클릭 : 아웃라인 켜기/끄기

## 핵심 원리
- 16방향으로 `width` 만큼 떨어진 곳의 알파 **최대값** = 팽창(dilate)된 실루엣
- 팽창 실루엣 − 원본 알파 = **테두리만** 남음
- [31번 글로우](31-glow-pulse.md)는 주변 알파의 **평균**(부드럽게 번짐), 이번엔 **최대값**(딱 떨어지는 테두리) → 같은 샘플링 구조로 다른 효과
- 두께에 `sin(_Time)` 맥박을 곱해 펄스

## 셰이더 프로퍼티
| 프로퍼티 | 기본값 | 설명 |
|---|---|---|
| `_OutlineColor` | 금색 | 외곽선 색 |
| `_OutlineWidth` | 0.025 | 두께 (uv) |
| `_OutlineAlpha` | 1 | 0 = 꺼짐, 1 = 켜짐 (스크립트가 트윈) |
| `_PulseAmount` / `_PulseSpeed` | 0.8 / 5 | 두께 맥박 |

## 스크립트
| 필드/메서드 | 설명 |
|---|---|
| `startSelected` | 시작 시 선택 상태 |
| `ToggleSelect()` | 아웃라인 페이드 + 선택 시 `DOPunchScale` |

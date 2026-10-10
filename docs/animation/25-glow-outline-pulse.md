# 25. 글로우/아웃라인 펄스 (Glow / Outline Pulse)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `25_GlowOutlinePulse` | [`GlowOutlinePulse.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/GlowOutlinePulse.cs) | [`71578bf`](https://github.com/jes79/UI-Effects-Playground/commit/71578bf87012235f536b469dcee6430356f13dc4) |

[← 위키 홈](../README.md)

## 무엇을 하나요
CTA 버튼 뒤에 깔린 부드러운 글로우와 버튼의 `Outline` 두께가 함께 숨 쉬듯 커졌다 작아지며 시선을 끕니다. (셰이더 없이 DOTween 만으로 구현한 버전 — 셰이더 버전은 [31. 글로우 펄스](../shader/31-glow-pulse.md))

## 조작
- **Toggle Pulse** : 펄스 켜기/끄기

## 구성
```
Glow          가장자리가 흐릿한 둥근 사각형 스프라이트 (Sliced), 버튼보다 96 크게
PlayButton    Outline + GlowOutlinePulse  (Glow 보다 아래 순서 = 위에 그려짐)
```

## 핵심 원리
- 글로우 알파 + 글로우 크기 + Outline 두께를 **하나의 Sequence** 로 묶고 `SetLoops(-1, Yoyo)`
- `Outline.effectDistance` 는 전용 함수가 없어서 `DOTween.To()` 사용
- 글로우는 9-Slice 스프라이트라 버튼 크기가 바뀌어도 모서리가 찌그러지지 않음 (`SoftGlow.png`, 빌더가 생성)

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `duration` | 0.8 | 반 주기 시간 |
| `glowMinAlpha` / `glowMaxAlpha` | 0.25 / 0.9 | 글로우 밝기 범위 |
| `glowMaxScale` | 1.08 | 글로우 최대 크기 |
| `outlineMinDistance` / `outlineMaxDistance` | (1,-1) / (4,-4) | 외곽선 두께 범위 |

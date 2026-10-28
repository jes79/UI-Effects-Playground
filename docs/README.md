# UI Effects Playground Wiki

Unity UI 연출(애니메이션 + 셰이더)을 항목별 씬으로 하나씩 구현해 보는 실험 프로젝트의 위키입니다.

- **애니메이션 연출** — DOTween(Free)으로 만드는 움직임 기반 연출 (등장, 클릭 반응, 알림, 수치 변화, 화면 전환 …)
- **셰이더 연출** — 커스텀 UI 셰이더(HLSL)로 만드는 비주얼 이펙트 (디졸브, 빛, 왜곡, 마스크 …)

> "어떻게 움직이는가"는 DOTween, "어떻게 보이는가"는 셰이더가 담당합니다. 셰이더 값도 DOTween 으로 트윈하기 때문에 둘을 자연스럽게 섞어 쓸 수 있어요.

## 환경

| 항목 | 내용 |
|---|---|
| Unity | 6000.6.4f1 (URP) |
| 트윈 | DOTween (Free) |
| 셰이더 | HLSL (UI/Default 호환 커스텀 UI 셰이더) |
| 입력 | Input System (`InputSystemUIInputModule`) |
| 텍스트 | TextMeshPro |

## 시작하기

1. 레포를 클론하고 Unity Hub 에서 `UI-Effects-Playground` 폴더를 엽니다
2. `Assets/00_Scenes` 에서 보고 싶은 항목의 씬을 열고 Play
3. 20번 이후 씬이 깨져 보이면 메뉴 `Tools > UI Effects > Build All Demo Scenes` 로 다시 생성할 수 있어요 → [데모 씬 빌더](scene-builder.md)

## 폴더 구조

```
Assets/
├─ 00_Scenes/              항목별 데모 씬 (순서번호_PascalCase)
├─ 01_Scripts/
│  ├─ Animation/           DOTween 연출 스크립트 (07~28)
│  ├─ Shader/              셰이더 제어 스크립트 (29~46)
│  └─ Editor/              데모 씬 빌더 (에디터 전용)
├─ 02_Shaders/UI/          커스텀 UI 셰이더 + 공통 include
├─ 04_Materials/UI/        셰이더 데모용 머티리얼
├─ 05_Sprites/Generated/   코드로 생성한 샘플 이미지
└─ Plugins/Demigiant/      DOTween
```

## 가이드

- [데모 씬 빌더](scene-builder.md) — 20~46번 씬을 코드로 만드는 에디터 도구
- [커스텀 UI 셰이더 구조](shader-architecture.md) — 29~46번 셰이더 공통 구조와 새 셰이더 만드는 법
- [강의 교안 (UI 이펙트 랩)](lessons/README.md) — 40개 씬을 8차시로 묶은 수업 교안 (시연 순서 · 실습 · 과제)

## 항목 목록

### 애니메이션 (DOTween)

| # | 항목 | 씬 | 문서 |
|---|---|---|---|
| 7 | 페이드 인/아웃 | `07_FadeInOut` | [FadeInOut.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/FadeInOut.cs) |
| 8 | 스케일 팝업 | `08_ScalePopup` | [ScalePopup.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ScalePopup.cs) |
| 9 | 슬라이드 인/아웃 | `09_SlideInOut` | [SlideInOut.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/SlideInOut.cs) |
| 10 | 눌림 효과 | `10_ButtonPress` | [ButtonPressEffect.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ButtonPressEffect.cs) |
| 11 | 호버 하이라이트 | `11_ButtonHover` | [ButtonHoverHighlight.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ButtonHoverHighlight.cs) |
| 12 | 리플 효과 | `12_ButtonRipple` | [ButtonRipple.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ButtonRipple.cs) |
| 13 | 흔들림(Shake) | `13_Shake` | [ShakeEffect.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ShakeEffect.cs) |
| 14 | 토스트 팝업 | `14_ToastPopup` | [ToastPopup.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ToastPopup.cs) |
| 15 | 뱃지 카운트 | `15_BadgeCount` | [BadgeCountAnimation.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/BadgeCountAnimation.cs) |
| 16 | 진동/펄스 | `16_Pulse` | [PulseEffect.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/PulseEffect.cs) |
| 17 | 체크마크 성공 이펙트 | `17_SuccessCheck` | [SuccessCheckEffect.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/SuccessCheckEffect.cs) |
| 18 | 카운트업 | `18_CountUp` | [CountUpAnimation.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/CountUpAnimation.cs) |
| 19 | 프로그레스바 채움 | `19_ProgressBarFill` | [ProgressBarFill.cs](../UI-Effects-Playground/Assets/01_Scripts/Animation/ProgressBarFill.cs) |
| 20 | 게이지 흔들림/강조 | `20_GaugeDamageDelay` | [문서](animation/20-gauge-damage-delay.md) |
| 21 | 팝업 오픈/클로즈 | `21_PopupOpenClose` | [문서](animation/21-popup-open-close.md) |
| 22 | 아코디언 펼침 | `22_AccordionExpand` | [문서](animation/22-accordion-expand.md) |
| 23 | 페이지 스와이프 | `23_PageSwipe` | [문서](animation/23-page-swipe.md) |
| 24 | 탭 전환 인디케이터 | `24_TabIndicator` | [문서](animation/24-tab-indicator.md) |
| 25 | 글로우/아웃라인 펄스 | `25_GlowOutlinePulse` | [문서](animation/25-glow-outline-pulse.md) |
| 26 | 파티클 연동 | `26_UIParticle` | [문서](animation/26-ui-particle.md) |
| 27 | 카드 플립 | `27_CardFlip` | [문서](animation/27-card-flip.md) |
| 28 | 스포트라이트(튜토리얼) | `28_TutorialSpotlight` | [문서](animation/28-tutorial-spotlight.md) |

### 셰이더

| # | 항목 | 씬 | 문서 |
|---|---|---|---|
| 29 | 커스텀 UI 셰이더 베이스 | `29_ShaderBase` | [문서](shader/29-shader-base.md) |
| 30 | 셔이머(Shimmer/Shine) | `30_Shimmer` | [문서](shader/30-shimmer.md) |
| 31 | 글로우 펄스 | `31_GlowPulseShader` | [문서](shader/31-glow-pulse.md) |
| 32 | 무지개/홀로그램 그라디언트 | `32_HolographicGradient` | [문서](shader/32-holographic-gradient.md) |
| 33 | 크로매틱 애버레이션 | `33_ChromaticAberration` | [문서](shader/33-chromatic-aberration.md) |
| 34 | 디졸브(Dissolve) | `34_Dissolve` | [문서](shader/34-dissolve.md) |
| 35 | 픽셀화 전환 | `35_PixelateTransition` | [문서](shader/35-pixelate-transition.md) |
| 36 | 홀로그램 스캔라인 | `36_HologramScanline` | [문서](shader/36-hologram-scanline.md) |
| 37 | 언락(Unlock) 리빌 | `37_UnlockReveal` | [문서](shader/37-unlock-reveal.md) |
| 38 | 액체 채움 | `38_LiquidFill` | [문서](shader/38-liquid-fill.md) |
| 39 | 그라디언트 마스크 페이드 | `39_GradientMaskFade` | [문서](shader/39-gradient-mask-fade.md) |
| 40 | 물결(Wave) 왜곡 | `40_WaveDistortion` | [문서](shader/40-wave-distortion.md) |
| 41 | 유리/굴절 효과 | `41_GlassRefraction` | [문서](shader/41-glass-refraction.md) |
| 42 | 글리치(Glitch) | `42_Glitch` | [문서](shader/42-glitch.md) |
| 43 | 히트 웨이브 | `43_HeatWave` | [문서](shader/43-heat-wave.md) |
| 44 | 텍스트 그라디언트 애니메이션 | `44_TextGradientShine` | [문서](shader/44-text-gradient-shine.md) |
| 45 | 아웃라인 펄스 | `45_IconOutlinePulse` | [문서](shader/45-icon-outline-pulse.md) |
| 46 | 스켈레톤 로딩(Shimmer) | `46_SkeletonLoading` | [문서](shader/46-skeleton-loading.md) |

## 자주 쓰는 DOTween 패턴

| 패턴 | 예시 | 쓰인 곳 |
|---|---|---|
| 진행 중 트윈 정리 후 새로 시작 | `target.DOKill();` | 거의 모든 항목 |
| 여러 트윈 동시 재생 | `Sequence.Append(...).Join(...)` | 21, 25, 27 |
| 특정 시점에 끼워 넣기 | `Sequence.Insert(time, tween)` | 24 |
| 중간에 코드 실행 | `Sequence.AppendCallback(...)` | 27, 35 |
| 확장 함수 없는 값 트윈 | `DOTween.To(getter, setter, end, t)` | 22, 25, 38 |
| 셰이더 값 트윈 | `material.DOFloat(end, "_Prop", t)` | 29~46 |
| 무한 왕복 | `.SetLoops(-1, LoopType.Yoyo)` | 25, 28, 29 |
| 타이머 | `DOVirtual.DelayedCall(t, cb)` | 42 |
| 시간차 등장 | `.SetDelay(i * stagger)` | 46 |

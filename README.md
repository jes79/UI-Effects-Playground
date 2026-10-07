# UI-Effects-Playground

Unity UI 연출(애니메이션 + 셰이더) 실험 프로젝트

## 사용 기술
- Unity 6000.3.9f1 (URP)
- DOTween (Free)
- Shader Graph / HLSL

## 구성
항목별로 개별 씬(Scene)에서 연출을 구현합니다.
씬 이름 규칙: `순서번호_PascalCase` (예: `07_FadeInOut`)

## 진행 목록
| 순서 | 항목 | 씬 |
|---|---|---|
| 7 | 페이드 인/아웃 | 07_FadeInOut |
| 8 | 스케일 팝업 | 08_ScalePopup |
| 9 | 슬라이드 인/아웃 | 09_SlideInOut |
| 10 | 눌림 효과 | 10_ButtonPress |
| 11 | 호버 하이라이트 | 11_ButtonHover |
| 12 | 리플 효과 | 12_ButtonRipple |
| 13 | 흔들림(Shake) | 13_Shake |
| 14 | 토스트 팝업 | 14_ToastPopup |
| 15 | 뱃지 카운트 애니메이션 | 15_BadgeCount |
| 16 | 진동/펄스 | 16_Pulse |
| 17 | 체크마크 성공 이펙트 | 17_SuccessCheck |
| 18 | 카운트업 애니메이션 | 18_CountUp |
| 19 | 프로그레스바 채움 | 19_ProgressBarFill |
| 20 | 게이지 흔들림/강조 | 20_GaugeDamageDelay |
| 21 | 팝업 오픈/클로즈 | 21_PopupOpenClose |
| 22 | 아코디언 펼침 | 22_AccordionExpand |
| 23 | 페이지 스와이프 | 23_PageSwipe |
| 24 | 탭 전환 인디케이터 | 24_TabIndicator |
| 25 | 글로우/아웃라인 펄스 | 25_GlowOutlinePulse |
| 26 | 파티클 연동 | 26_UIParticle |
| 27 | 카드 플립 | 27_CardFlip |
| 28 | 스포트라이트(튜토리얼) | 28_TutorialSpotlight |
| 29 | 커스텀 UI 셰이더 베이스 | 29_ShaderBase |
| 30 | 셔이머(Shimmer/Shine) | 30_Shimmer |
| 31 | 글로우 펄스 | 31_GlowPulseShader |
| 32 | 무지개/홀로그램 그라디언트 | 32_HolographicGradient |
| 33 | 크로매틱 애버레이션 | 33_ChromaticAberration |
| 34 | 디졸브(Dissolve) | 34_Dissolve |
| 35 | 픽셀화 전환 | 35_PixelateTransition |
| 36 | 홀로그램 스캔라인 | 36_HologramScanline |
| 37 | 언락(Unlock) 리빌 | 37_UnlockReveal |
| 38 | 액체 채움 | 38_LiquidFill |
| 39 | 그라디언트 마스크 페이드 | 39_GradientMaskFade |
| 40 | 물결(Wave) 왜곡 | 40_WaveDistortion |
| 41 | 유리/굴절 효과 | 41_GlassRefraction |
| 42 | 글리치(Glitch) | 42_Glitch |
| 43 | 히트 웨이브 | 43_HeatWave |
| 44 | 텍스트 그라디언트 애니메이션 | 44_TextGradientShine |
| 45 | 아웃라인 펄스 | 45_IconOutlinePulse |
| 46 | 스켈레톤 로딩(Shimmer) | 46_SkeletonLoading |

## 데모 씬 다시 만들기 (20번 이후)
20번 이후 씬은 에디터 스크립트(`Assets/01_Scripts/Editor/Scenes/BuildXX_*.cs`)로 구성됩니다.
씬이 깨졌거나 처음부터 다시 만들고 싶으면 Unity 메뉴에서 실행하세요.
- `Tools > UI Effects > Build All Demo Scenes` : 20~46번 씬 전체 재생성
- `Tools > UI Effects > Build Scene > XX_Name` : 해당 씬만 재생성

셰이더 연출(29번 이후)은 `Assets/02_Shaders/UI` 의 커스텀 UI 셰이더 + `Assets/01_Scripts/Shader` 의 제어 스크립트(DOTween 으로 머티리얼 프로퍼티 트윈) 구조입니다.
데모용 이미지는 `Assets/05_Sprites/Generated` 에 코드로 생성됩니다.

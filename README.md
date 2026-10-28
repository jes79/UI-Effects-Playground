# UI-Effects-Playground

Unity UI 연출(애니메이션 + 셰이더) 실험 프로젝트

📖 **[GitHub Wiki](https://github.com/jes79/UI-Effects-Playground/wiki)** — 항목별 원리 · 구성 · 파라미터 정리 ([docs/ 원본](docs/README.md))

## 사용 기술
- Unity 6000.6.4f1 (URP)
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

## 강의 모드

`Assets/Lecture/LectureHub.unity`(Build Settings 맨 앞 씬)를 열고 플레이하면 허브가 뜹니다. 인자 없이 켜면 **자유 모드**로, 씬 목록(07~46)에서 데모를 골라 봅니다. 데모 화면 왼쪽 위 **◀ 목록** 버튼이나 **Backspace**로 씬 목록에 돌아옵니다.
**F4**를 누르면 강의 용어 목록이 열리고, 용어를 누르면 개발실 팀원이 단계별(분야 소개 → ① 소개 → ② 개념 → ③ 직접 보기 → ④ AI에게 요청)로 설명합니다.
용어로 들어갈 때마다 그 용어의 데모 씬이 처음 상태로 새로 열립니다. 강의 화면은 씬이 바뀌어도 남고, 말풍선·카드·버튼 밖에서는 데모 버튼을 그대로 누를 수 있습니다.

| 키 | 기능 |
|---|---|
| PgDn / ] · PgUp / [ | 다음 · 이전 단계 |
| End · Home | 다음 · 이전 용어 |
| F3 | 말풍선 숨기기·보이기 |
| Backspace | 용어 목록 (강의 중이 아닐 때는 씬 목록) |
| F4 | 강의 모드 끄기·켜기 |
| Esc | 강의 프로그램 끝내기 (강의 중이 아닐 때) |

- 용어 데이터: `Assets/Lecture/Resources/UIEffectsGlossary.json` (OMP Studio 앱의 강의 준비 창도 같은 파일을 읽음). 용어별로 열 씬은 각 용어의 `lab.scene` 칸
- 강의 프로그램: **Tools → UI Effects → Build Lecture Player** → `Builds/LecturePlayer/UIEffectsLab.exe` (git 루트 기준, git에 올리지 않으므로 PC마다 한 번 빌드). Unity 6000.6.4f1
  - `UIEffectsLab.exe -glossaryStart 0304 -glossaryLecture` : 그 용어부터 강의 · `-glossaryStart 0304` : 그 용어의 데모 씬만 · 인자 없음 : 자유 모드(씬 목록)
- 허브 씬이 깨졌으면 **Tools → UI Effects → Build Lecture Hub** 로 다시 만듭니다 (Build Settings 씬 목록도 허브 + 07~46으로 맞춰 줌)

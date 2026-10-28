# Unity UI 이펙트 랩 — 강의 교안

> 강사: 동해님 · 프로젝트: `UI-Effects-Playground` · 브랜치: `feature/lecture-lab` · 장면: `Assets/00_Scenes/07_FadeInOut.unity` ~ `46_SkeletonLoading.unity` (40개)

이 폴더는 **UI 이펙트 랩** 으로 Unity UI 연출을 배우는 8차시 교안입니다.
한 차시는 용어집의 분류 하나와 같습니다. 용어 하나 = 데모 씬 하나입니다.
학생은 씬에서 버튼을 눌러 보며 눈으로 먼저 익히고, 그다음 DOTween C# 코드와 UI 셰이더를 조금씩 고쳐 봅니다.

> **DOTween**: 값(위치·크기·색·숫자)을 정한 시간 동안 부드럽게 바꿔 주는 무료 플러그인. 이 프로젝트는 `Assets/Plugins/Demigiant/` 에 들어 있습니다.
> **UI 셰이더**: UI 이미지의 픽셀 색을 GPU 에서 계산하는 작은 프로그램. 이 프로젝트는 `Assets/02_Shaders/UI/` 에 있습니다.

---

## 1. 과정 개요

| 항목 | 내용 |
|---|---|
| 대상 | Unity 기초(씬·게임 오브젝트·인스펙터·C# 스크립트 붙이기)를 아는 게임 개발 과정 수강생. DOTween·셰이더는 처음이어도 된다 |
| 목표 | ① UI 연출을 "무엇이(대상) · 얼마 동안(시간) · 어떤 느낌으로(Ease)" 로 나눠 말한다 ② DOTween 으로 등장·반응·피드백·전환 연출을 직접 만든다 ③ UI 셰이더의 뼈대를 읽고, 프로퍼티 하나를 더해 DOTween 으로 움직인다 |
| 분량 | 8차시 × 3시간 (180분) = 24시간 |
| 진행 방식 | 매 차시 **시연 → 개념 설명 → 따라 하기 실습 → 정리 → 과제**. 실습은 학생 각자 자기 브랜치에서 한다 |
| 평가 | 차시 과제 8개(각 채점 기준 표 있음) + 확인 질문 |

### 준비물

| 항목 | 버전·내용 | 비고 |
|---|---|---|
| Unity | **6000.6.4f1** (Unity 6) | 프로젝트는 원래 6000.3.9f1 로 만들었고, `feature/lecture-lab` 에서 6000.6.4f1 로 올렸다 (`ProjectSettings/ProjectVersion.txt`). `main` 을 열면 버전을 올린다는 창이 뜬다 |
| 렌더 파이프라인 | URP 17.6 | `Packages/manifest.json` 에 들어 있음 |
| 트윈 | DOTween (Free) | `Assets/Plugins/Demigiant/` 에 들어 있음. 따로 설치하지 않는다 |
| 입력 | Input System 패키지 (`InputSystemUIInputModule`) | 프로젝트에 들어 있음 |
| 텍스트 | TextMeshPro | 데모 글자는 영어. 기본 TMP 글꼴(LiberationSans)에 한글이 없기 때문 |
| Git | Git 2.23 이상 (`git switch` 명령) | Git LFS 는 필요 없음 |
| 코드 편집기 | Visual Studio / Rider / VS Code | `.shader` 색 표시가 되면 편함 |
| 강의 플레이어 (강사만) | `Builds/LecturePlayer/UIEffectsLab.exe` | git 에 올라가지 않음(`.gitignore`). 강사 PC 에서 빌드해 둔다 |

### 진행 방식 (한 차시 180분)

| 구간 | 시간 | 하는 일 |
|---|---|---|
| 도입·지난 차시 확인 | 10분 | 지난 과제 1~2개 같이 보기, 오늘 연습 브랜치 만들기 |
| 시연 | 25분 | 강사가 강의 플레이어(또는 Unity)에서 누르고 보여 준다 (학생은 보기만) |
| 개념 설명 | 45분 | 쉬운 말 + 코드 몇 줄 |
| 쉬는 시간 | 10분 | |
| 실습 | 70분 | 학생이 단계별로 따라 한다. 단계마다 확인 포인트 |
| 정리·확인 질문·과제 안내 | 20분 | |

---

## 2. 차시표

| 차시 | 파일 | 분류 (발표) | 용어 id · 씬 | 핵심 개념 | 과제 |
|---|---|---|---|---|---|
| 1 | [01_등장_퇴장.md](01_등장_퇴장.md) | 등장·퇴장 (dev) | 0101 `07` · 0102 `08` · 0103 `09` · 0104 `14` · 0105 `21` | 트윈, Ease, `DOKill`, CanvasGroup, Sequence, `OnComplete` | 내 패널 등장·퇴장 3종 |
| 2 | [02_버튼_반응.md](02_버튼_반응.md) | 버튼 반응 (dev) | 0201 `10` · 0202 `11` · 0203 `12` · 0204 `13` · 0205 `16` | 포인터 이벤트 인터페이스, 원래 크기 기억, 좌표 변환, Shake, 무한 Yoyo | 손맛 버튼 1개 (눌림+리플+호버) |
| 3 | [03_수치_피드백.md](03_수치_피드백.md) | 수치·피드백 (dev) | 0301 `15` · 0302 `17` · 0303 `18` · 0304 `19` · 0305 `20` | Punch, `Insert`, `DOTween.To`, Slider·Filled 이미지, `SetDelay` 잔상 | 보상 결과 화면 연출 |
| 4 | [04_화면구성_전환.md](04_화면구성_전환.md) | 화면 구성·전환 (dev) | 0401 `22` · 0402 `23` · 0403 `24` · 0404 `27` · 0405 `28` | Layout Group + 높이 트윈, 드래그 이벤트, 젤리 밑줄, 90° 나눠 돌리기, Anchor 구멍 | 튜토리얼 단계 추가 + 카드 한꺼번에 뒤집기 |
| 5 | [05_강조_파티클.md](05_강조_파티클.md) | 강조·파티클 (dev) | 0501 `25` · 0502 `26` | Sequence 반복, `Outline`, 9-Slice, Screen Space - Camera, Order in Layer, `Emit` | 코인 획득 버튼 |
| 6 | [06_UI셰이더기초_빛.md](06_UI셰이더기초_빛.md) | UI 셰이더 기초·빛 (art) | 0601 `29` · 0602 `30` · 0603 `31` · 0604 `32` · 0605 `44` · 0606 `45` · 0607 `46` | 셰이더 뼈대, 머티리얼 인스턴스, uv, `_Time`, 평균 vs 최대 샘플링, SDF, 캔버스 좌표 | 새 셰이더 `FlashTint` + 제어 스크립트 |
| 7 | [07_셰이더_등장_전환.md](07_셰이더_등장_전환.md) | 셰이더 등장·전환 (art) | 0701 `34` · 0702 `35` · 0703 `37` · 0704 `38` · 0705 `39` | 임계값, 노이즈, 셀 스냅, 원형 리빌, 사인 물결, 방향 마스크 | 디졸브로 이미지 바꾸기 |
| 8 | [08_왜곡_노이즈.md](08_왜곡_노이즈.md) | 왜곡·노이즈 (art) | 0801 `33` · 0802 `36` · 0803 `40` · 0804 `41` · 0805 `42` · 0806 `43` | uv 밀기, RGB 분리, 스캔라인, 리플, 배경 텍스처 굴절, 시간 양자화, Fbm 아지랑이 | 왜곡 효과 하나 고쳐 만들기 |

씬 칸의 `07` 은 `Assets/00_Scenes/07_FadeInOut.unity` 처럼 씬 번호입니다.
"발표"는 강의 플레이어의 발표자입니다 (dev = 개발, art = 아트).

### 왜 이 순서인가

- **1~5차시는 DOTween 만**, **6~8차시는 셰이더 + DOTween** 입니다. 6차시부터는 "셰이더 값도 DOTween 으로 트윈한다"(`docs/README.md`)는 점을 이어서 씁니다.
- 1차시에서 배운 `DOKill`·Ease·Sequence 를 2~5차시가 그대로 씁니다. 그래서 1차시가 가장 중요합니다.
- 5차시의 글로우(25번, 이미지를 따로 깐 방식)와 6차시의 글로우(31번, 셰이더가 모양을 따라 그림)를 일부러 붙여 놓았습니다. 같은 효과를 두 방법으로 비교합니다.
- 7차시(등장·전환)는 1차시(등장·퇴장)의 셰이더 판입니다. 8차시는 "색을 바꾸지 않고 uv 만 밀어서 그림을 휘게 하는" 왜곡을 모아 마무리합니다.
- 씬 번호 순서가 아니라 **분류 순서** 입니다. 예: 1차시에 14번·21번이 같이 들어갑니다.

---

## 3. 저장 지점 받는 법

### 저장 지점은 브랜치 하나

| 저장 지점 | 종류 | 내용 |
|---|---|---|
| `feature/lecture-lab` | 브랜치 | 데모 씬 40개 + 효과 스크립트·셰이더 + 효과 설명 문서(`docs/`) + 강의 모드 + 이 교안 |

> **이 프로젝트에는 태그가 없습니다.** 쉐이더 랩의 `shader-lab-v1` 같은 고정 저장 지점이 없습니다. 8차시 모두 같은 브랜치 `feature/lecture-lab` 에서 시작합니다.
> **브랜치(branch)**: 커밋이 이어지는 줄기. 새 커밋이 생기면 앞으로 움직입니다.

### 처음 한 번 (복제)

```bash
git clone https://github.com/jes79/UI-Effects-Playground.git
cd UI-Effects-Playground
git fetch origin                 # 원격 브랜치 받기
git branch -r                    # origin/feature/lecture-lab 이 보이면 OK
```

### 차시를 시작할 때 (내 연습 브랜치 만들기)

**Unity를 먼저 닫고** 아래를 실행합니다.

```bash
git status                                           # 바뀐 파일이 있으면 먼저 commit 하거나 git stash
git fetch origin
git switch -c lesson02 origin/feature/lecture-lab    # 강의 브랜치에서 'lesson02' 를 새로 만들어 이동
```

- 차시마다 `lesson01`, `lesson02` … 처럼 새 브랜치를 만듭니다. 지난 차시에 고친 내용은 그 브랜치에 그대로 남습니다.
- 새 브랜치는 늘 강의 브랜치에서 만드므로, 지난 차시에 고친 코드는 들어 있지 않습니다.
- 실습이 끝나면 `git add -A` → `git commit -m "2차시 실습"` 으로 저장합니다.

### 다른 방법

| 하고 싶은 일 | 명령 |
|---|---|
| 원래 공개된 예제(`main`)만 보기 | `git switch main` (강의 모드·교안은 없음) |
| 예전 Git 명령으로 브랜치 만들기 | `git checkout -b lesson05 origin/feature/lecture-lab` |
| 강의 브랜치와 내 브랜치 차이 보기 | `git diff origin/feature/lecture-lab --stat` |
| 씬 하나를 처음 상태로 | `git restore UI-Effects-Playground/Assets/00_Scenes/13_Shake.unity` |

### 받은 뒤 확인

1. Unity Hub → **Add** → 저장소 안의 `UI-Effects-Playground` 폴더(한 단계 안쪽)를 연다. 처음은 몇 분 걸린다.
   - 저장소 맨 위 폴더를 열면 Unity 프로젝트가 아니라서 열리지 않는다.
2. Project 창 `Assets/00_Scenes/07_FadeInOut.unity` 를 열고 ▶ 플레이.
3. **FadeInButton** 을 눌러 패널이 서서히 나타나면 성공.

---

## 4. 강의 플레이어로 시연하기 (강사)

강의 플레이어는 쉐이더 랩과 같은 강의 모드입니다. 용어를 열면 그 용어의 씬을 불러오고, 강의 화면(오버레이)은 씬이 바뀌어도 남습니다.

| 실행 | 하는 일 |
|---|---|
| `Builds/LecturePlayer/UIEffectsLab.exe -glossaryStart 0101 -glossaryLecture` | 용어 `0101` 부터 강의 모드로 시작 (씬 `07_FadeInOut`) |
| `Builds/LecturePlayer/UIEffectsLab.exe` | 인자 없이: 자유 모드. 씬 목록에서 골라 보기 |
| Unity 에서 `Assets/00_Scenes/NN_….unity` 를 열고 ▶ | 강의 화면 없이 씬만 (학생 실습은 이것) |

**단계**: Opening → SelfIntro → ① Intro(소개) → ② Concept(개념) → ③ Demo(시연) → ④ Prompt(AI 요청)

| 키 | 하는 일 |
|---|---|
| `PgDn` / `]` | 다음 단계 |
| `PgUp` / `[` | 이전 단계 |
| `End` / `Home` | 다음 용어 / 이전 용어 |
| `Backspace` | 용어 목록 |
| `F3` | 말풍선 숨기기/보이기 |
| `F4` | 강의 모드 켜기/끄기 |
| `Esc` | 강의 플레이어 종료 (강의 진행 중이 아닐 때) |

- ③ 시연 단계에서는 마우스로 씬의 버튼을 직접 누릅니다. 키보드는 단계 넘기기에만 씁니다.
- 각 차시 교안의 **시연 순서** 는 용어 id 순서 그대로입니다. 차시 첫 용어만 명령으로 열고, 나머지는 `End` 로 넘어갑니다.

---

## 5. 강사 준비 체크

| 확인 | 방법 |
|---|---|
| 강의 브랜치가 원격 저장소에 올라가 있는가 | `git ls-remote --heads origin feature/lecture-lab` 에 한 줄이 보여야 함 |
| 강의 플레이어가 있는가 | 저장소 맨 위 `Builds/LecturePlayer/UIEffectsLab.exe` (git 에 없으므로 강사 PC 에서 빌드) |
| 20~46번 씬이 깨졌을 때 | 메뉴 **Tools → UI Effects → Build All Demo Scenes** (전체) 또는 **Tools → UI Effects → Build Scene → NN_Name** (하나) |
| 07~19번 씬이 깨졌을 때 | 빌더 메뉴가 없다. `git restore UI-Effects-Playground/Assets/00_Scenes/NN_Name.unity` |
| 학생 PC 의 Unity 버전 | Unity Hub 에 6000.6.4f1 이 설치되어 있는지 |

> **주의:** Build Scene 메뉴는 씬을 **새로 만들어 덮어씁니다**(`docs/scene-builder.md`). 학생이 20~46번 씬에 직접 넣은 버튼·값은 사라집니다. 오래 남길 수정은 `Assets/01_Scripts/Editor/Scenes/BuildNN_*.cs` 에 코드로 넣습니다 (3·4·7·8차시 실습에서 연습).
> 머티리얼 에셋(`Assets/04_Materials/UI/*.mat`)은 지우지 않고 다시 씁니다. 다만 빌더가 정해 둔 값(예: Shimmer 의 `_ShineWidth` 0.08)은 다시 들어갑니다.

---

## 6. 차시 파일 구성

모든 차시 파일은 같은 순서입니다.

1. 맨 위 표: 수업 시간 · 난이도 · 시작 저장 지점 · 쓰는 씬(용어 id)
2. 학습 목표 (3줄)
3. 시작 저장 지점 (명령)
4. 시연 순서 — 용어 id 순서. 강사가 무엇을 누르고 무엇을 보여 줄지
5. 개념 설명 — 쉬운 말 + 코드 몇 줄과 줄 위치
6. 실습 — 학생이 따라 하는 단계 + 확인 포인트
7. 정리
8. 과제 — 제출물·채점 기준 표
9. 확인 질문 3개 (정답 포함)
10. 자주 막히는 곳

**줄 번호 안내**: 교안의 줄 번호는 `feature/lecture-lab` 기준입니다. 앞 실습에서 줄을 더했다면 번호가 밀리므로, 함께 적어 둔 **찾을 글자(Ctrl+F)** 로 찾습니다.

**효과 설명 문서**: 20~46번은 `docs/animation/NN-*.md`, `docs/shader/NN-*.md` 에 "조작 · 구성 · 핵심 원리 · 주요 파라미터" 가 있습니다. 교안의 기본값 숫자는 이 문서와 스크립트에서 가져왔습니다. 07~19번은 문서가 없어서 스크립트(`Assets/01_Scripts/Animation/*.cs`)와 씬에서 가져왔습니다.

## 7. 그림 자료

저장소에는 아직 스크린샷 폴더가 없습니다. 그림이 필요한 자리는 모두 `[그림: …]` 으로 표시했습니다. 강사가 수업 전에 씬을 캡처해 넣습니다.
움직이는 연출이 많으므로 정지 그림보다 짧은 GIF·영상이 좋습니다. 큰 파일은 Google Drive 에 둡니다.

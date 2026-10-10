# 21. 팝업 오픈/클로즈 (Popup Open / Close)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `21_PopupOpenClose` | [`PopupOpenClose.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/PopupOpenClose.cs) | [`b1d3ad8`](https://github.com/jes79/UI-Effects-Playground/commit/b1d3ad8874548bfa2b3234c63c18c771bc6c59af) |

[← 위키 홈](../README.md)

## 무엇을 하나요
뒤 배경을 어둡게 가리는 **딤**이 페이드인되면서 패널이 `OutBack` 으로 톡 튀어나오고, 닫을 때는 `InBack` 으로 살짝 빨려 들어가며 사라집니다.

## 조작
- **Open Popup** : 열기
- **Close** 버튼 또는 패널 바깥(딤) 클릭 : 닫기

## 구성
```
OpenButton
Popup                 (닫히면 비활성화)
├─ Dim                Image + CanvasGroup + Button(바깥 클릭 닫기)
└─ Panel              Image + CanvasGroup
   ├─ Title / Body
   └─ CloseButton
PopupController       (PopupOpenClose)  ← Popup 에 붙이면 Awake 에서 자기 자신을 꺼버림
```

## 핵심 원리
- 열기 `Ease.OutBack` ↔ 닫기 `Ease.InBack` 으로 짝을 맞추면 자연스러움
- 딤과 패널을 `Sequence.Join()` 으로 동시에 재생, 닫기가 **끝난 뒤** `OnComplete` 에서 비활성화
- 닫히는 동안 `panelGroup.interactable = false` 로 중복 클릭 방지

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `openDuration` | 0.35 | 열리는 시간 |
| `closeDuration` | 0.2 | 닫히는 시간 |
| `startScale` | 0.7 | 시작/종료 크기 |

## 관련 항목
[8. 스케일 팝업](../../UI-Effects-Playground/Assets/01_Scripts/Animation/ScalePopup.cs) · [7. 페이드 인/아웃](../../UI-Effects-Playground/Assets/01_Scripts/Animation/FadeInOut.cs)

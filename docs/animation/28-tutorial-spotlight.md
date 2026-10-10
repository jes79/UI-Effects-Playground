# 28. 스포트라이트 (Tutorial Spotlight)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `28_TutorialSpotlight` | [`TutorialSpotlight.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/TutorialSpotlight.cs) | [`8113ada`](https://github.com/jes79/UI-Effects-Playground/commit/8113ada2416ca84b4d061d99c91f1eda3708ed9c) |

[← 위키 홈](../README.md)

## 무엇을 하나요
화면을 어둡게 덮고 강조할 UI 만 **구멍처럼 밝게** 보여주는 튜토리얼 연출입니다. 화면을 클릭할 때마다 구멍이 다음 UI 로 이동하며 크기를 맞추고, 아래에 안내 문구가 따라다녀요.

## 조작
- **Start Tutorial** : 시작
- 화면 아무 곳 클릭 : 다음 단계 (마지막 이후 종료)

## 구성
```
TutorialOverlay       투명 Image + CanvasGroup + Button(다음 단계) + TutorialSpotlight
└─ Spotlight          가운데가 뚫린 원형 텍스처 (검정 alpha 0.78)
   ├─ Top / Bottom    어두운 사각형 (Anchor 로 구멍 위/아래에 붙음)
   ├─ Left / Right    어두운 사각형 (Anchor 로 구멍 좌/우에 붙음, 높이는 구멍과 같이 늘어남)
   └─ MessageBox      안내 문구 (구멍 아래)
```

## 핵심 원리
- 셰이더 없이 구멍을 만드는 방법: 구멍 이미지만 움직이면 Anchor 로 붙은 자식 사각형 4개가 같이 따라오고 늘어남
- `DOMove`(월드 위치) + `DOSizeDelta`(크기)를 동시에 트윈해서 다음 타겟으로 이동
- 시작 시 화면보다 큰 구멍에서 첫 타겟으로 줄어들며 시선 집중, 도착 후 `Yoyo` 루프로 숨 쉬듯 강조
- 꺼져 있을 땐 `blocksRaycasts = false` 로 아래 UI 클릭 허용

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `targets` / `messages` | - | 단계별 강조 UI / 안내 문구 |
| `padding` | 40 | 타겟 주변 여백 |
| `moveDuration` | 0.5 | 이동 시간 |
| `startSize` | 2600 | 시작 구멍 크기 |

# 24. 탭 전환 인디케이터 (Tab Indicator)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `24_TabIndicator` | [`TabIndicator.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/TabIndicator.cs) | [`6e37b4b`](https://github.com/jes79/UI-Effects-Playground/commit/6e37b4b7603854179db9ceac30a615dae3f33b53) |

[← 위키 홈](../README.md)

## 무엇을 하나요
탭을 누르면 밑줄이 **늘어나며 이동했다가** 도착하면서 원래 폭으로 돌아오는 젤리 느낌으로 움직이고, 탭 내용은 이동 방향 쪽에서 슬라이드되어 들어옵니다.

## 구성
```
TabBar               Image + TabIndicator
├─ Tab_Home ~ Profile  투명 Image + Button + Label
└─ Indicator         밑줄 (탭들과 같은 부모 → 탭의 anchoredPosition.x 를 그대로 목표로 사용)
Pages
└─ Page_Home ~ Profile CanvasGroup
```

## 핵심 원리
- 밑줄은 위치(`DOAnchorPos`)와 폭(`DOSizeDelta`)을 함께 트윈
- 초반엔 폭을 늘리고, `Sequence.Insert(duration * 0.4f, ...)` 로 후반에 원래 폭으로 → 젤리 효과
- 버튼 이벤트는 `SelectTab(int)` 에 인덱스를 넘겨 연결
- 내용 페이지는 이전 탭보다 오른쪽이면 오른쪽에서, 왼쪽이면 왼쪽에서 들어옴

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `duration` | 0.35 | 이동 시간 |
| `widthRatio` | 0.6 | 탭 폭 대비 밑줄 폭 |
| `stretchRatio` | 1.8 | 이동 중 늘어나는 정도 |
| `pageSlideDistance` | 60 | 내용 슬라이드 거리 |

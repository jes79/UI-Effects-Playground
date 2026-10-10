# 23. 페이지 스와이프 (Page Swipe)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `23_PageSwipe` | [`PageSwipe.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/PageSwipe.cs) | [`d6bdfab`](https://github.com/jes79/UI-Effects-Playground/commit/d6bdfabf528d0bbbc9ce9857cb6c0a3a54e8c26f) |

[← 위키 홈](../README.md)

## 무엇을 하나요
페이지를 손가락(마우스)으로 끌면 그대로 따라오고, 놓으면 가장 가까운 페이지로 **스냅**합니다. 짧게 휙 튕기는 플릭도 인식하고, 첫/마지막 페이지 바깥으로 끌면 저항감이 생겨요.

## 조작
- 좌우 드래그 / 플릭
- `<` `>` 버튼

## 구성
```
Viewport            Image(거의 투명, 드래그 수신) + RectMask2D + PageSwipe
└─ Content          Anchor/Pivot 왼쪽 가운데 (0, 0.5)
   └─ Page1~4       x = i * pageWidth + pageWidth / 2
Dot1~4              페이지 인디케이터
```

## 핵심 원리
- `IBeginDragHandler / IDragHandler / IEndDragHandler` 로 드래그 처리
- 드래그 중엔 `content.anchoredPosition` 을 직접 이동, 놓으면 `DOAnchorPosX` + `Ease.OutCubic` 스냅
- **거리 또는 속도** 중 하나만 넘어도 페이지 전환 → 플릭 인식
- `eventData.delta` 는 화면 픽셀이라 `canvas.scaleFactor` 로 나눠서 캔버스 단위로 맞춤

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `pageWidth` | 1000 | 페이지 폭 |
| `snapDuration` | 0.35 | 스냅 시간 |
| `swipeThreshold` | 0.2 | 페이지 폭 대비 넘김 거리 비율 |
| `flickSpeed` | 1500 | 플릭 판정 속도(캔버스 단위/초) |
| `edgeResistance` | 0.35 | 양 끝 저항 |

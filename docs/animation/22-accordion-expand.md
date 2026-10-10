# 22. 아코디언 펼침 (Accordion Expand)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `22_AccordionExpand` | [`AccordionItem.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/AccordionItem.cs) | [`40ddbdc`](https://github.com/jes79/UI-Effects-Playground/commit/40ddbdcfe2625fb2e0c10462d99a0cb0e1e27521) |

[← 위키 홈](../README.md)

## 무엇을 하나요
헤더를 클릭하면 본문이 펼쳐지면서 **아래 항목들이 자연스럽게 밀려납니다**. 다른 항목을 펼치면 열려 있던 항목은 접히고, `+` 아이콘이 45도 돌아 `×` 가 돼요.

## 구성
```
AccordionList        VerticalLayoutGroup + ContentSizeFitter(Vertical: Preferred)
└─ Item1~4           Image + VerticalLayoutGroup + AccordionItem
   ├─ Header         Button + LayoutElement(Preferred Height 90)
   │  ├─ Title
   │  └─ Icon ("+")
   └─ Content        LayoutElement(Preferred Height 0) + RectMask2D + CanvasGroup
      └─ Body
```

## 핵심 원리
- `DOTween.To()` 로 **`LayoutElement.preferredHeight`** 를 트윈 → Layout Group 이 매 프레임 다시 배치해서 아래 항목이 밀려남 (위치 계산 불필요)
- 본문은 `RectMask2D` 로 잘라내서 높이가 변하는 도중 글자가 삐져나오지 않음
- `closeOthers` 가 켜져 있으면 같은 부모의 다른 항목을 `Collapse()`

> ⚠️ `AccordionList` 에 `ContentSizeFitter` 가 없으면 리스트 높이가 0 이라서 Layout Group 이 항목들을 **최소 높이(0)로 찌그러뜨립니다.**

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `expandedHeight` | 200 (데모 150) | 펼쳤을 때 본문 높이 |
| `duration` | 0.3 | 펼침/접힘 시간 |
| `closeOthers` | true | 하나만 열리게 |
| `startExpanded` | false | 시작 시 펼침 |

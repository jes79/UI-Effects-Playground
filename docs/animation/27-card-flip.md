# 27. 카드 플립 (Card Flip)

| 씬 | 스크립트 | 커밋 |
|---|---|---|
| `27_CardFlip` | [`CardFlip.cs`](../../UI-Effects-Playground/Assets/01_Scripts/Animation/CardFlip.cs) | [`b8d50bb`](https://github.com/jes79/UI-Effects-Playground/commit/b8d50bb0b3b00f1e5df8ff0a37b7eea78787ce92) |

[← 위키 홈](../README.md)

## 무엇을 하나요
카드를 클릭하면 Y축으로 회전하며 뒷면(?)에서 앞면으로 뒤집힙니다. 뒤집는 동안 살짝 커져서 카드를 들어 올리는 느낌이 나요.

## 구성
```
Card_FIRE     투명 Image + Button + CardFlip (클릭만 받음)
├─ Back       뒷면 + "?"
└─ Front      앞면 (시작 시 비활성화)
```

## 핵심 원리
UI 는 뒷면이 따로 없어서 180도를 한 번에 돌리면 글자가 **좌우 반전**돼 보입니다. 그래서 회전을 둘로 나눠요.

1. 0° → 90° (카드가 옆면이 되어 안 보임)
2. `AppendCallback` 으로 앞/뒷면 교체 + 회전을 -90° 로 점프
3. -90° → 0° (새 면이 정상 방향으로 보임)

- 회전 중 `isFlipping` 으로 연타 방지

## 주요 파라미터
| 필드 | 기본값 | 설명 |
|---|---|---|
| `duration` | 0.5 | 전체 뒤집기 시간 |
| `liftScale` | 1.1 | 뒤집는 동안 커지는 정도 |
| `startFront` | false | 앞면부터 시작 |

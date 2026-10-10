# 데모 씬 빌더

20~46번 데모 씬은 에디터 스크립트로 구성되어 있어서, 씬 파일이 깨지거나 처음부터 다시 만들고 싶을 때 메뉴 한 번으로 재생성할 수 있습니다.

[← 위키 홈](README.md)

## 사용법

| 메뉴 | 동작 |
|---|---|
| `Tools > UI Effects > Build All Demo Scenes` | 20~46번 씬 전체 재생성 |
| `Tools > UI Effects > Build Scene > XX_Name` | 해당 씬만 재생성 |

- 씬은 `Assets/00_Scenes/XX_Name.unity` 에 저장(덮어쓰기)됩니다
- 셰이더 데모용 머티리얼은 `Assets/04_Materials/UI`, 샘플 이미지는 `Assets/05_Sprites/Generated` 에 생성됩니다
- 직접 씬을 수정해서 쓰는 경우 재생성하면 수정 내용이 사라지니 주의하세요

## 구성 파일

| 파일 | 역할 |
|---|---|
| [`DemoSceneKit.cs`](../UI-Effects-Playground/Assets/01_Scripts/Editor/DemoSceneKit.cs) | 공통 헬퍼: 씬 기본 구성(카메라 / EventSystem / Canvas / 타이틀), UI 생성, 직렬화 필드 연결, 버튼 이벤트 연결, 머티리얼·텍스처 저장 |
| [`DemoTextures.cs`](../UI-Effects-Playground/Assets/01_Scripts/Editor/DemoTextures.cs) | 샘플 이미지(풍경, 아이콘, 카드, 유리 배경)를 코드로 그려서 PNG 로 저장 |
| [`Scenes/BuildXX_*.cs`](../UI-Effects-Playground/Assets/01_Scripts/Editor/Scenes) | 항목별 씬 구성 (노션 교안의 "UI 오브젝트 준비 / 연결 설정" 단계를 코드로 옮긴 것) |

## 씬 기본 구성

`DemoSceneKit.NewScene()` 이 모든 데모 씬에 공통으로 만드는 것:

- `Main Camera` — Orthographic, Size 5.4, 배경색 다크 그레이
- `EventSystem` — `InputSystemUIInputModule` (프로젝트가 Input System 전용이라)
- `Canvas` — Screen Space Overlay(26번만 Screen Space - Camera), `CanvasScaler` 1920 x 1080 / Scale With Screen Size
- `Background`, `Title`, `Description` 텍스트

## 새 데모 씬 추가하기

```csharp
using UnityEditor;
using UnityEngine;
using static DemoSceneKit;

public static class Build47_MyEffect
{
    [DemoScene("47_MyEffect")]                                   // Build All 에 포함
    [MenuItem("Tools/UI Effects/Build Scene/47_MyEffect")]       // 개별 메뉴
    public static void Build()
    {
        var canvas = NewScene("My Effect", "설명 한 줄");
        var root = canvas.transform;

        var button = CreateButtonRow(root, -330f, "Play")[0];
        var effect = button.gameObject.AddComponent<MyEffect>();
        Set(effect, "target", button.transform);   // private [SerializeField] 연결
        OnClick(button, effect.Play);              // 인스펙터에 보이는 영구 리스너

        SaveScene("47_MyEffect");
    }
}
```

## 주의할 점

- **Layout Group 이 제어하는 RectTransform 값은 씬 파일에 저장되지 않아요** (Unity 가 0 으로 저장하고 로드할 때 다시 계산). 그래서 22번 아코디언처럼 레이아웃 그룹을 쓰는 경우, 컨테이너 크기가 0 이면 자식이 찌그러지므로 `ContentSizeFitter` 를 함께 둬야 해요
- 생성 텍스처는 Import 설정의 **Mesh Type = Full Rect** 로 저장해요. Tight 이면 투명 여백이 잘려서 글로우/아웃라인 셰이더가 그려질 공간이 없어져요
- 데모 씬 텍스트는 영어예요. 기본 TMP 폰트(LiberationSans)에 한글 글리프가 없기 때문이에요

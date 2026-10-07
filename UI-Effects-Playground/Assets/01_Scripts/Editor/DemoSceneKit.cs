using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// 데모 씬 빌더 메서드에 붙이는 속성. "Tools/UI Effects/Build All Demo Scenes" 메뉴가 이 속성이 붙은 메서드를 순서대로 실행함
[AttributeUsage(AttributeTargets.Method)]
public class DemoSceneAttribute : Attribute
{
    public readonly string SceneName;
    public DemoSceneAttribute(string sceneName) { SceneName = sceneName; }
}

// 연출 데모 씬을 코드로 구성하기 위한 에디터 전용 헬퍼 모음
// - 씬 기본 구성(카메라, EventSystem, Canvas, 타이틀)
// - UI 생성(Image/Text/Button), 직렬화 필드 연결, 버튼 이벤트 연결
// - 머티리얼/생성 텍스처 에셋 저장
public static class DemoSceneKit
{
    public const string SceneFolder = "Assets/00_Scenes";
    public const string MaterialFolder = "Assets/04_Materials/UI";
    public const string TextureFolder = "Assets/05_Sprites/Generated";

    public static readonly Color BgColor = new Color32(24, 26, 33, 255);
    public static readonly Color PanelColor = new Color32(45, 49, 61, 255);
    public static readonly Color AccentColor = new Color32(88, 140, 255, 255);
    public static readonly Color TextColor = new Color32(235, 238, 245, 255);
    public static readonly Color SubTextColor = new Color32(150, 156, 170, 255);

    private const string FontPath = "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset";

    public static TMP_FontAsset DefaultFont => AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
    public static Sprite RoundedSprite => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
    public static Sprite KnobSprite => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

    [MenuItem("Tools/UI Effects/Build All Demo Scenes")]
    public static void BuildAll()
    {
        var methods = TypeCache.GetMethodsWithAttribute<DemoSceneAttribute>()
            .OrderBy(m => m.GetCustomAttribute<DemoSceneAttribute>().SceneName);

        foreach (MethodInfo method in methods)
        {
            Debug.Log($"[DemoSceneKit] Build {method.GetCustomAttribute<DemoSceneAttribute>().SceneName}");
            method.Invoke(null, null);
        }

        AssetDatabase.SaveAssets();
    }

    // ───────────────────────── 씬 ─────────────────────────

    // 빈 씬을 만들고 카메라/EventSystem/Canvas/배경/타이틀까지 구성해서 Canvas를 돌려줌
    // cameraCanvas = true 면 Screen Space - Camera (파티클을 UI 위에 그릴 때 사용)
    public static Canvas NewScene(string title, string description, bool cameraCanvas = false)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var camGo = new GameObject("Main Camera");
        camGo.tag = "MainCamera";
        var cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = BgColor;
        cam.orthographic = true;
        cam.orthographicSize = 5.4f;
        cam.nearClipPlane = 0.3f;
        cam.farClipPlane = 100f;
        camGo.transform.position = new Vector3(0f, 0f, -10f);

        // InputSystemUIInputModule 은 에디터에서 추가될 때 기본 UI 액션이 자동으로 연결됨
        // (비어 있더라도 런타임 OnEnable 에서 기본 액션을 할당함)
        new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

        var canvasGo = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGo.layer = LayerMask.NameToLayer("UI");
        var canvas = canvasGo.GetComponent<Canvas>();
        if (cameraCanvas)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 5f;
        }
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        var scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0f;

        var bg = CreateImage("Background", canvas.transform, Vector2.zero, Vector2.zero, BgColor);
        Stretch(bg.rectTransform);
        bg.raycastTarget = false;

        var titleText = CreateText("Title", canvas.transform, new Vector2(0f, -70f), new Vector2(1700f, 80f), title, 56f, TextColor);
        AnchorTop(titleText.rectTransform);
        titleText.fontStyle = FontStyles.Bold;

        var descText = CreateText("Description", canvas.transform, new Vector2(0f, -130f), new Vector2(1700f, 50f), description, 28f, SubTextColor);
        AnchorTop(descText.rectTransform);

        return canvas;
    }

    public static void SaveScene(string sceneName)
    {
        EnsureFolder(SceneFolder);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), $"{SceneFolder}/{sceneName}.unity");
    }

    // ───────────────────────── UI 생성 ─────────────────────────

    public static RectTransform CreateRect(string name, Transform parent, Vector2 pos, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.layer = LayerMask.NameToLayer("UI");
        var rt = (RectTransform)go.transform;
        rt.SetParent(parent, false);
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        return rt;
    }

    public static Image CreateImage(string name, Transform parent, Vector2 pos, Vector2 size, Color color, Sprite sprite = null, bool sliced = false)
    {
        var rt = CreateRect(name, parent, pos, size);
        var img = rt.gameObject.AddComponent<Image>();
        img.color = color;
        img.sprite = sprite;
        if (sliced) img.type = Image.Type.Sliced;
        return img;
    }

    // 셰이더 데모용: 흰색 + 머티리얼이 적용된 이미지 (스프라이트를 Simple 로 써야 uv 가 0~1)
    public static Image CreateEffectImage(string name, Transform parent, Vector2 pos, Vector2 size, Sprite sprite, Material material)
    {
        var img = CreateImage(name, parent, pos, size, Color.white, sprite);
        img.material = material;
        return img;
    }

    // 데모 이미지 아래에 붙이는 작은 설명 문구
    public static TextMeshProUGUI CreateCaption(Transform parent, Vector2 pos, string text)
    {
        return CreateText("Caption", parent, pos, new Vector2(700f, 44f), text, 26f, SubTextColor);
    }

    public static TextMeshProUGUI CreateText(string name, Transform parent, Vector2 pos, Vector2 size, string text, float fontSize, Color color,
        TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        var rt = CreateRect(name, parent, pos, size);
        var tmp = rt.gameObject.AddComponent<TextMeshProUGUI>();
        tmp.font = DefaultFont;
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.raycastTarget = false;
        return tmp;
    }

    public static Button CreateButton(string name, Transform parent, Vector2 pos, Vector2 size, string label, Color? color = null, float fontSize = 30f)
    {
        var img = CreateImage(name, parent, pos, size, color ?? AccentColor, RoundedSprite, true);
        var btn = img.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;

        var txt = CreateText("Label", img.transform, Vector2.zero, Vector2.zero, label, fontSize, Color.white);
        Stretch(txt.rectTransform);
        txt.fontStyle = FontStyles.Bold;
        return btn;
    }

    // 화면 하단에 버튼을 가로로 나열 (데모 컨트롤용)
    public static Button[] CreateButtonRow(Transform parent, float y, params string[] labels)
    {
        const float width = 260f, spacing = 30f;
        float total = labels.Length * width + (labels.Length - 1) * spacing;
        var buttons = new Button[labels.Length];
        for (int i = 0; i < labels.Length; i++)
        {
            float x = -total * 0.5f + width * 0.5f + i * (width + spacing);
            buttons[i] = CreateButton(labels[i].Replace(" ", "") + "Button", parent, new Vector2(x, y), new Vector2(width, 80f), labels[i]);
        }
        return buttons;
    }

    public static void Stretch(RectTransform rt, float inset = 0f)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(inset, inset);
        rt.offsetMax = new Vector2(-inset, -inset);
    }

    public static void AnchorTop(RectTransform rt)
    {
        Vector2 pos = rt.anchoredPosition;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
        rt.anchoredPosition = pos;
    }

    // ───────────────────────── 직렬화 필드 / 이벤트 연결 ─────────────────────────

    private static SerializedProperty FindProperty(SerializedObject so, string field)
    {
        var p = so.FindProperty(field);
        if (p == null) throw new Exception($"{so.targetObject.GetType().Name}.{field} 필드를 찾을 수 없음");
        return p;
    }

    public static void Set(Object target, string field, Object value)
    {
        var so = new SerializedObject(target);
        FindProperty(so, field).objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetArray(Object target, string field, params Object[] values)
    {
        var so = new SerializedObject(target);
        var p = FindProperty(so, field);
        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++) p.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetStrings(Object target, string field, params string[] values)
    {
        var so = new SerializedObject(target);
        var p = FindProperty(so, field);
        p.arraySize = values.Length;
        for (int i = 0; i < values.Length; i++) p.GetArrayElementAtIndex(i).stringValue = values[i];
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetFloat(Object target, string field, float value)
    {
        var so = new SerializedObject(target);
        FindProperty(so, field).floatValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetBool(Object target, string field, bool value)
    {
        var so = new SerializedObject(target);
        FindProperty(so, field).boolValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetString(Object target, string field, string value)
    {
        var so = new SerializedObject(target);
        FindProperty(so, field).stringValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    // 버튼 OnClick 에 영구 리스너(인스펙터에 보이는 형태)로 연결
    public static void OnClick(Button button, UnityAction action) => UnityEventTools.AddPersistentListener(button.onClick, action);
    public static void OnClick(Button button, UnityAction<int> action, int arg) => UnityEventTools.AddIntPersistentListener(button.onClick, action, arg);
    public static void OnClick(Button button, UnityAction<float> action, float arg) => UnityEventTools.AddFloatPersistentListener(button.onClick, action, arg);

    // ───────────────────────── 에셋 ─────────────────────────

    public static Material CreateMaterial(string name, string shaderName, Action<Material> setup = null)
    {
        var shader = Shader.Find(shaderName);
        if (shader == null) throw new Exception($"셰이더를 찾을 수 없음: {shaderName}");

        EnsureFolder(MaterialFolder);
        string path = $"{MaterialFolder}/{name}.mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, path);
        }
        else
        {
            mat.shader = shader;
        }

        setup?.Invoke(mat);
        EditorUtility.SetDirty(mat);
        return mat;
    }

    // 픽셀 함수(u, v → 색)로 텍스처를 그려 PNG 스프라이트로 저장
    public static Sprite CreateSprite(string name, int width, int height, Func<float, float, Color> pixel, Vector4 border = default)
    {
        EnsureFolder(TextureFolder);
        string path = $"{TextureFolder}/{name}.png";

        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var pixels = new Color[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                pixels[y * width + x] = pixel((x + 0.5f) / width, (y + 0.5f) / height);
        tex.SetPixels(pixels);
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);

        AssetDatabase.ImportAsset(path);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.spriteBorder = border;

        // Tight 메시면 투명 영역이 잘려서 글로우/아웃라인 셰이더가 그려질 공간이 사라짐 → 사각형 그대로 사용
        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        importer.SetTextureSettings(settings);
        importer.SaveAndReimport();

        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    // 게이지 Fill 등에 쓰는 단색 스프라이트 (Image.Type.Filled 는 스프라이트가 있어야 동작함)
    public static Sprite WhiteSprite => AssetDatabase.LoadAssetAtPath<Sprite>($"{TextureFolder}/White.png")
                                        ?? CreateSprite("White", 8, 8, (u, v) => Color.white);

    public static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
        if (!string.IsNullOrEmpty(parent)) EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
    }

    // ───────────────────────── 수학 헬퍼 (텍스처 생성용) ─────────────────────────

    // 둥근 사각형 SDF (p: 중심 기준 좌표, halfSize: 반 크기, radius: 모서리 반경). 안쪽이 음수
    public static float RoundedBoxSdf(Vector2 p, Vector2 halfSize, float radius)
    {
        Vector2 q = new Vector2(Mathf.Abs(p.x), Mathf.Abs(p.y)) - halfSize + Vector2.one * radius;
        Vector2 qMax = Vector2.Max(q, Vector2.zero);
        return qMax.magnitude + Mathf.Min(Mathf.Max(q.x, q.y), 0f) - radius;
    }
}

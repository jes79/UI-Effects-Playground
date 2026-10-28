using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 강의 모드 첫 장면(허브) 만들기 + 빌드 씬 목록 정리
// - Assets/Lecture/LectureHub.unity : 카메라·EventSystem·배경·제목만 있는 빈 씬 (데모 씬과 같은 모양, DemoSceneKit 사용)
//   씬 목록·용어 목록은 LectureOverlay 가 UI Toolkit 으로 그 위에 그린다
// - Build Settings 씬 목록 = 허브(맨 앞) + 효과 데모 씬 07~46 (00_ShaderTest 같은 실험 씬은 뺌)
public static class LectureHubBuilder
{
    public const string HubPath = "Assets/Lecture/LectureHub.unity";

    // 효과 데모 씬: Assets/00_Scenes/NN_Name.unity 중 07~46
    static readonly Regex DemoSceneName = new Regex(@"^(\d\d)_");
    const int FirstDemo = 7, LastDemo = 46;

    [MenuItem("Tools/UI Effects/Build Lecture Hub")]
    public static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        DemoSceneKit.NewScene("UI Effects Lab", "Pick a demo scene below, or press F4 for the lecture");
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), HubPath);
        Debug.Log($"[LectureHubBuilder] {HubPath} 저장");

        UpdateBuildSettings();
    }

    // 허브가 없으면 만들고, Build Settings 씬 목록을 맞춘다 (강의 플레이어 빌드 전에 호출)
    public static void EnsureHub()
    {
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(HubPath) == null) Build();
        else UpdateBuildSettings();
    }

    // 허브 + 데모 씬 07~46 (번호 순서)
    public static string[] ScenePaths()
    {
        var demos = AssetDatabase.FindAssets("t:Scene", new[] { DemoSceneKit.SceneFolder })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(path =>
            {
                var m = DemoSceneName.Match(System.IO.Path.GetFileNameWithoutExtension(path));
                if (!m.Success) return false;
                int n = int.Parse(m.Groups[1].Value);
                return n >= FirstDemo && n <= LastDemo;
            })
            .OrderBy(path => path, System.StringComparer.Ordinal);
        return new[] { HubPath }.Concat(demos).ToArray();
    }

    static void UpdateBuildSettings()
    {
        var paths = ScenePaths();
        EditorBuildSettings.scenes = paths.Select(p => new EditorBuildSettingsScene(p, true)).ToArray();
        Debug.Log($"[LectureHubBuilder] Build Settings 씬 {paths.Length}개 (허브 + 데모 {paths.Length - 1}개)");
    }
}

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// 수업용 UI 이펙트 랩 강의 프로그램 Windows 빌드 (Builds/ 는 git 에 올리지 않음 → PC마다 한 번 빌드)
// UI_EFFECTS_LECTURE_PLAYER 정의를 붙여 빌드 → 테두리 없는 창 전체화면으로 시작, 강의 중이 아닐 때 Esc 로 종료 (LectureLaunch)
// 씬: 허브(LectureHub, 맨 앞) + 효과 데모 씬 40개(07~46)
// 실행 예: Builds/LecturePlayer/UIEffectsLab.exe -glossaryStart 0304 -glossaryLecture   (그 용어부터 강의)
//          Builds/LecturePlayer/UIEffectsLab.exe -glossaryStart 0304                    (강의 없이 그 용어의 데모 씬만)
//          Builds/LecturePlayer/UIEffectsLab.exe                                        (자유 모드: 씬 목록)
// 명령줄: Unity -batchmode -projectPath <프로젝트> -executeMethod UIEffectsLecturePlayer.BuildFromCommandLine -quit -logFile <로그>
public static class UIEffectsLecturePlayer
{
    public const string OutputPath = "../Builds/LecturePlayer/UIEffectsLab.exe"; // 프로젝트 폴더 기준 → git 루트의 Builds
    public const string Define = "UI_EFFECTS_LECTURE_PLAYER";

    [MenuItem("Tools/UI Effects/Build Lecture Player")]
    static void BuildFromMenu()
    {
        if (Build()) EditorUtility.RevealInFinder(OutputPath);
    }

    // 배치 모드용: 실패하면 종료 코드 1
    public static void BuildFromCommandLine()
    {
        if (!Build()) EditorApplication.Exit(1);
    }

    public static bool Build()
    {
        LectureHubBuilder.EnsureHub();
        string[] scenes = LectureHubBuilder.ScenePaths();

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = OutputPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None,
            extraScriptingDefines = new[] { Define },
        });
        Debug.Log($"[UIEffectsLecturePlayer] {report.summary.result} 씬 {scenes.Length}개 {report.summary.totalSize / (1024 * 1024)}MB {report.summary.totalTime} → {System.IO.Path.GetFullPath(OutputPath)}");
        return report.summary.result == BuildResult.Succeeded;
    }
}

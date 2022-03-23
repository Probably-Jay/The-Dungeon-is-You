using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Builder
{
    public class GameBuilder : MonoBehaviour
    {
        [MenuItem("Build/Build windows")]
        public static void PerformWindowsBuild() // do not rename
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] {"Assets/Scenes/SampleScene.unity"};
            buildPlayerOptions.locationPathName = "build/windows/theDungeonIsYou.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows;
            buildPlayerOptions.options = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build failed");
            }
            

        }
    }
}
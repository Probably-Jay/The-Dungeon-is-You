using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/*
 * Please just don't touch this file
 * Build pipeline relies on names
 * If it is broken, I am so sorry
 */

namespace Builder // do not rename
{
    public class GameBuilder : MonoBehaviour // do not rename
    {
        [MenuItem("Build/Build Windows")]
        public static void PerformWindowsBuild() // do not rename
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] {"Assets/Scenes/SampleScene.unity"}, // obvs add the rest here
                locationPathName = "build/windows/theDungeonIsYou.exe", 
                target = BuildTarget.StandaloneWindows,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded");
                    break;
                case BuildResult.Failed:
                    Debug.LogError("Build failed");
                    break;
            }
        } 
        
        [MenuItem("Build/Build Web")]
        public static void PerformWebBuild() // do not rename
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] {"Assets/Scenes/SampleScene.unity"},
                locationPathName = "build/WebGL",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded");
                    break;
                case BuildResult.Failed:
                    Debug.LogError("Build failed");
                    break;
            }
        }
    }
}
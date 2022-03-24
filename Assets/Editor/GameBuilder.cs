using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            var scenePaths = GetScenesInBuild();

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenePaths, 
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
            var scenePaths = GetScenesInBuild();
            
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenePaths,
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
        private static string[] GetScenesInBuild()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var scenePaths = new string[sceneCount];

            for (var i = 0; i < sceneCount; i++)
            {
                scenePaths[i] = SceneUtility.GetScenePathByBuildIndex(i);
            }

            return scenePaths;
        }
    }
}
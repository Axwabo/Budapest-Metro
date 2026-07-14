using System.IO;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class Build
{

    private const string Folder = "Build";

    public static void Windows() => Run("Windows");

    public static void Linux() => Run("Linux");

    private static void Run(string profile)
    {
        Directory.CreateDirectory(Folder);
        var buildProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>($"Assets/Settings/Build Profiles/{profile}.asset");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerWithProfileOptions
        {
            buildProfile = buildProfile,
            locationPathName = Folder
        });
        var summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        else if (summary.result == BuildResult.Failed)
            Debug.Log("Build failed");
    }

}

using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class Build
{

    private const string Folder = "Build";

    public static void Windows() => Run(BuildTarget.StandaloneWindows64, "Budapest-Metro.exe");

    public static void Linux() => Run(BuildTarget.StandaloneLinux64, "Budapest-Metro.x86_64");

    private static void Run(BuildTarget target, string filename)
    {
        Directory.CreateDirectory(Folder);
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            target = target,
            locationPathName = Path.Combine(Folder, target.ToString(), filename),
        });
        var summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        else if (summary.result == BuildResult.Failed)
            Debug.Log("Build failed");
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using Metro.Rail;
using UnityEditor;
using UnityEngine;

public static class SwitchCreator
{

    private static void DisplayError() => EditorUtility.DisplayDialog("Create Switch", "Select exactly 3 segments that have endings that meet in exactly one point!", "my bad");

    [MenuItem("Metro/Create Switch")]
    public static void Create()
    {
        var segments = new HashSet<TrackSegment>();
        foreach (var o in Selection.gameObjects)
            if (o.TryGetComponent(out TrackSegment segment))
                segments.Add(segment);
        if (segments.Count != 3)
        {
            DisplayError();
            return;
        }

        var points = new List<(TrackSegment Track, Pose Pose, bool End)>();
        foreach (var segment in segments)
        {
            points.Add((segment, segment.Sample(0), false));
            points.Add((segment, segment.Sample(segment.Length), true));
        }

        foreach (var point in points)
        {
            var current = point.Pose.position;
            var matching = new List<(TrackSegment Track, Pose Pose, bool End)>();
            foreach (var other in points)
                if (Vector3.Distance(other.Pose.position, current) < 0.1f)
                    matching.Add(other);
            if (Connect(matching))
                return;
        }

        DisplayError();
    }

    private static bool Connect(List<(TrackSegment Track, Pose Pose, bool End)> points)
    {
        var start = points.Where(e => !e.End).ToArray();
        var end = points.Where(e => e.End).ToArray();
        return (start.Length, end.Length) switch
        {
            (2, 1) => ConnectBranching(start, end),
            (1, 2) => ConnectJoining(start, end),
            _ => false
        };
    }

    private static bool ConnectBranching((TrackSegment Track, Pose Pose, bool End)[] start, (TrackSegment Track, Pose Pose, bool End)[] end)
    {
        throw new NotImplementedException();
    }

    private static bool ConnectJoining((TrackSegment Track, Pose Pose, bool End)[] start, (TrackSegment Track, Pose Pose, bool End)[] end)
    {
        throw new NotImplementedException();
    }

}

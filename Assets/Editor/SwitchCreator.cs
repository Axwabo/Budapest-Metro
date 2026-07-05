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

        var points = new List<Point>();
        foreach (var segment in segments)
        {
            points.Add(new Point(segment, segment.Sample(0), false));
            points.Add(new Point(segment, segment.Sample(segment.Length), true));
        }

        foreach (var point in points)
        {
            var current = point.Pose.position;
            var matching = new List<Point>();
            foreach (var other in points)
                if (Vector3.Distance(other.Pose.position, current) < 0.1f)
                    matching.Add(other);
            if (Connect(matching))
                return;
        }

        DisplayError();
    }

    private static bool Connect(List<Point> points)
    {
        var start = points.Where(e => !e.End).ToArray();
        var end = points.Where(e => e.End).ToArray();
        return (start.Length, end.Length) switch
        {
            (2, 1) => ConnectBranching(start[0], start[1], end[0]),
            (1, 2) => ConnectJoining(start, end),
            _ => false
        };
    }

    private static bool ConnectBranching(Point start1, Point start2, Point end)
    {
        var (left, right) = OrderLeftRight(start1, start2);
        Debug.Log("left", left.Track);
        Debug.Log("right", right.Track);
        /*var go = new GameObject("Branching Switch")
        {
            transform =
            {
                position = end.Pose.position
            }
        };
        var @switch = go.AddComponent<Switch>();
        @switch.fromLeft*/
        return true;
    }

    private static bool ConnectJoining(Point[] start, Point[] end)
    {
        throw new NotImplementedException();
    }

    private static (Point, Point) OrderLeftRight(Point one, Point two)
    {
        var oneToTwo = two.Pose.position - one.Pose.position;
        return Vector3.Dot(oneToTwo.normalized, one.Pose.right) < 0 ? (one, two) : (two, one);
    }

}

public sealed record Point(TrackSegment Track, Pose Pose, bool End);

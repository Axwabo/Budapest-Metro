using System.Collections.Generic;
using System.Linq;
using Metro.Rail;
using Metro.Rail.Sidings;
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
            (1, 2) => ConnectJoining(start[0], end[0], end[1]),
            _ => false
        };
    }

    private static bool ConnectBranching(Point start1, Point start2, Point end)
    {
        var (left, right) = OrderLeftRight(start1, start2);
        var go = new GameObject("Branching Switch")
        {
            transform =
            {
                position = end.Pose.position,
                rotation = end.Pose.rotation
            }
        };
        var @switch = go.AddComponent<Switch>();
        @switch.fromLeft = end.Track;
        @switch.toLeft = left.Track;
        @switch.fromRight = end.Track;
        @switch.toRight = right.Track;
        return true;
    }

    private static bool ConnectJoining(Point start, Point end1, Point end2)
    {
        var (left, right) = OrderLeftRight(end1, end2);
        var go = new GameObject("Joining Switch")
        {
            transform =
            {
                position = start.Pose.position,
                rotation = start.Pose.rotation
            }
        };
        var @switch = go.AddComponent<Switch>();
        @switch.fromLeft = left.Track;
        @switch.toLeft = start.Track;
        @switch.fromRight = right.Track;
        @switch.toRight = start.Track;
        return true;
    }

    private static (Point, Point) OrderLeftRight(Point one, Point two)
    {
        var onePose = one.SampleOther();
        var twoPose = two.SampleOther();
        var oneToTwo = twoPose.position - onePose.position;
        return Vector3.Dot(oneToTwo.normalized, onePose.right) > 0 ? (one, two) : (two, one);
    }

}

public sealed record Point(TrackSegment Track, Pose Pose, bool End)
{

    public Pose SampleOther() => End ? Track.Sample(0) : Track.Sample(Track.Length);

}

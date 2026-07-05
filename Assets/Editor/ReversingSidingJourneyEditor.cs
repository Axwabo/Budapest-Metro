using System.Collections.Generic;
using Metro.Journeys;
using Metro.Rail;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReversingSidingJourney))]
public sealed class ReversingSidingJourneyEditor : Editor
{

    private readonly List<SwitchHandle> _handles = new();

    private int _previousCount;

    private void OnEnable() => RefreshHandles(((ReversingSidingJourney) target).Switches);

    private void OnDisable() => _handles.Clear();

    private void OnSceneGUI()
    {
        var journey = (ReversingSidingJourney) target;
        var list = new List<SwitchState>(journey.Switches);
        if (list.Count != _previousCount)
            RefreshHandles(journey.Switches);
        var modified = false;
        Handles.color = Color.green;
        foreach (var handle in _handles)
        {
            var position = Handles.PositionHandle(handle.Handle, handle.Rotation);
            var toOrigin = Vector3.Distance(position, handle.Position);
            var toLeft = Vector3.Distance(position, handle.Left);
            var toRight = Vector3.Distance(position, handle.Right);
            if (toLeft < toOrigin && toLeft < toRight)
                modified |= Attach(list, handle, true);
            if (toRight < toOrigin && toRight < toLeft)
                modified |= Attach(list, handle, false);
        }

        if (!modified)
            return;
        _previousCount = list.Count;
        journey.Switches = list.ToArray();
        EditorUtility.SetDirty(journey);
    }

    private void RefreshHandles(SwitchState[] switches)
    {
        _handles.Clear();
        foreach (var @switch in FindObjectsByType<Switch>())
        {
            bool? isLeft = null;
            foreach (var state in switches)
                if (state.@switch == @switch)
                    isLeft = state.isLeft;
            _handles.Add(new SwitchHandle(@switch, isLeft));
        }

        _previousCount = switches.Length;
    }

    private static bool Attach(List<SwitchState> list, SwitchHandle handle, bool left)
    {
        handle.Handle = left ? handle.Left : handle.Right;
        Handles.DrawLine(handle.Handle, handle.Position);
        foreach (var state in list)
        {
            if (state.@switch != handle.Switch)
                continue;
            if (state.isLeft == left)
                return false;
            state.isLeft = left;
            return true;
        }

        list.Add(new SwitchState
        {
            @switch = handle.Switch,
            isLeft = left
        });
        return true;
    }

    private sealed class SwitchHandle
    {

        public SwitchHandle(Switch @switch, bool? isLeft)
        {
            @switch.transform.GetPositionAndRotation(out var position, out var rotation);
            Switch = @switch;
            Position = position;
            Rotation = rotation;
            var fromLeft = @switch.fromLeft;
            var fromRight = @switch.fromRight;
            var toLeft = @switch.toLeft;
            var toRight = @switch.toRight;
            var branching = fromLeft == fromRight;
            var (left, right) = branching ? (toLeft, toRight) : (fromLeft, fromRight);
            Left = Lerp(position, left, branching);
            Right = Lerp(position, right, branching);
            Handle = isLeft switch
            {
                true => Left,
                false => Right,
                null => position
            };
        }

        public Switch Switch { get; }

        public Vector3 Position { get; }

        public Quaternion Rotation { get; }

        public Vector3 Left { get; }

        public Vector3 Right { get; }

        public Vector3 Handle { get; set; }

        private static Vector3 Lerp(Vector3 origin, TrackSegment segment, bool branching) => Vector3.Lerp(origin, segment.Sample(branching ? segment.Length : 0).position, 0.25f);

    }

}

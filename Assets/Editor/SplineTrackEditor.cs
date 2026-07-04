using Metro.Rail;
using SplineMesh;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineTrack))]
[CanEditMultipleObjects]
public sealed class SplineTrackEditor : Editor
{

    private float _offset;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(5);
        _offset = EditorGUILayout.FloatField("Offset By X", _offset);
        if (GUILayout.Button("Offset"))
            Offset();
    }

    private void Offset()
    {
        var offset = Vector3.right * _offset;
        foreach (var o in targets)
        {
            using var serialized = new SerializedObject(o);
            var spline = (Spline) serialized.FindProperty("spline").boxedValue;
            foreach (var node in spline.nodes)
            {
                var transformed = Quaternion.LookRotation(node.Direction - node.Position) * offset;
                node.Position += transformed;
                node.Direction += transformed;
            }

            spline.RefreshCurves();
        }
    }

}

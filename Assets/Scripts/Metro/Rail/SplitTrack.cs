using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

namespace Metro.Rail
{

    public sealed class SplitTrack : MonoBehaviour
    {

        [SerializeField]
        private TrackSegment track;

        [SerializeField]
        private float[] points;

        private sealed class SplitSegment : TrackSegment
        {

            public TrackSegment Original { get; set; }

            public float Beginning { get; set; }

            public float End { get; set; }

            public override float Length => End - Beginning;

            public override Pose Sample(float distance) => Original.Sample(distance + Beginning);

            public override float GetDistance(Vector3 position) => Mathf.Clamp(Original.GetDistance(position) - Beginning, 0, Length);

        }

#if UNITY_EDITOR
        [ContextMenu("Recreate")]
        private void Recreate()
        {
            UOUtility.DestroyChildren(gameObject);
            var start = 0f;
            var list = new List<SplitSegment>();
            for (var i = 0; i < points.Length; i++)
            {
                var t = points[i];
                list.Add(Create(i, start, t));
                start = t;
            }

            list.Add(Create(points.Length, start, track.Length));
            for (var i = 0; i < list.Count - 1; i++)
                list[i].Next = list[i + 1];
            list[^1].Next = track.Next;
        }

        private SplitSegment Create(int i, float start, float end)
        {
            var go = new GameObject(i.ToString())
            {
                transform =
                {
                    parent = transform
                }
            };
            var segment = go.AddComponent<SplitSegment>();
            segment.Original = track;
            segment.Beginning = start;
            segment.End = end;
            return segment;
        }

        private void OnDrawGizmosSelected()
        {
            if (!track || points == null)
                return;
            Gizmos.color = Color.yellow;
            foreach (var point in points)
                Gizmos.DrawSphere(track.Sample(point).position, 0.5f);
        }
#endif

    }

}

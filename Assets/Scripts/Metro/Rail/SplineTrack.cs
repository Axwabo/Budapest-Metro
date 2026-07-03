using SplineMesh;
using UnityEngine;

namespace Metro.Rail
{

    public sealed class SplineTrack : TrackSegment
    {

        [SerializeField]
        [HideInInspector]
        private Spline spline;

        public override float Length => spline.Length;

#if UNITY_EDITOR
        private void OnValidate() => spline = GetComponent<Spline>();
#endif

        public override Pose Sample(float distance)
        {
            var sample = spline.GetSampleAtDistance(distance);
            return new Pose(sample.location, sample.Rotation);
        }

    }

}

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

        public override float GetDistance(Vector3 position)
        {
            var sample = spline.GetProjectionSample(position);
            var distance = sample.distanceInCurve;
            foreach (var curve in spline.curves)
            {
                if (curve == sample.curve)
                    break;
                distance += curve.Length;
            }

            var length = spline.Length;
            return Mathf.Clamp(distance, 0, length);
        }

    }

}

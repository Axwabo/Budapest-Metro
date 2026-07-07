using System.Collections.Generic;
using System.Linq;
using Metro.Rail.Controls;
using UnityEngine;

namespace Metro.Rail
{

    public abstract class TrackSegment : MonoBehaviour
    {

        [field: SerializeField]
        public TrackSegment Next { get; private set; }

        public TrackSegment Previous { get; private set; }

        public HashSet<ControlPoint> ControlPoints { get; } = new();

        public HashSet<StopPoint> StopPoints { get; private set; }

        public abstract float Length { get; }

        private void Start()
        {
            RefreshNext();
            if (Application.isPlaying)
                StopPoints = ControlPoints.OfType<StopPoint>().ToHashSet();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Sample(0).position, 0.5f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Sample(Length).position, 0.5f);
        }

        public void SetNext(TrackSegment next)
        {
            Next = next;
            RefreshNext();
        }

        private void RefreshNext()
        {
            if (Next)
                Next.Previous = this;
        }

        public void ConnectTo(TrackSegment segment)
        {
            Next = segment;
            segment.Previous = this;
        }

        public abstract Pose Sample(float distance);

        public abstract float GetDistance(Vector3 position);

    }

}

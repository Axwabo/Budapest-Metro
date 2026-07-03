using UnityEngine;

namespace Metro.Rail
{

    public abstract class TrackSegment : MonoBehaviour
    {

        [field: SerializeField]
        public TrackSegment Next { get; private set; }

        public TrackSegment Previous { get; private set; }

        public abstract float Length { get; }

        private void Start()
        {
            if (Next)
                Next.Previous = this;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Sample(0).position, 0.01f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Sample(Length).position, 0.01f);
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

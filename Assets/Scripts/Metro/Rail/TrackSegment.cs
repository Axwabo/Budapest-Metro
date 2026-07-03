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

        public void ConnectTo(TrackSegment segment)
        {
            Next = segment;
            segment.Previous = this;
        }

        public abstract Pose Sample(float distance);

    }

}

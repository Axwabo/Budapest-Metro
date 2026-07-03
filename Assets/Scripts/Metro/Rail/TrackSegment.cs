using UnityEngine;

namespace Metro.Rail
{

    public abstract class TrackSegment : MonoBehaviour
    {

        public abstract float Length { get; }

        public abstract Pose Sample(float distance);

    }

}

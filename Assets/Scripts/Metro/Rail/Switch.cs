using UnityEngine;

namespace Metro.Rail
{

    public sealed class Switch : MonoBehaviour
    {

        [SerializeField]
        private bool isLeft;

        [Header("Left State")]
        [InspectorName("From")]
        public TrackSegment fromLeft;

        [InspectorName("To")]
        public TrackSegment toLeft;

        [Header("Right State")]
        [InspectorName("From")]
        public TrackSegment fromRight;

        [InspectorName("To")]
        public TrackSegment toRight;

        public bool IsLeft
        {
            get => isLeft;
            set
            {
                isLeft = value;
                (value ? fromLeft : fromRight).SetNext(value ? toLeft : toRight);
            }
        }

    }

}

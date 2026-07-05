using UnityEngine;

namespace Metro.Rail.Sidings
{

    public sealed class Switch : MonoBehaviour
    {

        [SerializeField]
        private bool isLeft;

        [Header("Left State")]
        public TrackSegment fromLeft;

        public TrackSegment toLeft;

        [Header("Right State")]
        public TrackSegment fromRight;

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

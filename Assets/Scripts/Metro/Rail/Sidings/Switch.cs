using UnityEngine;

namespace Metro.Rail.Sidings
{

    public sealed class Switch : MonoBehaviour
    {

        private static readonly Quaternion Left = Quaternion.Euler(0, 0, 30);
        private static readonly Quaternion Right = Quaternion.Euler(0, 0, -30);

        [SerializeField]
        private bool isLeft;

        [SerializeField]
        private Transform rotor;

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
                rotor.localRotation = value ? Left : Right;
            }
        }

        private void Start() => rotor.localRotation = isLeft ? Left : Right;

    }

}

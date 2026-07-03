using UnityEngine;

namespace Metro.Rail
{

    [ExecuteInEditMode]
    public class StraightTrack : TrackSegment
    {

        private Vector3 _center;

        private float _halfLength;

        private float _length;

        private Quaternion _rotation;

        public sealed override float Length => _length;

        protected virtual void Awake()
        {
            var t = transform;
            _length = t.lossyScale.z;
            _halfLength = _length * 0.5f;
            _center = t.position;
            _rotation = t.rotation;
        }

#if UNITY_EDITOR
        private void OnValidate() => Awake();
#endif

        public sealed override Pose Sample(float distance)
        {
            var position = _center + _rotation * new Vector3(0, 0, distance - _halfLength);
            return new Pose(position, _rotation);
        }

    }

}

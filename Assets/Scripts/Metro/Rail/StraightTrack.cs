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

        private Transform _t;

        public sealed override float Length => _length;

        protected virtual void Awake()
        {
            _t = transform;
            _length = _t.lossyScale.z;
            _halfLength = _length * 0.5f;
            _center = _t.position;
            _rotation = _t.rotation;
        }

#if UNITY_EDITOR
        private void OnValidate() => Awake();
#endif

        public sealed override Pose Sample(float distance)
        {
            var position = _center + _rotation * new Vector3(0, 0, distance - _halfLength);
            return new Pose(position, _rotation);
        }

        public sealed override float GetDistance(Vector3 position)
        {
            var projected = _t.InverseTransformPoint(position);
            return (projected.z + 0.5f) * _length;
        }

    }

}

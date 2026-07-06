using UnityEngine;

namespace Metro.Trains.Cars
{

    public sealed class CarBody : CarComponent
    {

        [field: SerializeField]
        public Transform Front { get; private set; }

        [field: SerializeField]
        public Transform Back { get; private set; }

        [SerializeField]
        private Transform frontPivot;

        [SerializeField]
        private Transform backPivot;

        private float _lerp;

        private Transform _t;

        private void Awake()
        {
            _t = transform;
            _lerp = Mathf.InverseLerp(Inverse(frontPivot), Inverse(backPivot), _t.localPosition.z);
        }

        private void FixedUpdate()
        {
            frontPivot.GetPositionAndRotation(out var frontPosition, out var frontRotation);
            backPivot.GetPositionAndRotation(out var backPosition, out var backRotation);
            _t.SetPositionAndRotation(
                Vector3.Lerp(frontPosition, backPosition, _lerp),
                Quaternion.Lerp(frontRotation, backRotation, _lerp)
            );
        }

        public float Inverse(Transform t) => _t.InverseTransformPoint(t.position).z;

        protected override void OnInitialized() => FixedUpdate();

    }

}

using UnityEngine;

namespace Metro.Trains.Cars
{

    public sealed class CarBody : MonoBehaviour
    {

        [SerializeField]
        private Transform frontPivot;

        [SerializeField]
        private Transform backPivot;

        private Transform _t;

        private void Awake() => _t = transform;

        private void FixedUpdate()
        {
            frontPivot.GetPositionAndRotation(out var frontPosition, out var frontRotation);
            backPivot.GetPositionAndRotation(out var backPosition, out var backRotation);
            _t.SetPositionAndRotation(
                Vector3.Lerp(frontPosition, backPosition, 0.5f),
                Quaternion.Lerp(frontRotation, backRotation, 0.5f)
            );
        }

    }

}

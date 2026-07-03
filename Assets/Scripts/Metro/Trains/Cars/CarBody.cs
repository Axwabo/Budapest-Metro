using UnityEngine;

namespace Metro.Trains.Cars
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class CarBody : MonoBehaviour
    {

        [SerializeField]
        private Transform frontPivot;

        [SerializeField]
        private Transform backPivot;

        private Rigidbody _rb;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            frontPivot.GetPositionAndRotation(out var frontPosition, out var frontRotation);
            backPivot.GetPositionAndRotation(out var backPosition, out var backRotation);
            _rb.MovePosition(Vector3.Lerp(frontPosition, backPosition, 0.5f));
            _rb.MoveRotation(Quaternion.Lerp(frontRotation, backRotation, 0.5f));
        }

    }

}

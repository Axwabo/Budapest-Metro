using UnityEngine;
using UnityEngine.InputSystem;

namespace Metro.Movement
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class MovementController : MonoBehaviour
    {

        [SerializeField]
        private float speed = 5;

        private Transform _cam;

        private Rigidbody _cc;

        private Vector3 _desiredMove;

        private Transform _t;

        private void Awake()
        {
            _cc = GetComponent<Rigidbody>();
            _t = transform;
            _cam = GetComponentInChildren<Camera>().transform;
        }

        private void FixedUpdate() => _cc.AddForce(_desiredMove * (speed * Time.fixedDeltaTime));

        // SendMessage sucks :heartbreaking:
        private void OnMove(InputValue movement)
        {
            var vector = movement.Get<Vector2>();
            _desiredMove = new Vector3(vector.x, 0, vector.y);
        }

    }

}

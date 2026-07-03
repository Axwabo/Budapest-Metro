using UnityEngine;
using UnityEngine.InputSystem;

namespace Metro.Movement
{

    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementController : MonoBehaviour
    {

        [SerializeField]
        private float speed = 5;

        [SerializeField]
        private float sensitivity = 0.5f;

        private Transform _cam;

        private CharacterController _cc;

        private Vector3 _desiredMove;

        private float _pitch;

        private Transform _t;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            _t = transform;
            _cam = GetComponentInChildren<Camera>().transform;
        }

        private void Update() => _cc.Move(_desiredMove * (speed * Time.deltaTime));

        private void OnLook(InputValue look)
        {
            var vector = look.Get<Vector2>() * sensitivity;
            _t.Rotate(Vector3.up, vector.x);
            _pitch = Mathf.Clamp(_pitch - vector.y, -90, 90);
            var eulerAngles = _cam.localEulerAngles;
            eulerAngles.x = _pitch;
            _cam.localEulerAngles = eulerAngles;
        }

        // SendMessage sucks :heartbreaking:
        private void OnMove(InputValue movement)
        {
            var vector = movement.Get<Vector2>();
            _desiredMove = new Vector3(vector.x, 0, vector.y);
        }

    }

}

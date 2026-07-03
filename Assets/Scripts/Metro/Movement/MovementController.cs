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

        [SerializeField]
        private float jumpForce = 2;

        [SerializeField]
        private float gravity = -9.81f;

        private Transform _cam;

        private CharacterController _cc;

        private Vector3 _desiredMove;

        private bool _lockLook;

        private float _pitch;

        private float _upwards;

        private bool _wantsToJump;

        public Transform Transform { get; private set; }

        public Transform Mount { get; set; }

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            Transform = transform;
            _cam = GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            var move = Transform.TransformDirection(_desiredMove) * (speed * Time.deltaTime);
            if (_cc.isGrounded)
            {
                if (_wantsToJump)
                    _upwards = jumpForce;
            }
            else
                _upwards += gravity * Time.deltaTime;

            _wantsToJump = false;
            move.y = _upwards;
            if (move == Vector3.zero)
                return;
            if ((_cc.Move(move) & CollisionFlags.Below) != 0)
                _upwards = 0;
        }

        private void OnLook(InputValue look)
        {
            if (_lockLook)
                return;
            var vector = look.Get<Vector2>() * sensitivity;
            Transform.Rotate(Vector3.up, vector.x);
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

        private void OnJump() => _wantsToJump = true;

        private void OnAttack() => _lockLook = !_lockLook;

    }

}

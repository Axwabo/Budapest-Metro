using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Movement
{

    public sealed class Mountable : MonoBehaviour
    {

        private MetroCar _car;

        private bool _inCar;

        private Transform _t;

        private void Awake()
        {
            _t = transform;
            _inCar = _car = GetComponentInParent<MetroCar>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out MovementController controller))
                return;
            controller.Mount = _t;
            controller.Transform.parent = _t;
            if (_inCar)
                _car.IsPlayerMounted = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out MovementController controller))
                return;
            if (_inCar)
                _car.IsPlayerMounted = false;
            if (controller.Mount != _t)
                return;
            controller.Mount = null;
            controller.Transform.parent = null;
        }

    }

}

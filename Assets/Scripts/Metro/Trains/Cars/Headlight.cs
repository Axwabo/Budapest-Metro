using UnityEngine;

namespace Metro.Trains.Cars
{

    [RequireComponent(typeof(Light))]
    public sealed class Headlight : CarComponent
    {

        [SerializeField]
        private bool isRear;

        [SerializeField]
        private Color rearColor;

        [SerializeField]
        private float rearIntensity;

        private Color _headColor;

        private float _headIntensity;

        private bool? _wasReverse;

        private Light _light;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _headColor = _light.color;
            _headIntensity = _light.intensity;
        }

        private void Update()
        {
            var reverse = Assembly.Motor.Reverse;
            if (_wasReverse == reverse)
                return;
            _wasReverse = reverse;
            _light.color = isRear == reverse ? _headColor : rearColor;
            _light.intensity = isRear == reverse ? _headIntensity : rearIntensity;
        }

    }

}

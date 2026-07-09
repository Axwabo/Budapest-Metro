using UnityEngine;

namespace Metro
{

    public sealed class AnalogClock : MonoBehaviour
    {

        private const float HourDegrees = 360 / 12f;
        private const float MinuteDegrees = 360 / 60f;

        [SerializeField]
        private Transform hour;

        [SerializeField]
        private Transform minute;

        private int _lastMinute = -1;

        private void Update()
        {
            var now = Clock.Now;
            var currentMinute = now.Minutes;
            if (currentMinute == _lastMinute)
                return;
            _lastMinute = currentMinute;
            hour.localRotation = Quaternion.Euler(0, 0, now.Hours * HourDegrees);
            minute.localRotation = Quaternion.Euler(0, 0, currentMinute * MinuteDegrees);
        }

    }

}

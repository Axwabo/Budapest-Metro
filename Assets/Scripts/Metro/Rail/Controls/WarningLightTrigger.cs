using Metro.Stations;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class WarningLightTrigger : ControlPoint
    {

        [SerializeField]
        private Station station;

        public void Trigger() => (Reverse ? station.Left : station.Right).Light.State = LightState.Blinking;

    }

}

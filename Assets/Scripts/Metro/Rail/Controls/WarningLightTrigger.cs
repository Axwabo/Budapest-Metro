using Metro.Stations;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class WarningLightTrigger : ControlPoint
    {

        [SerializeField]
        private Station station;

        public override void OnPassed(Axle axle) => (Reverse ? station.Left : station.Right).Light.State = LightState.Blinking;

    }

}

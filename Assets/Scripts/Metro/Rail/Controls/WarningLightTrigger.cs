using Metro.Stations;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class WarningLightTrigger : ControlPoint
    {

        [SerializeField]
        private Station station;

        public override void OnPassed(Axle axle) => station.Track(Reverse).Light.State = LightState.Blinking;

    }

}

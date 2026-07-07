using Metro.Rail.Sidings;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class TurnoutChanger : ControlPoint
    {

        [SerializeField]
        private Switch @switch;

        [SerializeField]
        private bool isLeft;

        public override void OnPassed(Axle axle) => @switch.IsLeft = isLeft;

    }

}

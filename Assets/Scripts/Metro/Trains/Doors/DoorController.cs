using System.Collections.Generic;
using Metro.Trains.Driving;

namespace Metro.Trains.Doors
{

    public sealed class DoorController : AssemblyComponent
    {

        private const float CloseThreshold = 4;

        private readonly List<MetroDoor> _doors = new();

        private float _openDelay;

        private bool _target;

        private void Update()
        {
            if (_openDelay > 0 && (_openDelay -= Clock.Delta) <= 0)
                SetDoors(true);
            if (_target && Parent.Driver.SecondsToDeparture <= CloseThreshold)
                SetDoors(false);
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _doors.AddRange(car.Components<MetroDoor>());
            OnStateChanged();
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Stopped)
                _openDelay = 1;
            else
                SetDoors(false);
        }

        private void SetDoors(bool open)
        {
            if (_target == open)
                return;
            _target = open;
            foreach (var door in _doors)
                door.Open = _target && door.Reverse == Parent.Motor.Reverse; // TODO: use journey direction or track direction
        }

    }

}

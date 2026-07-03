using System.Collections.Generic;
using Metro.Trains.Driving;

namespace Metro.Trains.Doors
{

    public sealed class DoorController : AssemblyComponent
    {

        private const float CloseThreshold = 5;

        private readonly List<MetroDoor> _doors = new();

        private bool _target;

        private void Update()
        {
            if (_target && Parent.Driver.SecondsToDeparture <= CloseThreshold)
                SetDoors(false);
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _doors.AddRange(car.Components<MetroDoor>());
            OnStateChanged();
        }

        public override void OnStateChanged() => SetDoors(State == DriverState.Stopped);

        private void SetDoors(bool open)
        {
            _target = open;
            foreach (var door in _doors)
                door.Open = _target && door.Reverse == Parent.Motor.Reverse; // TODO: use journey direction or track direction
        }

    }

}

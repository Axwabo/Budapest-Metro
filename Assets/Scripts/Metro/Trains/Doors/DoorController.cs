using System.Collections.Generic;
using Metro.Stations;
using Metro.Trains.Cars;
using Metro.Trains.Driving;
using UnityEngine;

namespace Metro.Trains.Doors
{

    public sealed class DoorController : AssemblyComponent, IDepartureBlocker
    {

        [SerializeField]
        private AudioClip beep;

        private readonly List<MetroDoor> _doors = new();
        private readonly List<Speaker> _speakers = new();

        private float _closeDelay;

        private float _lastBeeped = float.MinValue;

        private float _openDelay;

        private bool _reverse;

        private bool _target;

        private void Update()
        {
            if (_openDelay > 0 && (_openDelay -= Clock.Delta) <= 0)
                SetDoors(true);
            if (State != DriverState.WaitingForDeparture)
                return;
            if (!CanDepart && Mathf.Abs(_lastBeeped - _closeDelay) >= 0.5f)
                Beep(_closeDelay);
            if (_closeDelay <= 0)
                SetDoors(false);
            _closeDelay -= Clock.Delta;
        }

        public bool CanDepart
        {
            get
            {
                foreach (var door in _doors)
                    if (!door.CanDepart)
                        return false;
                return true;
            }
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
            {
                _doors.AddRange(car.Components<MetroDoor>());
                _speakers.AddRange(car.Components<Speaker>());
            }
        }

        public override void OnStateChanged()
        {
            switch (State)
            {
                case DriverState.Stopped when IsInService:
                    _openDelay = JourneyManager.IsDestination ? 3 : 1;
                    _reverse = Station.TryGetLoadad(Stop.Name, out var station)
                        ? station.Track(Journey.Reverse).Reverse
                        : Journey.Reverse;
                    return;
                case DriverState.WaitingForDeparture when _target || JourneyManager.IsInService:
                    _closeDelay = 3;
                    break;
                case DriverState.Driving:
                    foreach (var door in _doors)
                        door.Diode.On = false;
                    break;
            }
        }

        public override void OnJourneyChanged() => OnStateChanged();

        private void Beep(float at)
        {
            foreach (var speaker in _speakers)
                speaker.PlayOneShit(beep);
            _lastBeeped = at;
            foreach (var door in _doors)
                if (door.Reverse == _reverse)
                    door.Diode.Toggle();
        }

        private void SetDoors(bool open)
        {
            if (_target == open)
                return;
            _target = open;
            foreach (var door in _doors)
                if (door.Reverse == _reverse)
                    door.Open = _target;
        }

    }

}

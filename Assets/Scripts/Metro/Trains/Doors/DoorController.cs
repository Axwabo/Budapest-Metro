using System.Collections.Generic;
using Metro.Trains.Cars;
using Metro.Trains.Driving;
using UnityEngine;

namespace Metro.Trains.Doors
{

    public sealed class DoorController : AssemblyComponent, IDepartureBlocker
    {

        private const float CloseThreshold = 4;

        [SerializeField]
        private AudioClip beep;

        private readonly List<MetroDoor> _doors = new();
        private readonly List<Speaker> _speakers = new();

        private float _lastBeeped;

        private float _openDelay;

        private bool _target;

        private void Update()
        {
            if (_openDelay > 0 && (_openDelay -= Clock.Delta) <= 0)
                SetDoors(true);
            if (State != DriverState.WaitingForDeparture)
                return;
            var seconds = Parent.Driver.SecondsSinceTargetDeparture;
            if (!CanDepart && Mathf.Abs(_lastBeeped - seconds) >= 0.5f)
                Beep(seconds);
            if (seconds >= CloseThreshold)
                SetDoors(false);
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

        private void Beep(float at)
        {
            foreach (var speaker in _speakers)
                speaker.PlayOneShit(beep);
            _lastBeeped = at;
            foreach (var door in _doors)
                if (door.Reverse == Parent.Motor.Reverse)
                    door.Diode.Toggle();
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
            if (State == DriverState.Stopped && JourneyManager.IsInService)
            {
                _openDelay = 1;
                return;
            }

            if (State != DriverState.Driving)
                return;
            foreach (var door in _doors)
                door.Diode.On = false;
        }

        public override void OnJourneyChanged() => OnStateChanged();

        private void SetDoors(bool open)
        {
            if (_target == open)
                return;
            _target = open;
            foreach (var door in _doors)
                if (door.Reverse == Parent.Motor.Reverse)
                    door.Open = _target; // TODO: use journey direction or track direction
        }

    }

}

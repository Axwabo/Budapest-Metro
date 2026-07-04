using System.Collections.Generic;
using Metro.Trains.Cars;
using Metro.Trains.Driving;
using UnityEngine;

namespace Metro.Trains.Doors
{

    public sealed class DoorController : AssemblyComponent
    {

        private const float BeepThreshold = 7;
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
            if (State == DriverState.Driving)
                return;
            var seconds = Parent.Driver.SecondsToDeparture;
            if (seconds is > 0 and < BeepThreshold && Mathf.Abs(_lastBeeped - seconds) >= 0.5f)
                Beep(seconds);
            if (seconds <= CloseThreshold)
                SetDoors(false);
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

            OnStateChanged();
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Stopped)
            {
                _openDelay = 1;
                return;
            }

            SetDoors(false);
            if (State != DriverState.Driving)
                return;
            foreach (var door in _doors)
                door.Diode.On = false;
        }

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

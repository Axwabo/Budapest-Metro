using System;
using System.Collections.Generic;
using System.Linq;
using Metro.Audio;
using Metro.Stations;
using Metro.Trains.Driving;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metro.Trains.Doors
{

    public sealed class DoorController : AssemblyComponent, IDepartureBlocker, IAudioSourceProvider
    {

        private const float BeepDelay = 0.5f;
        private const float SecondaryBeepDelay = 1.013f;

        [SerializeField]
        private AudioClip beep;

        [SerializeField]
        private AudioClip beep2;

        private readonly List<MetroDoor> _doors = new();

        private float _closeDelay;

        private AudioSource[] _doorSources;

        private bool _hasBeep2;

        private float _lastBeeped = float.MinValue;

        private float _lastBeeped2 = float.MinValue;

        private float _openDelay;

        private bool _reverse;

        private Speaker _speaker;

        private bool _target;

        private void Update()
        {
            if (_openDelay > 0 && (_openDelay -= Clock.Delta) <= 0)
                SetDoors(true);
            if (State != DriverState.WaitingForDeparture)
                return;
            if (!CanDepart)
            {
                if (Mathf.Abs(_lastBeeped - _closeDelay) >= BeepDelay)
                    Beep();
                if (_hasBeep2 && Mathf.Abs(_lastBeeped2 - _closeDelay) >= SecondaryBeepDelay)
                {
                    _speaker.PlayOneShit(beep2);
                    _lastBeeped2 = _closeDelay;
                }
            }

            if (_closeDelay <= 0)
                SetDoors(false);
            _closeDelay -= Clock.Delta;
        }

        public ReadOnlySpan<AudioSource> MultipleAudioSources => _doorSources;

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
            _hasBeep2 = beep2;
            _speaker = Parent.RequireComponent<Speaker>();
            foreach (var car in Parent.Cars)
                _doors.AddRange(car.Components<MetroDoor>());
            _doorSources = _doors.Select(e => e.SingleAudioSource).ToArray();
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
                    _lastBeeped2 = _closeDelay - Random.value * SecondaryBeepDelay * 3;
                    break;
                case DriverState.Driving:
                    foreach (var door in _doors)
                        door.Diode.On = false;
                    break;
            }
        }

        public override void OnJourneyChanged() => OnStateChanged();

        private void Beep()
        {
            _speaker.PlayOneShit(beep);
            _lastBeeped = _closeDelay;
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

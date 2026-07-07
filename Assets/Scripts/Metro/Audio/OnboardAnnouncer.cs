using System.Collections.Generic;
using Metro.Journeys.Routes;
using Metro.Trains;
using Metro.Trains.Cars;
using Metro.Trains.Driving;
using UnityEngine;

namespace Metro.Audio
{

    public sealed class OnboardAnnouncer : AssemblyComponent
    {

        [SerializeField]
        private float departureDelay = 5;

        [SerializeField]
        private float arrivingSpeedThreshold = 7;

        [SerializeField]
        private float stoppedDelay = 4;

        [Header("Traffic Area <-> Service Area")]
        [SerializeField]
        private AudioClip serviceArea;

        [SerializeField]
        private AudioClip trafficArea;

        private readonly List<Speaker> _speakers = new();

        private bool _arrivingPlayed;

        private float _delay;

        private bool _serviceAreaPlayed;

        private bool? _wasInService;

        private void Update()
        {
            if (!IsInService)
                return;
            if (_delay > 0 && (_delay -= Clock.Delta) <= 0)
            {
                Play(Route, Stop.Name, State == DriverState.Stopped ? AnnouncementType.Stopped : AnnouncementType.Next);
                return;
            }

            if (_arrivingPlayed || !JourneyManager.IsDestination && Parent.Motor.AbsoluteSpeed > arrivingSpeedThreshold || !Driver.IsOnTargetTrack)
                return;
            _arrivingPlayed = true;
            Play(Route, Stop.Name, AnnouncementType.Arriving);
        }

        private void Play(Route route, string station, AnnouncementType arriving)
        {
            if (route.Descriptor.Pack.TryGetClip(station, arriving, out var clip))
                Play(clip);
        }

        private void Play(AudioClip clip)
        {
            foreach (var speaker in _speakers)
                speaker.Play(clip);
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _speakers.AddRange(car.Components<Speaker>());
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Driving && !IsInService && !_serviceAreaPlayed)
            {
                _serviceAreaPlayed = true;
                Play(serviceArea);
            }

            if (IsInService && State == DriverState.Stopped)
                _delay = stoppedDelay;
        }

        public override void OnJourneyChanged()
        {
            var service = IsInService;
            if (_wasInService == service)
                return;
            var isNull = _wasInService == null;
            _wasInService = service;
            if (isNull)
                return;
            if (service)
                Play(trafficArea);
            else
                _serviceAreaPlayed = false;
        }

        public override void OnStopChanged()
        {
            _arrivingPlayed = JourneyManager.IsOrigin;
            if (IsInService && State == DriverState.Driving)
                _delay = departureDelay;
        }

    }

}

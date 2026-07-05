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

        private readonly List<Speaker> _speakers = new();

        private bool _arrivingPlayed;

        private float _delay;

        private void Update()
        {
            if (!IsInService)
                return;
            if (_delay > 0 && (_delay -= Clock.Delta) <= 0)
            {
                Play(Route, Stop.Name, State == DriverState.Stopped ? AnnouncementType.Stopped : AnnouncementType.Next);
                return;
            }

            if (_arrivingPlayed || !JourneyManager.IsDestination && Parent.Motor.AbsoluteSpeed > arrivingSpeedThreshold || !Parent.Driver.IsOnTargetTrack)
                return;
            _arrivingPlayed = true;
            Play(Route, Stop.Name, AnnouncementType.Arriving);
        }

        private void Play(Route route, string station, AnnouncementType arriving)
        {
            if (!route.Descriptor.Pack.TryGetClip(station, arriving, out var clip))
                return;
            foreach (var speaker in _speakers)
                speaker.Play(clip);
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _speakers.AddRange(car.Components<Speaker>());
        }

        public override void OnStopChanged()
        {
            _arrivingPlayed = JourneyManager.IsOrigin;
            if (JourneyManager.IsInService && State == DriverState.Driving)
                _delay = departureDelay;
        }

        public override void OnStateChanged()
        {
            if (JourneyManager.IsInService && State == DriverState.Stopped)
                _delay = stoppedDelay;
        }

    }

}

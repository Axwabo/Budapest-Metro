using System.Collections.Generic;
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

        private readonly List<Speaker> _speakers = new();

        private bool _arrivingPlayed;

        private float _delay;

        private void Update()
        {
            if (JourneyManager.Stop is not {Name: var station})
                return;
            // TODO: stopped state
            AnnouncementType? type = _delay > 0 && (_delay -= Clock.Delta) <= 0
                ? AnnouncementType.Next
                : !_arrivingPlayed && Parent.Motor.AbsoluteSpeed < 7 && Parent.Driver.IsOnTargetTrack
                    ? AnnouncementType.Arriving
                    : null;
            if (type == null || !CurrentJourney.Pack.TryGetClip(station, type.Value, out var clip))
                return;
            if (type == AnnouncementType.Arriving)
                _arrivingPlayed = true;
            foreach (var speaker in _speakers)
                speaker.Play(clip);
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _speakers.AddRange(car.Components<Speaker>());
        }

        public override void OnStationChanged()
        {
            _arrivingPlayed = false;
            if (JourneyManager.IsInService && State == DriverState.Driving)
                _delay = departureDelay;
        }

    }

}

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

        private float _delay;

        private void Update()
        {
            if (_delay > 0 && (_delay -= Clock.Delta) <= 0 && JourneyManager.Stop is {Name: var station} && CurrentJourney.Pack.TryGetClip(station, false, out var clip))
                Play(clip);
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _speakers.AddRange(car.Components<Speaker>());
        }

        public override void OnStationChanged()
        {
            if (JourneyManager.IsInService && State == DriverState.Driving)
                _delay = departureDelay;
        }

        private void Play(AudioClip clip)
        {
            foreach (var speaker in _speakers)
                speaker.Play(clip);
        }

    }

}

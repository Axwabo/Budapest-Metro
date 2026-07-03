using System;
using System.Collections.Generic;
using System.Linq;
using Metro.Rail;
using Metro.Trains.Cars;
using Metro.Trains.Driving;
using Metro.Trains.Routes;
using UnityEngine;

namespace Metro.Trains
{

    public sealed class MetroAssembly : MonoBehaviour
    {

        [SerializeField]
        public TrackSegment startingTrack;

        [SerializeField]
        [Range(0, Constants.MaxMps)]
        private float speed;

        [field: SerializeField]
        public bool Reverse { get; set; }

        private readonly List<AssemblyComponent> _components = new();

        private MetroCar[] _cars;

        public float AbsoluteSpeed => Reverse ? -speed : speed;

        public float RelativeSpeed
        {
            set => speed = Mathf.Clamp(value, 0, Constants.MaxMps);
        }

        public JourneyManager JourneyManager { get; private set; }

        public OnboardDisplayRenderer DisplayRenderer { get; private set; }

        public AutomaticDriver Driver { get; private set; }

        public ReadOnlySpan<MetroCar> Cars => _cars;

        private void Start()
        {
            this.GetComponentsInImmediateChildren(_components);
            _cars = _components.OfType<MetroCar>().ToArray();
            JourneyManager = _components.OfType<JourneyManager>().First();
            DisplayRenderer = _components.OfType<OnboardDisplayRenderer>().First();
            Driver = _components.OfType<AutomaticDriver>().First();
            this.InitializeComponents(_components);
            JourneyManager.Begin();
        }

        public void NotifyStateChanged()
        {
            foreach (var component in _components)
                component.OnStateChanged();
        }

        public void NotifyStationChanged()
        {
            foreach (var component in _components)
                component.OnStationChanged();
        }

    }

}

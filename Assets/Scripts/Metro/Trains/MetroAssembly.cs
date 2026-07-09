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

    public sealed class MetroAssembly : MonoBehaviour, ISubcomponentParent
    {

        [SerializeField]
        public TrackSegment startingTrack;

        private readonly List<AssemblyComponent> _components = new();

        private MetroCar[] _cars;

        public Motor Motor { get; private set; }

        public JourneyManager JourneyManager { get; private set; }

        public AutomaticDriver Driver { get; private set; }

        public ReadOnlySpan<MetroCar> Cars => _cars;

        public MetroCar PrimaryCar => _cars[Motor.Reverse ? ^1 : 0];

        public Axle PrimaryAxle => Motor.Reverse ? PrimaryCar.BackAxle : PrimaryCar.FrontAxle;

        private void Start()
        {
            this.GetComponentsInImmediateChildren(_components);
            _cars = Components<MetroCar>().ToArray();
            Motor = this.RequireComponent<Motor>();
            JourneyManager = this.RequireComponent<JourneyManager>();
            Driver = this.RequireComponent<AutomaticDriver>();
            this.InitializeComponents(_components);
            JourneyManager.Begin();
        }

        public IEnumerable<T> Components<T>() => _components.OfType<T>();

        public void NotifyStateChanged()
        {
            foreach (var component in _components)
                component.OnStateChanged();
        }

        public void NotifyJourneyChanged()
        {
            foreach (var component in _components)
                component.OnJourneyChanged();
        }

        public void NotifyTargetChanged()
        {
            foreach (var component in _components)
                component.OnTargetChanged();
        }

        public void NotifyStopChanged()
        {
            foreach (var component in _components)
                component.OnStopChanged();
        }

    }

}

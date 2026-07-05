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

        private readonly List<AssemblyComponent> _components = new();

        private MetroCar[] _cars;

        public Motor Motor { get; private set; }

        public JourneyManager JourneyManager { get; private set; }

        public AutomaticDriver Driver { get; private set; }

        public ReadOnlySpan<MetroCar> Cars => _cars;

        public Axle PrimaryAxle
        {
            get
            {
                var car = _cars[Motor.Reverse ? ^1 : 0];
                return Motor.Reverse ? car.BackAxle : car.FrontAxle;
            }
        }

        private void Start()
        {
            this.GetComponentsInImmediateChildren(_components);
            _cars = _components.OfType<MetroCar>().ToArray();
            Motor = RequireComponent<Motor>();
            JourneyManager = RequireComponent<JourneyManager>();
            Driver = RequireComponent<AutomaticDriver>();
            this.InitializeComponents(_components);
            if (JourneyManager.Route)
                JourneyManager.Begin(JourneyManager.Route);
            else
                JourneyManager.Idle();
        }

        public T RequireComponent<T>() => _components.OfType<T>().First();

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

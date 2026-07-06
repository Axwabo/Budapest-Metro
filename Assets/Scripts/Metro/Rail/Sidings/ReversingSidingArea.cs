using System;
using System.Collections.Generic;
using Metro.Journeys;
using Metro.Journeys.Routes;
using Metro.Stations;
using Metro.Trains;
using UnityEngine;

namespace Metro.Rail.Sidings
{

    public sealed class ReversingSidingArea : MonoBehaviour
    {

        [SerializeField]
        private ReversingSiding[] sidings;

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public RouteDescriptor Route { get; private set; }

        public HashSet<MetroAssembly> PassingThrough { get; } = new();

        public ReadOnlySpan<ReversingSiding> Sidings => sidings;

        private void Start()
        {
            foreach (var siding in sidings)
                siding.Area = this;
        }

#nullable enable

        public IJourney? Enter(MetroAssembly assembly)
        {
            if (PassingThrough.Count != 0)
                return null;
            foreach (var siding in sidings)
                if (siding.Enter(assembly) is { } journey)
                    return journey;
            return null;
        }

        public IJourney? Exit(MetroAssembly assembly)
        {
            if (PassingThrough.Count != 0 || !Route || !Station.TryGetLoadad(Route.Origin, out var station) || (Reverse ? station.Right : station.Left).Light.State != LightState.On)
                return null;
            foreach (var siding in sidings)
                if (siding.Exit(assembly) is { } journey)
                    return journey;
            return null;
        }

    }

}

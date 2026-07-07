using System;
using System.Collections.Generic;
using Metro.Journeys;
using Metro.Journeys.Routes;
using Metro.Rail.Controls;
using Metro.Stations;
using Metro.Trains;
using UnityEngine;
using UnityEngine.Serialization;

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

        [field: FormerlySerializedAs("<ServiceTarget>k__BackingField")]
        [field: SerializeField]
        public ServiceEntryStopPoint HouseTarget { get; private set; }

        [field: FormerlySerializedAs("<ServiceSwitches>k__BackingField")]
        [field: SerializeField]
        public SwitchGroup HouseSwitches { get; private set; }

        private ServiceJourney _house;

        public RouteRotor CarriageHouse { get; set; }

        public HashSet<MetroAssembly> PassingThrough { get; } = new();

        public ReadOnlySpan<ReversingSiding> Sidings => sidings;

        private void Start()
        {
            if (HouseTarget)
                _house = new ServiceJourney(HouseTarget);
        }

        private void OnEnable()
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

        public IJourney? Exit(MetroAssembly assembly, Route? route)
        {
            if (PassingThrough.Count != 0)
                return null;
            if (route != null)
            {
                if (!Route || route.Descriptor != Route || !Station.TryGetLoadad(Route.Origin, out var station) || station.Track(!Reverse).IsOccupied)
                    return null;
                foreach (var siding in sidings)
                    if (siding.Exit(assembly))
                        return new EnteringJourney(!Reverse, route);
                return null;
            }

            if (_house == null)
            {
                Debug.LogError("Cannot dispatch from/to house", this);
                throw new InvalidOperationException("Cannot dispatch from/to house");
            }

            foreach (var siding in sidings)
                if (siding.Exit(assembly))
                    return _house;
            return null;
        }

        public void NotifyArrived(MetroAssembly assembly) => CarriageHouse.NotifyArrived(assembly, this);

    }

}

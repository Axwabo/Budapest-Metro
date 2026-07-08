using System;
using System.Collections.Generic;
using Metro.Journeys;
using Metro.Journeys.Routes;
using Metro.Rail.Controls;
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
        [field: Header("Traffic")]
        public RouteDescriptor Route { get; private set; }

        [field: SerializeField]
        public SwitchGroup TrafficSwitches { get; private set; }

        [field: SerializeField]
        [field: Header("Service")]
        public ServiceEntryStopPoint ServiceTarget { get; private set; }

        [field: SerializeField]
        public SwitchGroup ServiceSwitches { get; private set; }

        public ServiceJourney ServiceJourney { get; private set; }

        public RouteRotor CarriageHouse { get; set; }

        public HashSet<MetroAssembly> PassingThrough { get; } = new();

        public ReadOnlySpan<ReversingSiding> Sidings => sidings;

        public bool ExitingPrevented => PassingThrough.Count != 0 || !StationTrackFree;

        private bool StationTrackFree => !Route || !Station.TryGetLoadad(Route.Origin, out var station) || !station.Track(!Reverse).IsOccupied;

        private void Start()
        {
            if (ServiceTarget)
                ServiceJourney = new ServiceJourney(ServiceTarget);
        }

        private void OnEnable()
        {
            foreach (var siding in sidings)
                siding.Area = this;
        }

#nullable enable

        public bool Enter(MetroAssembly assembly)
        {
            if (PassingThrough.Count != 0)
                return false;
            foreach (var siding in sidings)
                if (siding.Enter(assembly))
                    return true;
            return false;
        }

        public bool Exit(MetroAssembly assembly, bool isHouseDispatchOrRecall)
        {
            if (ExitingPrevented)
                return false;
            if (!isHouseDispatchOrRecall)
            {
                foreach (var siding in sidings)
                    if (siding.Exit(assembly))
                        return Activate(TrafficSwitches);
                return false;
            }

            foreach (var siding in sidings)
                if (siding.Exit(assembly))
                    return Activate(ServiceSwitches);
            return false;
        }

        private static bool Activate(SwitchGroup group)
        {
            if (group)
                group.Activate();
            return true;
        }

        public void NotifyArrived(MetroAssembly assembly) => CarriageHouse.NotifyArrived(assembly, this);

        public void NotifyReady(MetroAssembly assembly) => CarriageHouse.NotifyReady(assembly, this);

    }

}

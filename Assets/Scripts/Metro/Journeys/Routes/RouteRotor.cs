using System;
using System.Collections.Generic;
using Metro.Rail;
using Metro.Rail.Sidings;
using Metro.Stations;
using Metro.Trains;
using Metro.Trains.Routes;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    public sealed class RouteRotor : MonoBehaviour
    {

        private const float ReverseDelay = 10;
        private const float RecallDelay = 180;

        private static readonly TimeSpan StopTimeThreshold = TimeSpan.FromSeconds(22);
        private static readonly TimeSpan HouseToDeparture = TimeSpan.FromMinutes(4);
        private static readonly TimeSpan MaxEarlyDispatch = TimeSpan.FromMinutes(8);
        private static readonly TimeSpan MaxEarlyRotate = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan ReversingToDeparture = TimeSpan.FromSeconds(40);

        [SerializeField]
        private JourneyManager prefab;

        [SerializeField]
        private ReversingSidingArea house;

        [SerializeField]
        private ReversingSidingArea entry;

        [SerializeField]
        private ReversingSidingArea reverse;

        private readonly HashSet<JourneyManager> _dispatching = new();

        private readonly List<JourneyManager> _enteryMetros = new();

        private readonly List<JourneyManager> _housedMetros = new();
        private readonly HashSet<JourneyManager> _recalling = new();

        private readonly List<JourneyManager> _reverseMetros = new();

        private readonly HashSet<Route> _spawnedRoutes = new();

        private int _spawned;

        private int Max => house.Sidings.Length;

        private void Start() => reverse.CarriageHouse = entry.CarriageHouse = house.CarriageHouse = this;

        private void Update()
        {
            if (_spawned == 0)
            {
                Spawn();
                return;
            }

            Rotate(_enteryMetros, entry, true);
            Rotate(_reverseMetros, reverse, false);
            Dispatch();
        }

        private void Spawn()
        {
            Spawn(entry.Route);
            Spawn(reverse.Route);
            Spawn(_enteryMetros, entry);
            Spawn(_reverseMetros, reverse);
            foreach (var siding in house.Sidings)
                if (siding.UsedBy.Count == 0)
                    Spawn(_housedMetros, siding);
        }

        private void Spawn(RouteDescriptor descriptor)
        {
            var now = Clock.Now - StopTimeThreshold;
            foreach (var route in descriptor.GetRoutes())
            {
                if (_spawnedRoutes.Contains(route) || route.Destination.Time < now)
                    continue;
                for (var i = 0; i < route.IntermediateStops.Count; i++)
                {
                    var (stop, currentTime) = route.IntermediateStops[i];
                    if (!Station.TryGetLoadad(stop, out var station))
                        continue;
                    var previousTime = i == 0 ? route.Origin.Time : route.IntermediateStops[i - 1].Time;
                    if (previousTime > now || currentTime < now || ++_spawned > Max)
                        continue;
                    var manager = Spawn(station.Track(route.Reverse)).Item1;
                    manager.InitialJourney = route;
                    manager.InitialStopIndex = i;
                    _spawnedRoutes.Add(route);
                }
            }
        }

        private void Spawn(List<JourneyManager> metros, ReversingSidingArea area)
        {
            if (area.ExitingPrevented || Next(area) is not { } route)
                return;
            foreach (var siding in area.Sidings)
                if (siding.UsedBy.Count == 0 && _spawnedRoutes.Add(route))
                    Spawn(metros, siding);
        }

        private void Spawn(List<JourneyManager> metros, ReversingSiding siding)
        {
            if (++_spawned > Max)
                return;
            var (manager, assembly) = Spawn(siding.StopPoint.Track);
            manager.InitialJourney = new Afk {Target = siding.StopPoint};
            siding.UsedBy.Add(assembly);
            metros.Add(manager);
        }

        private (JourneyManager, MetroAssembly) Spawn(TrackSegment track)
        {
            var clone = Instantiate(prefab);
            var assembly = clone.GetComponentInParent<MetroAssembly>();
            assembly.startingTrack = track;
            assembly.gameObject.name = (499 + _spawned).ToString();
            return (clone, assembly);
        }

        private void Rotate(List<JourneyManager> metros, ReversingSidingArea area, bool checkRecall)
        {
            if (metros.Count == 0 || area.ExitingPrevented)
                return;
            var next = Next(area);
            var toHouse = next == null;
            if (toHouse && checkRecall && _recalling.Count != 0)
                return;
            var metro = metros[0];
            if (!area.Exit(metro.Parent, toHouse))
                return;
            if (toHouse && checkRecall)
            {
                _recalling.Add(metro);
                area.ServiceJourney.ToCarriageHouse = true;
            }

            metro.Driver.MarkReadyNow();
            metro.Begin(toHouse ? area.ServiceJourney : new EnteringJourney(!area.Reverse, next));
            metros.Remove(metro);
            if (next != null)
                _spawnedRoutes.Add(next);
        }

        private void Dispatch()
        {
            if (_housedMetros.Count == 0 || _dispatching.Count != 0 || house.ExitingPrevented)
                return;
            var next = entry.Route.Next(HouseToDeparture, MaxEarlyDispatch);
            if (next == null || _spawnedRoutes.Contains(next))
                return;
            foreach (var manager in _housedMetros)
            {
                if (!house.Exit(manager.Parent, true))
                    continue;
                manager.Begin(house.ServiceJourney);
                manager.Driver.MarkReadyNow();
                _housedMetros.Remove(manager);
                _spawnedRoutes.Add(next);
                _dispatching.Add(manager);
                break;
            }
        }

        public void NotifyArrived(MetroAssembly assembly, ReversingSidingArea area)
        {
            if (area == house)
            {
                var metro = assembly.JourneyManager;
                metro.Idle();
                _housedMetros.Add(metro);
                _recalling.Remove(metro);
            }
            else if (area == entry)
            {
                _dispatching.Remove(prefab);
                assembly.Driver.MarkReady(ReverseDelay);
            }
            else if (area == reverse)
            {
                var delay = Next(area) == null ? RecallDelay : ReverseDelay;
                assembly.Driver.MarkReady(delay);
            }
            // else where tf are you???? M2 > M3?
        }

        private static Route Next(ReversingSidingArea area) => area.Route.Next(ReversingToDeparture, MaxEarlyRotate);

        public void NotifyReady(MetroAssembly assembly, ReversingSidingArea area)
        {
            if (area == entry)
                _enteryMetros.Add(assembly.JourneyManager);
            else if (area == reverse)
                _reverseMetros.Add(assembly.JourneyManager);
        }

    }

}

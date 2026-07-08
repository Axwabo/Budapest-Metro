using System;
using System.Collections.Generic;
using Metro.Rail;
using Metro.Rail.Sidings;
using Metro.Trains;
using Metro.Trains.Routes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metro.Journeys.Routes
{

    public sealed class RouteRotor : MonoBehaviour
    {

        private static readonly TimeSpan StopTimeThreshold = TimeSpan.FromSeconds(22);
        private static readonly TimeSpan HouseToDeparture = TimeSpan.FromMinutes(4);
        private static readonly TimeSpan MaxEarlyDispatch = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan ReversingToDeparture = TimeSpan.FromSeconds(40);

        [SerializeField]
        private JourneyManager prefab;

        [SerializeField]
        private ReversingSidingArea house;

        [SerializeField]
        private ReversingSidingArea entry;

        [SerializeField]
        private ReversingSidingArea reverse;

        private readonly List<JourneyManager> _enteryMetros = new();

        private readonly List<JourneyManager> _housedMetros = new();

        private readonly List<JourneyManager> _reverseMetros = new();

        private readonly HashSet<Route> _spawned = new();
        private readonly HashSet<string> _spawnedStations = new();

        private bool _initiallySpawned;

        private Route _lastDispatched;

        private void Start() => reverse.CarriageHouse = entry.CarriageHouse = house.CarriageHouse = this;

        private void Update()
        {
            if (!_initiallySpawned)
            {
                Spawn();
                return;
            }

            Rotate(_enteryMetros, entry);
            Rotate(_reverseMetros, reverse);
            Dispatch();
            RecallFromReverse();
            Recall();
        }

        private void Spawn()
        {
            _initiallySpawned = true;
            foreach (var siding in house.Sidings)
            {
                if (siding.UsedBy.Count != 0)
                    continue;
                var (manager, assembly) = Spawn(siding.StopPoint.Track);
                manager.InitialJourney = new Afk {Target = siding.StopPoint};
                siding.UsedBy.Add(assembly);
                _housedMetros.Add(manager);
            }
        }

        private (JourneyManager, MetroAssembly) Spawn(TrackSegment track)
        {
            var clone = Instantiate(prefab);
            var assembly = clone.GetComponentInParent<MetroAssembly>();
            assembly.startingTrack = track;
            assembly.gameObject.name = Random.Range(500, 559).ToString();
            return (clone, assembly);
        }

        private static void Rotate(List<JourneyManager> metros, ReversingSidingArea area)
        {
            if (area.ExitingPrevented || metros.Count == 0)
                return;
            var next = area.Route.Next(ReversingToDeparture);
            if (next.Origin.Time > Clock.Now + MaxEarlyDispatch)
                return;
            var metro = metros[0];
            if (!area.Exit(metro.Parent, false))
                return;
            metro.Begin(new EnteringJourney(!area.Reverse, next));
            metros.Remove(metro);
        }

        private void Dispatch()
        {
            if (_housedMetros.Count == 0 || house.PassingThrough.Count != 0)
                return;
            var next = entry.Route.Next(HouseToDeparture);
            if (next == _lastDispatched || next.Origin.Time < Clock.Now + MaxEarlyDispatch)
                return;
            foreach (var manager in _housedMetros)
            {
                if (!house.Exit(manager.Parent, true))
                    continue;
                manager.Begin(house.ServiceJourney);
                manager.Driver.MarkReadyNow();
                _housedMetros.Remove(manager);
                _lastDispatched = next;
                break;
            }
        }

        private void RecallFromReverse()
        {
            if (_reverseMetros.Count == 0 || reverse.ExitingPrevented)
                return;
            var next = entry.Route.Next(ReversingToDeparture);
            if (next.Origin.Time < Clock.Now + MaxEarlyDispatch)
                return;
            var metro = _reverseMetros[0];
            if (!reverse.Exit(metro.Parent, true))
                return;
            metro.Begin(reverse.ServiceJourney);
            _reverseMetros.Remove(metro);
        }

        private void Recall()
        {
            if (_enteryMetros.Count == 0 || entry.ExitingPrevented)
                return;
            var next = entry.Route.Next(ReversingToDeparture);
            if (next.Origin.Time < Clock.Now + MaxEarlyDispatch)
                return;
            var metro = _enteryMetros[0];
            if (!entry.Exit(metro.Parent, true))
                return;
            // TODO: delay
            metro.Begin(entry.ServiceJourney);
            _enteryMetros.Remove(metro);
        }

        public void NotifyArrived(MetroAssembly assembly, ReversingSidingArea area)
        {
            if (area == house)
            {
                var metro = assembly.JourneyManager;
                metro.Idle();
                _housedMetros.Add(metro);
            }
            else if (area == entry)
            {
                assembly.Driver.MarkReady(10);
            }
            else if (area == reverse)
            {
                assembly.Driver.MarkReady(10);
            }
            // else where tf are you???? M2 > M3?
        }

        public void NotifyReady(MetroAssembly assembly, ReversingSidingArea area)
        {
            if (area == entry)
                _enteryMetros.Add(assembly.JourneyManager);
            else if (area == reverse)
                _reverseMetros.Add(assembly.JourneyManager);
        }

    }

}

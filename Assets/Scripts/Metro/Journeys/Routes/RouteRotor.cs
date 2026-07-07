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

        [SerializeField]
        private JourneyManager prefab;

        [SerializeField]
        private ReversingSidingArea house;

        [SerializeField]
        private ReversingSidingArea entry;

        [SerializeField]
        private ReversingSidingArea reverse;

        private readonly HashSet<JourneyManager> _housedMetros = new();

        private readonly HashSet<Route> _spawned = new();
        private readonly HashSet<string> _spawnedStations = new();

        private bool _initiallySpawned;

        private Route _lastDispatched;

        private void Start() => reverse.CarriageHouse = entry.CarriageHouse = house.CarriageHouse = this;

        private void Update()
        {
            if (_initiallySpawned)
            {
                DispatchAndRecall();
                return;
            }

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

        private void DispatchAndRecall()
        {
            // TODO: continuous
            if (_lastDispatched != null || _housedMetros.Count == 0 || house.PassingThrough.Count != 0)
                return;
            var next = entry.Route.Next(TimeSpan.FromMinutes(3));
            if (next == null)
                return;
            foreach (var manager in _housedMetros)
            {
                break;
            }
        }

        public void NotifyArrived(MetroAssembly assembly, ReversingSidingArea area)
        {
        }

    }

}

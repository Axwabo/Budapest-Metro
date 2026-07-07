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
        private int max; // TODO

        [SerializeField]
        private ReversingSidingArea house;

        [SerializeField]
        private ReversingSidingArea[] reversing;

        private readonly HashSet<JourneyManager> _housedMetros = new();

        private readonly HashSet<Route> _spawned = new();
        private readonly HashSet<string> _spawnedStations = new();

        private bool _initiallySpawned;

        private void Start()
        {
            house.House = this;
            foreach (var area in reversing)
                area.House = this;
        }

        private void Update()
        {
            /*
            Spawned.Add(area.Route.Next(TimeSpan.FromSeconds(40)));
            var now = Clock.Now - StopTimeThreshold;
            foreach (var route in area.Route.GetRoutes())
            {
                if (Spawned.Contains(route))
                    continue;
                for (var i = 0; i < route.IntermediateStops.Count; i++)
                {
                    var stop = route.IntermediateStops[i];
                    var previous = i == 0 ? route.Origin.Time : stop.Time;
                    if (SpawnedStations.Contains(stop.Name) || previous < now || stop.Time >= now || !Station.TryGetLoadad(stop.Name, out var station))
                        continue;
                    var (manager, _) = Spawn(station.Track(route.Reverse));
                    manager.InitialJourney = route;
                    manager.InitialStopIndex = i;
                    Spawned.Add(route);
                    SpawnedStations.Add(stop.Name);
                    break;
                }
            }
            */

            if (_initiallySpawned)
                return;
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

    }

}

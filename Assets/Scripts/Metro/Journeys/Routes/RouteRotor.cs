using System;
using System.Collections.Generic;
using Metro.Rail.Sidings;
using Metro.Stations;
using Metro.Trains;
using Metro.Trains.Routes;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    public sealed class RouteRotor : MonoBehaviour
    {

        private static readonly HashSet<Route> Spawned = new();

        [SerializeField]
        private JourneyManager prefab;

        [SerializeField]
        private ReversingSidingArea area;

        private void Update()
        {
            var start = Clock.Now - TimeSpan.FromSeconds(30);
            var end = start + TimeSpan.FromMinutes(1);
            foreach (var route in area.Route.GetRoutes())
            {
                if (!Spawned.Add(route))
                    continue;
                for (var i = 0; i < route.IntermediateStops.Count; i++)
                {
                    var stop = route.IntermediateStops[i];
                    if (stop.Time < start || stop.Time >= end || !Station.TryGetLoadad(stop.Name, out var station))
                        continue;
                    var clone = Instantiate(prefab);
                    var assembly = clone.GetComponentInParent<MetroAssembly>();
                    clone.InitialJourney = route;
                    clone.InitialStopIndex = i;
                    assembly.startingTrack = route.Reverse ? station.Left : station.Right;
                    break;
                }
            }

            if (area.PassingThrough.Count != 0)
                return;
            foreach (var siding in area.Sidings)
            {
                if (siding.UsedBy.Count != 0)
                    continue;
                var clone = Instantiate(prefab);
                var assembly = clone.GetComponentInParent<MetroAssembly>();
                clone.InitialJourney = new Afk {Target = siding.StopPoint};
                assembly.startingTrack = siding.StopPoint.Track;
                siding.UsedBy.Add(assembly);
            }

            Destroy(this);
        }

    }

}

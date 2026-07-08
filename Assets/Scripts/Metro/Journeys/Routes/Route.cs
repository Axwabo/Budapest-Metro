#nullable enable

using System;
using System.Collections.Generic;
using Metro.Rail.Controls;
using Metro.Stations;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    public sealed class Route : IJourney
    {

        public Route(RouteDescriptor descriptor, Stop origin, List<Stop> intermediateStops, Stop destination)
        {
            Descriptor = descriptor;
            Origin = origin;
            IntermediateStops = intermediateStops;
            Destination = destination;
        }

        public RouteDescriptor Descriptor { get; }

        public string Relation => Descriptor.Relation.Line;

        public Stop Origin { get; }

        public List<Stop> IntermediateStops { get; }

        public Stop Destination { get; }

        public bool Reverse => Descriptor.Reverse;

        public (StopPoint Target, Stop? Stop) GetTarget(int stopIndex)
        {
            var stop = stopIndex switch
            {
                IJourney.OutOfService => throw new ArgumentOutOfRangeException(nameof(stopIndex), "Cannot get out-of-service target for a route"),
                IJourney.Origin => Origin,
                IJourney.Destination => Destination,
                _ => IntermediateStops[stopIndex]
            };
            return Station.TryGetLoadad(stop.Name, out var station)
                ? (station.Track(Reverse).StopPoint, stop)
                : throw new MissingComponentException($"Station {stop.Name} not found");
        }

    }

}

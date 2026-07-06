#nullable enable

using System;
using Metro.Rail.Controls;
using Metro.Stations;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    public sealed class Route : IJourney
    {

        public Route(RouteDescriptor descriptor) => Descriptor = descriptor;

        public RouteDescriptor Descriptor { get; }

        public string Relation => Descriptor.Relation;

        public Stop Destination => Descriptor.Destination;

        public bool Reverse => Descriptor.Reverse;

        public (StopPoint Target, Stop? Stop) GetTarget(int stopIndex)
        {
            var stop = stopIndex switch
            {
                IJourney.OutOfService => throw new ArgumentOutOfRangeException(nameof(stopIndex), "Cannot get out-of-service target for a route"),
                IJourney.Origin => Descriptor.Origin,
                IJourney.Destination => Descriptor.Destination,
                _ => Descriptor.IntermediateStops[stopIndex]
            };
            return Station.TryGetLoadad(stop.Name, out var station)
                ? ((Reverse ? station.Left : station.Right).StopPoint, stop)
                : throw new MissingComponentException($"Station {stop.Name} not found");
        }

    }

}

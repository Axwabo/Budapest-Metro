using Metro.Rail.Controls;
using Metro.Stations;
using UnityEngine;

namespace Metro.Journeys
{

    public sealed class StopTarget : ITarget
    {

        public StopTarget(JourneyDescriptor journey, Stop stop)
        {
            if (!Station.TryGetLoadad(stop.Name, out var station))
                throw new MissingComponentException($"Station {stop.Name} not found");
            Stop = stop;
            StopPoint = (journey.Reverse ? station.Left : station.Right).StopPoint;
        }

        public Stop Stop { get; }

        public StopPoint StopPoint { get; }

    }

}

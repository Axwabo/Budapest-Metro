using System;
using Metro.Journeys.Routes;
using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class EnteringJourney : IJourney
    {

        public EnteringJourney(bool reverse, RouteDescriptor route)
        {
            Next = route.Next(TimeSpan.FromSeconds(40));
            Reverse = reverse;
        }

        public Route Next { get; }

        public bool Reverse { get; }

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (Next.GetTarget(IJourney.Origin).Target, null);

    }

}

using Metro.Journeys.Routes;
using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class EnteringJourney : IJourney
    {

        public EnteringJourney(bool reverse, RouteDescriptor route)
        {
            Next = new Route(route);
            Reverse = reverse;
        }

        public Route Next { get; }

        public bool Reverse { get; }

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (Next.GetTarget(IJourney.Origin).Target, null);

    }

}

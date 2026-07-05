using Metro.Journeys.Routes;
using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class EnteringJourney : IJourney
    {

        public EnteringJourney(RouteDescriptor route) => Next = new Route(route);

        public Route Next { get; }

        public bool Reverse => Next.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (Next.GetTarget(IJourney.Origin).Target, null);

    }

}

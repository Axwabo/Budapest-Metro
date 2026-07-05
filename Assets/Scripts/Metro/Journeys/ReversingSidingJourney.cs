using Metro.Rail;
using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class ReversingSidingJourney : IJourney
    {

        public ReversingSidingJourney(ReversingSiding siding) => Siding = siding;

        public ReversingSiding Siding { get; }

        public ReversingSidingArea Area => Siding.Area;

        public bool Reverse => Siding.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (Siding.StopPoint, null);

    }

}

using Metro.Rail.Controls;
using Metro.Rail.Sidings;

namespace Metro.Journeys
{

    public sealed class ReversingSidingJourney : IJourney
    {

        private readonly ReversingSiding _siding;

        public ReversingSidingJourney(ReversingSiding siding) => _siding = siding;

        public ReversingSidingArea Area => _siding.Area;

        public bool Reverse => _siding.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (_siding.StopPoint, null);

    }

}

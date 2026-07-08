using Metro.Rail.Controls;
using Metro.Rail.Sidings;

namespace Metro.Journeys
{

    public sealed class ReversingSidingJourney : ICarriageHouseJourney
    {

        private readonly ReversingSiding _siding;

        public ReversingSidingJourney(ReversingSiding siding) => _siding = siding;

        public ReversingSidingArea Area => _siding.Area;

        public bool ToCarriageHouse { get; set; }

        public bool Reverse => Area.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (_siding.StopPoint, null);

    }

}

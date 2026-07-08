using Metro.Rail.Controls;
using Metro.Rail.Sidings;

namespace Metro.Journeys
{

    public sealed class ServiceJourney : IJourney
    {

        private readonly ServiceAreaPointBase _target;

        public ServiceJourney(ServiceAreaPointBase target, ReversingSidingArea origin)
        {
            _target = target;
            Origin = origin;
        }

        public ReversingSidingArea Origin { get; }

        public bool ToCarriageHouse { get; set; }

        public bool Reverse => _target.Area.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (_target, null);

    }

}

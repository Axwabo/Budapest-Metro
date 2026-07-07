using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class ReversingEntryJourney : IJourney
    {

        private readonly ServiceEntryStopPoint _target;

        public ReversingEntryJourney(ServiceEntryStopPoint target) => _target = target;

        public bool Reverse => _target.Area.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (_target, null);

    }

}

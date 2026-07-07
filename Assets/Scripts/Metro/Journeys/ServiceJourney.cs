using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class ServiceJourney : IJourney
    {

        private readonly ServiceAreaPointBase _target;

        public ServiceJourney(ServiceAreaPointBase target) => _target = target;

        public bool Reverse => _target.Area.Reverse;

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (_target, null);

    }

}

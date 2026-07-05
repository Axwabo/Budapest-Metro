using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public interface IJourney
    {

        public const int OutOfService = int.MinValue;
        public const int Origin = -1;
        public const int Destination = int.MaxValue;

        StopPoint GetTarget(int stopIndex);

    }

}

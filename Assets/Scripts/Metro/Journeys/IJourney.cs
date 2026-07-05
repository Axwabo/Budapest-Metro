namespace Metro.Journeys
{

    public interface IJourney
    {

        public const int OutOfService = int.MinValue;
        public const int Origin = -1;
        public const int Destination = int.MaxValue;

        ITarget GetTarget(int stopIndex);

    }

}

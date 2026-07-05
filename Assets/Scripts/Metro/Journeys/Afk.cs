using Metro.Rail.Controls;

namespace Metro.Journeys
{

    public sealed class Afk : IJourney
    {

        public static Afk Instance { get; } = new();

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (null, null);

    }

}

using Metro.Stations;

namespace Metro.Rail.Controls
{

    public sealed class StopPoint : ControlPoint
    {

        public Station Station => ((StationTrack) Track).Station;

    }

}

using Metro.Rail;

namespace Metro.Stations
{

    public static class StationExtensions
    {

        public static StationTrack Track(this Station station, bool reverse) => reverse ? station.Left : station.Right;

    }

}

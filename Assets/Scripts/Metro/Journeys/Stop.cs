using System;
using Metro.Stations;

namespace Metro.Journeys
{

    public sealed record Stop(StationId Station, TimeSpan Time);

}

using Metro.Journeys;
using Metro.Stations;
using UnityEngine;

namespace Metro.Trains.Routes
{

    public sealed class JourneyManager : AssemblyComponent
    {

        [field: SerializeField]
        public JourneyDescriptor Current { get; private set; }

        // TODO
        private int _index = -1;

        public StationId Station { get; private set; }

        public void Begin()
        {
            Station = Current.Origin;
            Parent.NotifyStationChanged();
        }

    }

}

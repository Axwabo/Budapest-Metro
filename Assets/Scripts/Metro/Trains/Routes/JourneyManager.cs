using Metro.Journeys;
using UnityEngine;

namespace Metro.Trains.Routes
{

    public sealed class JourneyManager : AssemblyComponent
    {

        [field: SerializeField]
        public JourneyDescriptor Current { get; private set; }

        // TODO
        private int _index = -1;

        public Stop Stop { get; private set; }

        public void Begin()
        {
            Stop = Current.Origin;
            Parent.NotifyStationChanged();
        }

    }

}

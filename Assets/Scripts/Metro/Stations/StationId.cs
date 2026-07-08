using UnityEngine;

namespace Metro.Stations
{

    [CreateAssetMenu(fileName = "Station ID", menuName = "Metro/Station ID")]
    public sealed class StationId : ScriptableObject
    {

        [field: SerializeField]
        public Relation Relation { get; private set; }

        [field: SerializeField]
        public string Forehead { get; private set; }

        [field: SerializeField]
        public string Onboard { get; private set; }

        [field: Header("Transfers")]
        [field: SerializeField]
        public string Metros { get; private set; }

        [field: SerializeField]
        public bool Railways { get; private set; }

        [field: SerializeField]
        public bool RegionalBuses { get; private set; }

        [field: SerializeField]
        public string Trams { get; private set; }

        [field: SerializeField]
        public string Trolleys { get; private set; }

        [field: SerializeField]
        public string LocalBuses { get; private set; }

    }

}

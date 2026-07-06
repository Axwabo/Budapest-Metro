using UnityEngine;

namespace Metro.Stations
{

    [CreateAssetMenu(fileName = "Station ID", menuName = "Metro/Station ID")]
    public sealed class StationId : ScriptableObject
    {

        [field: SerializeField]
        public string Forehead { get; private set; }

        [field: SerializeField]
        public string Onboard { get; private set; }

    }

}

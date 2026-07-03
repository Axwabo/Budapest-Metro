using UnityEngine;

namespace Metro.Stations
{

    [CreateAssetMenu(fileName = "Station ID", menuName = "Metro/Station ID")]
    public sealed class StationId : ScriptableObject
    {

        private static StationId[] _cached;

        public static StationId[] All => _cached ??= Resources.LoadAll<StationId>("Stations");

    }

}

using System;
using Metro.Stations;
using UnityEngine;

namespace Metro.Journeys
{

    [CreateAssetMenu(fileName = "Journey Descriptor", menuName = "Metro/Journey Descriptor")]
    public sealed class JourneyDescriptor : ScriptableObject
    {

        [field: SerializeField]
        public StationId Origin { get; private set; }

        [SerializeField]
        private StationId[] intermediateStations;

        [field: SerializeField]
        public StationId Destination { get; private set; }

        public ReadOnlySpan<StationId> IntermediateStations => intermediateStations;

    }

}

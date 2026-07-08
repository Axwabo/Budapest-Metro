using System;
using Metro.Audio;
using Metro.Stations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metro.Journeys.Routes
{

    [CreateAssetMenu(fileName = "Journey Descriptor", menuName = "Metro/Journey Descriptor")]
    public sealed class RouteDescriptor : ScriptableObject
    {

        [field: SerializeField]
        [field: FormerlySerializedAs("source")]
        public TextAsset Stops { get; private set; }

        [field: SerializeField]
        public TextAsset Departures { get; private set; }

        [field: SerializeField]
        public Relation Relation { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public AnnouncementPack Pack { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public string Origin { get; private set; }

        [SerializeField]
        [HideInInspector]
        private string[] intermediateStops;

        [field: SerializeField]
        [field: HideInInspector]
        public string Destination { get; private set; }

        public ReadOnlySpan<string> IntermediateStops => intermediateStops;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Stops)
                return;
            intermediateStops = RouteCache.ReadTwoLines(Stops).Item1.Split(',');
            Origin = intermediateStops[0];
            Destination = intermediateStops[^1];
        }
#endif

    }

}

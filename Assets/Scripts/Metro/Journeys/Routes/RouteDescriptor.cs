using Metro.Audio;
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
        public string Relation { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public AnnouncementPack Pack { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public string Origin { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public string Destination { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Stops)
                return;
            var stops = RouteCache.ReadTwoLines(Stops).Item1.Split(',');
            Origin = stops[0];
            Destination = stops[^1];
        }
#endif

    }

}

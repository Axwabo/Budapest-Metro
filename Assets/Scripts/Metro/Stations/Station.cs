using System.Collections.Generic;
using Metro.Rail;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Metro.Stations
{

    public sealed class Station : MonoBehaviour
    {

        private static readonly Dictionary<string, Station> LoadedStations = new();

        [field: SerializeField]
        public StationId ID { get; private set; }

        [field: SerializeField]
        [field: Header("Tracks")]
        public StationTrack Left { get; private set; }

        [field: SerializeField]
        public StationTrack Right { get; private set; }

        [SerializeField]
        [Header("Auto-size")]
        private Transform platform;

        [SerializeField]
        private Transform leftTrack;

        [SerializeField]
        private Transform rightTrack;

        [SerializeField]
        private float usablePlatformArea = 800;

        private void Awake() => LoadedStations.Add(name, this);

        private void OnDestroy() => LoadedStations.Remove(name);

        public static bool TryGetLoadad(string name, out Station station) => LoadedStations.TryGetValue(name, out station);

#if UNITY_EDITOR
        private void OnValidate()
        {
            var platformWidth = usablePlatformArea / Constants.PlatformLengthMeters;
            if (platform)
            {
                var scale = platform.localScale;
                scale.x = platformWidth;
                platform.localScale = scale;
                RecordChanges(platform);
            }

            var trackPosition = platformWidth * 0.5f + Constants.AssemblyWidthMeters * 0.5f;
            if (leftTrack)
                SetPosition(leftTrack, -trackPosition);
            if (rightTrack)
                SetPosition(rightTrack, trackPosition);
        }

        private static void SetPosition(Transform t, float x)
        {
            var position = t.localPosition;
            position.x = x;
            t.localPosition = position;
            RecordChanges(t);
        }

        private static void RecordChanges(Transform target)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(target))
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
        }
#endif

    }

}

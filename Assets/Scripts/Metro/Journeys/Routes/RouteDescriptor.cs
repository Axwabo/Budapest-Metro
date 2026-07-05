using System;
using System.Collections.Generic;
using System.Globalization;
using Metro.Audio;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    [CreateAssetMenu(fileName = "Journey Descriptor", menuName = "Metro/Journey Descriptor")]
    public sealed class RouteDescriptor : ScriptableObject
    {

        // TODO: multiple route support

        private static readonly char[] NewLineChars = {'\n', '\r'};

        [SerializeField]
        private TextAsset source;

        [field: SerializeField]
        public string Relation { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public AnnouncementPack Pack { get; private set; }

        public Stop Origin { get; private set; }

        public List<Stop> IntermediateStops { get; } = new();

        public Stop Destination { get; private set; }

        private void Awake()
        {
            if (!source)
                return;
            // TODO: eliminate allocations (besides .text)
            var lines = source.text.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
                return;
            var stations = lines[0].Split(',');
            var times = lines[1].Split(',');
            Origin = Create(0, stations, times);
            Destination = Create(^1, stations, times);
            for (var i = 1; i < stations.Length - 1; i++)
                IntermediateStops.Add(Create(i, stations, times));
        }

        private void OnValidate() => Awake();

        private static Stop Create(Index index, string[] stations, string[] times) => new(stations[index], TimeSpan.ParseExact(times[index], "hh':'mm", CultureInfo.InvariantCulture));

    }

}

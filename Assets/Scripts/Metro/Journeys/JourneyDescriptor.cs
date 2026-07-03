using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Metro.Journeys
{

    [CreateAssetMenu(fileName = "Journey Descriptor", menuName = "Metro/Journey Descriptor")]
    public sealed class JourneyDescriptor : ScriptableObject
    {

        private static readonly char[] NewLineChars = {'\n', '\r'};

        [SerializeField]
        private TextAsset source;

        [field: SerializeField]
        public bool Reverse { get; private set; }

        public Stop Origin { get; private set; }

        public List<Stop> IntermediateStops { get; } = new();

        public Stop Destination { get; private set; }

        private void Awake()
        {
            if (!source)
                return;
            var lines = source.text.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
                return;
            Origin = Parse(lines[0]);
            for (var i = 1; i < lines.Length - 1; i++)
                IntermediateStops.Add(Parse(lines[i]));
            Destination = Parse(lines[^1]);
        }

        private void OnValidate() => Awake();

        private static Stop Parse(ReadOnlySpan<char> line)
        {
            var comma = line.IndexOf(',');
            var station = line[..comma];
            var time = line[(comma + 1)..];
            return new Stop(station.ToString(), TimeSpan.ParseExact(time, "hh':'mm", CultureInfo.InvariantCulture));
        }

    }

}

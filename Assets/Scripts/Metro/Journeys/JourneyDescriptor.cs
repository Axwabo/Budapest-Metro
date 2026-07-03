using System;
using System.Collections.Generic;
using System.Globalization;
using Metro.Stations;
using UnityEngine;

namespace Metro.Journeys
{

    [CreateAssetMenu(fileName = "Journey Descriptor", menuName = "Metro/Journey Descriptor")]
    public sealed class JourneyDescriptor : ScriptableObject
    {

        private static readonly char[] NewLineChars = {'\n', '\r'};

        [SerializeField]
        private TextAsset source;

        public Stop Origin { get; private set; }

        public List<Stop> IntermediateStops { get; } = new();

        public Stop Destination { get; private set; }

        private void OnValidate()
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

        private static Stop Parse(ReadOnlySpan<char> line)
        {
            var comma = line.IndexOf(',');
            var station = line[..comma];
            var time = line[(comma + 1)..];
            foreach (var stationId in StationId.All)
                // this allocates bc unity :DDDD
                if (stationId.name == station)
                    return new Stop(stationId, TimeSpan.ParseExact(time, "hh':'mm", CultureInfo.InvariantCulture));
            throw new MissingReferenceException($"Station {station.ToString()} not found");
        }

    }

}

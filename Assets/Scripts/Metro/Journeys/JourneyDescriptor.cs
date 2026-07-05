using System;
using System.Collections.Generic;
using System.Globalization;
using Metro.Audio;
using Metro.Rail.Controls;
using Metro.Stations;
using UnityEngine;

namespace Metro.Journeys
{

    [CreateAssetMenu(fileName = "Journey Descriptor", menuName = "Metro/Journey Descriptor")]
    public sealed class JourneyDescriptor : ScriptableObject, IJourney
    {

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
            var lines = source.text.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
                return;
            Origin = Parse(lines[0]);
            for (var i = 1; i < lines.Length - 1; i++)
                IntermediateStops.Add(Parse(lines[i]));
            Destination = Parse(lines[^1]);
        }

        private void OnValidate() => Awake();

#nullable enable
        public (StopPoint Target, Stop? Stop) GetTarget(int stopIndex)
        {
            var stop = stopIndex switch
            {
                IJourney.Origin => Origin,
                IJourney.Destination => Destination,
                _ => IntermediateStops[stopIndex]
            };
            return Station.TryGetLoadad(stop.Name, out var station)
                ? ((Reverse ? station.Left : station.Right).StopPoint, stop)
                : throw new MissingComponentException($"Station {stop.Name} not found");
        }
#nullable restore

        private static Stop Parse(ReadOnlySpan<char> line)
        {
            var comma = line.IndexOf(',');
            var station = line[..comma];
            var time = line[(comma + 1)..];
            return new Stop(station.ToString(), TimeSpan.ParseExact(time, "hh':'mm", CultureInfo.InvariantCulture));
        }

    }

}

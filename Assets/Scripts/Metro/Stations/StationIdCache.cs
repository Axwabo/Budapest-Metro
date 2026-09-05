#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Metro.Journeys;
using UnityEngine;

namespace Metro.Stations
{

    public static class StationIdCache
    {

        private static readonly Dictionary<string, Dictionary<string, StationId>> Ids = new(StringComparer.OrdinalIgnoreCase);

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Load("M3");
            Load("M4");
        }

        private static void Load(string relation)
        {
            var dictionary = new Dictionary<string, StationId>(StringComparer.OrdinalIgnoreCase);
            foreach (var id in Resources.LoadAll<StationId>($"Stations/{relation}"))
                dictionary[id.name] = id;
            Ids[relation] = dictionary;
        }

        public static bool TryGet(string name, string relation, [NotNullWhen(true)] out StationId? id)
        {
            if (Ids.TryGetValue(relation, out var stations) && stations.TryGetValue(name, out id))
                return true;
            id = null;
            return false;
        }

        public static string Forehead(this Stop stop, string relation) => TryGet(stop.Name, relation, out var id) && !string.IsNullOrEmpty(id.Forehead)
            ? id.Forehead
            : stop.Name;

        public static string Onboard(this Stop stop, string relation) => TryGet(stop.Name, relation, out var id) && !string.IsNullOrEmpty(id.Onboard)
            ? id.Onboard
            : stop.Name;

    }

}

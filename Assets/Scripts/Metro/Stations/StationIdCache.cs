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

        private static readonly Dictionary<string, StationId> Ids = new(StringComparer.OrdinalIgnoreCase);

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            foreach (var id in Resources.LoadAll<StationId>("Stations"))
                Ids[id.name] = id;
        }

        public static bool TryGet(string name, [NotNullWhen(true)] out StationId? id) => Ids.TryGetValue(name, out id);

        public static string Forehead(this Stop stop) => TryGet(stop.Name, out var id) && !string.IsNullOrEmpty(id.Forehead)
            ? id.Forehead
            : stop.Name;

        public static string Onboard(this Stop stop) => TryGet(stop.Name, out var id) && !string.IsNullOrEmpty(id.Onboard)
            ? id.Onboard
            : stop.Name;

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    public static class RouteCache
    {

        private static readonly char[] NewLineChars = {'\n', '\r'};

        private static readonly Dictionary<string, Dictionary<string, Route[]>> Routes = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            foreach (var relation in Resources.LoadAll<RouteDescriptor>("Journeys").GroupBy(e => e.Relation))
            {
                Routes[relation.Key] = relation.GroupBy(e => e.Destination)
            }
        }

        public static ReadOnlySpan<Route> GetRoutes(this RouteDescriptor descriptor)
        {
            // if(!Routes.TryGetValue(descriptor.Relation, out var directions) || !directions.TryGetValue())
            return ReadOnlySpan<Route>.Empty;
        }

    }

}

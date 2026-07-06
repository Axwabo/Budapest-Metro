using System;
using System.Collections.Generic;
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
        }

        public static ReadOnlySpan<Route> GetRoutes(this RouteDescriptor descriptor)
        {
            // if(!Routes.TryGetValue(descriptor.Relation, out var directions) || !directions.TryGetValue())
            return ReadOnlySpan<Route>.Empty;
        }

    }

}

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Pool;

namespace Metro.Journeys.Routes
{

    public static class RouteCache
    {

        private static readonly char[] NewLineChars = {'\n', '\r'};

        private static readonly Dictionary<string, Dictionary<string, Route[]>> Routes = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            foreach (var descriptor in Resources.LoadAll<RouteDescriptor>("Journeys"))
            {
                if (!Routes.TryGetValue(descriptor.Relation, out var directions))
                    directions = Routes[descriptor.Relation] = new Dictionary<string, Route[]>();
                directions[descriptor.Destination] = CreateRoutes(descriptor);
            }
        }

        private static Route[] CreateRoutes(RouteDescriptor descriptor)
        {
            var relative = new List<TimeSpan>();
            var (stationsLine, times) = ReadTwoLines(descriptor.Stops);
            var stations = stationsLine.Split(',');
            var departures = descriptor.Departures.text.Split();
            foreach (var time in times.Split(','))
                if (TryParse(time, out var timeSpan))
                    relative.Add(timeSpan);
            var initialDeparture = relative[0];
            relative.RemoveAt(0);
            for (var i = 0; i < relative.Count; i++)
                relative[i] -= initialDeparture;
            var routes = ListPool<Route>.Get();
            foreach (var departure in departures)
            {
                if (!TryParse(departure, out var departureTime))
                    continue;
                var stops = new List<Stop>();
                for (var i = 0; i < relative.Count; i++)
                    stops.Add(new Stop(stations[i + 1], initialDeparture + relative[i]));
                routes.Add(new Route(descriptor, new Stop(stations[0], departureTime), stops, new Stop(stations[^1], departureTime + relative[^1])));
            }

            var routeArray = routes.ToArray();
            ListPool<Route>.Release(routes);
            return routeArray;
        }

        public static (string, string) ReadTwoLines(TextAsset asset)
        {
            var array = asset.text.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            return (array[0], array[1]);
        }

        public static ReadOnlySpan<Route> GetRoutes(this RouteDescriptor descriptor)
            => Routes.TryGetValue(descriptor.Relation, out var directions)
               && directions.TryGetValue(descriptor.Destination, out var routes)
                ? routes
                : ReadOnlySpan<Route>.Empty;

        public static Route Next(this RouteDescriptor descriptor, TimeSpan minimumOffset = default) => descriptor.After(Clock.Now + minimumOffset);

        public static Route After(this RouteDescriptor descriptor, TimeSpan after)
        {
            var routes = descriptor.GetRoutes();
            foreach (var route in routes)
                if (route.Origin.Time >= after)
                    return route;
            return null;
        }

        private static bool TryParse(string time, out TimeSpan timeSpan)
            => TimeSpan.TryParseExact(time, "hh':'mm", CultureInfo.InvariantCulture, out timeSpan);

    }

}

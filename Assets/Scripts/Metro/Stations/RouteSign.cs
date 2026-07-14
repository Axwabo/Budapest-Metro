using System;
using Metro.Journeys;
using Metro.Journeys.Routes;
using Metro.Rail;
using UnityEngine;
using UnityEngine.UIElements;
using static Metro.Journeys.IJourney;

namespace Metro.Stations
{

    public sealed class RouteSign : MonoBehaviour
    {

        private const double SecondsToOnePercent = 100 / 60d;

        private static readonly TimeSpan TwoMinutes = TimeSpan.FromMinutes(2);

        [SerializeField]
        private RouteDescriptor descriptor;

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private RenderMaterial material;

        [SerializeField]
        private int targetMaterialIndex;

        [SerializeField]
        private MeshRenderer[] renderers;

        private readonly RouteSignViewModel _viewModel = new();

        private float _delay;

        private bool _everUpdated;

        private int _index = -1;

        private Route _route;

        private StationTrack _track;

        private bool _wasOccupied;

        private Route NextRoute
        {
            get
            {
                var now = Clock.Now;
                foreach (var route in descriptor.GetRoutes())
                {
                    var delta = Stop(route).Time - now;
                    if (delta >= TimeSpan.Zero && delta <= TimeSpan.FromMinutes(30))
                        return route;
                }

                return null;
            }
        }

        private void Awake()
        {
            material.Init(name, document);
            foreach (var meshRenderer in renderers)
            {
                var materials = meshRenderer.sharedMaterials;
                materials[targetMaterialIndex] = material.Material;
                meshRenderer.sharedMaterials = materials;
            }
        }

        private void Start()
        {
            var destinationName = StationIdCache.TryGet(descriptor.Destination, out var id) && !string.IsNullOrEmpty(id.StationTime)
                ? id.StationTime
                : descriptor.Destination;
            _viewModel.Destination = $"{destinationName} felé";
            var station = GetComponentInParent<Station>();
            _index = FindIndex(station.ID.name);
            _track = station.Track(descriptor.Reverse);
            if (_index == Origin)
                _viewModel.Description = "Várható indulási idő:";
            else if (_index == Destination)
                _viewModel.Description = "";
        }

        private void Update()
        {
            if ((_delay -= Clock.Delta) > 0)
                return;
            _delay = 7; // or something like that, idrk
            UpdateDisplay();
        }

        private void OnEnable() => document.rootVisualElement.dataSource = _viewModel;

        private int FindIndex(string stationName)
        {
            if (stationName.Equals(descriptor.Destination, StringComparison.OrdinalIgnoreCase))
                return Destination;
            var stops = descriptor.IntermediateStops;
            for (var i = 0; i < stops.Length; i++)
            {
                var stop = stops[i];
                if (!stop.Equals(stationName, StringComparison.OrdinalIgnoreCase))
                    continue;
                return i;
            }

            return Origin;
        }

        private Stop Stop(Route route) => _index switch
        {
            Origin => route.Origin,
            Destination => route.Destination,
            _ => route.IntermediateStops[_index]
        };

        private void UpdateDisplay()
        {
            var occupied = _track.IsOccupied;
            if (_wasOccupied != occupied)
            {
                _wasOccupied = occupied;
                if (!occupied)
                    _route = null;
            }

            _route ??= NextRoute;
            if (_route == null)
            {
                if (_index != Destination || _everUpdated)
                    return;
                _everUpdated = true;
                _viewModel.NoInformation = true;
                _viewModel.Minutes = "=";
                document.rootVisualElement.AddToClassList("no-information");
                return;
            }

            var delta = Stop(_route).Time - Clock.Now;
            if (delta < -TwoMinutes)
                _route = null; // késing/skipped
            if (delta < TimeSpan.Zero)
                delta = TimeSpan.Zero;
            var equals = _index == Destination && delta > TwoMinutes;
            document.rootVisualElement.EnableInClassList("no-information", equals);
            _viewModel.Minutes = equals ? "=" : delta.Minutes.ToString("00");
            _viewModel.Seconds = equals ? "" : delta.Seconds.ToString("00");
            if (_index == Destination)
                return;
            var length = Length.Percent(Mathf.Min(100, (float) (delta.TotalSeconds * SecondsToOnePercent)));
            if (_viewModel.Width != length)
                _viewModel.Width = length;
        }

    }

}

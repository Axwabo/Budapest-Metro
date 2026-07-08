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

        private VisualElement _bar;

        private float _delay;

        private bool _everUpdated;

        private int _index = -1;

        private Label _minutes;

        private VisualElement _root;

        private Route _route;
        private Label _seconds;

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

        private string DestinationName => StationIdCache.TryGet(descriptor.Destination, out var id) && !string.IsNullOrEmpty(id.StationTime)
            ? id.StationTime
            : descriptor.Destination;

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
            _root = document.rootVisualElement;
            var station = GetComponentInParent<Station>();
            var destination = _root.Q<Label>("Destination");
            var description = _root.Q<Label>("Description");
            destination.text = $"{DestinationName} felé";
            _index = FindIndex(station.ID.name);
            _minutes = _root.Q<Label>("Minutes");
            _seconds = _root.Q<Label>("Seconds");
            _bar = _root.Q("Bar");
            _track = station.Track(descriptor.Reverse);
            if (_index == Origin)
                description.text = "Várható indulási idő:";
            else if (_index == Destination)
                description.text = "";
        }

        private void Update()
        {
            if ((_delay -= Clock.Delta) > 0)
                return;
            _delay = 7; // or something like that, idrk
            UpdateDisplay();
        }

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
                _root.AddToClassList("no-information");
                _minutes.text = "=";
                return;
            }

            var delta = Stop(_route).Time - Clock.Now;
            if (delta < -TwoMinutes)
                _route = null; // késing/skipped
            if (delta < TimeSpan.Zero)
                delta = TimeSpan.Zero;
            var equals = _index == Destination && delta > TwoMinutes;
            _root.EnableInClassList("no-information", equals);
            _minutes.text = equals ? "=" : delta.Minutes.ToString("00");
            _seconds.text = equals ? "" : delta.Seconds.ToString("00");
            _bar.style.width = Length.Percent(Mathf.Min(100, (float) (delta.TotalSeconds * SecondsToOnePercent)));
        }

    }

}

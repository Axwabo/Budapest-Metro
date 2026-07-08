using System;
using Metro.Journeys.Routes;
using Metro.Rail;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public sealed class RouteSign : MonoBehaviour
    {

        private const double SecondsToOnePercent = 100 / 60d;

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

        private int _index;

        private Label _minutes;

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
                    var delta = route.IntermediateStops[_index].Time - now;
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
            var station = GetComponentInParent<Station>();
            var stationName = station.ID.name;
            var stops = descriptor.IntermediateStops;
            for (var i = 0; i < stops.Length; i++)
            {
                var stop = stops[i];
                if (!stop.Equals(stationName, StringComparison.OrdinalIgnoreCase))
                    continue;
                _index = i;
                break;
            }

            var destination = StationIdCache.TryGet(descriptor.Destination, out var id) && !string.IsNullOrEmpty(id.StationTime)
                ? id.StationTime
                : descriptor.Destination;
            var root = document.rootVisualElement;
            root.Q<Label>("Destination").text = $"{destination} felé";
            _minutes = root.Q<Label>("Minutes");
            _seconds = root.Q<Label>("Seconds");
            _bar = root.Q("Bar");
            _track = station.Track(descriptor.Reverse);
        }

        private void Update()
        {
            if ((_delay -= Clock.Delta) > 0)
                return;
            _delay = 7; // or something like that, idrk
            UpdateDisplay();
        }

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
                return;
            var delta = _route.IntermediateStops[_index].Time - Clock.Now;
            if (delta < TimeSpan.Zero)
                delta = TimeSpan.Zero;
            _minutes.text = delta.Minutes.ToString("00");
            _seconds.text = delta.Seconds.ToString("00");
            _bar.style.width = Length.Percent(Mathf.Min(100, (float) (delta.TotalSeconds * SecondsToOnePercent)));
        }

    }

}

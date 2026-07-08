using System;
using Metro.Journeys.Routes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public class RouteSign : MonoBehaviour
    {

        [SerializeField]
        private RouteDescriptor descriptor;

        [SerializeField]
        private UIDocument document;

        private VisualElement _bar;

        private float _delay;

        private int _index;

        private Label _minutes;
        private Label _seconds;

        private void Start()
        {
            var station = GetComponentInParent<Station>().ID.name;
            for (var i = 0; i < descriptor.IntermediateStops.Length; i++)
            {
                var stop = descriptor.IntermediateStops[i];
                if (!stop.Equals(station, StringComparison.OrdinalIgnoreCase))
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
            Initialize(root);
        }

        private void Update()
        {
            if ((_delay -= Clock.Delta) > 0)
                return;
            _delay = 7; // or something like that, idrk
            UpdateDisplay();
        }

        protected virtual void Initialize(VisualElement root)
        {
        }

        private void UpdateDisplay()
        {
            var now = Clock.Now;
            foreach (var route in descriptor.GetRoutes())
            {
                var delta = route.IntermediateStops[_index].Time - now;
                if (now < TimeSpan.Zero || now > TimeSpan.FromMinutes(30))
                    continue;
                _minutes.text = delta.Minutes.ToString();
                _seconds.text = delta.Seconds.ToString();
                break;
            }
        }

    }

}

using System;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class OnboardDisplayRenderer : DisplayRendererBase
    {

        private Label _stop;

        private VisualElement _destinationContainer;

        private Label _destination;

        private VisualElement _routeAndTime;

        private Label _relation;

        private Label _clock;

        private VisualElement _serviceArea;

        private VisualElement _previous;

        private VisualElement _current;

        private DisplaySection _section;

        private float _time;

        protected override void Update()
        {
            base.Update();
            if (!IsInService || (_time -= Clock.Delta) > 0)
                return;
            _time = 5;
            var previousSection = _section;
            // TODO: more advanced state machine
            _section = previousSection switch
            {
                DisplaySection.Destination => DisplaySection.Stop,
                DisplaySection.Stop => DisplaySection.Time,
                DisplaySection.Time => DisplaySection.Destination,
                DisplaySection.ServiceArea => DisplaySection.ServiceArea,
                _ => throw new InvalidOperationException()
            };

            if (_section != previousSection)
                UpdateSection();
        }

        private void UpdateSection()
        {
            if (_section == DisplaySection.Time)
                _clock.text = Clock.Now.ToString("hh':'mm");
            _current = _section switch
            {
                DisplaySection.Destination => _destinationContainer,
                DisplaySection.Time => _routeAndTime,
                DisplaySection.Stop => _stop,
                DisplaySection.ServiceArea => _serviceArea,
                _ => throw new InvalidOperationException()
            };
            _previous?.Display(false);
            _current.Display();
            Blink(0.2f);
        }

        protected override void Initialize(VisualElement root)
        {
            _stop = root.Q<Label>("Stop");
            _destinationContainer = root.Q("Destination");
            _destination = _destinationContainer.Q<Label>("Destination");
            _routeAndTime = root.Q("RouteAndTime");
            _relation = root.Q<Label>("Relation");
            _clock = root.Q<Label>("Clock");
            _serviceArea = root.Q("ServiceArea");
        }

        public override void OnStopChanged() => _stop.text = Stop?.Name ?? "";

        public override void OnJourneyChanged()
        {
            _destination.text = Route?.Destination ?? "";
            _relation.text = Route?.Relation ?? "";
        }

        private enum DisplaySection
        {

            Destination,
            Time,
            Stop,
            ServiceArea

        }

    }

}

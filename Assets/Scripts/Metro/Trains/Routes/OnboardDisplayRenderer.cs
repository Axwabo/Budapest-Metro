using System;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class OnboardDisplayRenderer : DisplayRendererBase
    {

        private const string Service = "Üzemi terület";

        private Label _label;

        private DisplaySection _section;

        private float _time;

        protected override void Update()
        {
            base.Update();
            if (!IsInService || (_time -= Clock.Delta) > 0)
                return;
            _time = 5;
            var previousSection = _section;
            _section = previousSection switch
            {
                DisplaySection.Destination => DisplaySection.Stop,
                DisplaySection.Stop => DisplaySection.Time,
                DisplaySection.Time => DisplaySection.Destination,
                DisplaySection.ServiceArea => DisplaySection.ServiceArea,
                _ => throw new InvalidOperationException()
            };
            _label.text = _section switch
            {
                DisplaySection.Destination => $"► {Route.Destination}",
                DisplaySection.Time => $"{Route.Relation}    {Clock.Now:hh':'mm}",
                DisplaySection.Stop => Stop.Name,
                DisplaySection.ServiceArea => Service,
                _ => throw new InvalidOperationException()
            };
            if (_section != previousSection)
                Blink(0.2f);
        }

        protected override void Initialize(VisualElement root) => _label = root.Q<Label>();

        public override void OnStopChanged()
        {
            if (_section == DisplaySection.ServiceArea)
                return;
            _label.text = Stop?.Name;
            _time = 5;
            _section = DisplaySection.Stop;
        }

        public override void OnJourneyChanged()
        {
            if (IsInService)
            {
                _section = DisplaySection.Stop;
                return;
            }

            _section = DisplaySection.ServiceArea;
            _label.text = Service;
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

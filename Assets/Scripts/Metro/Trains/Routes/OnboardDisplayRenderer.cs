using System;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class OnboardDisplayRenderer : DisplayRendererBase
    {

        private Label _label;

        private DisplaySection _section;

        private float _time;

        private void Update()
        {
            if ((_time -= Clock.Delta) > 0)
                return;
            _time = 5;
            _section = _section switch
            {
                DisplaySection.Destination => DisplaySection.Stop,
                DisplaySection.Stop => DisplaySection.Time,
                DisplaySection.Time => DisplaySection.Destination,
                _ => throw new InvalidOperationException()
            };
            _label.text = _section switch
            {
                DisplaySection.Destination => $"{CurrentJourney.Relation} ► {CurrentJourney.Destination.Name}",
                DisplaySection.Time => Clock.Now.ToString("hh':'mm"),
                DisplaySection.Stop => JourneyManager.Stop.Name,
                _ => throw new InvalidOperationException()
            };
        }

        protected override void Initialize(VisualElement root) => _label = root.Q<Label>();

        public override void OnStationChanged()
        {
            _label.text = JourneyManager.Stop?.Name;
            _time = 5;
            _section = DisplaySection.Stop;
        }

        private enum DisplaySection
        {

            Destination,
            Time,
            Stop

        }

    }

}

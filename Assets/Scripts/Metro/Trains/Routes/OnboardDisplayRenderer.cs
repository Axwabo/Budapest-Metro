using System;
using Metro.Stations;
using Metro.Trains.Driving;
using UnityEngine.UIElements;
using static Metro.Trains.Routes.SectionStateMachines;

namespace Metro.Trains.Routes
{

    public sealed class OnboardDisplayRenderer : DisplayRendererBase
    {

        private Label _clock;

        private VisualElement _current;

        private Label _destination;

        private VisualElement _destinationContainer;

        private VisualElement _previous;

        private Label _relation;

        private VisualElement _routeAndTime;

        private DisplaySection _section;

        private VisualElement _serviceArea;

        private SectionStateMachine _stateMachine = ServiceArea;

        private Label _stop;

        private VisualElement _terminus;

        private float _time;

        private VisualElement _transfers;

        private TransfersDisplay _transfersDisplay;

        private SectionStateMachine StoppedStateMachine => JourneyManager.IsDestination ? StoppedDestination : Stopped;

        private bool TransitionCompleted => _time <= 0 || _section != DisplaySection.Transfers || _transfersDisplay.TransitionCompleted;

        protected override void Update()
        {
            base.Update();
            if (_section == DisplaySection.Transfers)
                _transfersDisplay.Update();
            if (!TransitionCompleted || (_time -= Clock.Delta) > 0)
                return;
            _time = 5;
            var previousSection = _section;
            _section = _stateMachine(previousSection);
            if (_section != previousSection)
                UpdateSection();
        }

        private void UpdateSection()
        {
            if (_section == DisplaySection.RouteAndTime)
                _clock.text = Clock.Now.ToString("hh':'mm");
            else if (_section == DisplaySection.Transfers)
                _transfersDisplay.ResetPosition();
            _previous?.Display(false);
            _previous = _current = _section switch
            {
                DisplaySection.Destination => _destinationContainer,
                DisplaySection.RouteAndTime => _routeAndTime,
                DisplaySection.Stop => _stop,
                DisplaySection.Terminus => _terminus,
                DisplaySection.Transfers => _transfers,
                DisplaySection.ServiceArea => _serviceArea,
                _ => throw new InvalidOperationException()
            };
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
            _terminus = root.Q("Terminus");
            _transfers = root.Q("Transfers");
            _serviceArea = root.Q("ServiceArea");
            _transfersDisplay = new TransfersDisplay(_transfers);
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Stopped && IsInService)
                _stateMachine = StoppedStateMachine;
        }

        public override void OnStopChanged()
        {
            if (Stop is {Name: var stopName})
            {
                _stop.text = stopName;
                _transfersDisplay.Display(stopName);
            }

            if (State == DriverState.Stopped)
                _stateMachine = StoppedStateMachine;
            else if (State == DriverState.Driving)
                _stateMachine = JourneyManager.IsDestination ? ApproachingDestination : Approaching;
        }

        public override void OnJourneyChanged()
        {
            _destination.text = Route?.Destination.Onboard() ?? "";
            _relation.text = Route?.Relation ?? "";
            if (!IsInService)
                _stateMachine = ServiceArea;
        }

    }

}

using Metro.Journeys;
using Metro.Stations;
using Metro.Trains.Driving;
using Unity.Properties;
using UnityEngine.UIElements;
using static Metro.Trains.Routes.SectionStateMachines;

namespace Metro.Trains.Routes
{

    public sealed class OnboardDisplayRenderer : DisplayRendererBase
    {

        private DisplaySection _section;

        private SectionStateMachine _stateMachine = ServiceArea;

        private float _time;

        private TransfersDisplay _transfersDisplay;

        [CreateProperty]
        public string Time { get; private set; }

        [CreateProperty]
        public string Destination { get; private set; }

        [CreateProperty]
        public string Relation { get; private set; }

        [CreateProperty]
        public string StopName { get; private set; }

        [CreateProperty]
        public string Service { get; private set; }

        private SectionStateMachine StoppedStateMachine => JourneyManager.IsDestination ? StoppedDestination : Stopped;

        private bool TransitionCompleted => _time <= 0 || _section != DisplaySection.Transfers || _transfersDisplay.TransitionCompleted;

        protected override void Update()
        {
            base.Update();
            var previousSection = _section;
            if (Driver.IsOnTargetTrack && IsInService && State == DriverState.Driving)
                UpdateStopping();
            if (!TransitionCompleted || (_time -= Clock.Delta) > 0)
                return;
            _time = 5;
            _section = _stateMachine(previousSection);
            if (_section != previousSection)
                UpdateSection(previousSection);
        }

        private void UpdateStopping()
        {
            var target = JourneyManager.IsDestination ? StoppingDestination : Stopping;
            if (_stateMachine != target)
                _time = 0;
            _stateMachine = target;
        }

        private void UpdateSection(DisplaySection previousSection)
        {
            if (_section == DisplaySection.RouteAndTime)
                Time = Clock.Now.ToString("hh':'mm");
            SetClass(previousSection.ToString(), false);
            SetClass(_section.ToString(), true);
            Blink(0.2f);
        }

        /*
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
            _serviceArea = root.Q<Label>("ServiceArea");
            _transfersDisplay = new TransfersDisplay(_transfers);
        }
        */

        protected override void Bind(VisualElement root) => _transfersDisplay = new TransfersDisplay(root.Q("Transfers"));

        public override void OnStateChanged()
        {
            switch (State, IsInService)
            {
                case (DriverState.Driving, false):
                    _stateMachine = ServiceArea;
                    Service = Journey switch
                    {
                        ICarriageHouseJourney {ToCarriageHouse: true} => ToCarriageHouse,
                        Afk => "Üzemi terület", // valóságos ellentétben a hamissal (fake vs reality)
                        _ => None
                    };
                    _time = 0;
                    break;
                case (DriverState.Stopped, true):
                    _stateMachine = StoppedStateMachine;
                    _time = 0;
                    break;
            }
        }

        public override void OnStopChanged()
        {
            if (Stop is {Name: var stopName})
            {
                StopName = stopName;
                _transfersDisplay.Display(stopName);
            }

            if (State == DriverState.Stopped)
                _stateMachine = StoppedStateMachine;
            else if (State == DriverState.Driving)
                _stateMachine = JourneyManager.IsDestination ? ApproachingDestination : Approaching;
        }

        public override void OnJourneyChanged()
        {
            Destination = Route?.Destination.Onboard() ?? "";
            Relation = Route?.Relation ?? "";
        }

    }

}

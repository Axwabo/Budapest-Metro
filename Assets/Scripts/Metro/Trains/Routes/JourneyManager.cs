using Metro.Journeys;
using Metro.Trains.Driving;
using UnityEngine;
using static Metro.Journeys.IJourney;

namespace Metro.Trains.Routes
{

    public sealed class JourneyManager : AssemblyComponent
    {

        [field: SerializeField]
        public JourneyDescriptor Current { get; private set; }

        [SerializeField]
        private int initialStopIndex = Origin;

        private int _index = OutOfService;

        public Stop Stop { get; private set; }

        public ITarget Target { get; private set; }

        public bool IsInService { get; private set; }

        public bool IsOrigin => _index == initialStopIndex;

        public bool IsDestination => _index == Destination;

        public void Begin() => Begin(Current, initialStopIndex);

        [ContextMenu("Exit Service")]
        public void ExitService() => Begin(null, OutOfService);

        private void Begin(JourneyDescriptor journey, int index)
        {
            Current = journey;
            IsInService = journey;
            Parent.NotifyJourneyChanged();
            UpdateStop(index);
        }

        private void UpdateStop(int index)
        {
            _index = index;
            Stop = index switch
            {
                OutOfService => null,
                Origin => Current.Origin,
                Destination => Current.Destination,
                _ => Current.IntermediateStops[index]
            };
            Parent.NotifyStationChanged();
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Driving && IsInService)
                UpdateStop(++_index >= Current.IntermediateStops.Count ? Destination : _index);
        }

    }

}

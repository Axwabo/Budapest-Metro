using Metro.Journeys;
using Metro.Trains.Driving;
using UnityEngine;

namespace Metro.Trains.Routes
{

    public sealed class JourneyManager : AssemblyComponent
    {

        private const int OutOfService = int.MinValue;
        private const int Origin = -1;
        private const int Destination = int.MaxValue;

        [field: SerializeField]
        public JourneyDescriptor Current { get; private set; }

        private int _index = OutOfService;

        public Stop Stop { get; private set; }

        public bool IsInService { get; private set; }

        public bool IsOrigin => _index == Origin;

        public bool IsDestination => _index == Destination;

        public void Begin() => Begin(Current);

        [ContextMenu("Exit Service")]
        public void ExitService() => Begin(null);

        private void Begin(JourneyDescriptor journey)
        {
            Current = journey;
            IsInService = journey;
            Parent.NotifyJourneyChanged();
            UpdateStop(IsInService ? Origin : OutOfService);
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

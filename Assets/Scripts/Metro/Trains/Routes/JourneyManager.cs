using System.Diagnostics.CodeAnalysis;
using Metro.Journeys;
using Metro.Rail.Controls;
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

        private IJourney _journey;

#nullable enable
        public Stop? Stop { get; private set; }
#nullable restore

        public StopPoint Target { get; private set; }

        [MemberNotNullWhen(true, nameof(Stop))]
        public bool IsInService => _index == OutOfService;

        public bool IsOrigin => _index == initialStopIndex;

        public bool IsDestination => _index == Destination;

        public void Begin() => Begin(Current, initialStopIndex);

        [ContextMenu("Exit Service")]
        public void ExitService() => Begin(null, OutOfService);

        private void Begin(IJourney journey, int index)
        {
            _journey = journey;
            Current = journey as JourneyDescriptor;
            Parent.NotifyJourneyChanged();
            UpdateTarget(index);
        }

        private void UpdateTarget(int index)
        {
            var current = Stop;
            _index = index;
            (Target, Stop) = _journey.GetTarget(index);
            if (current != Stop)
                Parent.NotifyStopChanged();
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Driving && IsInService)
                UpdateTarget(++_index >= Current.IntermediateStops.Count ? Destination : _index);
        }

    }

}

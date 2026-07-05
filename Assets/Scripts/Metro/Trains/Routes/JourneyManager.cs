using System.Diagnostics.CodeAnalysis;
using Metro.Journeys;
using Metro.Rail.Controls;
using Metro.Trains.Driving;
using UnityEngine;
using UnityEngine.Serialization;
using static Metro.Journeys.IJourney;

#nullable enable

namespace Metro.Trains.Routes
{

    public sealed class JourneyManager : AssemblyComponent
    {

        [field: FormerlySerializedAs("<Current>k__BackingField")]
        [field: SerializeField]
        public new RouteDescriptor? Route { get; private set; }

        [SerializeField]
        [Min(Origin)]
        private int initialStopIndex = Origin;

        private int _index = OutOfService;

        public IJourney Current { get; private set; } = null!;

        public new Stop? Stop { get; private set; }

        public StopPoint Target { get; private set; } = null!;

        [MemberNotNullWhen(true, nameof(Route), nameof(Stop))]
        public new bool IsInService => _index != OutOfService;

        public bool IsOrigin => _index == initialStopIndex;

        public bool IsDestination => _index == Destination;

        public void Begin()
        {
            if (Route)
                Begin(Route, initialStopIndex);
            else
                ExitService();
        }

        [ContextMenu("Exit Service")]
        public void ExitService() => Begin(Afk.Instance, OutOfService);

        private void Begin(IJourney journey, int index)
        {
            _index = index;
            Current = journey;
            Route = journey as RouteDescriptor;
            Parent.NotifyJourneyChanged();
            UpdateTarget(index);
        }

        private void UpdateTarget(int index)
        {
            var current = Stop;
            _index = index;
            (Target, Stop) = Current.GetTarget(index);
            Parent.NotifyTargetChanged();
            if (current != Stop)
                Parent.NotifyStopChanged();
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Driving && IsInService)
                UpdateTarget(++_index >= Route.IntermediateStops.Count ? Destination : _index);
        }

    }

}

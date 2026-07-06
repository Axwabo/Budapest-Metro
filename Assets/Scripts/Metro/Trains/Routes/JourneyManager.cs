using System.Diagnostics.CodeAnalysis;
using Metro.Journeys;
using Metro.Journeys.Routes;
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

        [FormerlySerializedAs("<Route>k__BackingField")]
        [FormerlySerializedAs("<Current>k__BackingField")]
        [SerializeField]
        private RouteDescriptor? initialRoute;

        [field: FormerlySerializedAs("initialStopIndex")]
        [field: SerializeField]
        [field: Min(Origin)]
        public int InitialStopIndex { get; set; } = Origin;

        private int _index = OutOfService;

        public IJourney? InitialJourney { get; set; }

        public IJourney Current { get; private set; } = null!;

        public new Route? Route { get; private set; }

        public new Stop? Stop { get; private set; }

        public StopPoint Target { get; private set; } = null!;

        [MemberNotNullWhen(true, nameof(Route), nameof(Stop))]
        public new bool IsInService => _index != OutOfService;

        public bool IsOrigin => _index == InitialStopIndex;

        public bool IsDestination => _index == Destination;

        public void Begin()
        {
            if (InitialJourney != null)
                Begin(InitialJourney, InitialStopIndex);
            else if (initialRoute)
                Begin(initialRoute.Next(), InitialStopIndex);
            else
                Idle();
        }

        public void Begin(IJourney journey) => Begin(journey, Origin);

        [ContextMenu("Idle")]
        public void Idle() => Begin(Afk.Instance);

        private void Begin(IJourney journey, int index)
        {
            _index = journey is Route ? index : OutOfService;
            Current = journey;
            Route = journey as Route;
            Parent.NotifyJourneyChanged();
            UpdateTarget(_index);
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

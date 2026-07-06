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

        [SerializeField]
        [Min(Origin)]
        private int initialStopIndex = Origin;

        private int _index = OutOfService;

        public IJourney? InitialJourney { get; set; }

        public IJourney Current { get; private set; } = null!;

        public new Route? Route { get; private set; }

        public new Stop? Stop { get; private set; }

        public StopPoint Target { get; private set; } = null!;

        [MemberNotNullWhen(true, nameof(Route), nameof(Stop))]
        public new bool IsInService => _index != OutOfService;

        public bool IsOrigin => _index == initialStopIndex;

        public bool IsDestination => _index == Destination;

        public void Begin()
        {
            if (InitialJourney != null)
                Begin(InitialJourney);
            else if (initialRoute)
                Begin(new Route(initialRoute), initialStopIndex);
            else
                Idle();
        }

        public void Begin(IJourney journey) => Begin(journey, journey is Route ? Origin : OutOfService);

        [ContextMenu("Idle")]
        public void Idle() => Begin(Afk.Instance);

        private void Begin(IJourney journey, int index)
        {
            _index = index;
            Current = journey;
            Route = journey as Route;
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
                UpdateTarget(++_index >= Route.Descriptor.IntermediateStops.Count ? Destination : _index);
        }

    }

}

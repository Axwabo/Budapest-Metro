using Metro.Journeys;

namespace Metro.Trains.Driving
{

    public sealed class ServiceAreaController : AssemblyComponent, IDepartureBlocker
    {

        private ReversingSidingArea _entryPending;

        private void Update()
        {
            if (State != DriverState.WaitingForDeparture || !_entryPending || !_entryPending.TryEnter(Parent, out var siding))
                return;
            CanDepart = true;
            JourneyManager.Begin(siding);
        }

        public bool CanDepart { get; private set; } = true;

        public override void OnJourneyChanged() => _entryPending = null;

        public override void OnStateChanged()
        {
            if (State == DriverState.Stopped && Journey is ReversingSidingJourney reversing)
            {
                reversing.Area.PassingThrough.Remove(Parent);
                return;
            }

            if (State != DriverState.WaitingForDeparture || !JourneyManager.IsDestination)
                return;
            var area = JourneyManager.Target.SidingAreaAhead;
            if (!area)
                return;
            if (area.TryEnter(Parent, out var siding))
            {
                CanDepart = true;
                JourneyManager.Begin(siding);
                return;
            }

            CanDepart = false;
            JourneyManager.Begin(Afk.Instance);
            _entryPending = area;
        }

    }

}

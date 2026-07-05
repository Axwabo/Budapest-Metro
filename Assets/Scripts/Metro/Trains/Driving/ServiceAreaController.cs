using Metro.Journeys;
using Metro.Rail.Controls;

namespace Metro.Trains.Driving
{

    public sealed class ServiceAreaController : AssemblyComponent, IDepartureBlocker
    {

        private void Update()
        {
            switch (State, JourneyManager.Target)
            {
                case (DriverState.Stopped, ServiceAreaExitPoint {Siding: var siding}) when siding.Exit(Parent) is { } journey:
                    CanDepart = true;
                    JourneyManager.Begin(journey);
                    break;
                case (DriverState.WaitingForDeparture, ServiceEntryStopPoint {Area: var area}) when area.Enter(Parent) is { } journey:
                    CanDepart = true;
                    JourneyManager.Begin(journey);
                    break;
            }
        }

        public bool CanDepart { get; private set; } = true;

        public override void OnStateChanged()
        {
            if (State == DriverState.Stopped && Journey is ReversingSidingJourney reversing)
            {
                // TODO: to stop point
                reversing.Area.PassingThrough.Remove(Parent);
                return;
            }

            if (State != DriverState.WaitingForDeparture || !JourneyManager.IsDestination || JourneyManager.Target is not ServiceEntryStopPoint {Area: var area})
                return;
            if (area.Enter(Parent) is not { } journey)
            {
                CanDepart = false;
                return;
            }

            CanDepart = true;
            JourneyManager.Begin(journey);
        }

    }

}

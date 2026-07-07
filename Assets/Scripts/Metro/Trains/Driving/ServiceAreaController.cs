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
                case (DriverState.WaitingForDeparture, ServiceEntryStopPoint {Area: var area, TracksClear: true}) when area.Enter(Parent) is { } journey:
                    JourneyManager.Begin(journey);
                    break;
                case (DriverState.Driving, _) when Journey is EnteringJourney {Next: var next} && Driver.IsOnTargetTrack:
                    JourneyManager.Begin(next);
                    break;
            }
        }

        public bool CanDepart { get; private set; } = true;

        public override void OnStateChanged()
        {
            if (State == DriverState.Stopped && JourneyManager.Target is ServiceAreaExitPoint {Area: var exit})
            {
                exit.NotifyArrived(Parent);
                return;
            }

            var checkEntry = State switch
            {
                DriverState.Stopped => Journey is ServiceJourney && Driver.IsOnTargetTrack,
                DriverState.WaitingForDeparture => JourneyManager.IsDestination,
                _ => false
            };
            if (!checkEntry || JourneyManager.Target is not ServiceEntryStopPoint {Area: var area, TracksClear: var clear})
                return;
            Driver.MarkReadyNow();
            if (clear && area.Enter(Parent) is { } journey)
                JourneyManager.Begin(journey);
            else
                CanDepart = false;
        }

        public override void OnJourneyChanged() => CanDepart = true;

    }

}

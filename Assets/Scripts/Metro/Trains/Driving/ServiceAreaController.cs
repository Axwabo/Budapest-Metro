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
                case (DriverState.Stopped, ServiceAreaExitPoint {Area: var area}) when area.Exit(Parent) is { } journey:
                    var isAfk = Journey is Afk;
                    CanDepart = true;
                    JourneyManager.Begin(journey);
                    // TODO: mathn't
                    Driver.MarkReady(journey is ServiceJourney ? 180 : isAfk ? 0 : 10);
                    break;
                case (DriverState.WaitingForDeparture, ServiceEntryStopPoint {Area: var area, TracksClear: true}) when area.Enter(Parent) is { } journey:
                    CanDepart = true;
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
            var checkEntry = State switch
            {
                DriverState.Stopped => Journey is ServiceJourney && Driver.IsOnTargetTrack,
                DriverState.WaitingForDeparture => JourneyManager.IsDestination,
                _ => false
            };
            if (!checkEntry || JourneyManager.Target is not ServiceEntryStopPoint {Area: var area, TracksClear: var clear})
                return;
            Driver.MarkReadyNow();
            if (!clear || area.Enter(Parent) is not { } journey)
            {
                CanDepart = false;
                return;
            }

            CanDepart = true;
            JourneyManager.Begin(journey);
        }

    }

}

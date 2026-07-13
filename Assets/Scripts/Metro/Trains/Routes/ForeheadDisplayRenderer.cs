using Metro.Journeys;
using Metro.Stations;
using Metro.Trains.Driving;
using Unity.Properties;

namespace Metro.Trains.Routes
{

    public sealed class ForeheadDisplayRenderer : DisplayRendererBase
    {

        [CreateProperty]
        public string Text { get; private set; }

        public override void OnJourneyChanged()
        {
            Text = IsInService ? $"{Route.Relation} {Route.Destination.Forehead()}" : "";
            Blink(4);
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Driving && !IsInService && Journey is not Afk)
                Text = Journey is ICarriageHouseJourney {ToCarriageHouse: true} ? ToCarriageHouse : None;
        }

    }

}

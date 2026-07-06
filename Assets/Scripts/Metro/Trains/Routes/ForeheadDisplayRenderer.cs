using Metro.Journeys;
using Metro.Stations;
using Metro.Trains.Driving;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class ForeheadDisplayRenderer : DisplayRendererBase
    {

        private Label _label;

        protected override void Initialize(VisualElement root) => _label = root.Q<Label>();

        public override void OnJourneyChanged()
        {
            _label.text = IsInService ? $"{Route.Relation} {Route.Destination.Forehead()}" : "";
            Blink(4);
        }

        public override void OnStateChanged()
        {
            if (State == DriverState.Driving && !IsInService && Journey is not Afk)
                _label.text = "-";
        }

    }

}

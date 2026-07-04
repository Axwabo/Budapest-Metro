using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class ForeheadDisplayRenderer : DisplayRendererBase
    {

        private Label _label;

        protected override void Initialize(VisualElement root) => _label = root.Q<Label>();

        public override void OnJourneyChanged() => _label.text = JourneyManager.IsInService ? $"{CurrentJourney.Relation} {CurrentJourney.Destination.Name}" : "-";

    }

}

using Metro.Stations;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class TransfersDisplay
    {

        private readonly VisualElement _metroIcon;
        private readonly Label _metroList;
        private readonly VisualElement _railways;
        private readonly VisualElement _regionalBuses;

        public TransfersDisplay(VisualElement root)
        {
            _metroIcon = root.Q("MetroIcon");
            _metroList = root.Q<Label>("MetroList");
            _railways = root.Q("Railways");
            _regionalBuses = root.Q("RegionalBuses");
        }

        public void Display(string name)
        {
            if (!StationIdCache.TryGet(name, out var id))
                return; // TODO: clear or something?
            _metroIcon.Display(!string.IsNullOrEmpty(id.Metros));
            _metroList.text = id.Metros;
            _railways.Display(id.Railways);
            _regionalBuses.Display(id.RegionalBuses);
        }

    }

}

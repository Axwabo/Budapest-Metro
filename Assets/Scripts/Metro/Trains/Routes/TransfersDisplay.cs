using Metro.Stations;
using Unity.Properties;
using UnityEngine.UIElements;

// ReSharper disable NotAccessedField.Global

namespace Metro.Trains.Routes
{

    public sealed class TransfersDisplay
    {

        private float _size;

        private float _translate;

        [CreateProperty]
        public bool Bus;

        [CreateProperty]
        public string BusList;

        [CreateProperty]
        public bool Metro;

        [CreateProperty]
        public string MetroList;

        [CreateProperty]
        public bool Railways;

        [CreateProperty]
        public bool RegionalBuses;

        [CreateProperty]
        public bool Tram;

        [CreateProperty]
        public string TramList;

        [CreateProperty]
        public bool Trolley;

        [CreateProperty]
        public string TrolleyList;

        public TransfersDisplay(VisualElement root)
        {
            _root = root;
            root.dataSource = this;
        }

        public bool TransitionCompleted => _translate >= _size;

        public void Display(string name)
        {
            _size = 0;
            if (!StationIdCache.TryGet(name, out var id))
            {
                (Metro, MetroList) = DisplayList("");
                Railways = false;
                RegionalBuses = false;
                (Tram, TramList) = DisplayList("");
                (Trolley, TrolleyList) = DisplayList("");
                (Bus, BusList) = DisplayList("");
                return;
            }

            (Metro, MetroList) = DisplayList(id.Metros);
            Railways = id.Railways;
            RegionalBuses = id.RegionalBuses;
            (Tram, TramList) = DisplayList(id.Trams);
            (Trolley, TrolleyList) = DisplayList(id.Trolleys);
            (Bus, BusList) = DisplayList(id.LocalBuses);
        }

        private static (bool Icon, string List) DisplayList(string text) => (!string.IsNullOrEmpty(text), text);

    }

}

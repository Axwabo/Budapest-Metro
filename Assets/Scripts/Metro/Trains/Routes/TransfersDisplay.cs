using Metro.Stations;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class TransfersDisplay
    {

        private const float PixelsPerSecond = 300;

        private readonly VisualElement _busIcon;
        private readonly Label _busList;
        private readonly VisualElement _metroIcon;
        private readonly Label _metroList;
        private readonly VisualElement _railways;
        private readonly VisualElement _regionalBuses;

        private readonly VisualElement _root;
        private readonly VisualElement _tramIcon;
        private readonly Label _tramList;
        private readonly VisualElement _trolleyIcon;
        private readonly Label _trolleyList;

        private float _size;

        private float _translate;

        public TransfersDisplay(VisualElement root)
        {
            _root = root;
            _metroIcon = root.Q("MetroIcon");
            _metroList = root.Q<Label>("MetroList");
            _railways = root.Q("Railways");
            _regionalBuses = root.Q("RegionalBuses");
            _tramIcon = root.Q("TramIcon");
            _tramList = root.Q<Label>("TramList");
            _trolleyIcon = root.Q("TrolleyIcon");
            _trolleyList = root.Q<Label>("TrolleyList");
            _busIcon = root.Q("BusIcon");
            _busList = root.Q<Label>("BusList");
        }

        public bool TransitionCompleted => _translate >= _size;

        public void Display(string name)
        {
            ResetPosition();
            _root.RegisterCallbackOnce<GeometryChangedEvent>(UpdateGeometry); // holy GD reference
            if (!StationIdCache.TryGet(name, out var id))
                return; // TODO: clear or something?
            DisplayList(_metroIcon, _metroList, id.Metros);
            _railways.Display(id.Railways);
            _regionalBuses.Display(id.RegionalBuses);
            DisplayList(_tramIcon, _tramList, id.Trams);
            DisplayList(_trolleyIcon, _trolleyList, id.Trolleys);
            DisplayList(_busIcon, _busList, id.LocalBuses);
        }

        private void UpdateGeometry(GeometryChangedEvent ev)
        {
            // can't believe I have to do ts myself
            _size = TotalWidth(_root);
            var hierarchy = _root.hierarchy;
            var count = hierarchy.childCount;
            for (var i = 0; i < count; i++)
                _size += TotalWidth(hierarchy[i]);
        }

        private static float TotalWidth(VisualElement element) => element.resolvedStyle.width + element.resolvedStyle.paddingLeft + element.resolvedStyle.paddingRight;

        private static void DisplayList(VisualElement icon, Label list, string text)
        {
            icon.Display(!string.IsNullOrEmpty(text));
            list.text = text;
        }

        public void ResetPosition()
        {
            _root.style.translate = StyleKeyword.None;
            _translate = 0;
        }

        public void Update()
        {
            _root.style.translate = new Translate(-_translate, 0);
            _translate += Clock.Delta * PixelsPerSecond;
        }

    }

}

using Metro.Stations;
using Unity.Properties;
using UnityEngine.UIElements;

// ReSharper disable NotAccessedField.Global

namespace Metro.Trains.Routes
{

    public sealed class TransfersDisplay
    {

        private const float PixelsPerSecond = 400;
        private const string NoScroll = "no-scrolling";

        private readonly VisualElement _root;

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
            ResetPositionInternal();
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
            _root.RegisterCallbackOnce<GeometryChangedEvent>(UpdateGeometry); // holy GD reference
        }

        private void UpdateGeometry(GeometryChangedEvent ev)
        {
            // can't believe I have to do ts myself
            _size = -ev.newRect.width;
            var hierarchy = _root.hierarchy;
            var count = hierarchy.childCount;
            for (var i = 0; i < count; i++)
                _size += TotalWidth(hierarchy[i]);
            var noScroll = _size <= 0;
            _root.EnableInClassList(NoScroll, noScroll);
        }

        private static float TotalWidth(VisualElement element)
        {
            var textWidth = element is Label label
                ? label.MeasureTextSize(label.text, 0, VisualElement.MeasureMode.Undefined, 0, VisualElement.MeasureMode.Undefined).x
                : element.resolvedStyle.width;
            return textWidth + element.resolvedStyle.paddingLeft + element.resolvedStyle.paddingRight;
        }

        private static (bool Icon, string List) DisplayList(string text) => (!string.IsNullOrEmpty(text), text);

        public void ResetPosition()
        {
            if (_size > 0)
                ResetPositionInternal();
        }

        private void ResetPositionInternal()
        {
            _root.style.translate = StyleKeyword.None;
            _translate = 0;
        }

        public void Update()
        {
            if (_size <= 0)
                return;
            _root.style.translate = new Translate(-_translate, 0);
            _translate += Clock.Delta * PixelsPerSecond;
        }

    }

}

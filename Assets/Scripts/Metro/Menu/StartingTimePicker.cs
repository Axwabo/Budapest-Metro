using System;
using UnityEngine.UIElements;

namespace Metro.Menu
{

    public sealed class StartingTimePicker : DocumentComponent
    {

        private int _h;
        private int _m;
        private int _s;

        private Label _time;

        protected override void Init(VisualElement root)
        {
            var h = root.Q<SliderInt>("Hours");
            var m = root.Q<SliderInt>("Minutes");
            var s = root.Q<SliderInt>("Seconds");
            h.RegisterValueChangedCallback(evt =>
            {
                _h = evt.newValue;
                Refresh();
            });
            m.RegisterValueChangedCallback(evt =>
            {
                _m = evt.newValue;
                Refresh();
            });
            s.RegisterValueChangedCallback(evt =>
            {
                _s = evt.newValue;
                Refresh();
            });
            h.value = _h = Clock.Start.Hours;
            m.value = _m = Clock.Start.Minutes;
            s.value = _s = Clock.Start.Seconds;
            _time = root.Q<Label>("Time");
            Refresh();
        }

        private void Refresh() => _time.text = Clock.Format(Clock.Start = new TimeSpan(_h, _m, _s));

    }

}

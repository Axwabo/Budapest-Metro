using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Menu
{

    public sealed class TimeScale : DocumentComponent
    {

        private Label _label;
        private Slider _slider;

        private void OnDestroy() => Time.timeScale = 1;

        protected override void Init(VisualElement root)
        {
            _slider = root.Q<Slider>("TimeScale");
            _slider.RegisterValueChangedCallback(evt =>
            {
                Time.timeScale = evt.newValue;
                _label.text = $"{Time.timeScale:#.##}x";
            });
            _label = root.Q<Label>("TimeScale");
            root.Q<Button>("Reset").clicked += () =>
            {
                Time.timeScale = 1;
                _label.text = "1x";
                _slider.value = 1;
            };
        }

    }

}

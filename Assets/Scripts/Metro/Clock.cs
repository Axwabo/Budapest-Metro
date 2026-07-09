using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro
{

    public sealed class Clock : MonoBehaviour
    {

        private static DateTime _start = DateTime.Now;

        [SerializeField]
        private string start = "14:19:42";

        private UIDocument _document;

        private int _second;

        public static TimeSpan Start
        {
            set => _start = DateTime.Today + value;
        }

        public static TimeSpan Now => _start.AddSeconds(Time.timeSinceLevelLoadAsDouble).TimeOfDay;

        public static float Delta
        {
            get
            {
                var scale = Time.timeScale;
                return Mathf.Min(scale * 0.5f, Time.unscaledDeltaTime * scale);
            }
        }

        private void Awake()
        {
            if (TimeSpan.TryParse(start, out var startTime))
                Start = startTime;
            if (!TryGetComponent(out _document))
                Destroy(this);
        }

        private void Update()
        {
            var now = Now;
            var second = now.Seconds;
            if (second == _second)
                return;
            _second = second;
            _document.rootVisualElement.Q<Label>().text = Format(now);
        }

        public static string Format(TimeSpan now) => now.ToString("hh':'mm':'ss");

    }

}

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public sealed class OnboardDisplayRenderer : AssemblyComponent
    {

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private PanelSettings settingsReference;

        [SerializeField]
        private UIDocument document;

        private Label _label;

        private PanelSettings _settings;

        private bool _station;

        private float _time;

        public RenderTexture Texture { get; private set; }

        private void Awake()
        {
            Texture = new RenderTexture(width, height, GraphicsFormat.R8G8B8A8_SNorm, GraphicsFormat.None);
            _settings = Instantiate(settingsReference);
            _settings.targetTexture = Texture;
            document.panelSettings = _settings;
        }

        private void Update()
        {
            if ((_time -= Clock.Delta) > 0)
                return;
            _station = !_station;
            _time = 5;
            _label.text = _station ? JourneyManager.Stop?.Name : Clock.Now.ToString("hh':'mm");
        }

        private void OnDestroy()
        {
            Texture.Release();
            Destroy(Texture);
            Destroy(_settings);
        }

        protected override void OnInitialized() => _label = document.rootVisualElement.Q<Label>();

        public override void OnStationChanged()
        {
            _label.text = JourneyManager.Stop?.Name;
            _time = 5;
            _station = true;
        }

    }

}

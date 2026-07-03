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

        public RenderTexture Texture { get; private set; }

        private void Awake()
        {
            Texture = new RenderTexture(width, height, GraphicsFormat.R8G8B8A8_SNorm, GraphicsFormat.None);
            _settings = Instantiate(settingsReference);
            _settings.targetTexture = Texture;
            document.panelSettings = _settings;
        }

        private void OnDestroy()
        {
            Texture.Release();
            Destroy(Texture);
            Destroy(_settings);
        }

        protected override void OnInitialized() => _label = document.rootVisualElement.Q<Label>();

        public override void OnStationChanged() => _label.text = Parent.JourneyManager.Stop.Name;

    }

}

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public abstract class DisplayRendererBase : AssemblyComponent
    {

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private PanelSettings settingsReference;

        [SerializeField]
        private UIDocument document;

        private PanelSettings _settings;

        private VisualElement _content;

        private float _showIn;

        public RenderTexture Texture { get; private set; }

        private void Awake()
        {
            Texture = new RenderTexture(width, height, GraphicsFormat.R8G8B8A8_SNorm, GraphicsFormat.None);
            _settings = Instantiate(settingsReference);
            _settings.targetTexture = Texture;
            document.panelSettings = _settings;
        }

        protected virtual void Update()
        {
            if (_showIn > 0 && (_showIn -= Clock.Delta) <= 0)
                _content?.Display();
        }

        private void OnDestroy()
        {
            Texture.Release();
            Destroy(Texture);
            Destroy(_settings);
        }

        protected override void OnInitialized()
        {
            var root = document.rootVisualElement;
            _content = root.Q("Content");
            Initialize(root);
        }

        protected void Blink(float seconds)
        {
            _content?.Display(false);
            _showIn = seconds;
        }

        protected abstract void Initialize(VisualElement root);

    }

}

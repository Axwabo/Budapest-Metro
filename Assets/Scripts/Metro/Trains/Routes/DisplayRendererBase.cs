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

        private VisualElement _content;

        private PanelSettings _settings;

        private float _showIn;

        private RenderTexture _texture;

        public Material Material { get; private set; }

        private void Awake()
        {
            _texture = new RenderTexture(width, height, GraphicsFormat.R8G8B8A8_SNorm, GraphicsFormat.None);
            _settings = Instantiate(settingsReference);
            _settings.name = name;
            _settings.targetTexture = _texture;
            document.panelSettings = _settings;
            Material = new Material(Shader.Find("Universal Render Pipeline/Lit")) {mainTexture = _texture};
        }

        protected virtual void Update()
        {
            if (_showIn > 0 && (_showIn -= Clock.Delta) <= 0 && _content != null)
                _content.visible = true;
        }

        private void OnDestroy()
        {
            _texture.Release();
            Destroy(Material);
            Destroy(_texture);
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
            if (_content != null)
                _content.visible = false;
            _showIn = seconds;
        }

        protected abstract void Initialize(VisualElement root);

    }

}

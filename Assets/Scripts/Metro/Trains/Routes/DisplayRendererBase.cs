using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Trains.Routes
{

    public abstract class DisplayRendererBase : AssemblyComponent
    {

        protected const string ToCarriageHouse = "Kocsiszínbe";
        protected const string None = "-";

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private RenderMaterial material;

        private VisualElement _content;

        private float _showIn;

        public Material Material => material.Material;

        private void Awake() => material.Init(name, document);

        protected virtual void Update()
        {
            if (_showIn > 0 && (_showIn -= Clock.Delta) <= 0 && _content != null)
                _content.visible = true;
        }

        private void OnDestroy() => material.Destroy();

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

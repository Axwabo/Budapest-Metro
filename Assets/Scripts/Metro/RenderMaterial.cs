using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Metro
{

    [Serializable]
    public sealed class RenderMaterial
    {

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private Material parentMaterial;

        [SerializeField]
        private PanelSettings settingsReference;

        private PanelSettings _settings;

        private RenderTexture _texture;

        public Material Material { get; private set; }

        public void Init(string name, UIDocument document, ThemeStyleSheet theme = null)
        {
            _texture = new RenderTexture(width, height, GraphicsFormat.R8G8B8A8_SNorm, GraphicsFormat.None);
            _settings = Object.Instantiate(settingsReference);
            _settings.name = name;
            _settings.targetTexture = _texture;
            if (theme)
                _settings.themeStyleSheet = theme;
            Material = new Material(parentMaterial.shader)
            {
                parent = parentMaterial,
                mainTexture = _texture
            };
            document.panelSettings = _settings;
        }

        public void Destroy()
        {
            _texture.Release();
            Object.Destroy(Material);
            Object.Destroy(_texture);
            Object.Destroy(_settings);
        }

    }

}

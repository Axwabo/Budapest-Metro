using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    [RequireComponent(typeof(Station))]
    public sealed class StationSigns : MonoBehaviour
    {

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private Material parentMaterial;

        private bool _done;

        private Material _material;

        private RenderTexture _texture;

        private void Start()
        {
            throw new NotImplementedException();
        }

        private void LateUpdate()
        {
            if (_done)
                Dispose();
            else
                _done = true;
        }

        private void OnDestroy()
        {
            _texture.Release();
            Destroy(_texture);
        }

        private void Dispose()
        {
            Destroy(document);
            enabled = false;
        }

    }

}

using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public sealed class StationSigns : MonoBehaviour
    {

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private RenderMaterial material;

        [SerializeField]
        private MeshRenderer[] renderers;

        private bool _done;

        private Station _station;

        private void Awake()
        {
            _station = GetComponentInParent<Station>();
            material.Init(name, document);
            foreach (var meshRenderer in renderers)
                meshRenderer.sharedMaterial = material.Material;
        }

        private void Start()
        {
            var root = document.rootVisualElement;
            root.Q<Label>("Name").text = _station.name;
            root.RegisterCallbackOnce<GeometryChangedEvent>(_ => _done = true);
        }

        private void Update()
        {
            if (_done)
                Dispose();
        }

        private void OnDestroy() => material.Destroy();

        private void Dispose()
        {
            Destroy(document);
            enabled = false;
        }

    }

}

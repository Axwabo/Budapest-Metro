using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public abstract class StationSignBase : MonoBehaviour
    {

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private RenderMaterial material;

        [SerializeField]
        private MeshRenderer[] renderers;

        private bool _done;

        protected Station Station { get; set; }

        private void Awake()
        {
            Station = GetComponentInParent<Station>();
            material.Init(Station.name, document, Station.ID.Relation.Theme);
            foreach (var meshRenderer in renderers)
                meshRenderer.sharedMaterial = material.Material;
        }

        private void Start()
        {
            Station.RenderQueuedSigns.Add(this);
            var root = document.rootVisualElement;
            foreach (var label in root.Query<Label>("Current").Build())
                label.text = Station.name;
            root.RegisterCallbackOnce<GeometryChangedEvent>(_ => _done = true);
            var stops = root.Q("Stops");
            Build(stops, root);
        }

        private void Update()
        {
            if (_done)
                Dispose();
        }

        private void OnDestroy() => material.Destroy();

        protected virtual void Build(VisualElement stops, VisualElement root)
        {
        }

        private void Dispose()
        {
            Destroy(document);
            enabled = false;
            Station.RenderQueuedSigns.Remove(this);
        }

    }

}

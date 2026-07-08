using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public sealed class StationSigns : MonoBehaviour
    {

        [SerializeField]
        private bool reverse;

        [SerializeField]
        private UIDocument document;

        [SerializeField]
        private VisualTreeAsset stationTemplate;

        [SerializeField]
        private RenderMaterial material;

        [SerializeField]
        private MeshRenderer[] renderers;

        private bool _done;

        private Station _station;

        private void Awake()
        {
            _station = GetComponentInParent<Station>();
            material.Init(_station.name, document, _station.ID.Relation.Theme);
            foreach (var meshRenderer in renderers)
                meshRenderer.sharedMaterial = material.Material;
        }

        private void Start()
        {
            var root = document.rootVisualElement;
            root.Q<Label>("Current").text = _station.name;
            root.RegisterCallbackOnce<GeometryChangedEvent>(_ => _done = true);
            var stops = root.Q("Stops");
            var stations = reverse ? _station.ID.Relation.Reverse : _station.ID.Relation.Forwards;
            var inactive = true;
            for (var i = 0; i < stations.Length; i++)
            {
                var last = i >= stations.Length - 1;
                var station = stations[i];
                if (station == _station.ID)
                    inactive = false;
                var stop = stationTemplate.CloneTree();
                var fill = stop.Q("Fill");
                stop.EnableInClassList("inactive", inactive);
                fill.EnableInClassList("bg-accent", i == 0 || last || !string.IsNullOrEmpty(_station.ID.Metros));
                stop.Q<Label>("Name").text = station.name;
                stops.Add(stop);
                if (last)
                    continue;
                var line = new VisualElement();
                line.AddToClassList("line");
                line.AddToClassList("bg-accent");
                line.EnableInClassList("inactive", inactive);
                stops.Add(line);
            }

            if (_station.ID == stations[^1])
            {
                root.Q("OtherSide").Display();
                return;
            }

            var directiohn = root.Q("Direction");
            directiohn.Display();
            directiohn.Q<Label>("Name").text = stations[^1].name;
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

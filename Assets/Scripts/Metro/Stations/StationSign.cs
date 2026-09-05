using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public sealed class StationSign : StationSignBase
    {

        [SerializeField]
        private bool reverse;

        [SerializeField]
        private bool right;

        [SerializeField]
        private VisualTreeAsset stationTemplate;

        [SerializeField]
        private bool skipPreviousStations;

        protected override void Build(VisualElement stops, VisualElement root)
        {
            var stations = reverse ? Station.ID.Relation.Reverse : Station.ID.Relation.Forwards;
            var inactive = true;
            for (var i = 0; i < stations.Length; i++)
            {
                var last = i >= stations.Length - 1;
                var station = stations[i];
                var current = false;
                if (station == Station.ID)
                {
                    inactive = false;
                    current = true;
                }
                else if (inactive && skipPreviousStations)
                    continue;

                var accent = i != 0 && !last && string.IsNullOrEmpty(station.Metros);
                var stop = stationTemplate.CloneTree();
                var fill = stop.Q("Fill");
                stop.EnableInClassList("inactive", inactive);
                stop.EnableInClassList("important", !accent);
                stop.EnableInClassList("current", current);
                fill.EnableInClassList("bg-accent", accent);
                stop.Q<Label>("Name").text = station.name;
                stop.Q<Label>("Metros").text = station.Metros;
                stops.Add(stop);
                if (last)
                    continue;
                var line = new VisualElement();
                line.AddToClassList("line");
                line.AddToClassList("bg-accent");
                line.EnableInClassList("inactive", inactive);
                stops.Add(line);
            }

            if (Station.ID == stations[^1])
            {
                root.Q("OtherSide").Display();
                return;
            }

            var directiohn = root.Q("Direction");
            directiohn.Display();
            directiohn.Q<Label>("Name").text = stations[^1].name;
            if (right)
                directiohn.AddToClassList("right");
        }

    }

}

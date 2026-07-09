using Metro.Stations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Menu
{

    public sealed class StartingStationPicker : DocumentComponent
    {

        [SerializeField]
        private Relation relation;

        public static string Name { get; private set; }

        protected override void Init(VisualElement root)
        {
            var dropdown = root.Q<DropdownField>("Station");
            foreach (var id in relation.Forwards)
            {
                var station = id.name;
                Name ??= station;
                dropdown.choices.Add(station);
            }

            dropdown.index = 0;
        }

    }

}

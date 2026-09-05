using Metro.Stations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Menu
{

    public sealed class StartingStationPicker : DocumentComponent
    {

        [SerializeField]
        private Relation relation;

        private VisualElement _root;

        public static string Name { get; private set; }

        protected override void Init(VisualElement root)
        {
            _root = root;
            UpdateList();
        }

        public void UseRelation(Relation newRelation)
        {
            relation = newRelation;
            UpdateList();
        }

        private void UpdateList()
        {
            var dropdown = _root.Q<DropdownField>("Station");
            dropdown.choices.Clear();
            foreach (var id in relation.Forwards)
            {
                var station = id.name;
                Name ??= station;
                dropdown.choices.Add(station);
            }

            Name ??= dropdown.choices[0];
            dropdown.index = -1;
            dropdown.index = Mathf.Max(0, dropdown.choices.IndexOf(Name));
            dropdown.RegisterValueChangedCallback(evt => Name = evt.newValue);
        }

    }

}

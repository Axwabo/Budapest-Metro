using Metro.Stations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Menu
{

    public sealed class StartingRelationPicker : DocumentComponent
    {

        [SerializeField]
        private Relation relation;

        [SerializeField]
        private string id;

        [SerializeField]
        private StartingStationPicker picker;

        [SerializeField]
        private SceneSwitcher switcher;

        [SerializeField]
        private bool isDefault;

        [SerializeField]
        private string sceneName;

        [SerializeField]
        private StartingRelationPicker other;

        private Button _button;

        protected override void Init(VisualElement root)
        {
            _button = root.Q<Button>(id);
            _button.clicked += ButtonOnclicked;
            if (isDefault)
                _button.parent.AddToClassList("selected");
        }

        private void ButtonOnclicked()
        {
            _button.parent.AddToClassList("selected");
            other._button.parent.RemoveFromClassList("selected");
            picker.UseRelation(relation);
            switcher.sceneName = sceneName;
        }

    }

}

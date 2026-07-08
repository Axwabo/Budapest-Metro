using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Metro
{

    // this sucks sm
    // "you should disable all action maps on start," and then it complains when I do what it told me
    public sealed class GameMenu : MonoBehaviour
    {

        [SerializeField]
        private UIDocument document;

        private VisualElement _menu;

        private InputActionMap _player;

        private float _time;

        private InputActionMap _ui;

        private void Start()
        {
            //_menu = document.rootVisualElement.Q("Menu");
            foreach (var map in InputSystem.actions.actionMaps)
                if (map.name == "Player")
                {
                    _player = map;
                    map.Disable();
                }
                else if (map.name == "UI")
                {
                    _ui = map;
                    map.Disable();
                }
        }

        private void Update()
        {
            if ((_time += Clock.Delta) < 0.5f)
                return;
            enabled = false;
            OnMenu();
        }

        private void OnMenu()
        {
            if (_time < 0.5f)
                return;
            var show = _player.enabled;
            (show ? _ui : _player).Enable();
            (show ? _player : _ui).Disable();
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            _menu?.Display(show);
        }

    }

}

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

        [SerializeField]
        private InputActionAsset playerActions;

        private VisualElement _menu;

        private InputActionMap _player;

        private float _time;

        private InputActionMap _ui;

        public static GameMenu Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            _menu = document.rootVisualElement.Q("Menu");
            _ui = InputSystem.actions.FindActionMap("UI", true);
            _player = playerActions.FindActionMap("Player", true);
            _ui.Disable();
            _player.Disable();
        }

        private void Update()
        {
            if (_time < 0.5f && (_time += Clock.Delta) >= 0.5f || InputSystem.actions["Menu"].WasPressedThisFrame())
                OnMenu();
        }

        public void OnMenu()
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

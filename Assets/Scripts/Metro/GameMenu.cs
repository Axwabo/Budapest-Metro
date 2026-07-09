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

        private void Start()
        {
            _menu = document.rootVisualElement.Q("Menu");
            _player = playerActions.FindActionMap("Player", true);
            _player.Disable();
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
            if (show)
                _player.Disable();
            else
                _player.Enable();
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            _menu?.Display(show);
        }

    }

}

using UnityEngine;

namespace Metro.Stations
{

    [RequireComponent(typeof(MeshRenderer))]
    public sealed class WarningLight : MonoBehaviour
    {

        [SerializeField]
        private Material off;

        private float _delay;

        private Material _on;

        private bool _onAssigned = true;

        private MeshRenderer _renderer;

        private LightState _state;

        public LightState State
        {
            get => _state;
            set
            {
                _state = value;
                if (_state == LightState.On)
                    Assign(true);
                else if (_state == LightState.Off)
                    Assign(false);
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _on = _renderer.sharedMaterial;
        }

        private void Update()
        {
            if ((_delay -= Clock.Delta) > 0 || _state != LightState.Blinking)
                return;
            _delay = 0.5f;
            Assign(!_onAssigned);
        }

        private void Assign(bool on)
        {
            if (_onAssigned == on)
                return;
            _onAssigned = on;
            _renderer.sharedMaterial = on ? _on : off;
        }

    }

}

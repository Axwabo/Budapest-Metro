using UnityEngine;

namespace Metro.Trains.Doors
{

    [RequireComponent(typeof(MeshRenderer))]
    public sealed class AlarmDiode : MonoBehaviour
    {

        [SerializeField]
        private Material on;

        [SerializeField]
        private Material off;

        private bool _on;

        private MeshRenderer _renderer;

        public bool On
        {
            set
            {
                if (_on == value)
                    return;
                _on = value;
                _renderer.sharedMaterial = value ? on : off;
            }
        }

        private void Awake() => _renderer = GetComponent<MeshRenderer>();

        public void Toggle() => On = !_on;

    }

}

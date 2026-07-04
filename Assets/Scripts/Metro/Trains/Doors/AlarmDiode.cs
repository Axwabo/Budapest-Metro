using UnityEngine;

namespace Metro.Trains.Doors
{

    public sealed class AlarmDiode : MonoBehaviour
    {

        [SerializeField]
        private Material on;

        [SerializeField]
        private Material off;

        [SerializeField]
        private MeshRenderer[] renderers;

        private bool _on;

        public bool On
        {
            set
            {
                if (_on == value)
                    return;
                _on = value;
                foreach (var meshRenderer in renderers)
                    meshRenderer.sharedMaterial = value ? on : off;
            }
        }

        public void Toggle() => On = !_on;

    }

}

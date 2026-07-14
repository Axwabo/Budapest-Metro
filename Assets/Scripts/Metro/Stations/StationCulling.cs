using Metro.Movement;
using UnityEngine;

namespace Metro.Stations
{

    public sealed class StationCulling : MonoBehaviour
    {

        private const float MaxDistance = 500;

        [SerializeField]
        private GameObject[] objects;

        private bool _culled;

        private Vector3 _position;

        private Station _station;

        private float _time = 0.5f;

        private void Awake()
        {
            _position = transform.position;
            _station = GetComponent<Station>();
        }

        private void Update()
        {
            if (_station.RenderQueuedSigns.Count != 0 || (_time -= Time.unscaledDeltaTime) > 0)
                return;
            _time = 0.5f;
            var cull = Vector3.Distance(_position, MovementController.LastPosition) > MaxDistance;
            if (cull == _culled)
                return;
            _culled = cull;
            foreach (var o in objects)
                o.SetActive(!cull);
        }

    }

}

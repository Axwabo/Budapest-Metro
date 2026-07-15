using Metro.Movement;
using UnityEngine;

namespace Metro.Trains
{

    public sealed class AssemblyCulling : AssemblyComponent
    {

        private const float MaxDistance = 500;

        [SerializeField]
        private GameObject[] objects;

        private bool _culled;

        private float _time = 2;

        private void Update()
        {
            if ((_time -= Time.unscaledDeltaTime) > 0)
                return;
            _time = 0.5f;
            var frontPosition = Parent.Cars[0].BodyTransform.position;
            var rearPosition = Parent.Cars[^1].BodyTransform.position;
            var frontDistance = Vector3.Distance(frontPosition, MovementController.LastPosition);
            var rearDistance = Vector3.Distance(rearPosition, MovementController.LastPosition);
            var cull = Mathf.Min(frontDistance, rearDistance) > MaxDistance;
            if (cull == _culled)
                return;
            _culled = cull;
            foreach (var o in objects)
                o.SetActive(!cull);
        }

    }

}

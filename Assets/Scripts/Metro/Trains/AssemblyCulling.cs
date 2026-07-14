using Metro.Movement;
using UnityEngine;

namespace Metro.Trains
{

    public sealed class AssemblyCulling : AssemblyComponent
    {

        private const float MaxDistance = 200;

        [SerializeField]
        private GameObject[] objects;

        private bool _culled;

        private void Update()
        {
            var frontPosition = Parent.Cars[0].Transform.position;
            var rearPosition = Parent.Cars[^1].Transform.position;
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

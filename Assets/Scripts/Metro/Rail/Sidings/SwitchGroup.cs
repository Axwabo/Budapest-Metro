using Metro.Journeys;
using UnityEngine;

namespace Metro.Rail.Sidings
{

    public sealed class SwitchGroup : MonoBehaviour
    {

        public SwitchState[] switches;

        [ContextMenu("Activate")]
        public void Activate()
        {
            foreach (var state in switches)
                state.@switch.IsLeft = state.isLeft;
        }

    }

}

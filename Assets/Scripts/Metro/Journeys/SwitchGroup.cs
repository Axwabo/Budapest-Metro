using UnityEngine;

namespace Metro.Journeys
{

    public sealed class SwitchGroup : MonoBehaviour
    {

        public SwitchState[] switches;

        public void Activate()
        {
            foreach (var state in switches)
                state.@switch.IsLeft = state.isLeft;
        }

    }

}

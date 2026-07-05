using Metro.Journeys;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class PassedOutMarker : ControlPoint
    {

        [field: SerializeField]
        public ReversingSidingArea Area { get; private set; }

    }

}

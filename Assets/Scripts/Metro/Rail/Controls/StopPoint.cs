using Metro.Journeys;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class StopPoint : ControlPoint
    {

        [field: SerializeField]
        public ReversingSidingArea SidingAreaAhead { get; private set; }

    }

}

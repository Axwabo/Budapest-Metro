using Metro.Journeys;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class ServiceEntryStopPoint : StopPoint
    {

        [field: SerializeField]
        public ReversingSidingArea Area { get; private set; }

    }

}

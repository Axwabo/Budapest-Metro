using Metro.Journeys;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metro.Rail.Controls
{

    public class ServiceAreaStopPoint : StopPoint
    {

        [field: FormerlySerializedAs("<SidingAreaAhead>k__BackingField")]
        [field: SerializeField]
        public ReversingSidingArea Area { get; private set; }

    }

}

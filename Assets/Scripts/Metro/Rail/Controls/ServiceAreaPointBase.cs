using Metro.Rail.Sidings;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public abstract class ServiceAreaPointBase : StopPoint
    {

        [field: SerializeField]
        public ReversingSidingArea Area { get; private set; }

    }

}

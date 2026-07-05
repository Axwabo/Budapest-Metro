using Metro.Stations;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class WarningLightTrigger : ControlPoint
    {

        [field: SerializeField]
        public WarningLight Light { get; private set; }
        
    }

}

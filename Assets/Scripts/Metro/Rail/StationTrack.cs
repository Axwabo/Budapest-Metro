using Metro.Stations;
using UnityEngine;

namespace Metro.Rail
{

    public sealed class StationTrack : StraightTrack
    {

        [field: SerializeField]
        public bool Reverse { get; private set; }

        public Station Station { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Station = GetComponentInParent<Station>();
        }

    }

}

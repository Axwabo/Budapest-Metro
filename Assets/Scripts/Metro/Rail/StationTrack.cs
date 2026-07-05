using Metro.Stations;
using UnityEngine;

namespace Metro.Rail
{

    [ExecuteInEditMode]
    public sealed class StationTrack : StraightTrack
    {

        [field: SerializeField]
        public bool Reverse { get; private set; }

        public Station Station { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
                Station = GetComponentInParent<Station>();
        }

    }

}

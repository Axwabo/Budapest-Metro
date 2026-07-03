using System.Linq;
using Metro.Rail.Controls;
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

        public StopPoint StopPoint { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
                Station = GetComponentInParent<Station>();
        }

        protected override void Start()
        {
            if (!Application.isPlaying)
                return;
            base.Start();
            StopPoint = ControlPoints.OfType<StopPoint>().First();
        }

    }

}

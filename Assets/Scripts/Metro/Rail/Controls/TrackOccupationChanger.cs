using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class TrackOccupationChanger : ControlPoint
    {

        [SerializeField]
        private bool free;

        private new StationTrack Track => (StationTrack) base.Track;

        public override void OnPassed(Axle axle)
        {
            if (free)
                Track.Occupants.Remove(axle);
            else
                Track.Occupants.Add(axle);
        }

    }

}

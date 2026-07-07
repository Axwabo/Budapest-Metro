using Metro.Stations;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class TrackOccupationChanger : ControlPoint
    {

        [SerializeField]
        private bool free;

        private int _occupiedCount;

        private new StationTrack Track => (StationTrack) base.Track;

        public override void OnPassed(Axle axle)
        {
            if (!free)
            {
                Track.Occupants.Add(axle);
                return;
            }

            if (Track.Occupants.Count > _occupiedCount)
                Track.Light.State = LightState.On;
            Track.Occupants.Remove(axle);
            _occupiedCount = Track.Occupants.Count;
        }

    }

}

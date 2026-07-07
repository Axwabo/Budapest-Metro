using UnityEngine;

namespace Metro.Rail.Controls
{

    public sealed class ServiceEntryStopPoint : ServiceAreaPointBase
    {

        [SerializeField]
        private StationTrack[] clear;

        public bool TracksClear
        {
            get
            {
                foreach (var track in clear)
                    if (track.IsOccupied)
                        return false;
                return true;
            }
        }

    }

}

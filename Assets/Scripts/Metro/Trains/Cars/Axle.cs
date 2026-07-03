using Metro.Rail;
using UnityEngine;

namespace Metro.Trains.Cars
{

    // TODO: make it an assembly component maybe?
    public sealed class Axle : CarComponent
    {

        private float _distance;

        private Transform _t;

        private TrackSegment _track;

        private void FixedUpdate()
        {
            if (Assembly.Speed == 0)
                return;
            _distance += Time.fixedDeltaTime * Assembly.Speed;
            if (_distance > _track.Length)
            {
                if (!_track.Next)
                    return;
                _distance -= _track.Length;
                _track = _track.Next;
            }
            else if (_distance < 0)
            {
                if (!_track.Previous)
                    return;
                _track = _track.Previous;
                _distance = _track.Length + _distance;
            }

            UpdateLocation();
        }

        protected override void OnInitialized()
        {
            _t = transform;
            _track = Assembly.startingTrack;
            _distance = Assembly.transform.InverseTransformPoint(_t.position).z;
            UpdateLocation();
        }

        private void UpdateLocation()
        {
            var pose = _track.Sample(_distance);
            _t.SetPositionAndRotation(pose.position, pose.rotation);
        }

    }

}

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
            var speed = Assembly.Motor.AbsoluteSpeed;
            if (speed == 0)
                return;
            var previousDistance = _distance;
            _distance += Time.fixedDeltaTime * speed;
            if (_distance > _track.Length)
            {
                PassPoints(previousDistance);
                if (!_track.Next)
                    return;
                _distance -= _track.Length;
                _track = _track.Next;
                PassPoints(0);
            }
            else if (_distance < 0)
            {
                PassPoints(previousDistance);
                if (!_track.Previous)
                    return;
                _track = _track.Previous;
                _distance = _track.Length + _distance;
                PassPoints(_track.Length);
            }

            UpdateLocation();
            PassPoints(previousDistance);
        }

        private void PassPoints(float previousDistance)
        {
            var reverse = Assembly.Motor.Reverse;
            foreach (var point in _track.ControlPoints)
                if (reverse
                        ? point.Distance > previousDistance && point.Distance >= _distance
                        : point.Distance > previousDistance && point.Distance <= _distance)
                    Assembly.Driver.OnAxlePassed(point);
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

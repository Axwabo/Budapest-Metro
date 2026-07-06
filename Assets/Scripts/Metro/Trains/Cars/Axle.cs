using Metro.Rail;
using Metro.Rail.Controls;
using UnityEngine;

namespace Metro.Trains.Cars
{

    // TODO: make it an assembly component maybe?
    public sealed class Axle : CarComponent
    {

        private bool _locationUpdated;

        public Transform Transform { get; private set; }

        public float Distance { get; private set; }

        public TrackSegment Track { get; private set; }

        private void Update() => _locationUpdated = false;

        private void FixedUpdate()
        {
            var speed = Assembly.Motor.AbsoluteSpeed;
            if (speed == 0)
                return;
            var previousDistance = Distance;
            Distance += Time.fixedDeltaTime * speed;
            if (Distance > Track.Length)
            {
                PassPoints(previousDistance);
                if (!Track.Next)
                    return;
                Distance -= Track.Length;
                Track = Track.Next;
                PassPoints(0);
            }
            else if (Distance < 0)
            {
                PassPoints(previousDistance);
                if (!Track.Previous)
                    return;
                Track = Track.Previous;
                Distance = Track.Length + Distance;
                PassPoints(Track.Length);
            }

            if (!_locationUpdated)
                UpdateLocation();
            _locationUpdated = true;
            PassPoints(previousDistance);
        }

        private void PassPoints(float previousDistance)
        {
            var reverse = Assembly.Motor.Reverse;
            foreach (var point in Track.ControlPoints)
                if (point.Reverse == reverse && reverse
                        ? point.Distance <= previousDistance && point.Distance >= Distance
                        : point.Distance >= previousDistance && point.Distance <= Distance)
                    Pass(point);
        }

        private void Pass(ControlPoint point)
        {
            point.OnPassed(this);
            Assembly.Driver.OnAxlePassed(point);
        }

        protected override void OnInitialized()
        {
            Transform = transform;
            Track = Assembly.startingTrack;
            Distance = Assembly.transform.InverseTransformPoint(Transform.position).z;
            UpdateLocation();
        }

        private void UpdateLocation()
        {
            var pose = Track.Sample(Distance);
            Transform.SetPositionAndRotation(pose.position, pose.rotation);
        }

    }

}

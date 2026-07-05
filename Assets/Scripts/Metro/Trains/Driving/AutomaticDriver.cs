using System;
using System.Collections.Generic;
using Metro.Rail;
using Metro.Rail.Controls;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Trains.Driving
{

    public sealed class AutomaticDriver : AssemblyComponent
    {

        private const float SlowingHeadroom = 10;
        private const float BrakingHeadroom = 2;
        private const float StoppingHeadroom = 0.01f;

        private readonly List<IDepartureBlocker> _departureBlockers = new();

        private readonly HashSet<ControlPoint> _passedPoints = new();

        private TimeSpan _departAt = TimeSpan.MaxValue;

        private DriverState _previousState;

        public new DriverState State { get; private set; }

        public float SecondsSinceTargetDeparture => (float) (Clock.Now - _departAt).TotalSeconds;

        public bool IsOnTargetTrack => IsTargetTrack(FrontAxle.Track);

        public bool CanDepart
        {
            get
            {
                foreach (var blocker in _departureBlockers)
                    if (!blocker.CanDepart)
                        return false;
                return true;
            }
        }

        private Motor Motor => Parent.Motor;

        private Axle FrontAxle => Parent.PrimaryAxle;

        private StopPoint Target => JourneyManager.Target;

        private void Update()
        {
            if (State == DriverState.Driving)
                Drive();
            else if (Clock.Now >= _departAt)
                Depart();
            if (_previousState == State)
                return;
            _previousState = State;
            Parent.NotifyStateChanged();
        }

        protected override void OnInitialized() => _departureBlockers.AddRange(Parent.Components<IDepartureBlocker>());

        [ContextMenu("Mark Ready")]
        public void MarkReady()
        {
            if (State == DriverState.Stopped)
                _departAt = TimeSpan.MinValue;
        }

        private void Drive()
        {
            AdjustSpeed();
            if (Motor.AbsoluteSpeed != 0 || Motor.TargetSpeed != 0)
                return;
            State = DriverState.Stopped;
            _departAt = Clock.Now + TimeSpan.FromSeconds(Constants.MinStaySeconds);
            _passedPoints.Clear();
        }

        private void AdjustSpeed()
        {
            if (Motor.TargetSpeed == 0)
                return;
            var axle = FrontAxle;
            var brakingDistance = Motor.BrakingDistance;
            if (Motor.TargetSpeed > Constants.SlowMps && ShouldSlowDown(axle, brakingDistance))
            {
                Motor.TargetSpeed = Constants.SlowMps;
                return;
            }

            if (axle.Track.StopPoint != Target)
                return;
            var distanceToStopPoint = Mathf.Abs(axle.Distance - Target.Distance);
            if (brakingDistance + StoppingHeadroom > distanceToStopPoint)
                Motor.TargetSpeed = 0;
            else if (brakingDistance + BrakingHeadroom > distanceToStopPoint)
                Motor.TargetSpeed = 0.5f;
        }

        private bool ShouldSlowDown(Axle axle, float brakingDistance) => brakingDistance + SlowingHeadroom > Vector3.Distance(axle.Transform.position, Target.Position);

        public override void OnTargetChanged()
        {
            if (!IsInService)
                return;
            if (JourneyManager.IsDestination)
            {
                _departAt = TimeSpan.MaxValue;
                return;
            }

            var departMinStay = Clock.Now + TimeSpan.FromSeconds(Constants.MinStaySeconds);
            _departAt = Stop.Time < departMinStay
                ? departMinStay
                : Stop.Time;
        }

        private void Depart()
        {
            State = State switch
            {
                DriverState.Stopped => DriverState.WaitingForDeparture,
                DriverState.WaitingForDeparture when CanDepart => DriverState.Driving,
                _ => State
            };
            if (State != DriverState.Driving)
                return;
            Motor.Reverse = Journey.Reverse;
            Motor.TargetSpeed = IsInService ? Constants.MaxMps : Constants.SlowMps;
        }

        public void OnAxlePassed(ControlPoint point)
        {
            if (!_passedPoints.Add(point))
                return;
            if (point is StopPoint stop && stop == Target)
                Motor.TargetSpeed = 0;
        }

        private bool IsTargetTrack(TrackSegment track) => track.StopPoint == Target;

    }

}

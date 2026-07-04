using System;
using System.Collections.Generic;
using Metro.Rail;
using Metro.Rail.Controls;
using Metro.Stations;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Trains.Driving
{

    public sealed class AutomaticDriver : AssemblyComponent
    {

        private const float SlowingHeadroom = 10;
        private const float BrakingHeadroom = 2;
        private const float StoppingHeadroom = 0.01f;

        private readonly HashSet<ControlPoint> _passedPoints = new();

        private TimeSpan _departAt = TimeSpan.MaxValue;

        private DriverState _previousState;

        public new DriverState State { get; private set; }

        public float SecondsToDeparture => (float) (_departAt - Clock.Now).TotalSeconds;

        public bool IsOnTargetTrack => IsTargetTrack(FrontAxle.Track);

        private Motor Motor => Parent.Motor;

        private Axle FrontAxle => Parent.PrimaryAxle;

        private bool CarsReady
        {
            get
            {
                foreach (var car in Parent.Cars)
                    if (!car.CanDepart)
                        return false;
                return true;
            }
        }

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

        private void Drive()
        {
            // TODO: handle out-of-service stop points
            if (JourneyManager.IsInService)
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

            if (axle.Track is not StationTrack stationTrack || stationTrack.Station.name != JourneyManager.Stop.Name)
                return;
            var distanceToStopPoint = Mathf.Abs(axle.Distance - stationTrack.StopPoint.Distance);
            if (brakingDistance + StoppingHeadroom > distanceToStopPoint)
                Motor.TargetSpeed = 0;
            else if (brakingDistance + BrakingHeadroom > distanceToStopPoint)
                Motor.TargetSpeed = 0.5f;
        }

        private bool ShouldSlowDown(Axle axle, float brakingDistance) => Station.TryGetLoadad(JourneyManager.Stop.Name, out var station)
                                                                         && brakingDistance + SlowingHeadroom > Vector3.Distance(axle.Transform.position, GetStopPoint(station));

        private Vector3 GetStopPoint(Station station) => (Motor.Reverse ? station.Left : station.Right).StopPoint.Position;

        public override void OnStationChanged()
        {
            if (JourneyManager.Stop is null || JourneyManager.IsDestination)
            {
                _departAt = TimeSpan.MaxValue;
                return;
            }

            var departMinStay = Clock.Now + TimeSpan.FromSeconds(Constants.MinStaySeconds);
            _departAt = JourneyManager.Stop.Time < departMinStay
                ? departMinStay
                : JourneyManager.Stop.Time;
        }

        private void Depart()
        {
            State = State switch
            {
                DriverState.Stopped or DriverState.WaitingForDeparture when CarsReady => DriverState.Driving,
                DriverState.Stopped => DriverState.WaitingForDeparture,
                _ => State
            };
            if (State != DriverState.Driving)
                return;
            if (CurrentJourney)
                Motor.Reverse = CurrentJourney.Reverse;
            Motor.TargetSpeed = Constants.MaxMps;
        }

        public void OnAxlePassed(ControlPoint point)
        {
            if (!_passedPoints.Add(point))
                return;
            // TODO: stop earlier lol
            if (point is StopPoint stop && IsTargetTrack(stop.Track))
                Motor.TargetSpeed = 0;
        }

        private bool IsTargetTrack(TrackSegment track) => !JourneyManager.IsInService || track is StationTrack stationTrack && stationTrack.Station.name == JourneyManager.Stop.Name;

    }

}

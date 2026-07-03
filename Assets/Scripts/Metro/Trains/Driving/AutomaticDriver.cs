using System;
using System.Collections.Generic;
using Metro.Journeys;
using Metro.Rail;
using Metro.Rail.Controls;
using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Trains.Driving
{

    public sealed class AutomaticDriver : AssemblyComponent
    {

        private readonly HashSet<ControlPoint> _passedPoints = new();

        private TimeSpan _departAt = TimeSpan.MaxValue;

        private DriverState _previousState;

        public new DriverState State { get; private set; }

        private JourneyDescriptor Journey => JourneyManager.Current;

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
            var axle = FrontAxle;
            // TODO: handle out-of-service stop points
            if (JourneyManager.IsInService && axle.Track is StationTrack stationTrack && stationTrack.Station.name == JourneyManager.Stop.Name)
            {
                var distanceToStopPoint = Mathf.Abs(axle.Distance - stationTrack.StopPoint.Distance);
                var brakingDistance = Motor.BrakingDistance;
                if (brakingDistance > distanceToStopPoint - 0.01f)
                    Motor.TargetSpeed = 0;
                else if (brakingDistance > distanceToStopPoint - 2)
                    Motor.TargetSpeed = 0.5f;
            }

            if (Motor.AbsoluteSpeed != 0)
                return;
            State = DriverState.Stopped;
            _departAt = Clock.Now + TimeSpan.FromSeconds(Constants.MinStaySeconds);
            _passedPoints.Clear();
        }

        public override void OnStationChanged()
        {
            if (JourneyManager.Stop is null)
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
            if (Journey)
                Motor.Reverse = Journey.Reverse;
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

using System;
using System.Collections.Generic;
using Metro.Journeys;
using Metro.Rail.Controls;

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
            if (Motor.AbsoluteSpeed != 0)
                return;
            State = DriverState.Stopped;
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
            Motor.RelativeSpeed = Constants.MaxMps;
        }

        public void OnAxlePassed(ControlPoint point)
        {
            if (!_passedPoints.Add(point))
                return;
            // TODO: stop earlier lol
            if (point is StopPoint stop && (!JourneyManager.IsInService || stop.Station.name == JourneyManager.Stop.Name))
                Motor.RelativeSpeed = 0;
        }

    }

}

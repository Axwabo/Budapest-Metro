using System;
using Metro.Journeys;

namespace Metro.Trains.Driving
{

    public sealed class AutomaticDriver : AssemblyComponent
    {

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
            if (State == DriverState.Driving && Motor.AbsoluteSpeed == 0)
                State = DriverState.Stopped;
            else if (Clock.Now >= _departAt)
                Depart();
            if (_previousState == State)
                return;
            _previousState = State;
            Parent.NotifyStateChanged();
        }

        public override void OnStationChanged() => _departAt = JourneyManager.Stop?.Time ?? TimeSpan.MaxValue;

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

    }

}

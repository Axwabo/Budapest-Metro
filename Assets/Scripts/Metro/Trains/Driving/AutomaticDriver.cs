namespace Metro.Trains.Driving
{

    public sealed class AutomaticDriver : AssemblyComponent
    {

        private DriverState _previousState;

        public new DriverState State { get; private set; }

        private void Update()
        {
            if (_previousState != State)
                Parent.NotifyStateChanged();
        }

    }

}

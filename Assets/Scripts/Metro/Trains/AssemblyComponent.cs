using Metro.Trains.Driving;

namespace Metro.Trains
{

    public abstract class AssemblyComponent : Subcomponent<MetroAssembly>
    {

        public DriverState State => Parent.Driver.State;

        public virtual void OnStateChanged()
        {
        }

        public virtual void OnStationChanged()
        {
        }

    }

}

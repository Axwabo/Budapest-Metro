using Metro.Trains.Driving;
using Metro.Trains.Routes;

namespace Metro.Trains
{

    public abstract class AssemblyComponent : Subcomponent<MetroAssembly>
    {

        public DriverState State => Parent.Driver.State;

        public JourneyManager JourneyManager => Parent.JourneyManager;

        public virtual void OnStateChanged()
        {
        }

        public virtual void OnStationChanged()
        {
        }

    }

}

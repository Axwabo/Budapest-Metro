using Metro.Trains.Driving;

namespace Metro.Trains.Cars
{

    public abstract class CarComponent : Subcomponent<MetroCar>
    {

        public MetroAssembly Assembly => Parent.Parent;

        public DriverState State => Parent.State;

        public virtual void OnStateChanged()
        {
        }

        public virtual void OnStationChanged()
        {
        }

    }

}

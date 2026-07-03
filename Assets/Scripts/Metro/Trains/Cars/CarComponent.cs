namespace Metro.Trains.Cars
{

    public abstract class CarComponent : Subcomponent<MetroCar>
    {

        public MetroAssembly Assembly => Parent.Parent;

    }

}

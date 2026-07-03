namespace Metro.Trains
{

    public abstract class AssemblyComponent : Subcomponent<MetroAssembly>
    {

        public virtual void OnStationChanged()
        {
        }

    }

}

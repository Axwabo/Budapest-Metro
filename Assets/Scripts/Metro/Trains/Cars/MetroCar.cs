using System.Collections.Generic;

namespace Metro.Trains.Cars
{

    public sealed class MetroCar : AssemblyComponent
    {

        private readonly List<CarComponent> _components = new();

        public bool CanDepart
        {
            get
            {
                foreach (var component in _components)
                    if (component is IDepartureBlocker {CanDepart: true})
                        return false;
                return true;
            }
        }

        protected override void OnInitialized() => this.GetAndInitializeComponents(_components);

        public override void OnStateChanged()
        {
            foreach (var component in _components)
                component.OnStateChanged();
        }

        public override void OnStationChanged()
        {
            foreach (var component in _components)
                component.OnStationChanged();
        }

    }

}

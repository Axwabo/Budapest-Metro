using System.Collections.Generic;

namespace Metro.Trains.Cars
{

    public sealed class MetroCar : AssemblyComponent
    {

        private readonly List<CarComponent> _components = new();

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

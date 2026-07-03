using System.Collections.Generic;
using System.Linq;

namespace Metro.Trains.Cars
{

    public sealed class MetroCar : AssemblyComponent
    {

        private readonly List<CarComponent> _components = new();

        public Axle FrontAxle { get; private set; }

        public Axle BackAxle { get; private set; }

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

        protected override void OnInitialized()
        {
            this.GetAndInitializeComponents(_components);
            var axles = _components.OfType<Axle>().OrderByDescending(e => e.Distance).ToArray();
            FrontAxle = axles[0];
            BackAxle = axles[^1];
        }

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

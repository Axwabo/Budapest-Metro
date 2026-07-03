using System.Collections.Generic;

namespace Metro.Trains.Cars
{

    public sealed class MetroCar : AssemblyComponent
    {

        private readonly List<CarComponent> _components = new();

        protected override void OnInitialized() => this.InitializeComponents(_components);

    }

}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metro.Trains.Cars
{

    public sealed class MetroCar : AssemblyComponent, ISubcomponentParent
    {

        private readonly List<CarComponent> _components = new();

        public Axle FrontAxle { get; private set; }

        public Axle BackAxle { get; private set; }

        public float FrontAxleOffset { get; private set; }

        public float BackAxleOffset { get; private set; }

        public Collider Bounds { get; private set; }

        public bool IsPlayerMounted { get; set; }

        public IEnumerable<T> Components<T>() => _components.OfType<T>();

        protected override void OnInitialized()
        {
            this.GetAndInitializeComponents(_components);
            var axles = Components<Axle>().OrderByDescending(e => e.Distance).ToArray();
            FrontAxle = axles[0];
            BackAxle = axles[^1];
            var body = this.RequireComponent<CarBody>();
            FrontAxleOffset = Mathf.Abs(body.Inverse(FrontAxle.Transform) - body.Inverse(body.Front));
            BackAxleOffset = Mathf.Abs(body.Inverse(BackAxle.Transform) - body.Inverse(body.Back));
            Bounds = body.Bounds;
        }

        public override void OnStateChanged()
        {
            foreach (var component in _components)
                component.OnStateChanged();
        }

        public override void OnStopChanged()
        {
            foreach (var component in _components)
                component.OnStationChanged();
        }

    }

}

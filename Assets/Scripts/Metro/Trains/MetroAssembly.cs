using System.Collections.Generic;
using System.Linq;
using Metro.Rail;
using Metro.Trains.Routes;
using UnityEngine;

namespace Metro.Trains
{

    public sealed class MetroAssembly : MonoBehaviour
    {

        [SerializeField]
        public TrackSegment startingTrack;

        [field: SerializeField]
        [field: Range(-Constants.MaxMps, Constants.MaxMps)]
        public float Speed { get; set; }

        private readonly List<AssemblyComponent> _components = new();

        public JourneyManager JourneyManager { get; private set; }

        public OnboardDisplayRenderer DisplayRenderer { get; private set; }

        private void Start()
        {
            this.GetComponentsInImmediateChildren(_components);
            JourneyManager = _components.OfType<JourneyManager>().First();
            DisplayRenderer = _components.OfType<OnboardDisplayRenderer>().First();
            this.InitializeComponents(_components);
            JourneyManager.Begin();
        }

        public void NotifyStationChanged()
        {
            foreach (var component in _components)
                component.OnStationChanged();
        }

    }

}

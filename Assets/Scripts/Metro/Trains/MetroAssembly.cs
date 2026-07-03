using System.Collections.Generic;
using Metro.Rail;
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

        private void Start() => this.InitializeComponents(_components, true);

    }

}

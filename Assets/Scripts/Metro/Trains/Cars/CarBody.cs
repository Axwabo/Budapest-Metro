using UnityEngine;

namespace Metro.Trains.Cars
{

    public sealed class CarBody : Connector
    {

        [field: SerializeField]
        public Transform Front { get; private set; }

        [field: SerializeField]
        public Transform Back { get; private set; }

        [field: SerializeField]
        public Collider Bounds { get; private set; }

    }

}

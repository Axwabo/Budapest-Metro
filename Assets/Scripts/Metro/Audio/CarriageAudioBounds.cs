using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Audio
{

    public sealed class CarriageAudioBounds : CarComponent
    {

        [field: SerializeField]
        public Collider Collider { get; private set; }

    }

}

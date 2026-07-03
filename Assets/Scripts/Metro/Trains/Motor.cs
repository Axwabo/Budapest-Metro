using UnityEngine;

namespace Metro.Trains
{

    public sealed class Motor : AssemblyComponent
    {

        [SerializeField]
        [Range(0, Constants.MaxMps)]
        private float speed;

        [field: SerializeField]
        public bool Reverse { get; set; }

        public float AbsoluteSpeed => Reverse ? -speed : speed;

        public float RelativeSpeed
        {
            set => speed = Mathf.Clamp(value, 0, Constants.MaxMps);
        }

    }

}

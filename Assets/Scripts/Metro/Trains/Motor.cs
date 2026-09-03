using UnityEngine;

namespace Metro.Trains
{

    public sealed class Motor : AssemblyComponent
    {

        [SerializeField]
        [Range(0, Constants.MaxMps)]
        private float speed;

        [SerializeField]
        [Min(0)]
        private float acceleration;

        [SerializeField]
        [Min(0)]
        private float deceleration;

        [field: SerializeField]
        public bool Reverse { get; set; }

        public float AbsoluteSpeed => Reverse ? -speed : speed;

        public float TargetSpeed { get; set; }

        public float BrakingDistance
        {
            get
            {
                var time = speed / acceleration;
                return time * speed * 0.5f;
            }
        }

        private void FixedUpdate()
        {
            if (TargetSpeed > speed)
                speed = Mathf.Min(TargetSpeed, speed + acceleration * Time.fixedDeltaTime);
            else if (TargetSpeed < speed)
                speed = Mathf.Max(TargetSpeed, speed - deceleration * Time.fixedDeltaTime);
        }

    }

}

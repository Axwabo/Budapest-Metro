using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Trains.Doors
{

    [RequireComponent(typeof(Animator))]
    public sealed class MetroDoor : CarComponent, IDepartureBlocker
    {

        private static readonly int Hash = Animator.StringToHash("Open");

        [field: SerializeField]
        public bool Reverse { get; private set; }

        private Animator _animator;

        public bool Open
        {
            set => _animator.SetBool(Hash, value);
        }

        private void Awake() => _animator = GetComponent<Animator>();

        public bool CanDepart
        {
            get
            {
                if (_animator.IsInTransition(0))
                    return false;
                var info = _animator.GetCurrentAnimatorStateInfo(0);
                return info.IsName("Close") && info.normalizedTime >= 1;
            }
        }

    }

}

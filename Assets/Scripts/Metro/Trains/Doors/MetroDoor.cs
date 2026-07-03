using Metro.Trains.Cars;
using UnityEngine;

namespace Metro.Trains.Doors
{

    [RequireComponent(typeof(Animator), typeof(AudioSource))]
    public sealed class MetroDoor : CarComponent, IDepartureBlocker
    {

        private static readonly int Hash = Animator.StringToHash("Open");

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [SerializeField]
        private AudioClip open;

        [SerializeField]
        private AudioClip close;

        private Animator _animator;

        private AudioSource _source;

        public bool Open
        {
            set
            {
                _animator.SetBool(Hash, value);
                _source.PlayOneShot(value ? open : close);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _source = GetComponent<AudioSource>();
        }

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

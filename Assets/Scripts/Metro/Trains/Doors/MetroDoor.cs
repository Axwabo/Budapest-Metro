using Metro.Audio;
using Metro.Trains.Cars;
using Metro.Trains.Driving;
using UnityEngine;

namespace Metro.Trains.Doors
{

    [RequireComponent(typeof(Animator), typeof(AudioSource))]
    public sealed class MetroDoor : CarComponent, IDepartureBlocker, IAudioSourceProvider
    {

        private static readonly int Hash = Animator.StringToHash("Open");

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public AlarmDiode Diode { get; private set; }

        [SerializeField]
        private AudioClip open;

        [SerializeField]
        private AudioClip close;

        private Animator _animator;

        private float _lastOpenFlash;

        private bool _targetOpen;

        public bool Open
        {
            set
            {
                _lastOpenFlash = -10;
                _targetOpen = value;
                _animator.SetBool(Hash, value);
                SingleAudioSource.PlayOneShot(value ? open : close);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SingleAudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_targetOpen || State != DriverState.Stopped)
                return;
            var info = _animator.GetCurrentAnimatorStateInfo(0);
            if (!info.IsName("Open") || info.normalizedTime >= 1)
            {
                Diode.On = false;
                return;
            }

            if (Time.time - _lastOpenFlash < 0.5f)
                return;
            _lastOpenFlash = Time.time;
            Diode.Toggle();
        }

        public AudioSource SingleAudioSource { get; private set; }

        public bool CanDepart => !_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).IsName("Empty");

    }

}

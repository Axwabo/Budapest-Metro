using Metro.Movement;
using Metro.Trains;
using Metro.Trains.Driving;
using UnityEngine;
using UnityEngine.Audio;

namespace Metro.Audio
{

    public sealed class MixerController : AssemblyComponent
    {

        [SerializeField]
        private AudioMixer mixer;

        private bool? _wasMounted;

        private void LateUpdate()
        {
            var mounted = IsPlayerMounted;
            if (mounted)
                UpdateMountedVolume();
            if (_wasMounted == mounted)
                return;
            if (mounted || Mountable.IsPlayerMounted)
                return;
            SetVolume(0);
            mixer.SetFloat("OnboardVolume", -40);
        }

        private void UpdateMountedVolume()
        {
            SetVolume(State == DriverState.Driving ? 1 : 0);
            mixer.SetFloat("OnboardVolume", 0);
        }

        private void SetVolume(float reductionRatio) => mixer.SetFloat("OutsideVolume", -40 * reductionRatio);

    }

}

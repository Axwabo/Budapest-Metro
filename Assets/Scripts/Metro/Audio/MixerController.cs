using System.Collections.Generic;
using Metro.Movement;
using Metro.Trains;
using Metro.Trains.Doors;
using Metro.Trains.Driving;
using UnityEngine;
using UnityEngine.Audio;

namespace Metro.Audio
{

    public sealed class MixerController : AssemblyComponent
    {

        [SerializeField]
        private AudioMixer mixer;

        [SerializeField]
        private AudioMixerGroup outsideClosed;

        [SerializeField]
        private AudioMixerGroup outsideOpen;

        [SerializeField]
        private AudioMixerGroup @this;

        [SerializeField]
        private AudioMixerGroup anotherOpen;

        [SerializeField]
        private AudioMixerGroup anotherClosed;

        private readonly List<IAudioSourceProvider> _providers = new();

        private readonly List<AudioSource> _sources = new();

        private DoorController _doors;

        private MountedState _previousState;

        private bool? _wasMounted;

        private MountedState MountState => (IsPlayerMounted, Mountable.IsPlayerMounted, _doors.CanDepart) switch
        {
            (true, _, _) => MountedState.This,
            (false, false, true) => MountedState.OutsideClosed,
            (false, false, false) => MountedState.OutsideOpen,
            (false, true, true) => MountedState.AnotherClosed,
            (false, true, false) => MountedState.AnotherOpen
        };

        private void LateUpdate()
        {
            var state = MountState;
            if (mounted)
                UpdateMountedVolume();
            if (_wasMounted == mounted)
                return;
            EnsureCached();
            var group = mounted ? onboard : outside;
            foreach (var provider in _sources)
                provider.outputAudioMixerGroup = group;
            if (!mounted && !Mountable.IsPlayerMounted)
                SetVolume(0);
        }

        private void UpdateMountedVolume() => SetVolume(State == DriverState.Driving ? 1 : 0);

        private void SetVolume(float reductionRatio) => mixer.SetFloat("OutsideVolume", -40 * reductionRatio);

        protected override void OnInitialized()
        {
            _providers.AddRange(Parent.Components<IAudioSourceProvider>());
            _doors = Parent.RequireComponent<DoorController>();
        }

        private void EnsureCached()
        {
            if (_sources.Count != 0)
                return;
            foreach (var provider in _providers)
            {
                var multiple = provider.MultipleAudioSources;
                if (multiple.Length == 0)
                {
                    _sources.Add(provider.SingleAudioSource);
                    continue;
                }

                foreach (var source in multiple)
                    _sources.Add(source);
            }
        }

        private enum MountedState
        {

            OutsideClosed,
            OutsideOpen,
            This,
            AnotherOpen,
            AnotherClosed

        }

    }

}

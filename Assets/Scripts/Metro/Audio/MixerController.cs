using System;
using System.Collections.Generic;
using Metro.Movement;
using Metro.Trains;
using Metro.Trains.Doors;
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

        private MountedState? _previousState;

        private void LateUpdate()
        {
            var canDepart = _doors.CanDepart;
            var state = (IsPlayerMounted, Mountable.IsPlayerMounted, canDepart) switch
            {
                (true, _, _) => MountedState.This,
                (false, false, true) => MountedState.OutsideClosed,
                (false, false, false) => MountedState.OutsideOpen,
                (false, true, true) => MountedState.AnotherClosed,
                (false, true, false) => MountedState.AnotherOpen
            };
            if (state == MountedState.This)
                UpdateMountedVolume(canDepart);
            if (_previousState == state)
                return;
            _previousState = state;
            EnsureCached();
            var group = state switch
            {
                MountedState.OutsideClosed => outsideClosed,
                MountedState.OutsideOpen => outsideOpen,
                MountedState.This => @this,
                MountedState.AnotherOpen => anotherOpen,
                MountedState.AnotherClosed => anotherClosed,
                _ => throw new InvalidOperationException()
            };
            foreach (var provider in _sources)
                provider.outputAudioMixerGroup = group;
        }

        private void UpdateMountedVolume(bool canDepart) => mixer.SetFloat("OnboardOthers", canDepart ? -20 : 0);

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

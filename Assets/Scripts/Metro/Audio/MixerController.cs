using System.Collections.Generic;
using Metro.Trains;
using UnityEngine;
using UnityEngine.Audio;

namespace Metro.Audio
{

    public sealed class MixerController : AssemblyComponent
    {

        [SerializeField]
        private AudioMixerGroup onboard;

        [SerializeField]
        private AudioMixerGroup outside;

        private readonly List<IAudioSourceProvider> _providers = new();

        private readonly List<AudioSource> _sources = new();

        private bool _wasMounted;

        private void LateUpdate()
        {
            var mounted = IsPlayerMounted;
            if (_wasMounted == mounted)
                return;
            EnsureCached();
            var group = mounted ? onboard : outside;
            foreach (var provider in _sources)
                provider.outputAudioMixerGroup = group;
        }

        protected override void OnInitialized() => _providers.AddRange(Parent.Components<IAudioSourceProvider>());

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

    }

}

using System.Collections.Generic;
using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : AssemblyComponent, IAudioSourceProvider
    {

        private const float Delay = 0.5f;

        private readonly List<(AudioClip, double, bool)> _scheduled = new();

        private void Start() => SingleAudioSource = GetComponent<AudioSource>();

        private void Update()
        {
            var now = Time.timeSinceLevelLoadAsDouble;
            for (var i = _scheduled.Count - 1; i >= 0; i--)
            {
                var (clip, time, oneShot) = _scheduled[i];
                if (time > now)
                    continue;
                if (oneShot)
                    SingleAudioSource.PlayOneShot(clip);
                else
                {
                    SingleAudioSource.clip = clip;
                    SingleAudioSource.Play();
                }

                _scheduled.RemoveAt(i);
            }
        }

        public AudioSource SingleAudioSource { get; private set; }

        public void PlayOneShit(AudioClip clip) => Schedule(clip, true);

        public void Play(AudioClip clip) => Schedule(clip, false);

        private void Schedule(AudioClip clip, bool oneShot) => _scheduled.Add((clip, Time.timeSinceLevelLoadAsDouble + Delay, oneShot));

    }

}

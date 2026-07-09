using System.Collections.Generic;
using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : AssemblyComponent
    {

        private const float Delay = 0.5f;

        private readonly List<(AudioClip, double, bool)> _scheduled = new();

        private AudioSource _source;

        private void Start() => _source = GetComponent<AudioSource>();

        private void Update()
        {
            var now = Time.timeSinceLevelLoadAsDouble;
            for (var i = _scheduled.Count - 1; i >= 0; i--)
            {
                var (clip, time, oneShot) = _scheduled[i];
                if (time > now)
                    continue;
                if (oneShot)
                    _source.PlayOneShot(clip);
                else
                {
                    _source.clip = clip;
                    _source.Play();
                }

                _scheduled.RemoveAt(i);
            }
        }

        public void PlayOneShit(AudioClip clip) => Schedule(clip, true);

        public void Play(AudioClip clip) => Schedule(clip, false);

        private void Schedule(AudioClip clip, bool oneShot) => _scheduled.Add((clip, Time.timeSinceLevelLoadAsDouble + Delay, oneShot));

    }

}

using System.Collections.Generic;
using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : AssemblyComponent
    {

        private const float Delay = 0.5f;

        private readonly List<(AudioClip, double)> _scheduledOneShot = new();

        private AudioSource _source;

        private void Start() => _source = GetComponent<AudioSource>();

        private void Update()
        {
            var now = AudioSettings.dspTime;
            for (var i = _scheduledOneShot.Count - 1; i >= 0; i--)
            {
                var (clip, time) = _scheduledOneShot[i];
                if (time > now)
                    continue;
                _source.PlayOneShot(clip);
                _scheduledOneShot.RemoveAt(i);
            }
        }

        public void PlayOneShit(AudioClip clip) => _scheduledOneShot.Add((clip, AudioSettings.dspTime + Delay));

        public void Play(AudioClip clip)
        {
            _source.clip = clip;
            _source.PlayDelayed(Delay);
        }

    }

}

using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : AssemblyComponent
    {

        private AudioSource _source;

        private void Awake() => _source = GetComponent<AudioSource>();

        public void PlayOneShit(AudioClip clip, float volumeScale = 1) => _source.PlayOneShot(clip, volumeScale);

        public void Play(AudioClip clip)
        {
            _source.clip = clip;
            _source.Play();
        }

    }

}

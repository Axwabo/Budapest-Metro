using UnityEngine;

namespace Metro.Trains.Cars
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : CarComponent
    {

        private AudioSource _source;

        private void Awake() => _source = GetComponent<AudioSource>();

        public void PlayOneShit(AudioClip clip, float volumeScale = 1) => _source.PlayOneShot(clip, volumeScale);

    }

}

using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : AssemblyComponent
    {

        private readonly object _lock = new();

        private float[] _buffer;

        private int _delaySamples;
        private int _position;

        private AudioSource _source;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _delaySamples = AudioSettings.outputSampleRate / 2;
            _buffer = new float[_delaySamples * 4];
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            lock (_lock)
            {
            }
        }

        public void PlayOneShit(AudioClip clip, float volumeScale = 1) => _source.PlayOneShot(clip, volumeScale);

        public void Play(AudioClip clip)
        {
            _source.clip = clip;
            _source.Play();
        }

    }

}

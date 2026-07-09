using System;
using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class Speaker : AssemblyComponent
    {

        private float[] _buffer;

        private int _delaySamples;
        private double _previousTime;

        private AudioSource _source;
        private int _writeHead;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _delaySamples = AudioSettings.outputSampleRate / 2;
            _buffer = new float[_delaySamples * 4];
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            lock (_buffer)
            {
                var now = AudioSettings.dspTime;
                if ()
                    Write(data);
            }
        }

        private void Write(float[] data)
        {
            var dataSpan = data.AsSpan();
            var forward = _buffer.AsSpan(_writeHead);
            var backward = _buffer.AsSpan(0, _writeHead);
            if (dataSpan.Length <= forward.Length)
            {
                dataSpan.CopyTo(forward);
                _writeHead += dataSpan.Length;
            }
            else
            {
                dataSpan[..forward.Length].CopyTo(forward);
                var remaining = dataSpan[forward.Length..];
                remaining.CopyTo(backward);
                _writeHead = remaining.Length;
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

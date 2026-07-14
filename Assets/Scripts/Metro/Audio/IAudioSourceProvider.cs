using System;
using UnityEngine;

namespace Metro.Audio
{

    public interface IAudioSourceProvider
    {

        AudioSource SingleAudioSource => null;

        ReadOnlySpan<AudioSource> MultipleAudioSources => default;

    }

}

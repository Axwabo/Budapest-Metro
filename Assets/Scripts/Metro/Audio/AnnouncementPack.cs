using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metro.Audio
{

    [CreateAssetMenu(fileName = "Announcement Pack", menuName = "Metro/Announcement Pack")]
    public sealed class AnnouncementPack : ScriptableObject
    {

        [SerializeField]
        private AudioClip[] next;

        [SerializeField]
        private AudioClip[] arriving;

        private readonly Dictionary<string, AudioClip> _arriving = new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, AudioClip> _next = new(StringComparer.OrdinalIgnoreCase);

        private void Awake()
        {
            Rebuild(next, _next);
            Rebuild(arriving, _arriving);
        }

#if UNITY_EDITOR
        private void OnValidate() => Awake();
#endif

        public bool TryGetClip(string station, bool stopping, out AudioClip clip)
        {
            var source = stopping ? _arriving : _next;
            return source.TryGetValue(station, out clip);
        }

        private static void Rebuild(AudioClip[] clips, Dictionary<string, AudioClip> dictionary)
        {
            if (clips == null)
                return;
            foreach (var clip in clips)
                dictionary[clip.name] = clip;
        }

    }

}

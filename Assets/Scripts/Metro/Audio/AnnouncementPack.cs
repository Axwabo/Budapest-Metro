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

        [SerializeField]
        private AudioClip[] stopped;

        private readonly Dictionary<string, AudioClip> _arriving = new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, AudioClip> _next = new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, AudioClip> _stopped = new(StringComparer.OrdinalIgnoreCase);

        private void Awake()
        {
            Rebuild(next, _next);
            Rebuild(arriving, _arriving);
            Rebuild(stopped, _stopped);
        }

#if UNITY_EDITOR
        private void OnValidate() => Awake();
#endif

        public bool TryGetClip(string station, AnnouncementType type, out AudioClip clip)
        {
            var source = type switch
            {
                AnnouncementType.Next => _next,
                AnnouncementType.Arriving => _arriving,
                AnnouncementType.Stopped => _stopped,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown announcement type")
            };
            return source.TryGetValue(station, out clip);
        }

        private static void Rebuild(AudioClip[] clips, Dictionary<string, AudioClip> dictionary)
        {
            if (clips == null)
                return;
            foreach (var clip in clips)
                if (clip)
                    dictionary[clip.name] = clip;
        }

    }

}

using UnityEngine;

namespace Metro.Rail.Controls
{

    public abstract class ControlPoint : MonoBehaviour
    {

        [field: SerializeField]
        public TrackSegment Track { get; private set; }

        [field: SerializeField]
        public float Distance { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        public Vector3 Position { get; private set; }

        private void Awake() => Position = transform.position;

#if UNITY_EDITOR
        [ContextMenu("Recalculate")]
        private void OnValidate()
        {
            if (!Track || !Track.didAwake)
                return;
            var t = transform;
            Distance = Track.GetDistance(t.position);
            Reverse = Vector3.Dot(t.forward, Track.Sample(Distance).forward) < 0;
        }
#endif

    }

}

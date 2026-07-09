using Metro.Trains.Cars;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

        private void Awake()
        {
            Position = transform.position;
            if (Track)
                Track.ControlPoints.Add(this);
        }

        private void OnDestroy()
        {
            if (Track)
                Track.ControlPoints.Remove(this);
        }

        public virtual void OnPassed(Axle axle)
        {
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Track && Track.didAwake)
                Recalculate();
        }

        [ContextMenu("Recalculate")]
        private void Recalculate()
        {
            var previousDistance = Distance;
            var previousReverse = Reverse;
            var t = transform;
            var distance = Track.GetDistance(t.position);
            Distance = Mathf.Approximately(0, distance) ? 0 : Mathf.Clamp(distance, 0, Track.Length);
            Reverse = Vector3.Dot(t.forward, Track.Sample(Distance).forward) < 0;
            if (Mathf.Approximately(previousDistance, Distance) && previousReverse == Reverse)
                return;
            EditorUtility.SetDirty(this);
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }

        private void OnDrawGizmosSelected()
        {
            if (!Track)
                return;
            Gizmos.color = Color.orange;
            Gizmos.DrawSphere(Track.Sample(Distance).position, 0.5f);
        }
#endif

    }

}

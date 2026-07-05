using UnityEngine;

namespace Metro.Rail
{

    public sealed class SplitSegment : TrackSegment
    {

        [field: SerializeField]
        public TrackSegment Original { get; set; }

        [field: SerializeField]
        public float Beginning { get; set; }

        [field: SerializeField]
        public float End { get; set; }

        public override float Length => End - Beginning;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            var delta = Length / 20;
            var previous = Sample(0).position;
            for (var i = delta; i < Length; i += delta)
            {
                var sample = Sample(i).position;
                Gizmos.DrawLine(previous, sample);
                previous = sample;
            }

            Gizmos.DrawLine(previous, Sample(Length).position);
        }

        public override Pose Sample(float distance) => Original.Sample(distance + Beginning);

        public override float GetDistance(Vector3 position) => Mathf.Clamp(Original.GetDistance(position) - Beginning, 0, Length);

    }

}

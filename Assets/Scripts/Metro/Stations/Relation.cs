using System;
using UnityEngine;

namespace Metro.Stations
{

    [CreateAssetMenu(fileName = "Relation", menuName = "Metro/Relation")]
    public sealed class Relation : ScriptableObject
    {

        [field: SerializeField]
        public string Line { get; private set; }

        [field: SerializeField]
        public Color Color { get; private set; }

        [SerializeField]
        private StationId[] forwards;

        [SerializeField]
        private StationId[] reverse;

        public ReadOnlySpan<StationId> Forwards => forwards;

        public ReadOnlySpan<StationId> Reverse => reverse;

    }

}

using System.Collections.Generic;
using Metro.Rail.Controls;
using Metro.Trains;
using UnityEngine;

namespace Metro.Journeys
{

    public sealed class ReversingSidingJourney : MonoBehaviour, IJourney
    {

        [SerializeField]
        private StopPoint target;

        [field: SerializeField]
        public SwitchGroup Switches { get; private set; }

        public ReversingSidingArea Area { get; set; }

        public HashSet<MetroAssembly> UsedBy { get; } = new();

        [field: SerializeField]
        public bool Reverse { get; private set; }

        public (StopPoint Target, Stop Stop) GetTarget(int stopIndex) => (target, null);

    }

}

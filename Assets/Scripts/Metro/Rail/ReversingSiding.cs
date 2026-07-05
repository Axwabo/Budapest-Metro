using System.Collections.Generic;
using Metro.Journeys;
using Metro.Rail.Controls;
using Metro.Trains;
using UnityEngine;

namespace Metro.Rail
{

    public sealed class ReversingSiding : MonoBehaviour
    {

        [SerializeField]
        private SwitchGroup @in;

        [SerializeField]
        private SwitchGroup @out;

        [field: SerializeField]
        public ServiceAreaStopPoint StopPoint { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        private ReversingSidingJourney _journey;

        public ReversingSidingArea Area { get; set; }

        public HashSet<MetroAssembly> UsedBy { get; } = new();

        private void Awake() => _journey = new ReversingSidingJourney(this);

#nullable enable

        public ReversingSidingJourney? Enter(MetroAssembly assembly)
        {
            if (UsedBy.Count != 0)
                return null;
            @in.Activate();
            UsedBy.Add(assembly);
            Area.PassingThrough.Add(assembly);
            return _journey;
        }

    }

}

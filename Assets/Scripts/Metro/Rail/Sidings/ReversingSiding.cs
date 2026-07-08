using System.Collections.Generic;
using Metro.Journeys;
using Metro.Rail.Controls;
using Metro.Trains;
using UnityEngine;

namespace Metro.Rail.Sidings
{

    public sealed class ReversingSiding : MonoBehaviour
    {

        [SerializeField]
        private SwitchGroup @in;

        [SerializeField]
        private SwitchGroup @out;

        [field: SerializeField]
        public ServiceAreaExitPoint StopPoint { get; private set; }

        private ReversingSidingJourney _entry;

        public ReversingSidingArea Area { get; set; }

        public HashSet<MetroAssembly> UsedBy { get; } = new();

        private void Awake() => _entry = new ReversingSidingJourney(this);

        // boots 'n' cats
        public void MarkAsHouse() => _entry.ToCarriageHouse = true;

#nullable enable

        public bool Enter(MetroAssembly assembly)
        {
            if (UsedBy.Count != 0)
                return false;
            @in.Activate();
            UsedBy.Add(assembly);
            Area.PassingThrough.Add(assembly);
            assembly.JourneyManager.Begin(_entry);
            assembly.Driver.MarkReadyNow();
            return true;
        }

        public bool Exit(MetroAssembly assembly)
        {
            if (!UsedBy.Remove(assembly))
                return false;
            @out.Activate();
            Area.PassingThrough.Add(assembly);
            return true;
        }

    }

}

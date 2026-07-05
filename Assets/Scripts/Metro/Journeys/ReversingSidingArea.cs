using System.Collections.Generic;
using Metro.Rail;
using Metro.Trains;
using UnityEngine;

namespace Metro.Journeys
{

    public sealed class ReversingSidingArea : MonoBehaviour
    {

        [SerializeField]
        private ReversingSiding[] sidings;

        public HashSet<MetroAssembly> PassingThrough { get; } = new();

        private void Start()
        {
            foreach (var siding in sidings)
                siding.Area = this;
        }

#nullable enable

        public IJourney? Enter(MetroAssembly assembly)
        {
            if (PassingThrough.Count != 0)
                return null;
            foreach (var siding in sidings)
                if (siding.Enter(assembly) is { } journey)
                    return journey;
            return null;
        }

    }

}

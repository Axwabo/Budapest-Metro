using System.Collections.Generic;
using Metro.Trains;
using UnityEngine;

namespace Metro.Journeys
{

    public sealed class ReversingSidingArea : MonoBehaviour
    {

        [SerializeField]
        private ReversingSidingJourney[] journeys;

        public HashSet<MetroAssembly> PassingThrough { get; } = new();

        private void Start()
        {
            foreach (var journey in journeys)
                journey.Area = this;
        }

        public bool TryEnter(MetroAssembly assembly, out ReversingSidingJourney siding)
        {
            if (PassingThrough.Count != 0)
            {
                siding = null;
                return false;
            }

            foreach (var journey in journeys)
            {
                if (journey.UsedBy.Count != 0)
                    continue;
                PassingThrough.Add(assembly);
                journey.UsedBy.Add(assembly);
                journey.Switches.Activate();
                siding = journey;
                return true;
            }

            siding = null;
            return false;
        }

    }

}

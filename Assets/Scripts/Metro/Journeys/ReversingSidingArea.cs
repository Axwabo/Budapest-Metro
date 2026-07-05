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

        public bool TryEnter(MetroAssembly assembly, out ReversingSidingJourney journey)
        {
            if (PassingThrough.Count != 0)
            {
                journey = null;
                return false;
            }

            foreach (var siding in sidings)
            {
                journey = siding.Enter(assembly);
                if (journey != null)
                    return true;
            }

            journey = null;
            return false;
        }

    }

}

using Metro.Rail.Sidings;
using Metro.Trains;
using Metro.Trains.Routes;
using UnityEngine;

namespace Metro.Journeys.Routes
{

    public sealed class RouteRotor : MonoBehaviour
    {

        [SerializeField]
        private JourneyManager prefab;

        [SerializeField]
        private ReversingSidingArea area;

        private void Update()
        {
            foreach (var siding in area.Sidings)
            {
                if (siding.UsedBy.Count != 0)
                    continue;
                var clone = Instantiate(prefab);
                var assembly = clone.GetComponentInParent<MetroAssembly>();
                clone.InitialJourney = new Afk {Target = siding.StopPoint};
                assembly.startingTrack = siding.StopPoint.Track;
            }
        }

    }

}

using System;
using System.Collections.Generic;
using Metro.Journeys;
using Metro.Journeys.Routes;
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

        private ServiceJourney _target;

        public ReversingSidingArea Area { get; set; }

        public HashSet<MetroAssembly> UsedBy { get; } = new();

        private void Start()
        {
            _entry = new ReversingSidingJourney(this);
            if (Area.ServiceTarget)
                _target = new ServiceJourney(Area.ServiceTarget);
        }

#nullable enable

        public IJourney? Enter(MetroAssembly assembly)
        {
            if (UsedBy.Count != 0)
                return null;
            @in.Activate();
            UsedBy.Add(assembly);
            Area.PassingThrough.Add(assembly);
            return _entry;
        }

        public IJourney? Exit(MetroAssembly assembly)
        {
            if (!UsedBy.Remove(assembly))
                return null;
            @out.Activate();
            Area.PassingThrough.Add(assembly);
            var next = Area.Route.Next(TimeSpan.FromSeconds(40));
            if (next.Origin.Time <= Clock.Now + TimeSpan.FromMinutes(10) || !Area.ServiceTarget)
                return new EnteringJourney(!Area.Reverse, next);
            if (Area.ServiceSwitches)
                Area.ServiceSwitches.Activate();
            return _target;
        }

    }

}

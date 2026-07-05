#nullable enable

using System.Diagnostics.CodeAnalysis;
using Metro.Journeys;
using Metro.Journeys.Routes;
using Metro.Trains.Driving;
using Metro.Trains.Routes;

namespace Metro.Trains
{

    public abstract class AssemblyComponent : Subcomponent<MetroAssembly>
    {

        public DriverState State => Parent.Driver.State;

        public JourneyManager JourneyManager => Parent.JourneyManager;

        public IJourney Journey => JourneyManager.Current;

        [MemberNotNullWhen(true, nameof(Route), nameof(Stop))]
        public bool IsInService => JourneyManager.IsInService;

        public Route? Route => JourneyManager.Route;

        public Stop? Stop => JourneyManager.Stop;

        public virtual void OnStateChanged()
        {
        }

        public virtual void OnJourneyChanged()
        {
        }

        public virtual void OnTargetChanged()
        {
        }

        public virtual void OnStopChanged()
        {
        }

    }

}

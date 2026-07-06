namespace Metro.Trains.Routes
{

    public delegate DisplaySection SectionStateMachine(DisplaySection previous);

    public static class SectionStateMachines
    {

        public static SectionStateMachine ServiceArea { get; } = _ => DisplaySection.ServiceArea;

        public static SectionStateMachine Approaching { get; } = previous => previous switch
        {
            DisplaySection.Stop => DisplaySection.Transfers,
            DisplaySection.Transfers => DisplaySection.RouteAndTime,
            _ => DisplaySection.Stop
        };

        public static SectionStateMachine ApproachingDestination { get; } = previous => previous switch
        {
            DisplaySection.Stop => DisplaySection.Terminus,
            DisplaySection.Terminus => DisplaySection.Transfers,
            DisplaySection.Transfers => DisplaySection.RouteAndTime,
            _ => DisplaySection.Stop
        };

        public static SectionStateMachine Stopping { get; } = previous => previous == DisplaySection.Stop ? DisplaySection.RouteAndTime : DisplaySection.Stop;

        public static SectionStateMachine StoppingDestination { get; } = previous => previous == DisplaySection.Stop ? DisplaySection.Terminus : DisplaySection.Stop;

        public static SectionStateMachine Stopped { get; } = previous => previous == DisplaySection.RouteAndTime ? DisplaySection.Destination : DisplaySection.RouteAndTime;

        public static SectionStateMachine StoppedDestination { get; } = _ => DisplaySection.Terminus;

    }

}

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

        public static SectionStateMachine Stopping { get; } = Alternating(DisplaySection.Stop, DisplaySection.RouteAndTime);

        public static SectionStateMachine StoppingDestination { get; } = Alternating(DisplaySection.Stop, DisplaySection.Terminus);

        public static SectionStateMachine Stopped { get; } = Alternating(DisplaySection.RouteAndTime, DisplaySection.Destination);

        public static SectionStateMachine StoppedDestination { get; } = _ => DisplaySection.Terminus;

        private static SectionStateMachine Alternating(DisplaySection a, DisplaySection b) => previous => previous == a ? b : a;

    }

}

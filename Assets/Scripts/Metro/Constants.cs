namespace Metro
{

    public static class Constants
    {

        public const float PlatformLengthMeters = 80;

        public const float TrackGaugeMeters = 1.435f;

        public const float AssemblyWidthMeters = 2.780f;

        public const float MpsToKmh = 3.6f;

        public const float KmhToMps = 1 / MpsToKmh;

        public const float MaxKmh = 80;

        public const float MaxMps = MaxKmh * KmhToMps;

        public const double MinStaySeconds = 22;

    }

}

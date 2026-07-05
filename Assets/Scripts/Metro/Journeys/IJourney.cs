#nullable enable

using Metro.Rail.Controls;
using Metro.Trains;

namespace Metro.Journeys
{

    public interface IJourney
    {

        public const int OutOfService = int.MinValue;
        public const int Origin = -1;
        public const int Destination = int.MaxValue;

        bool Reverse { get; }

        bool CanBegin(MetroAssembly parent) => true;

        (StopPoint Target, Stop? Stop) GetTarget(int stopIndex);

    }

}

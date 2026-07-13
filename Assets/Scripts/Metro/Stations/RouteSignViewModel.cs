using Unity.Properties;
using UnityEngine.UIElements;

namespace Metro.Stations
{

    public sealed class RouteSignViewModel
    {

        [CreateProperty]
        public string Destination { get; set; }

        [CreateProperty]
        public string Description { get; set; }

        [CreateProperty]
        public StyleLength Width { get; set; } = 0;

        [CreateProperty]
        public bool NoInformation { get; set; }

        [CreateProperty]
        public string Minutes { get; set; }

        [CreateProperty]
        public string Seconds { get; set; }

    }

}

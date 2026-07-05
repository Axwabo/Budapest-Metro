using System;
using Metro.Rail.Sidings;

namespace Metro.Journeys
{

    [Serializable]
    public sealed class SwitchState
    {

        public Switch @switch;

        public bool isLeft;

    }

}

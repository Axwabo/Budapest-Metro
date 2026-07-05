using System;
using UnityEngine;

namespace Metro
{

    public static class Clock
    {

        private static readonly TimeSpan Start = new(14, 08, 42);

        public static TimeSpan Now => Start + TimeSpan.FromSeconds(Time.timeSinceLevelLoadAsDouble);

        public static float Delta => Time.unscaledDeltaTime * Time.timeScale;

    }

}

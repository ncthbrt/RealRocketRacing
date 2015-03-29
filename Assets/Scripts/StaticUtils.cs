using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class StaticUtils
    {
        public static string ToRaceTimeString(TimeSpan time)
        {
            return (Math.Floor(time.TotalMinutes) + "").PadLeft(2, '0') + ":" + (time.Seconds + "").PadLeft(2, '0');
        }
    }
}

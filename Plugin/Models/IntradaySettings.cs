// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntradaySettings.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IntradaySettings
    {
        public int TimeShift; /* In hours */

        public int FilterAfterHours;

        public ulong SessionStart; /* Bit encoding HHHHH.MMMMMM.0000   hours << 10 | ( minutes << 4 ) */

        public ulong SessionEnd; /* Bit encoding HHHHH.MMMMMM.0000   hours << 10 | ( minutes << 4 ) */

        public int FilterWeekends;

        public DailyCompressionMode DailyCompressionMode;

        public ulong NightSessionStart;

        public ulong NightSessionEnd;
    }
}

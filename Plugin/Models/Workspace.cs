// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Workspace.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Workspace
    {
        public DataSourceType DataSourceType;

        public DataSourceMode DataSourceMode;

        public int NumBars;

        public int TimeBase;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public unsafe fixed int ReservedB[8];

        public int AllowMixedEODIntra;

        public int RequestDataOnSave;

        public int PadNonTradingDays;

        public int ReservedC;

        public IntradaySettings IntradaySettings;

        public int ReservedD;
    }
}

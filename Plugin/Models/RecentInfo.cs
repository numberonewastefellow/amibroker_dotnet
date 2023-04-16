// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecentInfo.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RecentInfo
    {
        public int StructSize;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Exchange;

        public RecentInfoStatus Status;

        /// <summary>
        /// Describes which fields are valid. Bitmap
        /// </summary>
        public RecentInfiField ValidFields;

        public float Open;

        public float High;

        public float Low;

        public float Last;

        public int TradeVolOld;

        public int TotalVolOld;

        public float OpenInt;

        public float Change;

        public float Prev;

        public float Bid;

        public int BidSize;

        public float Ask;

        public int AskSize;

        public float EPS;

        public float Dividend;

        public float DivYield;

        /// <summary>
        /// Shares outstanding
        /// </summary>
        public int Shares;

        public float High52Week;

        public int HighDate52Week;

        public float Low52Week;

        public int LowDate52Week;

        /// <summary>
        /// Format YYYYMMDD
        /// </summary>
        public int DateChanged;

        /// <summary>
        /// Format HHMMSS
        /// </summary>
        public int TimeChanged;

        /// <summary>
        /// Format YYYYMMDD
        /// </summary>
        public int DateUpdated;

        /// <summary>
        /// Format HHMMSS
        /// </summary>
        public int TimeUpdated;

        /// <summary>
        /// NEW 5.27 field
        /// </summary>
        public float TradeVol;

        /// <summary>
        /// NEW 5.27 field
        /// </summary>
        public float TotalVol;
    }
}

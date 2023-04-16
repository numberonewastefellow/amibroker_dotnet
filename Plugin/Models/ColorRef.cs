// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorRef.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    /// <summary>
    /// The COLORREF value is used to specify or retrieve an RGB color
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRef
    {
        public byte R;
        public byte G;
        public byte B;
    }
}

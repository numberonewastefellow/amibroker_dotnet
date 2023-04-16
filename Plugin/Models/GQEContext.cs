// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GQEContext.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GQEContext
    {
        public int StructSize;
    }
}

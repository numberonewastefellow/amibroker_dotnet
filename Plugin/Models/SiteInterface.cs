// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteInterface.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace MilkyAmiBroker.Plugins.Models
{
    /// <summary>
    /// SiteInterface structure defines call-back function pointers the structure is filled
    /// with correct pointers by the AmiBroker and passed to DLL via SetSiteInterface() function call
    /// SiteInterface is used as a way to call-back AmiBroker built-in functions
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SiteInterface
    {
        public int StructSize;
    }
}

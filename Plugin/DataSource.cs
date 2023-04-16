// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSource.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using MilkyAmiBroker.Plugins.Models;
namespace MilkyAmiBroker.Plugins
{
    

    public class DataSource
    {
        public DataSource(string databasePath, IntPtr mainWnd)
        {
            LogMe.Log("configuring datasource");
            this.DatabasePath = databasePath;
            this.MainWnd = mainWnd;
            this.Broker = Activator.CreateInstance(Type.GetTypeFromProgID("Broker.Application", true));

            if (this.Broker.ActiveDocument == null)
            {
                var processes = Process.GetProcesses().Where(x => x.ProcessName.Contains("AmiBroker") && x.MainWindowHandle != this.MainWnd);
                
                foreach (var proc in processes)
                {
                    MessageBox.Show("Please close AmiBroker application with Process ID: " + proc.Id, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    proc.WaitForExit();
                }

                this.Broker = Activator.CreateInstance(Type.GetTypeFromProgID("Broker.Application", true));
            }
        }

        public string DatabasePath { get; set; }

        /// <summary>
        /// Gets the pointer to AmiBroker's main window.
        /// </summary>
        public IntPtr MainWnd { get; private set; }

        /// <summary>
        /// Gets AmiBroker's OLE automation object.
        /// </summary>
        public dynamic Broker { get; private set; }

        public Quotation[] GetQuotes(string ticker, Periodicity periodicity, int limit, Quotation[] existingQuotes)
        {
            Random r =new Random();
            // TODO: Return the list of quotes for the specified ticker.
            return new Quotation[]
            {
                new Quotation(){DateTime = new AmiDate(DateTime.Now),High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Quotation(){DateTime = new AmiDate(DateTime.Now),High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Quotation(){DateTime = new AmiDate(DateTime.Now),High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Quotation(){DateTime = new AmiDate(DateTime.Now),High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Quotation(){DateTime = new AmiDate(DateTime.Now),High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Quotation(){DateTime = new AmiDate(DateTime.Now),High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
            };
        }
        public static Ohclv[] GetRandomOhclv()
        {
            Random r = new Random();
            // TODO: Return the list of quotes for the specified ticker.
            return new Ohclv[]
            {
                new Ohclv(){DateTime = DateTime.Now,High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Ohclv(){DateTime = DateTime.Now,High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Ohclv(){DateTime = DateTime.Now,High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Ohclv(){DateTime = DateTime.Now,High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Ohclv(){DateTime = DateTime.Now,High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
                new Ohclv(){DateTime = DateTime.Now,High = r.Next(910,920),Low = r.Next(900,909),Open = r.Next(900,920),Volume = r.Next(9000,100000),Price = r.Next(900,920)},
            };
        }
    }
}

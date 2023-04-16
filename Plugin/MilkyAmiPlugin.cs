// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MilkyAmiPlugin.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Xml.Xsl;
using MilkyAmiBroker.Plugins.Controls;
using MilkyAmiBroker.Plugins.Models;
using RGiesecke.DllExport;
using System.Collections.Generic;

namespace MilkyAmiBroker.Plugins
{
    public static class Utils
    {
        public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);//.ToUniversalTime(); // ToLocalTime();
            return dtDateTime;
        }

    }
    public class Ohclv
    {
        public string StockName { get; set; }
        public int Token { get; set; }
        public DateTime DateTime { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public Ohclv() { }
        public int Volume { get; set; }
        public double Price { get; set; }
    }

    public static class LogMe
    {
        public static void Log(string message)
        {
            File.AppendAllText("C:\\TEMP2\\amilog2.txt", message + " >> " + DateTime.Now + Environment.NewLine);
        }
    }

    /// <summary>
    /// Standard implementation of a typical AmiBroker plug-ins.
    /// </summary>
    public class MilkyAmiPlugin
    {
        /// <summary>
        /// MilkyAmiPlugin status code
        /// </summary>
        static StatusCode Status = StatusCode.OK;

        /// <summary>
        /// Default encoding
        /// </summary>
        static Encoding encoding = Encoding.GetEncoding("windows-1251"); // TODO: Update it based on your preferences

        static DataSource DataSource;

        /// <summary>
        /// WPF user control which is used to display right-click context menu.
        /// </summary>
        static RightClickMenu RightClickMenu;

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static void GetPluginInfo(ref PluginInfo pluginInfo)
        {
            pluginInfo.Name = "Milky\x00ae data Plug-in";
            pluginInfo.Vendor = "Milky LLC";
            pluginInfo.Type = PluginType.Data;
            pluginInfo.Version = 10000; // v1.0.0
            pluginInfo.IDCode = new PluginID("TEST");
            pluginInfo.Certificate = 0;
            pluginInfo.MinAmiVersion = 5600000; // v5.60
            pluginInfo.StructSize = Marshal.SizeOf((PluginInfo)pluginInfo);
            LogMe.Log("Get plugn info");
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static void Init()
        {
            LogMe.Log("INIT PLUING started...");
            TestMethod1234();
            TestMethod("test meothod another with param");
            StartSocketWithoutTask(new[] { "from init websockt called" });
            LogMe.Log("init finished completed");
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static void Release()
        {
        }
        static IntPtr mainWnd = IntPtr.Zero;


        private static Random random = new Random();
        private static void Wss_MessageReceived(string msg)
        {
            try
            {
                LogMe.Log("MSG>> " + msg);
                jsonAnswer = msg;
                //ohclv =  //new Ohclv() // JsonSerializer.Serialize(person); // JsonConvert.DeserializeObject<Ohclv>(jsonAnswer);

                ohclv = getRandomOhclv();

                NativeMethods.SendMessage(mainWnd, 0x0400 + 13000, IntPtr.Zero, IntPtr.Zero);
                LogMe.Log("send nsg after batuve nethid");
            }
            catch (Exception ex)
            {

                LogMe.Log("websocket open error " + ex.Message);
            }
        }


        public static Ohclv getRandomOhclv()
        {
            return new Ohclv()
            {
                StockName = "INFY",
                Token = 123456789,
                Close = random.Next(102, 105),
                High = random.Next(102, 105),
                Low = random.Next(102, 105),
                Open = random.Next(102, 105),
                Volume = random.Next(100000, 200000),
                Price = random.Next(100, 105),
                DateTime = DateTime.Now
            };
        }

        private static bool isSOcketInited = false;
        public static async Task StartSocket(string[] args)
        {
            LogMe.Log("going to start task socket");

            try
            {
                if (isSOcketInited)
                {
                    LogMe.Log("socket is already initied isSOcketInited");
                    return;
                }

                await Task.Run(async () =>
                {
                    LogMe.Log("initing limli webscok clinet");
                    var client = new ClientWebSocket();
                    var uri = new Uri("ws://localhost:8080/");
                    await client.ConnectAsync(uri, CancellationToken.None);

                    var buffer = new byte[1024];
                    var segment = new ArraySegment<byte>(buffer);
                    isSOcketInited = true;

                    while (client.State == System.Net.WebSockets.WebSocketState.Open)
                    {
                        try
                        {
                            var result = await client.ReceiveAsync(segment, CancellationToken.None);
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                var message = Encoding.UTF8.GetString(segment.Array, 0, result.Count);
                                var x = getRandomOhclv();//JsonConvert.DeserializeObject<Ohclv>(message);
                                File.AppendAllLines($"C:\\temp2\\{x.StockName}.ami.csv", new[] { $"{x.StockName},{x.DateTime:yyyy-MM-dd HH:mm:ss},{x.Open},{x.Low},{x.High},{x.Close},{x.Volume}" });
                                Console.WriteLine($"O:{x.Open} L:{x.Low} H:{x.High} C:{x.Close} V:{x.Volume}");

                                Wss_MessageReceived(message);
                            }
                        }
                        catch (Exception exception)
                        {
                            LogMe.Log($"Error while intit milky sockts {exception.Message}");
                            Console.WriteLine(exception.Message);
                        }
                    }
                    LogMe.Log("closing socket");
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    LogMe.Log("closed socket");
                });
            }
            catch (Exception exception)
            {

                LogMe.Log("from starting socket" + exception.Message);
            }

        }

        public static void TestMethod(string test)
        {
            LogMe.Log("from testmetod " + test);
        }

        public static void TestMethod1234()
        {
            LogMe.Log("from testmetod 123" + DateTime.Now);
        }

        public static async Task StartSocketWithoutTask(string[] args)
        {
            LogMe.Log("going to start task socket " + args[0]);

            try
            {
                if (isSOcketInited)
                {
                    LogMe.Log("socket is already initied isSOcketInited");
                    return;
                }


                LogMe.Log("initing limli webscok clinet");
                var client = new ClientWebSocket();
                var uri = new Uri("ws://localhost:8080/");
                await client.ConnectAsync(uri, CancellationToken.None);

                var buffer = new byte[1024];
                var segment = new ArraySegment<byte>(buffer);
                isSOcketInited = true;

                while (client.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    try
                    {
                        var result = client.ReceiveAsync(segment, CancellationToken.None);
                        if (result.Result.MessageType == WebSocketMessageType.Text)
                        {
                            var message = Encoding.UTF8.GetString(segment.Array, 0, result.Result.Count);
                            var x = getRandomOhclv();//JsonConvert.DeserializeObject<Ohclv>(message);
                            File.AppendAllLines($"C:\\temp2\\{x.StockName}.ami.csv", new[] { $"{x.StockName},{x.DateTime:yyyy-MM-dd HH:mm:ss},{x.Open},{x.Low},{x.High},{x.Close},{x.Volume}" });
                            Console.WriteLine($"O:{x.Open} L:{x.Low} H:{x.High} C:{x.Close} V:{x.Volume}");

                            Wss_MessageReceived(message);
                        }
                    }
                    catch (Exception exception)
                    {
                        LogMe.Log($"Error while intit milky sockts {exception.Message}");
                        Console.WriteLine(exception.Message);
                    }
                }
                LogMe.Log("closing socket");
                var clsoing = client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                LogMe.Log("closed socket " + clsoing.Status);

            }
            catch (Exception exception)
            {

                LogMe.Log("from starting socket" + exception.Message);
            }

        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static unsafe void Notify(PluginNotification* notification)
        {
            MessageBox.Show($"hi {notification->DatabasePath}");
            LogMe.Log("NOTIFYED....");
            switch (notification->Reason)
            {
                case PluginNotificationReason.DatabaseLoaded:
                    LogMe.Log("db loaded....");
                    DataSource = new DataSource(databasePath: Marshal.PtrToStringAnsi(notification->DatabasePath),
                        mainWnd: notification->MainWnd);
                    mainWnd = notification->MainWnd;

                    RightClickMenu = new RightClickMenu(DataSource);
                    break;
                case PluginNotificationReason.DatabaseUnloaded:
                    DataSource.DatabasePath = null;
                    break;
                case PluginNotificationReason.StatusRightClick:
                    RightClickMenu.ContextMenu.IsOpen = true;
                    break;
                case PluginNotificationReason.SettingsChange:
                    break;
            }
        }

        /// <summary>
        /// GetQuotesEx function is functional equivalent fo GetQuotes but
        /// handles new Quotation format with 64 bit date/time stamp and floating point volume/open int
        /// and new Aux fields
        /// it also takes pointer to context that is reserved for future use (can be null)
        /// Called by AmiBroker 5.27 and above 
        /// </summary>
      //  [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static unsafe int GGGGGGGetQuotesEx(string ticker, Periodicity periodicity, int lastValid, int size, Quotation* quotes, GQEContext* context)
        {
            Debug.WriteLine("GetQuotesEx(ticker: " + ticker + ", periodicity: " + periodicity + ", lastValid: " + lastValid + ", size: " + size + ", ...)");


            var existingQuotes = new Quotation[0];


            if (lastValid > 2)
            {
                Array.Resize<Quotation>(ref existingQuotes, lastValid + 1);

                for (var i = 0; i <= lastValid; i++)
                {
                    existingQuotes[i] = new Quotation
                    {
                        DateTime = quotes[i].DateTime,
                        Open = quotes[i].Open,
                        High = quotes[i].High,
                        Low = quotes[i].Low,
                        Price = quotes[i].Price,
                        Volume = quotes[i].Volume,
                        OpenInterest = quotes[i].OpenInterest,
                        AuxData1 = quotes[i].AuxData1,
                        AuxData2 = quotes[i].AuxData2
                    };
                }

                Array.Sort<Quotation>(existingQuotes, new Comparison<Quotation>((q1, q2) => q1.DateTime.CompareTo(q2.DateTime)));
            }

            var newQuotes = DataSource.GetQuotes(ticker, periodicity, size, existingQuotes);

            if (newQuotes.Any())
            {
                lastValid = 0;
                for (var i = 0; i < newQuotes.Length; i++)
                {
                    quotes[i].DateTime = newQuotes[i].DateTime;
                    quotes[i].Price = newQuotes[i].Price;
                    quotes[i].Open = newQuotes[i].Open;
                    quotes[i].High = newQuotes[i].High;
                    quotes[i].Low = newQuotes[i].Low;
                    quotes[i].Volume = newQuotes[i].Volume;
                    quotes[i].OpenInterest = newQuotes[i].OpenInterest;
                    quotes[i].AuxData1 = newQuotes[i].AuxData1;
                    quotes[i].AuxData2 = newQuotes[i].AuxData2;
                    lastValid++;
                }

                return lastValid;
            }

            // return 'lastValid + 1' if no updates are found and you want to keep all existing records
            return lastValid + 1;
        }


        /// <summary>
        /// GetQuotesEx function is functional equivalent fo GetQuotes but
        /// handles new Quotation format with 64 bit date/time stamp and floating point volume/open int
        /// and new Aux fields
        /// it also takes pointer to context that is reserved for future use (can be null)
        /// Called by AmiBroker 5.27 and above 
        /// </summary>
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static unsafe int GetQuotesEx(string ticker, Periodicity periodicity, int lastValid, int size, Quotation* quotes, GQEContext* context)
        {
            LogMe.Log($"in get quote: {ticker}, {periodicity} ,{lastValid} frm GetQuotesEx");
            // Статус - в ожидании данных
            Status = StatusCode.Wait;

            if (ohclv != null)
            {

                AddTick(ticker, ohclv);
            }

            #region There was no data - the database is empty
            if (lastValid < 0)
            {
                List<Ohclv> last24hBars = getLast24hBars(ticker);

                //Checking if it's not empty
                if (last24hBars == null || last24hBars.Count == 0)
                    return lastValid + 1;

                lastValid = 0;

                // If we fit into the window, then i = 0; otherwise last24hBars.Count - size

                for (var i = (last24hBars.Count < size) ? 0 : last24hBars.Count - size; i < last24hBars.Count; i++)
                {
                    quotes[lastValid].DateTime = new AmiDate(last24hBars[i].DateTime);
                    quotes[lastValid].Open = (float)last24hBars[i].Open;
                    quotes[lastValid].High = (float)last24hBars[i].High;
                    quotes[lastValid].Low = (float)last24hBars[i].Low;
                    quotes[lastValid].Price = (float)last24hBars[i].Close;
                    quotes[lastValid].Volume = last24hBars[i].Volume;

                    lastValid++;
                }

                // Store the value of a character pair
                lastTicker = ticker;

                return lastValid;
            }
            #endregion

            LogMe.Log("isConn: " + isSocketConnected + " lastT: " + lastTicker + " TC: " + ticker);

            // Проверка на то что не переключили символы + первый запуск
            if (String.IsNullOrEmpty(lastTicker) || !lastTicker.Equals(ticker))
            {
                isFirstRun = true;

                /*
                if (wss != null)
                    wss.Close();

                // Starting a socket
                wss = new WebSocket(String.Format("wss://stream.binance.com:9443/ws/{0}@kline_{1}",ticker.ToLower(),"1m"));
                wss.Opened += new EventHandler(WebSockedOpened);
                wss.MessageReceived += Wss_MessageReceived;
               // wss.Closed += Wss_Closed;
                wss.Error += Wss_Error;
                wss.Open();      
   */
                //if (!BinanceHelper.CreateWSS(ticker, periodicity))
                //{
                //    Log.Write("Create WSS failed!");
                //    isSocketConnected = false;
                //    return lastValid + 1;
                //}
                // BinanceHelper.onWSSMessage = Wss_MessageReceived;
                // isSocketConnected = true;  

            }
            else
                isFirstRun = false;

            // Обозначаем тикер
            lastTicker = ticker;

            #region Данные есть и это первый запуск
            if (isFirstRun && lastValid > 0)
            {
                // Получаем данные
                List<Ohclv> last24hBars = getLast24hBars(ticker);

                // Проверка что не пусто
                if (last24hBars == null || last24hBars.Count == 0)
                    return lastValid + 1;

                // Кастрируем массив
                for (var i = 0; i < last24hBars.Count; i++)
                {
                    AmiDate lastDate = new AmiDate(quotes[lastValid].DateTime);
                    AmiDate requestedDate = new AmiDate(last24hBars[i].DateTime);

                    if (requestedDate.CompareTo(lastDate) <= 0)
                    {
                        last24hBars.RemoveAt(0);
                        i--;
                    }
                    else
                        // Вываливаемся из цикла так как последний элемент явно старше
                        break;
                }

                // Вариант 1 - Count > size - переносим данные
                if (last24hBars.Count > size)
                {
                    lastValid = 0;

                    // Перенос последних = size данных
                    for (var i = last24hBars.Count - size; i < last24hBars.Count; i++)
                    {
                        quotes[lastValid].DateTime = new AmiDate(last24hBars[i].DateTime);
                        quotes[lastValid].Open = (float)last24hBars[i].Open;
                        quotes[lastValid].High = (float)last24hBars[i].High;
                        quotes[lastValid].Low = (float)last24hBars[i].Low;
                        quotes[lastValid].Price = (float)last24hBars[i].Close;
                        quotes[lastValid].Volume = last24hBars[i].Volume;

                        lastValid++;
                    }

                    return lastValid;
                }

                // Вариант 2 - Count < size и входит в окно - добавить в список
                if ((last24hBars.Count < size) && (last24hBars.Count < (size - lastValid)))
                {
                    // Перенос всех из массива
                    for (var i = 0; i < last24hBars.Count; i++)
                    {
                        quotes[lastValid].DateTime = new AmiDate(last24hBars[i].DateTime);
                        quotes[lastValid].Open = (float)last24hBars[i].Open;
                        quotes[lastValid].High = (float)last24hBars[i].High;
                        quotes[lastValid].Low = (float)last24hBars[i].Low;
                        quotes[lastValid].Price = (float)last24hBars[i].Close;
                        quotes[lastValid].Volume = last24hBars[i].Volume;

                        lastValid++;
                    }

                    return lastValid;
                }
                else
                    if ((last24hBars.Count < size) && (last24hBars.Count > (size - lastValid)))
                {
                    // Вариант 3 - данные в окно не входят - нужно сдвигать массив

                    lastValid = 0;

                    // Сколько элементов останется 
                    var j = size - last24hBars.Count;
                    // Индекс начала копирования
                    var index = lastValid - j;

                    // Смещение первой части
                    while (lastValid < j)
                    {

                        LogMe.Log("I: " + lastValid + " HIGH: " + quotes[index].High + " VOL: " + quotes[index].Volume);

                        quotes[lastValid].DateTime = quotes[index].DateTime;
                        quotes[lastValid].Open = quotes[index].Open;
                        quotes[lastValid].High = quotes[index].High;
                        quotes[lastValid].Low = quotes[index].Low;
                        quotes[lastValid].Price = quotes[index].Price;
                        quotes[lastValid].Volume = quotes[index].Volume;

                        lastValid++;
                        index++;
                    }

                    // КОпируем остатки
                    foreach (var item in last24hBars)
                    {
                        LogMe.Log("I: " + lastValid + " HIGH: " + item.High + " VOL: " + item.Volume);

                        quotes[lastValid].DateTime = new AmiDate(item.DateTime);
                        quotes[lastValid].Open = (float)item.Open;
                        quotes[lastValid].High = (float)item.High;
                        quotes[lastValid].Low = (float)item.Low;
                        quotes[lastValid].Price = (float)item.Close;
                        quotes[lastValid].Volume = item.Volume;

                        lastValid++;
                    }

                    return lastValid;
                }
            }

            #endregion

            // не первый запуск - просто обновляем

            // Если не подключились - нечего разбирать
            if (!isSocketConnected)
                return lastValid + 1;

            // Показывает что данные тикера устарели
            //bool isTooOld = false;
            Ohclv data = null;

            if (!String.IsNullOrEmpty(jsonAnswer))
            {
                // Парсинг
                try
                {
                    // data = JsonConvert.DeserializeObject<Ohclv>(jsonAnswer);
                    data = getRandomOhclv();
                }
                catch (Exception e)
                {
                    LogMe.Log("Parse Error: " + e.Message);
                    return lastValid + 1;
                }
            }

            // В БД есть какие-то данные
            if (lastValid >= 0)
            {
                ulong lastDate = quotes[lastValid].DateTime;
                ulong tickerDate = (new AmiDate(data.DateTime)).ToUInt64();

                /*
                if (tickerDate < lastDate)
                    isTooOld = true;
                */

                if (tickerDate > lastDate)
                    lastValid++;
            }
            else
            {
                // Если пусто в БД - начинаем писать с 0го  индекса
                lastValid = 0;
            }


            // Поправка на лимит
            if (size > 0 && lastValid == size)
            {
                // Сдвигание массива влево
                for (int i = 0; i < size - 1; i++)
                {
                    quotes[i].DateTime = quotes[i + 1].DateTime;
                    quotes[i].Open = quotes[i + 1].Open;
                    quotes[i].High = quotes[i + 1].High;
                    quotes[i].Low = quotes[i + 1].Low;
                    quotes[i].Price = quotes[i + 1].Price;
                    quotes[i].Volume = quotes[i + 1].Volume;
                }

                lastValid--;
            }

            // Правим
            //if (!isTooOld)
            {
                AmiDate tickerDate = new AmiDate(data.DateTime);

                quotes[lastValid].DateTime = tickerDate.ToUInt64();
                quotes[lastValid].Open = (float)data.Open; //float.Parse(data.k.o.Replace(".", ","));
                quotes[lastValid].High = (float)data.High;
                quotes[lastValid].Low = (float)data.Low;
                quotes[lastValid].Price = (float)data.Price;
                quotes[lastValid].Volume = data.Volume;
                quotes[lastValid].AuxData1 = 0;
                quotes[lastValid].AuxData2 = 0;
            }

            Status = StatusCode.OK;
            return lastValid + 1;
        }

        private static ConcurrentDictionary<string, List<Ohclv>> tickData =
            new ConcurrentDictionary<string, List<Ohclv>>();
        //private static List<Ohclv> lastBaars = new List<Ohclv>();

        private static List<Ohclv> getLast24hBars(string ticker)
        {
            List<Ohclv> lastBaars;
            tickData.TryGetValue(ticker, out lastBaars);
            if (lastBaars == null)
            {
                tickData[ticker] = new List<Ohclv>();
                return tickData[ticker];
            }
            return lastBaars;
        }

        private static void AddTick(string ticker, Ohclv tick)
        {
            List<Ohclv> lastBaars;
            tickData.TryGetValue(ticker, out lastBaars);
            if (lastBaars == null)
            {
                tickData[ticker] = new List<Ohclv>();
            }
            tickData[ticker].Add(tick);
        }

        public unsafe delegate void* Alloc(uint size);

        ///// <summary>
        ///// GetExtra data is optional function for retrieving non-quotation data
        ///// </summary>
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static AmiVar GetExtraData(string ticker, string name, int arraySize, Periodicity periodicity, Alloc alloc)
        {
            return new AmiVar();
        }

        /// <summary>
        /// GetSymbolLimit function is optional, used only by real-time plugins
        /// </summary>
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static int GetSymbolLimit()
        {
            return 10000;
        }

        /// <summary>
        /// GetStatus function is optional, used mostly by few real-time plugins
        /// </summary>
        /// <param name="statusPtr">A pointer to <see cref="PluginStatus"/></param>
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        public static void GetStatus(IntPtr statusPtr)
        {
            switch (Status)
            {
                case StatusCode.OK:
                    SetStatus(statusPtr, StatusCode.OK, Color.LightGreen, "OK", "Connected");
                    break;
                case StatusCode.Wait:
                    SetStatus(statusPtr, StatusCode.Wait, Color.LightBlue, "WAIT", "Trying to connect...");
                    break;
                case StatusCode.Error:
                    SetStatus(statusPtr, StatusCode.Error, Color.Red, "ERR", "An error occured");
                    break;
                default:
                    SetStatus(statusPtr, StatusCode.Unknown, Color.LightGray, "Ukno", "Unknown status");
                    break;
            }
        }

        #region Helper Functions

        /// <summary>
        /// Notify AmiBroker that new streaming data arrived
        /// </summary>
        static void NotifyStreamingUpdate()
        {
            NativeMethods.SendMessage(DataSource.MainWnd, 0x0400 + 13000, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Update status of the plugin
        /// </summary>
        /// <param name="statusPtr">A pointer to <see cref="PluginStatus"/></param>
        static void SetStatus(IntPtr statusPtr, StatusCode code, Color color, string shortMessage, string fullMessage)
        {
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 4), (int)code);
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 8), color.R);
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 9), color.G);
            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 10), color.B);

            var msg = encoding.GetBytes(fullMessage);

            for (int i = 0; i < (msg.Length > 255 ? 255 : msg.Length); i++)
            {
                Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 12 + i), msg[i]);
            }

            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 12 + msg.Length), 0x0);

            msg = encoding.GetBytes(shortMessage);

            for (int i = 0; i < (msg.Length > 31 ? 31 : msg.Length); i++)
            {
                Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 268 + i), msg[i]);
            }

            Marshal.WriteInt32(new IntPtr(statusPtr.ToInt32() + 268 + msg.Length), 0x0);
        }

        #endregion

        #region AmiBroker Method Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int GetStockQtyDelegate();

        private static GetStockQtyDelegate GetStockQty;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int SetCategoryNameDelegate(int category, int item, string name);

        private static SetCategoryNameDelegate SetCategoryName;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate string GetCategoryNameDelegate(int category, int item);

        private static GetCategoryNameDelegate GetCategoryName;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int SetIndustrySectorDelegate(int industry, int sector);

        private static SetIndustrySectorDelegate SetIndustrySector;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int GetIndustrySectorDelegate(int industry);

        private static GetIndustrySectorDelegate GetIndustrySector;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        public delegate IntPtr AddStockDelegate(string ticker); // returns a pointer to StockInfo

        private static AddStockDelegate AddStock;
        private static string jsonAnswer;
        private static Ohclv ohclv;
        private static string lastTicker;
        private static bool isSocketConnected;
        private static bool isFirstRun;

        #endregion
    }
}

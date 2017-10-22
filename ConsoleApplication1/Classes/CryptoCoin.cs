using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class CryptoCoin
    {
        public string MarketName { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public double Last { get; set; }
        public double BaseVolume { get; set; }
        public string TimeStamp { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public string OpenBuyOrders { get; set; }
        public string OpenSellOrders { get; set; }
        public string PrevDay { get; set; }
        public string Created { get; set; }
        public string MarketDisplayMarketNameName { get; set; }
        public Thread thread;
        public double[] data = new double[2160];
        private int currentIndex;
        public string min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6;

        public void StartThread()
        {
            Console.WriteLine("Starting coin thread");
            //thread = new Thread(new ThreadStart(Ticker));
            //thread.Start();
        }

        private void Tick()
        {
                data[MathHelper.Mod(currentIndex, data.Length)] = GetCoinData();
                currentIndex = MathHelper.Mod(currentIndex, data.Length);
                currentIndex++;
                CalculateChangePercentages();
                PrintPercentages();         
        }

        public double GetCoinData()
        {
            bool flag = true;
            string html = string.Empty;
            while (flag)
            {
                flag = false;
                string url = "https://bittrex.com/api/v1.1/public/getticker?market=" + MarketName + "";              
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine("Timeout retrying");
                    flag = true;
                }
            }
            //Console.WriteLine(html);
            try
            {
                CryptoCoinData data = JsonConvert.DeserializeObject<CryptoCoinData>(html);
                if (data.result != null)
                    return data.result.Last;
                else
                {
                    Console.WriteLine("Failed to read: "+MarketName);
                    return 0;
                }
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine("Cannot serialize");
                return double.MinValue;
            }
        }

        public void StopThread()
        {
            thread.Abort();
        }

        private void CalculateChangePercentages()
        {
                Calculate1MinChange();
                Calculate3MinChange();
                Calculate5MinChange();
                Calculate10MinChange();
                Calculate15MinChange();
                Calculate30MinChange();
                Calculate1HourChange();
                Calculate2HourChange();
                Calculate3HourChange();
        }

        private void Calculate1MinChange()
        {
            min1 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate3MinChange()
        {
            min3 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate5MinChange()
        {
            min5 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min5InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min5InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate10MinChange()
        {
            min10 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min10InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min10InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate15MinChange()
        {
            min15 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate30MinChange()
        {
            min30 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min30InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min30InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate1HourChange()
        {
            hour1 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate2HourChange()
        {
            hour2 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour2InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour2InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate3HourChange()
        {
            hour3 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }

        private void Calculate6HourChange()
        {
            hour6 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour6InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour6InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
        }



        private void PrintPercentages()
        {
            TableDataRow row = new TableDataRow(MarketName, min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6);
            BitTrex.instance.InsertRow(row);
            /*if (Convert.ToDouble(min1) > 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(MarketName+"::"+min1 + ":" + min3 + ":" + min5 + ":" + min10 + ":" + min15 + ":" + min30 + ":" + hour1 + ":" + hour2 + ":" + hour3);
                Console.ResetColor();
            }
            else if(Convert.ToDouble(min1) < -2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(MarketName + "::" + min1 + ":" + min3 + ":" + min5 + ":" + min10 + ":" + min15 + ":" + min30 + ":" + hour1 + ":" + hour2 + ":" + hour3);
                Console.ResetColor();
            }*/
        }


    }
}

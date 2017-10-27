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
        public double[] data = new double[2170];
        public int currentIndex;
        public string min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6;
        private DateTime lastNotifyTime = DateTime.Now;

        public void Tick()
        {
            double lastPrice = GetCoinData();
            data[MathHelper.Mod(currentIndex, data.Length)] = lastPrice;
            currentIndex = MathHelper.Mod(currentIndex, data.Length);
            currentIndex++;
            CalculateChangePercentages();
            PrintPercentages();
        }

        public double GetCoinData()
        {
            string html = string.Empty;

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
                Console.WriteLine("Timeout ");
            }
            //Console.WriteLine(html);
            try
            {
                if (html == "" || html == null)
                {
                    Console.WriteLine("Failed to gather data");
                    return 0;
                }
                CryptoCoinData data = JsonConvert.DeserializeObject<CryptoCoinData>(html);
                if (data.result != null)
                    return data.result.Last;
                else
                {
                    Console.WriteLine("Failed to read: " + MarketName);
                    BitTrex.instance.RemoveCurrency(MarketName);
                    return 0;
                }
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine("Cannot serialize");
                return double.MinValue;
            }
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
            Calculate6HourChange();
        }

        public double Calculate1MinChange()
        {
            double min1PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min1 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            if(min1PercentageDifference > 5 && data[currentIndex - 1] > 0.00000100 && min1PercentageDifference < 200 && DateTime.Now >= lastNotifyTime.AddMinutes(2))
            {
                lastNotifyTime = DateTime.Now;
                BitTrex.instance.NotifyUser(MarketName + " is increased " + min1PercentageDifference + " in 1 min");
            }
            return min1PercentageDifference;
        }

        public double Calculate3MinChange()
        {
            double min3PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min3 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return min3PercentageDifference;
        }

        public double Calculate5MinChange()
        {
            double min5PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min5InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min5InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min5 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min5InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min5InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return min5PercentageDifference;
        }

        public double Calculate10MinChange()
        {
            double min10PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min10InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min10InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min10 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min10InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min10InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return min10PercentageDifference;
        }

        public double Calculate15MinChange()
        {
            double min15PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min15 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return min15PercentageDifference;
        }

        public double Calculate30MinChange()
        {
            double min30PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min30InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min30InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min30 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min30InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min30InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return min30PercentageDifference;
        }

        public double Calculate1HourChange()
        {
            double hour1PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            hour1 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return hour1PercentageDifference;
        }

        public double Calculate2HourChange()
        {
            double hour2PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour2InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour2InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            hour2 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour2InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour2InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return hour2PercentageDifference;
        }

        public double Calculate3HourChange()
        {
            double hour3PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            hour3 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return hour3PercentageDifference;
        }

        public double Calculate6HourChange()
        {
            double hour6PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour6InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour6InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            hour6 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour6InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.hour6InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            return hour6PercentageDifference;
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

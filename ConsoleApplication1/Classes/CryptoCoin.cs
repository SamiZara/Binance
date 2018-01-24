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
        public string Symbol { get; set; }
        public double Last { get; set; }
        public double[] data = new double[6 * 60 * 60 / (Constants.tickerIntervalInMilliSeconds / 1000)];
        public int currentIndex;
        public string min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6, volatility15Min;
        private DateTime lastNotifyTime = DateTime.Now;

        public CryptoCoin(string symbol)
        {
            Symbol = symbol;
        }

        public void Tick(float lastPrice)
        {
            data[MathHelper.Mod(currentIndex, data.Length)] = lastPrice;
            currentIndex = MathHelper.Mod(currentIndex, data.Length);
            currentIndex++;
            CalculateChangePercentages();
            PrintData();
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
            Calculate15MinVolatility();
        }

        public double Calculate1MinChange()
        {
            double min1PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min1 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min1InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            if((min1PercentageDifference > 10 && Symbol.Contains("BNB") || (min1PercentageDifference > 5 && Symbol.Contains("BTC"))) && data[currentIndex - 1] > 0.00000100 && min1PercentageDifference < 200 && DateTime.Now >= lastNotifyTime.AddMinutes(2))
            {
                lastNotifyTime = DateTime.Now;
                BitTrex.instance.NotifyUser(Symbol + " is increased " + min1PercentageDifference + " in 1 min");
            }
            else if((min1PercentageDifference < -10 && Symbol.Contains("BNB") || (min1PercentageDifference < -5 && Symbol.Contains("BTC"))) && data[currentIndex - 1] > 0.00000100 && min1PercentageDifference > -50 && DateTime.Now >= lastNotifyTime.AddMinutes(2))
            {
                lastNotifyTime = DateTime.Now;
                BitTrex.instance.NotifyUser(Symbol + " is decreased " + min1PercentageDifference + " in 1 min");
            }
            return min1PercentageDifference;
        }

        public double Calculate3MinChange()
        {
            double min3PercentageDifference = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100);
            min3 = ((data[currentIndex - 1] - data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))]) / data[(MathHelper.Mod(currentIndex - 1 - (Constants.min3InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000)), data.Length))] * 100).ToString("0.00");
            if ((min3PercentageDifference > 20 && Symbol.Contains("BNB") || (min3PercentageDifference > 10 && Symbol.Contains("BTC"))) && data[currentIndex - 1] > 0.00000100 && min3PercentageDifference < 200 && DateTime.Now >= lastNotifyTime.AddMinutes(2))
            {
                lastNotifyTime = DateTime.Now;
                BitTrex.instance.NotifyUser(Symbol + " is increased " + min3PercentageDifference + " in 3 min");
            }
            else if ((min3PercentageDifference < -20 && Symbol.Contains("BNB") || (min3PercentageDifference <- 10 && Symbol.Contains("BTC"))) && data[currentIndex - 1] > 0.00000100 && min3PercentageDifference > -50 && DateTime.Now >= lastNotifyTime.AddMinutes(2))
            {
                lastNotifyTime = DateTime.Now;
                BitTrex.instance.NotifyUser(Symbol + " is decreased " + min3PercentageDifference + " in 3 min");
            }
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

        public void Calculate15MinVolatility()
        {
            double volatility = 0;

            for(int i=0;i< Constants.min15InSeconds / (Constants.tickerIntervalInMilliSeconds / 1000); i++)
            {
                double delta = 0;
                if (data[MathHelper.Mod(currentIndex - 1 - i, data.Length)] != 0 && data[MathHelper.Mod(currentIndex - 2 - i, data.Length)] != 0)
                {
                    delta = data[currentIndex - 1 - i] / data[currentIndex - 2 - i] * 100;
                    if (delta != 0 && delta <= 100)
                        delta = 100 - delta;
                    else if (delta != 0 && delta > 100)
                        delta =  delta - 100;
                    //if(delta != 0)
                    volatility += delta;
                }
            }
            //Console.WriteLine(volatility15Min);
            volatility15Min = volatility.ToString("0.00");
            //return volatility15Min;
        }

        private void PrintData()
        {
            TableDataRow row = new TableDataRow(Symbol, min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6, volatility15Min);
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

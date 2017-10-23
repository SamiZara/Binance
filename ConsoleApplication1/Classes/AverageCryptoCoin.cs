using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class AverageCryptoCoin
    {

        private int currentIndex;
        public string min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6, MarketName;
        public static AverageCryptoCoin Instance;

        public AverageCryptoCoin(string MarketName)
        {
            Instance = this;
            this.MarketName = MarketName;
        }


        public void CalculateChangePercentages()
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
            double count = 0;
            double sum = 0;
            foreach(KeyValuePair<string,CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate3MinChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate3MinChange();
                    count++;
                }
            }
            min3 = (sum / count).ToString("0.00");
        }

        private void Calculate5MinChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate5MinChange();
                    count++;
                }
            }
            min5 = (sum / count).ToString("0.00");
        }

        private void Calculate10MinChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate15MinChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate30MinChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate1HourChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate2HourChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate3HourChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        private void Calculate6HourChange()
        {
            double count = 0;
            double sum = 0;
            foreach (KeyValuePair<string, CryptoCoin> coin in BitTrex.instance.coinList)
            {
                if (coin.Value.currentIndex != 0)
                {
                    sum += coin.Value.Calculate1MinChange();
                    count++;
                }
            }
            min1 = (sum / count).ToString("0.00");
        }

        public void PrintPercentages()
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

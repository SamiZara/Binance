using ConsoleApplication1.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class BitTrex : Form
    {
        NotifyIcon notifyIcon;
        const double minimumVolume = 100;
        const string targetMarket = "BTC";
        public Dictionary<string, CryptoCoin> coinList;
        public static BitTrex instance;
        public delegate void AddRowDelegate(TableDataRow row);
        private AverageCryptoCoin avgCoin;
        public DateTime programStartTime;

        public BitTrex()
        {
            InitializeComponent();
            Initialize();
            Thread updateCoinPricesThread = new Thread(new ThreadStart(UpdateCoinPrices));
            updateCoinPricesThread.Start();
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            programStartTime = DateTime.Now;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void InsertRow(TableDataRow row)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AddRowDelegate(InsertRow), row);
                return;
            }
            string searchValue = row.name;
            foreach (DataGridViewRow rowTemp in dataGridView1.Rows)
            {
                if (rowTemp.Cells[0].Value != null)
                {
                    if (rowTemp.Cells[0].Value.ToString().Equals(searchValue))
                    {
                        int index = dataGridView1.Rows.IndexOf(rowTemp);
                        if(row.name == "AllMarket")
                        {
                            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Gold;
                        }
                        //Console.WriteLine(row.volatility15Min);
                        dataGridView1.Rows[index].Cells["Column1"].Value = row.name;
                        dataGridView1.Rows[index].Cells["Column2"].Value = Convert.ToDouble(row.min1);
                        dataGridView1.Rows[index].Cells["Column3"].Value = Convert.ToDouble(row.min3);
                        dataGridView1.Rows[index].Cells["Column4"].Value = Convert.ToDouble(row.min5);
                        dataGridView1.Rows[index].Cells["Column5"].Value = Convert.ToDouble(row.min10);
                        dataGridView1.Rows[index].Cells["Column6"].Value = Convert.ToDouble(row.min15);
                        dataGridView1.Rows[index].Cells["Column7"].Value = Convert.ToDouble(row.min30);
                        dataGridView1.Rows[index].Cells["Column8"].Value = Convert.ToDouble(row.hour1);
                        dataGridView1.Rows[index].Cells["Column9"].Value = Convert.ToDouble(row.hour2);
                        dataGridView1.Rows[index].Cells["Column10"].Value = Convert.ToDouble(row.hour3);
                        dataGridView1.Rows[index].Cells["Column11"].Value = Convert.ToDouble(row.hour6);
                        dataGridView1.Rows[index].Cells["Column12"].Value = Convert.ToDouble(row.volatility15Min);

                        if (dataGridView1.SortOrder == SortOrder.Ascending)
                            dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Ascending);
                        else if(dataGridView1.SortOrder == SortOrder.Descending)
                            dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Descending);       
                         
                        return;
                    }
                }
            }
            //Console.WriteLine(row.volatility15Min);
            dataGridView1.Rows.Add(row.name, Convert.ToDouble(row.min1), Convert.ToDouble(row.min3), Convert.ToDouble(row.min5), Convert.ToDouble(row.min10), Convert.ToDouble(row.min15), Convert.ToDouble(row.min30), Convert.ToDouble(row.hour1), Convert.ToDouble(row.hour2), Convert.ToDouble(row.hour3), Convert.ToDouble(row.hour6), Convert.ToDouble(row.volatility15Min));
            if (dataGridView1.SortOrder == SortOrder.Ascending)
                dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Ascending);
            else if (dataGridView1.SortOrder == SortOrder.Descending)
                dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Descending);
            
            
            /*if (!dataGridView1.Rows.(row))
            {

            }*/
        }

        public void Initialize()
        {
            coinList = new Dictionary<string, CryptoCoin>();
            instance = this;
            avgCoin = new AverageCryptoCoin("AllMarket");
            /* string html = string.Empty;
             string url = @"https://bittrex.com/api/v1.1/market/getopenorders?apikey=52d1be040242428a912602350cbcecf5&nonce=1";

             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
             request.Headers["apisign"] = "2f1d6a8e673dff079ff58f5d7ea55aebfec1c816e85e7ee11337f24df273bdd45fc33019ce43a5a194fc6cee030db182993dd8137cbfbf3fa7191cb78540eab7";
             using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
             using (Stream stream = response.GetResponseStream())
             using (StreamReader reader = new StreamReader(stream))
             {
                 html = reader.ReadToEnd();
             }
             Console.WriteLine(html);*/
        }

        public void RemoveCurrency(string marketName)
        {
            coinList.Remove(marketName);
        }

        private void UpdateCoinPrices()
        {
            while (true)
            {
                string html = string.Empty;
                string url = @"https://api.binance.com/api/v3/ticker/price";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                var marketData = JArray.Parse(html);


                foreach (var coin in marketData)
                {
                    //Thread coinTick = new Thread(new ThreadStart(coin.Value.Tick));
                    //coinTick.Start();
                    string symbol = coin.ToString().Substring(16, coin.ToString().Substring(16).IndexOf('"'));
                    //string j = coin.ToString().Substring(coin.ToString().IndexOf("price") + 9, 10);
                    float price = float.Parse(coin.ToString().Substring(coin.ToString().IndexOf("price")+9, 10).Replace('.',','));
                    //foreach (string x in coinData)
                    //{
                    if (!coinList.ContainsKey(symbol))  //&& (symbol.Contains("BNB") || symbol.Contains("BTC"))
                    {
                        coinList.Add(symbol, new CryptoCoin(symbol));
                        if((DateTime.Now-programStartTime).TotalMinutes > 1)
                        {
                            NotifyUserNewCoin(symbol + " New coin mofo be fast");
                        }
                    }
                    if(coinList.ContainsKey(symbol))
                        coinList[symbol].Tick(price);

                    
                    //}
                }  
                avgCoin.CalculateChangePercentages();
                avgCoin.PrintData();
                Thread.Sleep(Constants.tickerIntervalInMilliSeconds);
            }
        }

        public void NotifyUser(string message)
        {
            try
            {
                notifyIcon.BalloonTipText = message;
                notifyIcon.ShowBalloonTip(10000);     
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                NotifyUser(message);
            }
        }

        public void NotifyUserNewCoin(string message)
        {
            try
            {
                notifyIcon.BalloonTipText = message;
                notifyIcon.ShowBalloonTip(100000);
                System.Media.SystemSounds.Hand.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                NotifyUser(message);
            }
        }
    }
}

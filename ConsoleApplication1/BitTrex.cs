﻿using ConsoleApplication1.Classes;
using Newtonsoft.Json;
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
        const double minimumVolume = 50;
        const string targetMarket = "BTC";
        public Dictionary<string, CryptoCoin> coinList;
        public static BitTrex instance;
        public delegate void AddRowDelegate(TableDataRow row);
        private AverageCryptoCoin avgCoin;

        public BitTrex()
        {
            InitializeComponent();
            Initialize();
            Thread updateCurrenciesThread = new Thread(new ThreadStart(UpdateCurrencies));
            updateCurrenciesThread.Start();
            Thread updateCoinPricesThread = new Thread(new ThreadStart(UpdateCoinPrices));
            updateCoinPricesThread.Start();
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

        public MarketData GetMarketSummaries()
        {
            string html = string.Empty;
            string url = "https://bittrex.com/api/v1.1/public/getmarketsummaries";
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
                return GetMarketSummaries();
            }
            return JsonConvert.DeserializeObject<MarketData>(html);
        }

        private void UpdateCurrencies()
        {
            while (true)
            {
                Console.WriteLine("Updating currencies");
                MarketData data = GetMarketSummaries();
                lock (coinList)
                {
                    foreach (CryptoCoin coin in data.result)
                    {
                        string[] pair = coin.MarketName.Split('-');
                        if (coin.BaseVolume > minimumVolume && pair[0] == targetMarket)
                        {
                            if (!coinList.ContainsKey(coin.MarketName))
                            {
                                coinList.Add(coin.MarketName, coin);
                            }
                        }
                        else
                        {
                            if (coinList.ContainsKey(coin.MarketName) && coin.BaseVolume < minimumVolume * 0.8f)
                            {
                                coinList.Remove(coin.MarketName);
                            }
                        }
                    }

                }
                Thread.Sleep(600000);
            }
        }

        public void RemoveCurrency(string marketName)
        {
            coinList.Remove(marketName);
        }

        private void UpdateCoinPrices()
        {
            while (true)
            {
                Console.WriteLine("Updating coin prices");
                lock (coinList)
                {
                    foreach (KeyValuePair<string, CryptoCoin> coin  in coinList)
                    {
                        Thread coinTick = new Thread(new ThreadStart(coin.Value.Tick));
                        coinTick.Start();
                    }
                }
                Thread.Sleep(Constants.tickerIntervalInMilliSeconds);
                avgCoin.CalculateChangePercentages();
                avgCoin.PrintData();
            }
        }

        public void NotifyUser(string message)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(10000);
        }

    }
}

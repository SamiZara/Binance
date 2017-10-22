using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class MarketData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<CryptoCoin> result { get; set; }
    }
}

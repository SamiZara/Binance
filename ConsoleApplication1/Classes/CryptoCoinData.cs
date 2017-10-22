using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class CryptoCoinData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public CryptoCoinTicker result { get; set; }
    }
}

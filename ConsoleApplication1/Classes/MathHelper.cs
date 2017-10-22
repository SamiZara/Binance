using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class MathHelper
    {
        public static int Mod(int number, int mod)
        {
            while (number < 0)
                number += mod;
            return number % mod;
        }
    }
}

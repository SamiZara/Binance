using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class TableDataRow
    {
        public string name, min1, min3, min5, min10, min15, min30, hour1, hour2, hour3, hour6;

        public TableDataRow(string name, string min1, string min3, string min5, string min10, string min15, string min30, string hour1, string hour2, string hour3, string hour6)
        {
            this.name = name;
            this.min1 = min1;
            this.min3 = min3;
            this.min5 = min5;
            this.min10 = min10;
            this.min15 = min15;
            this.min30 = min30;
            this.hour1 = hour1;
            this.hour2 = hour2;
            this.hour3 = hour3;
            this.hour6 = hour6;
        }
    }
}

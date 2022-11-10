using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsItems
    {
        public string pos_num { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_gr { get; set; }
        public int item_seq { get; set; }
        public double item_cost { get; set; }
        public clsEnum.Item_vat item_vat { get; set; }
    }
}

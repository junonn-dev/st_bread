using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsBill_Items   
    {
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }
        
        public clsBill_Items()
        {

        }

        ~clsBill_Items()
        {
            Dispose(false);
        }


        private double _dVat = 0; //상품부과세
        private double _DAmt = 0;

        public string bill_pos { set; get; } //기기 번호               
        public int bill_date { set; get; }  //영수증 날짜
        public clsEnum.bill_isCancel bill_iscancel { get; set; }    //영수증취소여부


        public string item_code { get; set; }
        public string item_name { get; set; }
        public double item_cost { get; set; }
        public clsEnum.Item_vat item_vat { get; set; }
        public int item_seq { get; set; }
        public int item_qty { get; set; }
        

        public double dBill_Amt 
        {
            set
            {
                if (item_vat == clsEnum.Item_vat.item_tax)
                {
                    _DAmt = value;
                    _dVat = Set_Amt(_DAmt);
                }
                else
                {
                    _DAmt = value;
                    _dVat = 0;
                }
            }
            get
            {
                return _DAmt;
            }
        }


        public double dItem_Vat 
        {            
            get
            {
                return _dVat;
            }
        }


        public int orgtable_no { get; set; }

        private double  Set_Amt(Double dAmt)
        {
            double dTemp = Math.Round(dAmt / 1.1, 0);
            return dTemp;
        }

    }
}

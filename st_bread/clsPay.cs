using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsPay
    {
        public string sPayjum { set; get; }
        public string sPayMartcd { set; get; }
        public string sPaydate { set; get; }
        public string sPaytime { set; get; }
        public string sPayno { set; get; }
        public string sPaysyn { set; get; }
        public string sPaytp { set; get; }
        public string sPaymemcd { set; get; }
        public int    iPaycount { set; get; }
        public double dPaytotal { set; get; }
        public double dPayvat { set; get; }
        public double dPayamt { set; get; }
        public string sPaybuysa { set; get; }
        public string sPaycardsa { set; get; }
        public string sPaygetid { set; get; }
        public string sPayvan { set; get; }
        public string sPaypad { set; get; }
        public string sPaymon { set; get; }
        public string sPaycartno { set; get; }
        public string sPayagent { set; get; }
        public string sPayauth { set; get; }
        public string sPaycashno { set; get; }
        public double dPaycard { set; get; }
        public double dPaycash { set; get; }
        public string sPaytid { set; get; }
        public string sPayorder { set; get; }
        public string sPaymartocd { set; get; }
        public string sPayono { set; get; }
        public string sPayodate { set; get; }
        public string sPaykind { set; get; }

        public int iTotal { set; get; }
        public int iPan { set; get; }
        public int iVat { set; get; }


        public string sPaydateAuth()
        {

            DateTime dt = Convert.ToDateTime(sPaydate);

            string sTemp = String.Format("{0:yyMMdd}", dt); 
            
            return sTemp;
        }

        //승인 일시 
        public string sPayAuthDateTime()
        {
            //DateTime dt = Convert.ToDateTime(sPaydate);
            string sTemp = sPaydate + " " + sPaytime;

            return sTemp;
        }

        public void Set_VatAmt(double dAmt)
        {
            double dTemp = Math.Round(dAmt / 1.1, 0);

            iPan = (int)dTemp; //Int32.Parse(dTemp.ToString());
            iVat = (int)dAmt - iPan;
            iTotal = (int)dAmt;
        }


    }
}

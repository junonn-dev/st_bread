using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsRecipInfo
    {       
        public int iRecipNum { set; get; }
        public int iSerialNum { set; get; }
        public int iAccCustomNum { set; get; }
        public int iTotal { set; get;}
        public int iPan { set; get; }
        public int iVat { set; get; }
        public int iCashCard { set; get; } //0-카드  1-현금영수증 2-현금 3-쿠폰
        
        public int iRecv { set; get; } //받은돈 DB에는 저장하지 않으나 영수증 출력을 위해 저장
        public int iRest { set; get; } //거스름돈 

        public string sPosNO { set; get; }


        public clsRecipInfo(int iNum, int iSerial,int iAccCustom)
        {
            iRecipNum = iNum + 1;
            iSerialNum = iSerial + 1;
            iAccCustomNum = iAccCustom + 1;
        }


        public void Set_VatAmt(int iAmt)
        {
            double dTemp = Math.Round(iAmt / 1.1, 0);

            iPan = Int32.Parse(dTemp.ToString());
            iVat = iAmt - iPan;
            iTotal = iAmt;

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

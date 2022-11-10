using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread.Bill
{
    public interface Inter_Bill
    {
        void Send_SelectBill(string bill_date);

        void Send_SelectBill(string sPosNum, string bill_date);

        void Send_NFC(string card_num);

        void Send_Recv(double dRest, double dRecieved,clsEnum.Payment_Kind oKind);


    }
}

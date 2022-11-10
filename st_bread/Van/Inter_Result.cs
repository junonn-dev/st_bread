using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace st_bread.CLASS.Van
{
    public interface Inter_Result
    {
        //void Send_VanResutl(clsVanRep oRep,bool IsCanel);

        void Send_VanResutl(clsBill oBill);
        
        void Send_Cancel();



    }
}

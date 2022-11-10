using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace st_bread.CLASS.Van
{
    public class clsVanRep : IDisposable
    {
        private bool disposed = false;

        public double RepAuthAmt { get; set; }

        public string RepType { get; set; }
        public string terminalID { get; set; }

        public string RepCode { get; set; }
        public string RepAuthNum { get; set; }
        public int RepAuthDateTime { get; set; }
        public string RepOrgApprovalNum { get; set; }

        public string RepCmpny { get; set; }

        public string RepBuyCmpny { get; set; }
        public string RepBuyCmpny_Code { get; set; }

        public string RepCardCmpny { get; set; }
        public string RepCardCmpny_Code { get; set; }

        public string RepCardKind { get; set; }
        public string RepErrDesc { set; get; } //밴사별 응답 오류 메세지
        public string RepCardNum { get; set; }

        public string msg1 { get; set; }
        public string msg2 { get; set; }
        public string msg3 { get; set; }
        public string msg4 { get; set; }
        public string msg5 { get; set; }
        public string msg6 { get; set; }



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

        ~clsVanRep()
        {
            Dispose(false);
        }
    }



    //응답 값 변수
    public class clsKovan_Rep : IDisposable
    {
        private bool disposed = false;

        public string rTranType { set; get; }
        public string rErrCode { set; get; }
        public string rCardno { set; get; }
        public string rHalbu { set; get; }
        public string rTamt { set; get; }
        public string rTranDate { set; get; }
        public string rTranTime { set; get; }
        public string rAuthNo { set; get; }
        public string rMerNo { set; get; }
        public string rTranSerial { set; get; }
        public string rIssueCard { set; get; }
        public string rPurchaseCard { set; get; }
        public string rSignPath { set; get; }
        public string rMsg1 { set; get; }
        public string rMsg2 { set; get; }
        public string rMsg3 { set; get; }
        public string rMsg4 { set; get; }
        public string rFiller { set; get; }
        public string rErr_Msg { get; set; }

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

        ~clsKovan_Rep()
        {
            Dispose(false);
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace st_bread.CLASS.Van
{
    public class clsVanAuth : IDisposable 
    {

        private bool disposed = false;

        public string Tid { set; get; }
        public string halbu { set; get; }
        public double tamt { set; get; }        
        public string authdate { set; get; }
        public string authno { set; get; }
        public int tran_serial { set; get; }        
        public clsEnum.Payment_Kind oKind { set; get; }
        public bool isCancel { set; get; }
        


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

        ~clsVanAuth()
        {
            Dispose(false);
        }


    }
}

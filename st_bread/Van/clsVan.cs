using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace st_bread.CLASS.Van
{
    class clsVan : IDisposable
    {private bool disposed = false;

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

        ~clsVan()
        {
            Dispose(false);
        }
        

        public  clsVanRep  Send_Auth(clsVanAuth oAuth)
        {
            clsVanRep oRep = null;

            try
            {
                switch (clsPos.van_cmpny )
                {
                    case clsEnum.Van_Cmpny.kovan:                                           
                        break;
                    case clsEnum.Van_Cmpny.jtnet:
                        break;
                    case clsEnum.Van_Cmpny.kicc:
                        break;
                    case clsEnum.Van_Cmpny.ksnet:
                        break;
                    case clsEnum.Van_Cmpny.nice:
                        break;
                    case clsEnum.Van_Cmpny.smartro:
                        break;
                }
                return oRep;
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {

            }
            
        }    
    }
}

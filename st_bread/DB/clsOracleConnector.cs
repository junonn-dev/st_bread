using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data;

namespace st_bread
{
    class clsOracleConnector : IDisposable
    {
        //[DefaultValue("ALPHA_MEAT")]
        //public string DB_User { get; set; }
        //[DefaultValue("jd890bls")]
        //public string DB_Password { get; set; }
        //[DefaultValue("121.254.162.107")]
        //public string DB_Ip { get; set; }
        //[DefaultValue("1521")]
        //public string DB_Port { get; set; }
        //[DefaultValue("ORCL")]
        //public string DB_Service { get; set; }

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

        public clsOracleConnector()
        {
           
        }


        ~clsOracleConnector()
        {
            Dispose(false);
        }

        public OracleConnection DB_Connect()
        {
            OracleConnection oCon = null;
            try
            {
                oCon = new OracleConnection(clsStaticString.DB_Str  );
                oCon.Open();
                return oCon;
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

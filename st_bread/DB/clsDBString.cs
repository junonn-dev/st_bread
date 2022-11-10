using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsDBString : IDisposable
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

        ~clsDBString()
        {
            Dispose(false);
        }

        public string Get_ConStr(string DB_User,string DB_Password,string DB_Ip,string DB_Port,string DB_Service)
        {
            return string.Format("Data Source = " +
                               " (DESCRIPTION = " +
                               " (ADDRESS = " +
                               " (PROTOCOL = TCP)" +
                               " (HOST = {0})" +
                               " (PORT = {1}))" +
                               " (CONNECT_DATA = (SID = {2})));" +
                               " User ID = {3};" +
                               " Password={4};" +
                               " Pooling=true;" +
                               " Min Pool Size=0;" +
                               " Connection Lifetime = 180;" +
                               " Max Pool Size=50;" +
                               " Incr Pool Size=5;",
                               DB_Ip,
                               DB_Port,
                               DB_Service,
                               DB_User,
                                DB_Password
                                );


            
        }
    }
}

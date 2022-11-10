using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsTable: IDisposable
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
        ~clsTable()
        {
            Dispose(false);
        }

        public clsTable(int _table,int _x,int _y)
        {
            sTable = _table;
            xPos = _x;
            yPos = _y;
        }


        public int sTable { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }

    }
}

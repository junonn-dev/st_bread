using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Reflection; //현재 자신의 함수 이름 가져올때 사용

namespace st_bread
{
    /// <summary>
    /// xm 내용을 dataset을 반환
    /// 
    /// </summary>
    class clsDataSetToXml : IDisposable
    {
        private bool disposed = false;

        private string xml_file;
        private DataSet ds;

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
                    ds.Dispose();

                }
                disposed = true;

            }

        }

        ~clsDataSetToXml()
        {
            Dispose(false);
        }


        public clsDataSetToXml(string XML_File)
        {
            xml_file = XML_File;
            readXML();
        }


        private void readXML()
        {
            try
            {
                ds = new DataSet();
                ds.ReadXml(xml_file);

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }
        }

        public DataTable GetXMLTAble(string sStr)
        {
            try
            {
                return ds.Tables[sStr];
            }
            catch (Exception ex)
            {
                throw new Exception("There are no invoice_rows table defined in the XML file " + xml_file);
            }
        }

        public DataSet GetXMLSet()
        {
            try
            {
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("There are no invoice_rows table defined in the XML file " + xml_file);
            }

        }

        public int SaveXML(DataSet set)
        {
            try
            {
                if (File.Exists(xml_file)) //기존 파일 있다면 지운다.
                {
                    File.Delete(xml_file);
                }

                FileStream fs = new FileStream(xml_file, FileMode.Create);
                set.WriteXml(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }

            return 0;
        }

    }
}

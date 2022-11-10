using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection; //현재 자신의 함수 이름 가져올때 사용
using System.IO;
using System.Diagnostics;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace st_bread
{
    class clsLog
    {
        public static int WriteErrLog(string sClass, string sMethod, string sArg)
        {

#if DEBUG
            int iReturn = 0;

            string sFile = string.Format("Err_{0}.txt", System.DateTime.Today.ToString("yyyyMMdd"));

            sFile = Path.Combine(clsSetting.ERR_DIR(), sFile);

            try
            {
                StreamWriter swWrite = new StreamWriter(new FileStream(sFile, FileMode.Append, FileAccess.Write));
                swWrite.WriteLine("[{0}]\t{1}\t{2}\t{3}",  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") , sClass , sMethod , sArg);
                swWrite.Flush();
                swWrite.Close();
            }
            catch (Exception ex)
            {
                //windows eventlog에 올린다.                

                if (!EventLog.SourceExists(clsStaticString.APP_Name))
                    EventLog.CreateEventSource(clsStaticString.APP_Name, "Application");

                EventLog.WriteEntry(
                            clsStaticString.APP_Name,
                            ex.Message.ToString(),
                            EventLogEntryType.Error,
                            3838
                            );

                iReturn = -1;
            }

            return iReturn;

#else
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {   

                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PACK_Machine.Insert_Machine_Error";
                cmd_SEL.Parameters.Add("MachineCode", OracleDbType.Varchar2).Value = "POS01";
                cmd_SEL.Parameters.Add("err_date_num", OracleDbType.Varchar2).Value = clsSetting._Today();
                cmd_SEL.Parameters.Add("err_date", OracleDbType.Varchar2).Value = clsSetting.Now_DateTime();

                cmd_SEL.Parameters.Add("err_class", OracleDbType.Varchar2).Value = sClass;
                cmd_SEL.Parameters.Add("err_method", OracleDbType.Varchar2).Value = sMethod;
                cmd_SEL.Parameters.Add("err_desc", OracleDbType.Varchar2).Value = sArg;
                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return -1;
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();
            }
#endif


        }


        public static int WriteLog(string sClass, string sMethod, string sArg)
        {
            int iReturn = 0;

            string sFile = System.DateTime.Today.ToString("yyyyMMdd") + ".txt";

            sFile = Path.Combine(clsSetting.ERR_DIR(), sFile);

            try
            {
                StreamWriter swWrite = new StreamWriter(new FileStream(sFile, FileMode.Append, FileAccess.Write));
                swWrite.WriteLine("[{0}]\t{1}\t{2}\t{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), sClass, sMethod, sArg);                
                swWrite.Flush();
                swWrite.Close();
            }
            catch (Exception ex)
            {
                //windows eventlog에 올린다.                

                if (!EventLog.SourceExists(clsStaticString.APP_Name))
                    EventLog.CreateEventSource(clsStaticString.APP_Name, "Application");

                EventLog.WriteEntry(
                            clsStaticString.APP_Name,
                            ex.Message.ToString(),
                            EventLogEntryType.Error,
                            3838
                            );

                iReturn = -1;
            }

            return iReturn;
        }




        public static int WriteSerialLog(string sCom,string sArg)
        {
            int iReturn = 0;

            string sFile = System.DateTime.Today.ToString("yyyyMMdd") + ".txt";

            sFile = Path.Combine(clsSetting.SERIAL_DIR(), sFile);

            try
            {
                StreamWriter swWrite = new StreamWriter(new FileStream(sFile, FileMode.Append, FileAccess.Write));

                swWrite.WriteLine("[{0}]\t{1}\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), sCom,  sArg);                

                
                swWrite.Flush();
                swWrite.Close();
            }
            catch (Exception ex)
            {
                
            }

            return iReturn;
        }

        /// <summary>
        /// 기기 작동 로그 들어온 시점에서저장 하지 않고 list에 샇았다가 한꺼번에 전송 한다.
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sArg"></param>
        /// <returns></returns>
        public static OracleCommand WriteMachineLog(string sClass, string sMethod, string sArg)
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PACK_Machine.Insert_Machine_Log";
                cmd_SEL.Parameters.Add("MachineCode", OracleDbType.Varchar2).Value = "POS01";
                cmd_SEL.Parameters.Add("err_date_num", OracleDbType.Varchar2).Value = clsSetting._Today();
                cmd_SEL.Parameters.Add("err_date", OracleDbType.Varchar2).Value = clsSetting.Now_DateTime();
                cmd_SEL.Parameters.Add("err_class", OracleDbType.Varchar2).Value = sClass;
                cmd_SEL.Parameters.Add("err_method", OracleDbType.Varchar2).Value = sMethod;
                cmd_SEL.Parameters.Add("err_desc", OracleDbType.Varchar2).Value = sArg;
                cmd_SEL.BindByName = true;
                //clsDBExcute.ExcuteQuery(cmd_SEL);

                return cmd_SEL;
            }
            catch (Exception ex)
            {
                WriteLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return null;
            }
            finally
            {
                //if (cmd_SEL != null)
                //    cmd_SEL.Dispose();
            }
        }

    }
}


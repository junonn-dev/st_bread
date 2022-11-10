using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsPosOpen
    {
        public static string pos_num { get; set; }
        public static int open_date { get; set; }
        public static int close_date { get; set; }
        public static int sale_customs { get; set; }
        public static double sale_amount { get; set; }


        public static double sale_Cardamount { get; set; }
        public static double sale_Cashamount { get; set; }


        /// <summary>
        /// 5만원
        /// </summary>
        public static int FOThou { get; set; }
        /// <summary>
        /// 일만원
        /// </summary>
        public static int OOThou { get; set; }
        /// <summary>
        /// 오천원
        /// </summary>
        public static int FThou { get; set; }
        /// <summary>
        /// 일천원
        /// </summary>
        public static int OThou { get; set; }
        public static int FHund { get; set; }
        public static int OHund { get; set; }
        public static int FO { get; set; }
        public static int IO { get; set; }
        public static double  dtotal { get; set; }

        /// <summary>
        /// 마감 오만원
        /// </summary>
        public static int CFOThou { get; set; }
        /// <summary>
        /// 마감 일만원
        /// </summary>
        public static int COOThou { get; set; }
        public static int CFThou { get; set; }
        public static int COThou { get; set; }
        public static int CFHund { get; set; }
        public static int COHund { get; set; }
        public static int CFO { get; set; }
        public static int CIO { get; set; }
        public static double Cdtotal { get; set; }


        public static double cash_io_out { get; set; }
        public static double cash_io_in { get; set; }

        public static bool  Set_Open()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.insert_POS_OPEN";
                
                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                cmd_SEL.Parameters.Add("opendate", OracleDbType.Varchar2).Value = clsPosOpen.open_date;
                cmd_SEL.Parameters.Add("closedate", OracleDbType.Varchar2).Value = clsPosOpen.close_date;
                cmd_SEL.Parameters.Add("cumcount", OracleDbType.Varchar2).Value = clsPosOpen.sale_customs;
                cmd_SEL.Parameters.Add("tamount", OracleDbType.Double).Value = clsPosOpen.sale_amount;

                cmd_SEL.Parameters.Add("card", OracleDbType.Double).Value = clsPosOpen.sale_Cardamount;
                cmd_SEL.Parameters.Add("cash", OracleDbType.Double).Value = clsPosOpen.sale_Cashamount;

                cmd_SEL.Parameters.Add("fo_thou", OracleDbType.Varchar2).Value = clsPosOpen.FOThou;
                cmd_SEL.Parameters.Add("oo_thou", OracleDbType.Varchar2).Value = clsPosOpen.OOThou;
                cmd_SEL.Parameters.Add("f_thou", OracleDbType.Varchar2).Value = clsPosOpen.FThou;
                cmd_SEL.Parameters.Add("o_thou", OracleDbType.Varchar2).Value = clsPosOpen.OThou;
                cmd_SEL.Parameters.Add("f_hund", OracleDbType.Varchar2).Value = clsPosOpen.FHund;
                cmd_SEL.Parameters.Add("o_hund", OracleDbType.Varchar2).Value = clsPosOpen.OHund;
                cmd_SEL.Parameters.Add("f_o", OracleDbType.Varchar2).Value = clsPosOpen.FO;
                cmd_SEL.Parameters.Add("i_o", OracleDbType.Varchar2).Value = clsPosOpen.IO;
                cmd_SEL.Parameters.Add("d_total", OracleDbType.Varchar2).Value = clsPosOpen.dtotal;
                cmd_SEL.Parameters.Add("cashio_in", OracleDbType.Varchar2).Value = cash_io_in;
                cmd_SEL.Parameters.Add("cashio_out", OracleDbType.Varchar2).Value = cash_io_out;
                

                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);

                return true;

            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();
            }
        }
        public static bool Set_Close()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.insert_POS_CLOSE";

                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                cmd_SEL.Parameters.Add("opendate", OracleDbType.Varchar2).Value = clsPosOpen.open_date;
                cmd_SEL.Parameters.Add("closedate", OracleDbType.Varchar2).Value = clsPosOpen.close_date;
                cmd_SEL.Parameters.Add("cumcount", OracleDbType.Varchar2).Value = clsPosOpen.sale_customs;
                cmd_SEL.Parameters.Add("tamount", OracleDbType.Varchar2).Value = clsPosOpen.sale_amount;
                cmd_SEL.Parameters.Add("card", OracleDbType.Double).Value = clsPosOpen.sale_Cardamount;
                cmd_SEL.Parameters.Add("cash", OracleDbType.Double).Value = clsPosOpen.sale_Cashamount;
                cmd_SEL.Parameters.Add("fo_thou", OracleDbType.Varchar2).Value = clsPosOpen.CFOThou;
                cmd_SEL.Parameters.Add("oo_thou", OracleDbType.Varchar2).Value = clsPosOpen.COOThou;
                cmd_SEL.Parameters.Add("f_thou", OracleDbType.Varchar2).Value = clsPosOpen.CFThou;
                cmd_SEL.Parameters.Add("o_thou", OracleDbType.Varchar2).Value = clsPosOpen.COThou;
                cmd_SEL.Parameters.Add("f_hund", OracleDbType.Varchar2).Value = clsPosOpen.CFHund;
                cmd_SEL.Parameters.Add("o_hund", OracleDbType.Varchar2).Value = clsPosOpen.COHund;
                cmd_SEL.Parameters.Add("f_o", OracleDbType.Varchar2).Value = clsPosOpen.CFO;
                cmd_SEL.Parameters.Add("i_o", OracleDbType.Varchar2).Value = clsPosOpen.CIO;
                cmd_SEL.Parameters.Add("d_total", OracleDbType.Varchar2).Value = clsPosOpen.Cdtotal;
                cmd_SEL.Parameters.Add("cashio_in", OracleDbType.Varchar2).Value = cash_io_in;
                cmd_SEL.Parameters.Add("cashio_out", OracleDbType.Varchar2).Value = cash_io_out;
                


                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);

                return true;

            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();
            }
        }


    }
}

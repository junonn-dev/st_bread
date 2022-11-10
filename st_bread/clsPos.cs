using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsPos
    {
        public static string store_code { get; set; }
        public static string store_name { get; set; }

        public static string store_address1 { get; set; }
        public static string store_address2 { get; set; }
        public static string store_number { get; set; }
        public static string store_tel { get; set; }

        public static string pos_num { get; set; }

        public static string port { get; set; }
        public static string speed { get; set; }
        
        public static clsEnum.Van_Cmpny van_cmpny { get; set; }
        public static string tid { get; set; }
        public static int reg_date { get; set; }

        public static bool  isPrint { get; set; }


        public static void Set_Pos_Info()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.insert_POS_INFO";
                cmd_SEL.Parameters.Add("storecode", OracleDbType.Varchar2).Value = clsPos.store_code;
                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("comport", OracleDbType.Varchar2).Value = clsPos.port;
                cmd_SEL.Parameters.Add("portspeed", OracleDbType.Varchar2).Value = clsPos.speed;
                cmd_SEL.Parameters.Add("vancmpny", OracleDbType.Varchar2).Value = (int)clsPos.van_cmpny;
                cmd_SEL.Parameters.Add("vantid", OracleDbType.Varchar2).Value = clsPos.tid;
                cmd_SEL.Parameters.Add("regdate", OracleDbType.Varchar2).Value = clsSetting._Today();
                cmd_SEL.Parameters.Add("print_YN", OracleDbType.Varchar2).Value = clsSetting.Let_Int(clsPos.isPrint);

                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);
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

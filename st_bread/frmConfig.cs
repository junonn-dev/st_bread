using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmConfig : Form,Inter_Set 
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            bool bFail = true;

            if (Check_Config())
            {
                if (clsStaticString.DB_Str.Length > 0)
                {
                    OracleConnection oOraCon = null;

                    try
                    {
                        if (clsStaticString.DB_Str.Length > 0)
                        {
                            clsOracleConnector oCon = new clsOracleConnector();
                            oOraCon = oCon.DB_Connect();

                            if (oOraCon.State != ConnectionState.Open)
                            {
                                MessageBox.Show("DB 와 통신이 원할하지 않습니다.", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                bFail = false;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                        bFail = false;
                    }
                    finally
                    {
                        if (oOraCon != null)
                        {
                            if (oOraCon.State == ConnectionState.Open)
                            {
                                oOraCon.Close();
                            }
                            oOraCon.Dispose();
                        }

                    }
                }
                if (!bFail)
                {
                    clsSetting.Exit_Program();
                }

            }
            else
            {
                //설정 파일이 없는경우
                //

            }

        }

        private void frmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btn_PrintC_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Ban_Click(object sender, EventArgs e)
        {
            frmSerialSetting oSetting = new frmSerialSetting(this as Inter_Set, clsEnum.Select_SerialSet.van);
            oSetting.ShowDialog();
        }




        public void Inter_Set_Setting(string sVal, clsEnum.Select_SerialSet serialSet)
        {
            switch (serialSet)
            {
                case clsEnum.Select_SerialSet.port:
                    txt_Port.Text = sVal;
                    break;
                case clsEnum.Select_SerialSet.speed:
                    txt_Speed.Text = sVal;
                    break;                
                case clsEnum.Select_SerialSet.van:
                    clsPos.van_cmpny  = clsSetting.GetVanEnum(sVal);
                    txt_Van.Text = sVal;
                    break;
            }
        }




        private void btn_Port_Click(object sender, EventArgs e)
        {
            frmSerialSetting oSetting = new frmSerialSetting(this as Inter_Set, clsEnum.Select_SerialSet.port);
            oSetting.ShowDialog();
        }

        private void btn_Speed_Click(object sender, EventArgs e)
        {
            frmSerialSetting oSetting = new frmSerialSetting(this as Inter_Set, clsEnum.Select_SerialSet.speed);
            oSetting.ShowDialog();
        }

        private void btn_PrintS_Click(object sender, EventArgs e)
        {
            if (txt_Store.Text.Length == 0)
            {
                MessageBox.Show("가맹점이 없습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            if (txt_Pos.Text.Length == 0)
            {
                MessageBox.Show("포스번호가 없습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txt_Port.Text.Length > 0)
            {
                if (txt_Speed.Text.Length == 0)
                {

                    MessageBox.Show("영수증 포트 속도를 기재해 주세요.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (clsPos.van_cmpny == clsEnum.Van_Cmpny.none)
            {
                if (txt_TID.Text.Length > 0)
                {
                    //밴사가 없으면 tid지워라
                    MessageBox.Show("무결제 이면 tid를 지워주세요.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
            }
            else
            {
                if (txt_TID.Text.Length == 0)
                {
                    MessageBox.Show("tid를 확인해주세요.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            clsPos.store_code = txt_Store.Text;
            clsPos.pos_num = txt_Pos.Text;
            clsPos.port = txt_Port.Text;
            clsPos.speed = txt_Speed.Text;
            clsPos.van_cmpny = clsSetting.GetVanEnum(txt_Van.Text);
            clsPos.tid = txt_TID.Text;




            Save_Setting();
        }

        clsDataSetToXml dsXml = null;

        private void Save_Setting()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                DataSet set = new DataSet("config");

                DataTable tbl_info = new DataTable("appinfo");
                tbl_info.Columns.Add("store");
                tbl_info.Columns.Add("pos");
                tbl_info.Columns.Add("port");
                tbl_info.Columns.Add("speed");
                tbl_info.Columns.Add("van");
                tbl_info.Columns.Add("tid");

                tbl_info.Rows.Add(
                    clsPos.store_code,
                    clsPos.pos_num,
                    clsPos.port,
                    clsPos.speed,                    
                    clsPos.van_cmpny,
                    clsPos.tid                    
                    );

                set.Tables.Add(tbl_info);


                if (dsXml != null)
                    dsXml.SaveXML(set);
                else
                {
                    dsXml = new clsDataSetToXml(clsStaticString.SETTING_FILE);
                    dsXml.SaveXML(set);
                }

                clsPos.isPrint = false;

                clsPos.Set_Pos_Info();

                MessageBox.Show("저장 하였습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                MessageBox.Show("저장 실패." + "\n" + ex.Message.ToString());
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();



            }

        }
        
        /// <summary>
        /// config 설정 값을 가져와서 정상적인지 체크 후 값 리턴
        /// </summary>
        /// <returns></returns>
        private Boolean Check_Config()
        {
            Boolean isOk = false;

            isOk = true;
            clsDataSetToXml dsXml = new clsDataSetToXml(clsStaticString.DB_FILE);
            DataSet ds = dsXml.GetXMLSet();


            //디비 관련 내용 있는지 체크
            DataTable tbl_db = ds.Tables["dbinfo"];
            if (tbl_db != null)
            {
                if (tbl_db.Rows.Count == 0)
                {
                    isOk = false;
                }
                else
                {
                    //db연결 문자열 생성
                    clsDBString oDBstring = new clsDBString();
                    clsEncryption oEncryption = new clsEncryption();

                    foreach (DataRow dr in tbl_db.Rows)
                    {
                        clsStaticString.DB_Str = oDBstring.Get_ConStr(
                            dr["user"].ToString(),
                            oEncryption.DESDecrypt(dr["pass"].ToString()),
                            dr["ip"].ToString(),
                            dr["port"].ToString(),
                            dr["service"].ToString());
                    }

                    oDBstring.Dispose();
                    oEncryption.Dispose();
                }
            }
            else
            {
                isOk = false;
            }


            return isOk;
        }


        private void Get_Machine_Info(string sCode)
        {
            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.get_POS_INFO";

                //cmd_SEL.Parameters.Add("StoreCode", OracleDbType.Varchar2).Value = sTore;
                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = sCode;
                cmd_SEL.Parameters.Add("cur_pos", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {

                    clsPos.store_code = dr["store_code"].ToString();
                    clsPos.store_name = dr["store_name"].ToString();
                    
                    clsPos.pos_num = dr["pos"].ToString();
                    clsPos.port = dr["port"].ToString();

                    clsPos.speed = dr["speed"].ToString();
                    clsPos.van_cmpny = (clsEnum.Van_Cmpny)Convert.ToInt16(dr["van"]);
                    clsPos.tid = dr["tid"].ToString();
                    clsPos.reg_date = Convert.ToInt32(dr["reg"]);
                    clsPos.isPrint = clsSetting.Let_Boolean(dr["printyn"].ToString());

                }


            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());                
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();

                if (dt != null)
                    dt.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Get_Machine_Info(txt_Pos.Text );

            txt_Pos.Text = clsPos.pos_num;
            txt_Port.Text = clsPos.port;
            txt_Speed.Text = clsPos.speed;
            txt_Store.Text = clsPos.store_code;
            lbl_Store.Text = clsPos.store_name;
            txt_TID.Text = clsPos.tid;
            txt_Van.Text = clsSetting.GetDescription(clsPos.van_cmpny);

        }

        private void btn_Test_Click(object sender, EventArgs e)
        {

            clsPos.port  = txt_Port.Text;
            clsPos.speed = txt_Speed.Text;


            using (clsPrint oPrint = new clsPrint())
            {
                oPrint.Test_Print();
            }

        //    SerialPort port = new SerialPort(clsPos.port , Int32.Parse(clsPos.speed ), Parity.None, 8, StopBits.One);

        //    port.Encoding = Encoding.Default;

        //    port.Open();

        //    port.WriteLine("TEST PRINT");

        //    if (port.IsOpen == true)
        //        port.Close();
        }


    }
}

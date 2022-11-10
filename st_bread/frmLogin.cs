
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using System.Data.OleDb;
using System.Net;
using System.Threading;
using System.Reflection; //현재 실행중 함수명
using System.Diagnostics;
using System.ServiceProcess;
using Oracle.ManagedDataAccess.Client;

namespace st_bread
{
    public partial class frmLogin : Form
    {
        //cls
        clsFile oFile = new clsFile();
        clsDbCon vDbCon = new clsDbCon();
        clsMemad oMemad = new clsMemad();


        public frmLogin()
        {
            InitializeComponent();



            //실행경로 xml에 저장 service, autoupdate 가 사용
            oFile.Set_XMLVal(clsPrintSet.SETTING_FILE(), "APP", "PATH", Application.StartupPath);

            //xml값 읽어온다. 전역변수로 옮기면서 사용 안함
            //MainHT = oFile.ReadXml(clsPrintSet.SETTING_FILE());
            //전역 세팅값
            clsPrintSet.SettingHT = oFile.ReadXml(clsPrintSet.SETTING_FILE());







        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string sMode = string.Empty;
         

#if DEBUG
            sMode = "DEBUG";

#else
            sMode = "RELEASE";
#endif

            lbl_Version.Text = sMode + "V 1.0.0";

            #region DB연결 문자열 생성
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
                                MessageBox.Show("DB 와 통신이 원할하지 않습니다.");

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
            #endregion










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



        private void pic_login_Click(object sender, EventArgs e)
        {
            //설정 화면으로 전환
            frmConfig2 newForm = new frmConfig2(oMemad);
            newForm.StartPosition = FormStartPosition.Manual;
            newForm.Location = new Point(100, 100);
            newForm.Show();
            Program.ac.MainForm = newForm;
            this.Close();


        }

        private void pic_Cancel_Click(object sender, EventArgs e)
        {


            try
            {
                clsSetting.Exit_Program();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }


            this.Close();
        }

        //함수
        private void Save_Memad(clsMemad o_Memad)
        {
            OdbcConnection conn_local = new OdbcConnection();

            try
            {

                conn_local = vDbCon.MysqlOdbcConnect(
                     clsPrintSet.SettingHT["IP2"].ToString(),
                     clsPrintSet.SettingHT["DBNAME2"].ToString(),
                     clsPrintSet.SettingHT["USER2"].ToString(),
                     clsPrintSet.SettingHT["PASS2"].ToString(),
                     clsPrintSet.SettingHT["PORT2"].ToString()
                     );
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }


            try
            {
                if (conn_local.State == ConnectionState.Open)
                {
                    OdbcCommand oCmd_InDel = conn_local.CreateCommand();
                    oCmd_InDel.CommandText = "TRUNCATE TABLE memad ";
                    oCmd_InDel.ExecuteNonQuery();

                    OdbcCommand oCmd_In = conn_local.CreateCommand();
                    oCmd_In.CommandText = "REPLACE INTO memad " +
                                          " (memad_jum,memad_mart_cd,memad_id,memad_pw,memad_nm,memad_div,memad_leave,memad_web) " +
                                          " values " +
                                          " (?,?,?,?,?,?,?,?)";
                    oCmd_In.Parameters.Add("@memad_jum", SqlDbType.Char).Value = o_Memad.memad_jum;
                    oCmd_In.Parameters.Add("@memad_mart_cd", SqlDbType.Char).Value = o_Memad.memad_mart_cd;
                    oCmd_In.Parameters.Add("@memad_id", SqlDbType.Char).Value = o_Memad.memad_id;
                    oCmd_In.Parameters.Add("@memad_pw", SqlDbType.Char).Value = o_Memad.memad_pw;
                    oCmd_In.Parameters.Add("@memad_nm", SqlDbType.Char).Value = o_Memad.memad_nm;
                    oCmd_In.Parameters.Add("@memad_div", SqlDbType.Char).Value = o_Memad.memad_div;
                    oCmd_In.Parameters.Add("@memad_leave", SqlDbType.Char).Value = o_Memad.memad_leave;
                    oCmd_In.Parameters.Add("@memad_web", SqlDbType.Char).Value = o_Memad.memad_web;
                    oCmd_In.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Collections;

using System.Net;
using System.Threading;
using System.Reflection; //현재 실행중 함수명
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Drawing.Printing;
using Oracle.ManagedDataAccess.Client;

namespace st_bread
{
    public partial class frmConfig2 : Form
    {
        clsMemad oMemad = null;

        clsFile oFile = new clsFile();
        clsMart oMart = new clsMart();

        Label[] lbl_Num = new Label[42];
        uscCalendar[] oCalender = new uscCalendar[42];

        List<clsSaleStat> oSaleStat = new List<clsSaleStat>();
        
        //고객 디스플레이
        frmCustomDisplay oCustom = null;







        public frmConfig2(clsMemad o_Memad)
        {
            InitializeComponent();
            oMemad = o_Memad;
        }


        public frmConfig2(clsMemad o_Memad, frmCustomDisplay o_Custom)
        {
            InitializeComponent();
            oMemad = o_Memad;
            oCustom = o_Custom;

        }

        private void frmConfig2_Load(object sender, EventArgs e)
        {
            lbl_MartNm.Text = oMemad.memad_nm;

            Get_MartInfo();

            com_Port.BeginUpdate();
            foreach (string comport in SerialPort.GetPortNames())
            {
                com_Port.Items.Add(comport);
            }
            com_Port.EndUpdate();

            txt_Date.Text = DateTime.Now.ToString("yyyy년MM월"); 
            //txt_Hidden.Text = 

            Panel_CloseAll();
            pan_Calendar.Controls.Clear();



            Set_SaleSate();
            Show_Calendar();
            pan_Calendar.Visible = true;
            pan_Calendar.Dock = DockStyle.Fill;

            if (clsPrintSet.SettingHT["DUAL"].ToString() == "1")
            {
                 Show_CustomDisplay();

            }

            //db 추가 사항
            DB_MODIFY();

            try
            {
                comboBox_listen.Text = clsPrintSet.SettingHT["SOUNDCAL"].ToString();
            }
            catch (Exception ex)
            {   
                //SOUNDCA 없어서 에러 발생시 1으로 초기값을 넣는다.
                oFile.Check_PutSetting("APP", "SOUNDCAL", "1");

            }
        }

        private void pic_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                clsSContoller oSC = new clsSContoller(clsPrintSet.SettingHT["SERVICE"].ToString(), Application.StartupPath);
                //로그인시 서비스 시작 한다.
                oSC.Service_Stop();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }


            this.Close();
        }

        private void Get_MartInfo()
        {
            OracleCommand oCmd = new OracleCommand();
            DataTable dt = null;
            try
            {   
                oCmd.CommandText = " SELECT * FROM ALPHA_MEAT.pos_info  " +
                         " WHERE store_code = ? AND pos = ? ";
                oCmd.Parameters.Add("@store_code", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@pos", SqlDbType.Char).Value = oMemad.memad_mart_cd;


                dt = clsDBExcute.SelectQuery(oCmd);
                
                foreach (DataRow dr in dt.Rows)
                {
                    oMart.sMart_jum = dr["mart_jum"].ToString();
                    oMart.sMart_cd = oMemad.memad_mart_cd;
                    oMart.sMart_nm = dr["mart_nm"].ToString();                    
                    oMart.sMart_getid = dr["mart_getid"].ToString();
                }
                  
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
            finally
            {
                if (oCmd != null)
                    oCmd.Dispose();

                if (dt != null)
                    dt.Dispose();
 
            }
        }














        private void pic_Recv_Click(object sender, EventArgs e)
        {
            Panel_CloseAll();
            pan_RecvResult.Visible = true;
            pan_RecvResult.Dock = DockStyle.Fill;


            Mart_InfoRecv();
            Gr_InfoRecv();
            Master_InfoRecv();

            
        }

        private void Show_Calendar()
        {
            for (int i = 0; i <= lbl_Num.GetUpperBound(0); i++)
            {
                oCalender[i].sDate = "";
                oCalender[i].SetDateText = "";
                
            }

            int iStart_day = 1;
            int[] iDays_a_Month = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int iYear = Int32.Parse(txt_Date.Text.Substring(0, 4));
            int iMonth = Int32.Parse(txt_Date.Text.Substring(5, 2)) - 1;

            if ((iYear % 4 == 0 && iYear % 100 != 0) || iYear % 400 == 0) iDays_a_Month[1] = 29;

            //연 계산
            for (int i = 1; i < iYear; i++)
            {
                if ((i % 4 == 0 && i % 100 != 0) || i % 400 == 0)
                    iStart_day += 366;
                else
                    iStart_day += 365;
            }

            //월계산
            for (int i = 0; i < iMonth; i++)
            {
                iStart_day += iDays_a_Month[i];
            }
            iStart_day = iStart_day % 7;

            for (int i = 0; i < iDays_a_Month[iMonth]; i++)
            {
                oCalender[iStart_day + i].sDate = txt_Date.Text.Substring(0, 4) + "-" + txt_Date.Text.Substring(5, 2) + "-" + string.Format("{0:00}", (i + 1));
                oCalender[iStart_day + i].SetDateText = ((int)(i + 1)).ToString();


                foreach (clsSaleStat o_Sale in oSaleStat )
                {
                    if (o_Sale.sDate  == oCalender[iStart_day + i].sDate)
                    {
                        oCalender[iStart_day + i].SetAmtText = string.Format("{0:#,##0}",o_Sale.dAmt);
                        oCalender[iStart_day + i].SetCntText = o_Sale.iCnt.ToString();
                        oCalender[iStart_day + i].SetIsCloseText = o_Sale.sState;
                    }
                }


            }

            for (int i = 0; i <= lbl_Num.GetUpperBound(0); i++)
            {
                oCalender[i].SetVisible();

            }
        }

        private void Set_SaleSate()
        {

            string sSql = "SELECT  " +
                                    " a.pay_date  ,COUNT(*) AS allcnt,  SUM(IF(a.pay_tp=1 ,a.pay_total,0)) AS authamt,b.kick_salestate  " +
                                " FROM pay a   JOIN bread_kick b ON a.pay_jum = b.kick_jum AND a.pay_pos_cd = b.kick_pos AND a.pay_date = b.kick_date " +
                                " WHERE a.pay_jum = ? AND a.pay_mart_cd = ? AND a.pay_pos_cd = ?  " +
                                " GROUP BY a.pay_date   ORDER BY a.pay_date ";


            OracleCommand oCmd = new OracleCommand();

            oCmd.CommandText = sSql;

            oCmd.Parameters.Add("@pay_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            oCmd.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            oCmd.Parameters.Add("@pay_pos_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();



            //OdbcDataReader reader = oCmd.ExecuteReader();



            //while (reader.Read())
            //{
            //    clsSaleStat o_Sale = new clsSaleStat();

            //    o_Sale.sDate = string.Format("{0:yyyy-MM-dd}", reader["pay_date"]);
            //    o_Sale.dAmt = double.Parse(reader["authamt"].ToString());
            //    o_Sale.iCnt = Int32.Parse(reader["allcnt"].ToString());
            //    o_Sale.sState = reader["kick_salestate"].ToString();

            //    oSaleStat.Add(o_Sale);

            //}

            //reader.Close();

        }



        #region 자료 수신
        private void Mart_InfoRecv()
        {
            try
            {
                OracleCommand oCmd = new OracleCommand();
                oCmd.CommandText = " SELECT " +
                                    " * " +
                                    " FROM mart  " +
                                    " WHERE mart_jum =? AND mart_cd = ? ";
                oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                
                //OdbcDataReader reader = oCmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    OdbcCommand oCmd_InDel = conn_local.CreateCommand();
                //    oCmd_InDel.CommandText = "TRUNCATE TABLE mart ";
                //    oCmd_InDel.ExecuteNonQuery();

                //    OdbcCommand oCmd_In = conn_local.CreateCommand();
                //    oCmd_In.CommandText = "REPLACE INTO mart " +
                //                          " (mart_jum,mart_cd,mart_nm,mart_address,mart_str,mart_end,mart_comno,mart_getid,mart_dnm,mart_dnmhp1,mart_dnmhp2,mart_dnmhp3,mart_ceo,mart_ceohp1,mart_ceohp2,mart_ceohp3,mart_leave) " +
                //                          " values " +
                //                          " (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                //    oCmd_In.Parameters.Add("@mart_jum", SqlDbType.Char).Value = reader["mart_jum"].ToString();
                //    oCmd_In.Parameters.Add("@mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //    oCmd_In.Parameters.Add("@mart_nm", SqlDbType.Char).Value = reader["mart_nm"].ToString();

                //    oCmd_In.Parameters.Add("@mart_address", SqlDbType.Char).Value = reader["mart_address"].ToString();


                //    if (reader["mart_str"].ToString() == string.Empty)
                //        oCmd_In.Parameters.Add("@mart_str", SqlDbType.Date).Value = "";
                //    else
                //        oCmd_In.Parameters.Add("@mart_str", SqlDbType.Date).Value = string.Format("{0:yyyy-MM-dd}", reader["mart_str"]);

                //    if (reader["mart_end"].ToString() == string.Empty)
                //        oCmd_In.Parameters.Add("@mart_end", SqlDbType.Date).Value = "";
                //    else
                //        oCmd_In.Parameters.Add("@mart_end", SqlDbType.Date).Value = string.Format("{0:yyyy-MM-dd}", reader["mart_end"]);


                //    oCmd_In.Parameters.Add("@mart_comno", SqlDbType.Char).Value = reader["mart_comno"].ToString();
                //    oCmd_In.Parameters.Add("@mart_getid", SqlDbType.Char).Value = reader["mart_getid"].ToString();
                //    oCmd_In.Parameters.Add("@mart_dnm", SqlDbType.Char).Value = reader["mart_dnm"].ToString();
                //    oCmd_In.Parameters.Add("@mart_dnmhp1", SqlDbType.Char).Value = reader["mart_dnmhp1"].ToString();
                //    oCmd_In.Parameters.Add("@mart_dnmhp2", SqlDbType.Char).Value = reader["mart_dnmhp2"].ToString();
                //    oCmd_In.Parameters.Add("@mart_dnmhp3", SqlDbType.Char).Value = reader["mart_dnmhp3"].ToString();
                //    oCmd_In.Parameters.Add("@mart_ceo", SqlDbType.Char).Value = reader["mart_ceo"].ToString();
                //    oCmd_In.Parameters.Add("@mart_ceohp1", SqlDbType.Char).Value = reader["mart_ceohp1"].ToString();
                //    oCmd_In.Parameters.Add("@mart_ceohp2", SqlDbType.Char).Value = reader["mart_ceohp2"].ToString();
                //    oCmd_In.Parameters.Add("@mart_ceohp3", SqlDbType.Char).Value = reader["mart_ceohp3"].ToString();
                //    oCmd_In.Parameters.Add("@mart_leave", SqlDbType.Char).Value = reader["mart_leave"].ToString();
                //    oCmd_In.ExecuteNonQuery();

                //    Application.DoEvents();

                //    txt_Result.Text += "트럭 정보" + Environment.NewLine;
                //    txt_Result.Text += reader["mart_nm"].ToString() + " 정보 저장 성공" + Environment.NewLine;
                //    txt_Result.Text += Environment.NewLine;
                //    txt_Result.Text += Environment.NewLine;

                //}
                //else
                //{
                //    txt_Result.Text += "트럭 정보" + Environment.NewLine;
                //    txt_Result.Text += "정보 없음" + Environment.NewLine;
                //    txt_Result.Text += Environment.NewLine;
                //    txt_Result.Text += Environment.NewLine;



                //}
                //reader.Close();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                txt_Result.Text += "ERR" + Environment.NewLine;
                txt_Result.Text += ex.Message.ToString() + Environment.NewLine;
                txt_Result.Text += Environment.NewLine;
                txt_Result.Text += Environment.NewLine;
            }
        }

        private void Gr_InfoRecv()
        {
            int iCnt = 0;
            try
            {
                //OdbcCommand oCmd = conn.CreateCommand();
                //oCmd.CommandText = " SELECT * FROM gr  " +
                //         " WHERE gr_jum = ? and gr_mart_cd = ? ORDER BY gr_rank";
                //oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //OdbcDataReader reader = oCmd.ExecuteReader();



                //OdbcCommand oCmd_InDel = conn_local.CreateCommand();
                //oCmd_InDel.CommandText = "TRUNCATE TABLE gr ";
                //oCmd_InDel.ExecuteNonQuery();

                //while (reader.Read())
                //{


                //    OdbcCommand oCmd_In = conn_local.CreateCommand();
                //    oCmd_In.CommandText = "REPLACE INTO gr(gr_jum,gr_mart_cd,gr_cd,gr_nm,gr_leave) values (?,?,?,?,?)";
                //    oCmd_In.Parameters.Add("@gr_jum", SqlDbType.Char).Value = reader["gr_jum"].ToString();
                //    oCmd_In.Parameters.Add("@gr_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //    oCmd_In.Parameters.Add("@gr_cd", SqlDbType.Char).Value = reader["gr_cd"].ToString();
                //    oCmd_In.Parameters.Add("@gr_nm", SqlDbType.Char).Value = reader["gr_nm"].ToString();
                //    oCmd_In.Parameters.Add("@gr_leave", SqlDbType.Char).Value = reader["gr_leave"].ToString();
                //    oCmd_In.ExecuteNonQuery();

                //    Application.DoEvents();
                //    iCnt++;

                //}
                //reader.Close();

                txt_Result.Text += "메뉴그룹 정보" + Environment.NewLine;
                txt_Result.Text += iCnt + "개  정보 저장 성공" + Environment.NewLine;
                txt_Result.Text += Environment.NewLine;
                txt_Result.Text += Environment.NewLine;

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                txt_Result.Text += "ERR" + Environment.NewLine;
                txt_Result.Text += ex.Message.ToString() + Environment.NewLine;
                txt_Result.Text += Environment.NewLine;
                txt_Result.Text += Environment.NewLine;
            }
        }

        private void Master_InfoRecv()
        {
            int iCnt = 0;
            try
            {
                //OdbcCommand oCmd = conn.CreateCommand();
                //oCmd.CommandText = " SELECT * FROM master  " +
                //         " WHERE master_jum = ? AND master_mart_cd = ? ";
                //oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //OdbcDataReader reader = oCmd.ExecuteReader();

                //OdbcCommand oCmd_InDel = conn_local.CreateCommand();
                //oCmd_InDel.CommandText = "TRUNCATE TABLE master ";
                //oCmd_InDel.ExecuteNonQuery();



                //while (reader.Read())
                //{
                //    OdbcCommand oCmd_In = conn_local.CreateCommand();
                //    oCmd_In.CommandText = "REPLACE INTO master(master_seq,master_jum,master_mart_cd,master_gr_cd,master_cd,master_nm,master_img,master_cost,master_date,master_time,master_best,master_rank,master_leave) values " +
                //                            " (?,?,?,?,?,?,?,?,?,?,?,?,?)";
                //    oCmd_In.Parameters.Add("@master_seq", SqlDbType.Int).Value = Int32.Parse(reader["master_seq"].ToString());
                //    oCmd_In.Parameters.Add("@master_jum", SqlDbType.Char).Value = reader["master_jum"].ToString();

                //    oCmd_In.Parameters.Add("@master_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //    oCmd_In.Parameters.Add("@master_gr_cd", SqlDbType.Char).Value = reader["master_gr_cd"].ToString();
                //    oCmd_In.Parameters.Add("@master_cd", SqlDbType.Char).Value = reader["master_cd"].ToString();
                //    oCmd_In.Parameters.Add("@master_nm", SqlDbType.Char).Value = reader["master_nm"].ToString();
                //    oCmd_In.Parameters.Add("@master_img", SqlDbType.Char).Value = reader["master_img"].ToString();
                //    oCmd_In.Parameters.Add("@master_cost", SqlDbType.Char).Value = reader["master_cost"].ToString();

                //    if (reader["master_date"].ToString() == string.Empty)
                //        oCmd_In.Parameters.Add("@master_date", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                //    else
                //        oCmd_In.Parameters.Add("@master_date", SqlDbType.Date).Value = string.Format("{0:yyyy-MM-dd}", reader["master_date"]);


                //    oCmd_In.Parameters.Add("@master_time", SqlDbType.Char).Value = reader["master_time"].ToString();

                //    oCmd_In.Parameters.Add("@master_time", SqlDbType.Char).Value = reader["master_best"].ToString();
                //    oCmd_In.Parameters.Add("@master_time", SqlDbType.Char).Value = reader["master_rank"].ToString();

                //    oCmd_In.Parameters.Add("@master_leave", SqlDbType.Char).Value = reader["master_leave"].ToString();

                //    oCmd_In.ExecuteNonQuery();

                //    Application.DoEvents();
                //    iCnt++;

                //}
                //reader.Close();

                txt_Result.Text += "메뉴 정보" + Environment.NewLine;
                txt_Result.Text += iCnt + "개  정보 저장 성공" + Environment.NewLine;

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                txt_Result.Text += "ERR" + Environment.NewLine;
                txt_Result.Text += ex.Message.ToString() + Environment.NewLine;
            }
        }
        #endregion

        private void btn_PrintS_Click(object sender, EventArgs e)
        {
            try
            {
                //conHt["COMP"] = com_Port.Text;
                //conHt["SPEED"] = txt_Speed.Text;
                //oFile.Set_XMLVal(clsPrintSet.SETTING_FILE(), "VPOS", "COMP", conHt["COMP"].ToString());
                //oFile.Set_XMLVal(clsPrintSet.SETTING_FILE(), "VPOS", "SPEED", conHt["SPEED"].ToString());

                oFile.Check_PutSetting("VPOS", "COMP", com_Port.Text);
                oFile.Check_PutSetting("VPOS", "SPEED", txt_Speed.Text);
                oFile.Check_PutSetting("VPOS", "POSNO", txt_Pos.Text);

                //Check_PutSetting("POSNO", txt_POSNO.Text);

                SerialPort port = new SerialPort(clsPrintSet.SettingHT["COMP"].ToString(), Int32.Parse(clsPrintSet.SettingHT["SPEED"].ToString()), Parity.None, 8, StopBits.One);

                port.Encoding = Encoding.Default;

                port.Open();

                port.WriteLine("TEST PRINT");

                if (port.IsOpen == true)
                    port.Close();


            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

        }

        private void btn_PrintC_Click(object sender, EventArgs e)
        {
            pan_PrintR.Visible = false;
            pan_Calendar.Visible = true;
            pan_Calendar.Dock = DockStyle.Fill;
        }
        private void Panel_CloseAll()
        {
            pan_RecvResult.Visible = false;
           // pan_ShowConR.Visible = false;
            pan_PrintR.Visible = false;
           // pan_SaleCR.Visible = false;
            pan_Calendar.Visible = false;
            pan_SaleCR.Visible = false;
            pan_Config.Visible = false;


        }

        private void pic_Print_Click(object sender, EventArgs e)
        {
            Panel_CloseAll();
            pan_PrintR.Visible = true;
            pan_PrintR.Dock = DockStyle.Fill;

            com_Port.Text = clsPrintSet.SettingHT["COMP"].ToString(); // conHt["COMP"].ToString();
            txt_Speed.Text = clsPrintSet.SettingHT["SPEED"].ToString();// conHt["SPEED"].ToString();
            txt_Pos.Text = clsPrintSet.SettingHT["POSNO"].ToString();// conHt["SPEED"].ToString();


        }

        private void pic_Start_Click(object sender, EventArgs e)
        {

            //현재 날짜에 판매 중이면 넘어간다.
            Boolean bOk = false;

            string sSql = "SELECT  *   " +
                               " FROM bread_kick " +
                               " WHERE kick_jum = ? AND kick_mart = ? AND kick_pos = ? AND kick_date = ?";

            //OdbcCommand oCmd = conn_local.CreateCommand();
            //oCmd.CommandText = sSql;

            //oCmd.Parameters.Add("@kick_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //oCmd.Parameters.Add("@kick_mart", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //oCmd.Parameters.Add("@kick_pos", SqlDbType.Char).Value =  clsPrintSet.SettingHT["POSNO"].ToString();
            //oCmd.Parameters.Add("@kick_date", SqlDbType.Char).Value = DateTime.Now.ToString("yyyy-MM-dd");

            //OdbcDataReader reader = oCmd.ExecuteReader();

            //if (reader.Read())
            //{
            //    bOk = true;
            //}
            //else
            //{
            //    frmStartSale frmStar = new frmStartSale(oMemad,oMart );
            //    if (DialogResult.OK == frmStar.ShowDialog())
            //    {
            //        bOk = true;

            //        //하루 판매 시작시 이벤트 고객 숫자 초기화
            //        oFile.Check_PutSetting("APP", "ACCCNT", "0");
            //    }
            //    else
            //    {
            //        bOk = false;
 
            //    }
            //}

            //reader.Close();
            
            //판매화면으로 이동
            if (bOk == true)
            {
                frmOrder_Type2 newForm = new frmOrder_Type2( );
                newForm.Show();
                Program.ac.MainForm = newForm;
                
                this.Close();
                
            }
        }

        private void pic_SaleC_Click(object sender, EventArgs e)
        {
            Panel_CloseAll();
            pan_SaleCR.Visible = true;
            pan_SaleCR.Dock = DockStyle.Fill;

            DateTime result = DateTime.Today;
            txt_Date2.Value = result;
        }

    

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            DateTime dt_temp = Convert.ToDateTime(txt_Date.Text);

            DateTime dt = dt_temp.AddMonths(-1);
            txt_Date.Text = dt.ToString("yyyy년MM월");
            Show_Calendar();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DateTime dt_temp = Convert.ToDateTime(txt_Date.Text);

            DateTime dt = dt_temp.AddMonths(1);
            txt_Date.Text = dt.ToString("yyyy년MM월");
            Show_Calendar();

        }

        private void btn_RecvC_Click(object sender, EventArgs e)
        {
            pan_RecvResult.Visible = false;
            pan_Calendar.Visible = true;
            pan_Calendar.Dock = DockStyle.Fill;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pan_SaleCR.Visible = false;
            pan_Prog.Visible = false;
            
            Set_SaleSate();
            Show_Calendar();

            pan_Calendar.Visible = true;
            pan_Calendar.Dock = DockStyle.Fill;
        }

        private void btn_SaleC_Click(object sender, EventArgs e)
        {
            int iResult = 0; //마감 정상 여부 0 - 정상 1-실패
            //정산서 출력
            oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, txt_Date.Text + " 마감 업무 시작");

            lbl_SaleCR.Text = "마감 업무 시작";
            pan_Prog.Visible = true;


            pAll.Maximum = 5;
            pAll.Value = 0;

            lbl_SaleCR.Text = "off line 전표 체크";
            pAll.Value += 1;

            //1단계 off 전표 체크
            iResult = Step1_CheckOffline();
            if (iResult == 0)
            {
                lbl_SaleCR.Text = "총 매출 집계";
                pAll.Value += 1;

                //2단계 총 매출 
                iResult = Step2_InputSalec();
                if (iResult == 0)
                {
                    lbl_SaleCR.Text = "카드사별 매출 집계";
                    pAll.Value += 1;

                    //카드사별 매출
                    iResult = Step3_InputCardc();
                    if (iResult == 0)
                    {
                        lbl_SaleCR.Text = "상품별 매출 집계";
                        pAll.Value += 1;

                        //상품 별 매출
                        iResult = Step4_InputMasterc();
                    }
                    else
                    {
                        iResult = 1;
                    }
                }
                else
                {
                    iResult = 1;
                }
            }
            else
            {
                iResult = 1;
            }


            try
            {

                lbl_SaleCR.Text = "마감 결과 저장";
                pAll.Value = 5;

                //완료 로그 작성
                string sSql2 = "INSERT INTO end_logc " +
                                "    (" +
                                    "	log_jum, " +
                                    "	log_mart_cd, " +
                                    "	log_date, " +
                                    "	log_time, " +
                                    "	log_id, " +
                                    "	log_cdate, " +
                                    "	log_result" +
                                "    )  VALUES (?,?,?,?,?,?,?); ";
                

                //OdbcCommand oCmd1 = conn.CreateCommand();
                //oCmd1.CommandText = sSql2;
                //oCmd1.Parameters.Add("@log_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd1.Parameters.Add("@log_mart_cd", SqlDbType.Char).Value = oMart.sMart_cd;
                //oCmd1.Parameters.Add("@log_date", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");  //string.Format("{0:yyyy-MM-dd}", reader["mart_str"]);                
                ////상품정보
                //oCmd1.Parameters.Add("@log_time", SqlDbType.Char).Value = DateTime.Now.ToString("HH:mm:ss");
                //oCmd1.Parameters.Add("@log_id", SqlDbType.Char).Value = oMemad.memad_id;
                //oCmd1.Parameters.Add("@log_cdate", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");
                //oCmd1.Parameters.Add("@log_result", SqlDbType.Char).Value = iResult.ToString();
                //oCmd1.ExecuteNonQuery();


                ////시제금에 마감 표시
                //string sSql = "UPDATE  bread_kick SET kick_SaleState = '1' " +
                //               " WHERE kick_jum = ? AND kick_pos = ? AND kick_date ";



                //OdbcCommand oCmd = conn_local.CreateCommand();
                //oCmd.CommandText = sSql;

                //oCmd.Parameters.Add("@kick_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd.Parameters.Add("@kick_pos", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
                //oCmd.Parameters.Add("@kick_date", SqlDbType.Char).Value = DateTime.Now.ToString("yyyy-MM-dd");
                //oCmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //마감은 다른 파일에 로그 저장
                oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                lbl_SaleCR.Text = "마감 에러발생 ";
            }

            lbl_SaleCR.Text = "마감 완료";
            oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, txt_Date.Text + " 마감 업무 종료");

            Print_Result();
        }


        #region 마감
        private int Step2_InputSalec()
        {

            //try
            //{
            //    string sSql = "SELECT  " +
            //                        " COUNT(*) as allcnt, " +                                                             //총고객수
            //                        " SUM(IF(pay_tp=1 ,1,0)) AS authall, " +                                    //승인(취소안한) 고객수
            //                        " SUM(IF(pay_tp=5 ,1,0)) AS banall ,  " +                                   //취소 한넘들
            //                        " SUM(pay_total) AS totamt, " +                                             //총 매출
            //                        " SUM(IF(pay_tp=1 ,pay_total,0)) AS authamt, " +                            //승인 매출
            //                        " SUM(IF(pay_tp=5 ,pay_total,0)) AS banamt, " +                             //취소 매출
            //                        " SUM(IF(pay_kind=0 ,pay_total,0)) AS cardamt, " +                          //카드 매출
            //                        " SUM(IF(pay_tp=1 ,IF(pay_kind=0 ,pay_total,0),0)) AS cardauthamt, " +      //카드 승인 매출
            //                        " SUM(IF(pay_tp=5 ,IF(pay_kind=0 ,pay_total,0),0)) AS cardbanamt, " +       //카드 취소
            //                        " SUM(IF(pay_kind=1 ,pay_total,0)) AS cashamt,     " +                      //현금영수증 매출
            //                        " SUM(IF(pay_tp=1 ,IF(pay_kind=1 ,pay_total,0),0)) AS cashauthamt, " +      //현금 영수증 승인 매출
            //                        " SUM(IF(pay_tp=5 ,IF(pay_kind=1 ,pay_total,0),0)) AS cashbanamt,      " +  //현금 영수증 취소 매출
            //                        " SUM(IF(pay_kind=2 ,pay_total,0)) AS noauthcashamt, " +                    //걍 현금 매출
            //                        " SUM(IF(pay_tp=1 ,IF(pay_kind=2 ,pay_total,0),0)) AS noauthcashauthamt, " +//현금 승인 매출
            //                        " SUM(IF(pay_tp=5 ,IF(pay_kind=2 ,pay_total,0),0)) AS noauthcashbanamt, " +  //현금 취소 매출
            //                        " SUM(IF(pay_kind in (3,4) ,pay_total,0)) AS couponamt, " +                    //쿠폰 구매 금액
            //                        " SUM(IF(pay_tp=1 ,IF(pay_kind in (3,4) ,pay_total,0),0)) AS couponauthamt, " +//쿠폰 승인 매출
            //                        " SUM(IF(pay_tp=5 ,IF(pay_kind in (3,4) ,pay_total,0),0)) AS couponcashbanamt " +  //쿠폰 취소 매출
            //                    " FROM pay     " +
            //                    " WHERE pay_jum = ? AND pay_mart_cd = ? AND pay_pos_cd = ? AND  pay_date = ? ";



            //    OdbcCommand oCmd = conn_local.CreateCommand();
            //    oCmd.CommandText = sSql;

            //    oCmd.Parameters.Add("@pay_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    oCmd.Parameters.Add("@pay_pos_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
            //    oCmd.Parameters.Add("@pay_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

            //    OdbcDataReader reader = oCmd.ExecuteReader();
            //    if (reader.Read())
            //    {
            //        //기존 마감 자료 삭제
            //        string sSql3 = "DELETE  " +
            //                   " FROM end_salec     " +
            //                   " WHERE salec_jum = ? AND salec_mart_cd = ? AND salec_pos_cd = ? AND  salec_date = ? ";

            //        OdbcCommand oCmd_DEL = conn.CreateCommand();
            //        oCmd_DEL.CommandText = sSql3;

            //        oCmd_DEL.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //        oCmd_DEL.Parameters.Add("@salec_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //        oCmd_DEL.Parameters.Add("@salec_pos_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
            //        oCmd_DEL.Parameters.Add("@salec_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");
            //        oCmd_DEL.ExecuteNonQuery();

            //        string sSql2 = "INSERT INTO end_salec " +
            //                    "    (" +
            //                    "    salec_jum," +
            //                    "    salec_mart_cd, " +
            //                    "    salec_pos_cd, " +
            //                    "    salec_date, " +
            //                    "    salec_totalcnt, " +
            //                    "    salec_salecnt, " +
            //                    "    salec_bancnt, " +
            //                    "    salec_totalamt, " +
            //                    "    salec_authamt, " +
            //                    "    salec_banamt, " +
            //                    "   salec_cardamt," +
            //                    "   salec_cardauthamt," +
            //                    "   salec_cardbanamt," +
            //                    "   salec_cashamt," +
            //                    "   salec_cashauthamt," +
            //                    "   salec_cashbanamt," +
            //                    "   salec_cashamt_noauth," +
            //                    "   salec_cashauthamt_noauth," +
            //                    "   salec_cashbanamt_noauth," +
            //                    "   salec_couponamt," +
            //                    "   salec_couponauthamt," +
            //                    "   salec_couponbamt," +
            //                    "   salec_cusavg," +
            //                    "   salec_creatdate " +
            //                    "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";


            //        OdbcCommand oCmd1 = conn.CreateCommand();
            //        oCmd1.CommandText = sSql2;

            //        oCmd1.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //        oCmd1.Parameters.Add("@salec_martcd", SqlDbType.Char).Value = oMart.sMart_cd;
            //        oCmd1.Parameters.Add("@salec_pos_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
            //        oCmd1.Parameters.Add("@salec_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");
            //        //객수
            //        oCmd1.Parameters.Add("@salec_totalcnt", SqlDbType.Int).Value = Int32.Parse(reader["allcnt"].ToString());
            //        oCmd1.Parameters.Add("@salec_salecnt", SqlDbType.Int).Value = Int32.Parse(reader["authall"].ToString());
            //        oCmd1.Parameters.Add("@salec_bancnt", SqlDbType.Int).Value = Int32.Parse(reader["banall"].ToString());
            //        //전체 매출
            //        oCmd1.Parameters.Add("@salec_totalamt", SqlDbType.Decimal).Value = double.Parse(reader["totamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_authamt", SqlDbType.Decimal).Value = double.Parse(reader["authamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_banamt", SqlDbType.Decimal).Value = double.Parse(reader["banamt"].ToString());
            //        //카드매출
            //        oCmd1.Parameters.Add("@salec_cardamt", SqlDbType.Decimal).Value = double.Parse(reader["cardamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_cardauthamt", SqlDbType.Decimal).Value = double.Parse(reader["cardauthamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_cardbanamt", SqlDbType.Decimal).Value = double.Parse(reader["cardbanamt"].ToString());
            //        //현금영수증매출
            //        oCmd1.Parameters.Add("@salec_cashamt", SqlDbType.Decimal).Value = double.Parse(reader["cashamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_cashauthamt", SqlDbType.Decimal).Value = double.Parse(reader["cashauthamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_cashbanamt", SqlDbType.Decimal).Value = double.Parse(reader["cashbanamt"].ToString());
            //        //현금매출
            //        oCmd1.Parameters.Add("@salec_cashamt_noauth", SqlDbType.Decimal).Value = double.Parse(reader["noauthcashamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_cashauthamt_noauth", SqlDbType.Decimal).Value = double.Parse(reader["noauthcashauthamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_cashbanamt_noauth", SqlDbType.Decimal).Value = double.Parse(reader["noauthcashbanamt"].ToString());

            //        //쿠폰 매출
            //        oCmd1.Parameters.Add("@salec_couponamt", SqlDbType.Decimal).Value = double.Parse(reader["couponamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_couponauthamt", SqlDbType.Decimal).Value = double.Parse(reader["couponauthamt"].ToString());
            //        oCmd1.Parameters.Add("@salec_couponbamt", SqlDbType.Decimal).Value = double.Parse(reader["couponcashbanamt"].ToString());

            //        int iAmt = Int32.Parse(reader["authamt"].ToString());
            //        int iCnt = Int32.Parse(reader["authall"].ToString());
            //        int iCAvg = 0;

            //        //객단가 계산
            //        if (iCnt > 0)
            //            iCAvg = iAmt / iCnt;

            //        //객단가
            //        oCmd1.Parameters.Add("@salec_cusavg", SqlDbType.Decimal).Value = double.Parse(iCAvg.ToString());
            //        //전표생성일
            //        oCmd1.Parameters.Add("@salec_creatdate", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");

            //        oCmd1.ExecuteNonQuery();
            //        Application.DoEvents();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //마감은 다른 파일에 로그 저장
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    return -1;
            //}
            //finally
            //{
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, "STEP 2 OK");
            //}


            return 0;
        }

        private int Step3_InputCardc()
        {

            //try
            //{
            //    string sSql = "SELECT  " +
            //                        " pay_buysa, " +
            //                        " SUM(pay_total) AS cardamt, " +
            //                        " SUM(IF(pay_tp=1 ,pay_total,0)) AS cardauthamt, " +      //카드 승인 매출
            //                        " SUM(IF(pay_tp=5 ,pay_total,0)) AS cardbanamt " +       //카드 취소                                    
            //                    " FROM pay     " +
            //                    " WHERE pay_jum = ? AND pay_mart_cd = ? AND pay_pos_cd = ?  AND  pay_date = ? AND pay_kind = 0 " +
            //                    " GROUP BY pay_buysa ";


            //    //OdbcCommand oCmd = conn_local.CreateCommand();
            //    //oCmd.CommandText = sSql;

            //    //oCmd.Parameters.Add("@pay_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    //oCmd.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    //oCmd.Parameters.Add("@pay_pos_cd", SqlDbType.Char).Value =  clsPrintSet.SettingHT["POSNO"].ToString();
            //    //oCmd.Parameters.Add("@pay_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

            //    //OdbcDataReader reader = oCmd.ExecuteReader();


            //    ////기존 마감 자료 삭제
            //    //string sSql3 = "DELETE  " +
            //    //           " FROM end_cardc     " +
            //    //           " WHERE cardc_jum = ? AND cardc_mart_cd = ? AND cardc_pos_cd = ?  AND  cardc_date = ? ";

            //    //OdbcCommand oCmd_DEL = conn.CreateCommand();
            //    //oCmd_DEL.CommandText = sSql3;

            //    //oCmd_DEL.Parameters.Add("@cardc_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    //oCmd_DEL.Parameters.Add("@cardc_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    //oCmd_DEL.Parameters.Add("@cardc_pos_cd", SqlDbType.Char).Value =  clsPrintSet.SettingHT["POSNO"].ToString();
            //    //oCmd_DEL.Parameters.Add("@cardc_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");
            //    //oCmd_DEL.ExecuteNonQuery();



            //    //while (reader.Read())
            //    //{


            //    //    string sSql2 = "INSERT INTO end_cardc " +
            //    //                "    (" +
            //    //                "    cardc_jum," +
            //    //                "    cardc_mart_cd, " +
            //    //                "    cardc_pos_cd, " +
            //    //                "    cardc_date, " +
            //    //                "    cardc_buysa, " +
            //    //                "    cardc_amt," +
            //    //                "    cardc_authamt," +
            //    //                "    cardc_banamt," +
            //    //                "    cardc_creatdate " +
            //    //                "    )  VALUES (?,?,?,?,?,?,?,?,?); ";


            //    //    OdbcCommand oCmd1 = conn.CreateCommand();
            //    //    oCmd1.CommandText = sSql2;

            //    //    oCmd1.Parameters.Add("@cardc_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    //    oCmd1.Parameters.Add("@cardc_mart_cd", SqlDbType.Char).Value = oMart.sMart_cd;
            //    //    oCmd1.Parameters.Add("@cardc_pos_cd", SqlDbType.Char).Value =  clsPrintSet.SettingHT["POSNO"].ToString();
            //    //    oCmd1.Parameters.Add("@cardc_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");
            //    //    oCmd1.Parameters.Add("@card_buysa", SqlDbType.Char).Value = reader["pay_buysa"].ToString();

            //    //    //카드매출
            //    //    oCmd1.Parameters.Add("@cardc_cardamt", SqlDbType.Decimal).Value = double.Parse(reader["cardamt"].ToString());
            //    //    oCmd1.Parameters.Add("@cardc_cardauthamt", SqlDbType.Decimal).Value = double.Parse(reader["cardauthamt"].ToString());
            //    //    oCmd1.Parameters.Add("@cardc_cardbanamt", SqlDbType.Decimal).Value = double.Parse(reader["cardbanamt"].ToString());

            //    //    //전표생성일
            //    //    oCmd1.Parameters.Add("@cardc_creatdate", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");

            //    //    oCmd1.ExecuteNonQuery();
            //    //    Application.DoEvents();
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    //마감은 다른 파일에 로그 저장
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    return -1;
            //}
            //finally
            //{
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, "STEP 3 OK");
            //}


            return 0;
        }

        private int Step4_InputMasterc()
        {

            try
            {
                string sSql = "SELECT " +
                                " a.gd_jum,a.gd_martcd,a.gd_date, " +
                                " IF(ISNULL(a.gd_gr_cd),'999',a.gd_gr_cd) AS gd_cd, " +
                                " IF(ISNULL(a.gd_gr_nm),'분류없음',a.gd_gr_nm) AS gd_nma,  " +
                                " gd_master_cd,gd_master_nm,  " +
                                " SUM(IF(b.pay_tp=1 ,a.gd_count,0)) AS cnt,  " +            //판매수량
                                " SUM(IF(b.pay_tp=5 ,a.gd_count,0)) AS bancnt,  " +           //취소 수량
                                " SUM(a.gd_count) AS totalcnt, " +
                                " SUM(IF(b.pay_tp=1 ,a.gd_total,0)) AS amt,  " +            //판매금액
                                " SUM(IF(b.pay_tp=5 ,a.gd_total,0)) AS banamt,  " +            //취소 금액
                                " SUM(a.gd_total) AS total  " +
                                 " FROM (SELECT * FROM pay_gd   " +
                                " WHERE gd_jum = ? AND gd_martcd = ? AND gd_date = ?  " +
                                " ) a INNER JOIN pay b ON a.gd_no = b.pay_no  " +
                                " WHERE b.pay_jum = ? AND b.pay_mart_cd = ? AND pay_pos_cd = ? AND b.pay_date = ?  " +
                                " GROUP BY a.gd_master_cd  " +
                                " ORDER BY a.gd_master_cd  ";

                //OdbcCommand oCmd = conn_local.CreateCommand();
                //oCmd.CommandText = sSql;

                //oCmd.Parameters.Add("@gd_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd.Parameters.Add("@gd_martcd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //oCmd.Parameters.Add("@gd_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

                //oCmd.Parameters.Add("@pay_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //oCmd.Parameters.Add("@pay_pos_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
                //oCmd.Parameters.Add("@pay_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");


                //OdbcDataReader reader = oCmd.ExecuteReader();

                ////기존 마감 자료 삭제
                //string sSql3 = "DELETE  " +
                //           " FROM end_masterc     " +
                //           " WHERE masterc_jum = ? AND masterc_mart_cd = ? AND masterc_pos_cd = ? AND  masterc_date = ? ";

                //OdbcCommand oCmd_DEL = conn.CreateCommand();
                //oCmd_DEL.CommandText = sSql3;

                //oCmd_DEL.Parameters.Add("@masterc_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd_DEL.Parameters.Add("@masterc_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                //oCmd_DEL.Parameters.Add("@masterc_pos_cd", SqlDbType.Char).Value =  clsPrintSet.SettingHT["POSNO"].ToString();
                //oCmd_DEL.Parameters.Add("@masterc_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");
                //oCmd_DEL.ExecuteNonQuery();

                //while (reader.Read())
                //{
                //    string sSql2 = "INSERT INTO end_masterc " +
                //                    "    (" +
                //                        "	masterc_jum, " +
                //                        "	masterc_mart_cd, " +
                //                        "	masterc_pos_cd, " +
                //                        "	masterc_date, " +
                //                        "	masterc_gr_cd, " +
                //                        "	masterc_gr_nm, " +
                //                        "	masterc_cd, " +
                //                        "	masterc_nm, " +
                //                        "	masterc_cnt, " +
                //                        "   masterc_bancnt," +
                //                        "   masterc_totcnt," +
                //                        "	masterc_amt, " +
                //                        "	masterc_banamt, " +
                //                        "	masterc_total, " +
                //                        "	masterc_createdate " +
                //                    "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";


                //    OdbcCommand oCmd1 = conn.CreateCommand();
                //    oCmd1.CommandText = sSql2;

                //    oCmd1.Parameters.Add("@masterc_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                //    oCmd1.Parameters.Add("@masterc_mart_cd", SqlDbType.Char).Value = oMart.sMart_cd;
                //    oCmd1.Parameters.Add("@masterc_pos_cd", SqlDbType.Char).Value =  clsPrintSet.SettingHT["POSNO"].ToString();
                //    oCmd1.Parameters.Add("@masterc_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

                //    //상품정보
                //    oCmd1.Parameters.Add("@masterc_gr_cd", SqlDbType.Char).Value = reader["gd_cd"].ToString();
                //    oCmd1.Parameters.Add("@masterc_gr_nm", SqlDbType.Char).Value = reader["gd_nma"].ToString();
                //    oCmd1.Parameters.Add("@masterc_cd", SqlDbType.Char).Value = reader["gd_master_cd"].ToString();
                //    oCmd1.Parameters.Add("@masterc_nm", SqlDbType.Char).Value = reader["gd_master_nm"].ToString();

                //    //수량
                //    oCmd1.Parameters.Add("@masterc_cnt", SqlDbType.Int).Value = Int32.Parse(reader["cnt"].ToString());
                //    oCmd1.Parameters.Add("@masterc_bancnt", SqlDbType.Int).Value = Int32.Parse(reader["bancnt"].ToString());
                //    oCmd1.Parameters.Add("@masterc_totcnt", SqlDbType.Int).Value = Int32.Parse(reader["totalcnt"].ToString());

                //    //판매금액
                //    oCmd1.Parameters.Add("@masterc_amt", SqlDbType.Decimal).Value = double.Parse(reader["amt"].ToString());
                //    oCmd1.Parameters.Add("@masterc_banamt", SqlDbType.Decimal).Value = double.Parse(reader["banamt"].ToString());
                //    oCmd1.Parameters.Add("@masterc_total", SqlDbType.Decimal).Value = double.Parse(reader["total"].ToString());

                //    //전표생성일
                //    oCmd1.Parameters.Add("@cardc_creatdate", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");

                //    oCmd1.ExecuteNonQuery();
                //    Application.DoEvents();
                //}

               
            }
            catch (Exception ex)
            {
                //마감은 다른 파일에 로그 저장
                oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return -1;
            }
            finally
            {
                oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, "STEP 4 OK");
            }


            return 0;
        }

        private int Step1_CheckOffline()
        {


            //try
            //{

            //    OdbcCommand oCmd = conn_local.CreateCommand();
            //    oCmd.CommandText = " SELECT * FROM pay  " +
            //             " WHERE pay_jum=? AND pay_syn  = 0 ORDER BY pay_no  ";

            //    oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
            //    OdbcDataReader reader = oCmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        string sDate = string.Format("{0:yyyy-MM-dd}", reader["pay_date"]);
            //        //전표의 상품 먼저 저장
            //        Save_PayGD(reader["pay_no"].ToString(), sDate);
            //        //전표 저장
            //        int iRet = Save_Pay(reader["pay_no"].ToString(), sDate);

            //        //sys = 1로 저장
            //        if (iRet == 0) //저장 성공시만 pay_syn = 1로 변경 실패 하면 그대로 두고 다음데 다시 저장 시도 한다. repalce 사용 하기 때문에 중복 저장 않됨
            //        {
            //            oFile.WriteLog(MethodBase.GetCurrentMethod().Name, sDate + "-" + reader["pay_no"].ToString());
            //            OdbcCommand oCmd_check = conn_local.CreateCommand();
            //            oCmd_check.CommandText = " UPDATE pay  SET pay_syn = 1 " +
            //                     " WHERE pay_jum = ?  " +
            //                     " AND pay_no = ?  " +
            //                     " AND pay_date = ?  ";
            //            oCmd_check.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
            //            oCmd_check.Parameters.Add("@pay_no", SqlDbType.Char).Value = reader["pay_no"].ToString();
            //            oCmd_check.Parameters.Add("@pay_date", SqlDbType.Char).Value = sDate;
            //            oCmd_check.ExecuteNonQuery();
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    //마감은 다른 파일에 로그 저장
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    return -1;
            //}
            //finally
            //{
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, "STEP 1 OK");
            //}
            return 0;
        }

        private int Save_PayGD(string sNo, string date)
        {
            //try
            //{
            //    OdbcCommand oCmd = conn_local.CreateCommand();
            //    oCmd.CommandText = " SELECT * FROM pay_gd  " +
            //             " WHERE gd_jum =? AND gd_no = ? AND gd_date = ? ORDER BY gd_seq ";
            //    oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = sNo;
            //    oCmd.Parameters.Add("@date", SqlDbType.Char).Value = date;

            //    OdbcDataReader reader = oCmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        string sSql = "REPLACE INTO pay_gd " +
            //                "    (" +
            //                "    gd_jum," +
            //                "    gd_martcd, " +
            //                "    gd_pos_cd, " +
            //                "    gd_date, " +
            //                "    gd_time, " +
            //                "    gd_no, " +
            //                "    gd_tp, " +
            //                "    gd_seq, " +
            //                "    gd_gr_cd, " +
            //                "    gd_gr_nm, " +
            //                "    gd_master_cd, " +
            //                "    gd_master_nm, " +
            //                "    gd_mem_cd, " +
            //                "    gd_count, " +
            //                "    gd_cost," +
            //                "    gd_total" +
            //                "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";


            //        OdbcCommand oCmd_Ser = conn.CreateCommand();
            //        oCmd_Ser.CommandText = sSql;

            //        oCmd_Ser.Parameters.Add("@gd_jum", SqlDbType.Char).Value = reader["gd_jum"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_martcd", SqlDbType.Char).Value = reader["gd_martcd"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_pos_cd", SqlDbType.Char).Value = reader["gd_pos_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_date", SqlDbType.Date).Value = string.Format("{0:yyyy-MM-dd}", reader["gd_date"]); // reader["gd_date"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_time", SqlDbType.Char).Value = reader["gd_time"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_no", SqlDbType.Int).Value = reader["gd_no"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_tp", SqlDbType.Char).Value = reader["gd_tp"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_seq", SqlDbType.Int).Value = reader["gd_seq"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_gr_cd", SqlDbType.Char).Value = reader["gd_gr_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_gr_nm", SqlDbType.Char).Value = reader["gd_gr_nm"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_master_cd", SqlDbType.Char).Value = reader["gd_master_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_master_nm", SqlDbType.Char).Value = reader["gd_master_nm"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_mem_cd", SqlDbType.Char).Value = reader["gd_mem_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_count", SqlDbType.Int).Value = reader["gd_count"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_cost", SqlDbType.Int).Value = reader["gd_cost"].ToString();
            //        oCmd_Ser.Parameters.Add("@gd_total", SqlDbType.Int).Value = reader["gd_total"].ToString();
            //        oCmd_Ser.ExecuteNonQuery();

            //    }
            //    reader.Close();
            //}
            //catch (Exception ex)
            //{
            //    oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    return -1;
            //}
            return 0;
        }

        private int Save_Pay(string sNo, string date)
        {
            //try
            //{
            //    OdbcCommand oCmd = conn_local.CreateCommand();
            //    oCmd.CommandText = " SELECT * FROM pay  " +
            //             " WHERE pay_jum = ? AND pay_no = ? AND pay_date = ? ";
            //    oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = sNo;
            //    oCmd.Parameters.Add("@date", SqlDbType.Char).Value = date;
            //    OdbcDataReader reader = oCmd.ExecuteReader();



            //    while (reader.Read())
            //    {
            //        string sSql = "REPLACE INTO pay " +
            //                "    (" +
            //                "    pay_jum," +
            //                "    pay_mart_cd, " +   //푸드트럭코드
            //                "    pay_pos_cd, " +
            //                "    pay_date, " +
            //                "    pay_time, " +
            //                "    pay_no, " +    //주문순번(하루 기준으로 1로 시작해야됨)
            //                "    pay_syn, " +    //온라인,오프라인
            //                "    pay_tp, " +     //1결제 5취소
            //                "    pay_mem_cd, " +   //회원 코드
            //                "    pay_count, " +    //상품가지수
            //                "    pay_total, " +    //총합(결제 총금액)
            //                "    pay_vat, " +    //부가세
            //                "    pay_amt, " +    //부가세 제외 총금액
            //                "    pay_buysa, " +    //매입사
            //                "    pay_cardsa," +    //카드사
            //                "    pay_getid," +    //신용카드 결제
            //                "    pay_van," +       //밴
            //                "    pay_pad," +       //사인패드
            //                "    pay_mon," +      //할부
            //                "    pay_cartno," +     //카드번호
            //                "    pay_agent," +     //가맹점번호
            //                "    pay_auth," +      //승인번호
            //                "    pay_cashno," +     //현금영수증
            //                "    pay_card," +      //카드 금액
            //                "    pay_cash," +      //현금 금액
            //                "    pay_tid," +       //다날 결제
            //                "    pay_order," +      //주방 주문확인 0:확인전 1: 확인
            //                "    pay_mart_ocd," +    //여기서부터 취소
            //                "    pay_ono," +
            //                "    pay_odate," +
            //                "    pay_kind" +
            //                "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";




            //        OdbcCommand oCmd_Ser = conn.CreateCommand();
            //        oCmd_Ser.CommandText = sSql;
            //        oCmd_Ser.Parameters.Add("@pay_jum", SqlDbType.Char).Value = reader["pay_jum"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = reader["pay_mart_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_pos_cd", SqlDbType.Char).Value = reader["pay_pos_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_date", SqlDbType.Date).Value = string.Format("{0:yyyy-MM-dd}", reader["pay_date"]);  //reader["pay_date"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_time", SqlDbType.Char).Value = reader["pay_time"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_no", SqlDbType.Int).Value = reader["pay_no"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_syn", SqlDbType.Char).Value = "1";
            //        oCmd_Ser.Parameters.Add("@pay_tp", SqlDbType.Char).Value = reader["pay_tp"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_mem_cd", SqlDbType.Char).Value = reader["pay_mem_cd"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_count", SqlDbType.Int).Value = reader["pay_count"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_total", SqlDbType.Int).Value = reader["pay_total"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_vat", SqlDbType.Int).Value = reader["pay_vat"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_amt", SqlDbType.Int).Value = reader["pay_amt"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_buysa", SqlDbType.Char).Value = reader["pay_buysa"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_cardsa", SqlDbType.Char).Value = reader["pay_cardsa"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_getid", SqlDbType.Char).Value = reader["pay_getid"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_van", SqlDbType.Char).Value = reader["pay_van"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_pad", SqlDbType.Char).Value = reader["pay_pad"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_mon", SqlDbType.Char).Value = reader["pay_mon"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_cartno", SqlDbType.Char).Value = reader["pay_cartno"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_agent", SqlDbType.Char).Value = reader["pay_agent"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_auth", SqlDbType.Char).Value = reader["pay_auth"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_cashno", SqlDbType.Char).Value = reader["pay_cashno"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_card", SqlDbType.Int).Value = reader["pay_card"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_cash", SqlDbType.Int).Value = reader["pay_cash"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_tid", SqlDbType.Char).Value = reader["pay_tid"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_order", SqlDbType.Char).Value = reader["pay_order"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_mart_ocd", SqlDbType.Char).Value = reader["pay_mart_ocd"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_ono", SqlDbType.Int).Value = reader["pay_ono"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_odate", SqlDbType.Date).Value = string.Format("{0:yyyy-MM-dd}", reader["pay_odate"]);  // reader["pay_odate"].ToString();
            //        oCmd_Ser.Parameters.Add("@pay_kind", SqlDbType.Date).Value = reader["pay_kind"].ToString();

            //        oCmd_Ser.ExecuteNonQuery();

            //    }
            //    reader.Close();
            //}
            //catch (Exception ex)
            //{
            //    oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    return -1;
            //}
            return 0;
        }

        private int Print_Result()
        {
            //// TODO 매출 관련 변수 화 하여 매출 - 취소 - 쿠폰 등 계산할것.
            //SerialPort port = new SerialPort(clsPrintSet.SettingHT["COMP"].ToString(), Int32.Parse(clsPrintSet.SettingHT["SPEED"].ToString()), Parity.None, 8, StopBits.One);

            //int iEmpty = Int32.Parse(clsPrintSet.SettingHT["EMPTY"].ToString());
            //int iSep = Int32.Parse(clsPrintSet.SettingHT["SEP"].ToString());
            ////string COMMAND = ESC + "@" + GS + "V" + (char)1;
            //try
            //{

            //    port.Encoding = Encoding.Default;
            //    port.Open();

            //    port.WriteLine(clsPrintSet.Cmd_DrawOpen());
            //    port.WriteLine(clsPrintSet.Cmd_Close());


            //    port.WriteLine(clsPrintSet.Cmd_Font1());
                
            //    port.WriteLine("매  장  명 : " + oMart.sMart_nm);
                
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));

                

            //    port.WriteLine(clsPrintSet.Cmd_Close());

            //    port.WriteLine("담 당 자 : " + oMemad.memad_nm);
            //    port.WriteLine("정 산 일시 : " + DateTime.Now.ToString("yyyy-MM-dd (ddd) HH:mm:ss"));


            //    OdbcCommand oCmd = conn.CreateCommand();
            //    oCmd.CommandText = "SELECT *  " +
            //               " FROM end_salec     " +
            //               " WHERE salec_jum = ? AND salec_mart_cd = ? AND  salec_date = ? ";
            //    oCmd.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd.Parameters.Add("@salec_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    oCmd.Parameters.Add("@salec_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

            //    OdbcDataReader reader = oCmd.ExecuteReader();


            //    if (reader.Read())
            //    {

            //        port.WriteLine("고 객 수");
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //        port.WriteLine("구 매 고 객 수 : " + reader["salec_salecnt"].ToString() + " 명");
            //        port.WriteLine("취 소 고 객 수 : " + reader["salec_bancnt"].ToString() + " 명");
            //        port.WriteLine("합          계 : " + reader["salec_totalcnt"].ToString() + " 명");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine("매 출 액");
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //        port.WriteLine("구 매 금 액:" + string.Empty.PadLeft(iEmpty - reader["salec_authamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_authamt"].ToString())) + " 원");
            //        port.WriteLine("취 소 금 액:" + string.Empty.PadLeft(iEmpty - reader["salec_banamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_banamt"].ToString())) + " 원");
            //        port.WriteLine("합 계 금 액:" + string.Empty.PadLeft(iEmpty - reader["salec_totalamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_totalamt"].ToString())) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine("객  단  가 :" + string.Empty.PadLeft(iEmpty - reader["salec_cusavg"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cusavg"].ToString())) + " 원");

            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));


            //        //총매출 = 총금액 - 쿠폰금액
            //        port.WriteLine("실 매 출 액(매출액-이벤트");
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));
            //        int iAuth = Int32.Parse(reader["salec_authamt"].ToString()) - Int32.Parse(reader["salec_couponamt"].ToString());
            //        port.WriteLine("실매출액 :" + string.Empty.PadLeft(iEmpty - iAuth.ToString().Length, ' ') + string.Format("{0:#,##0}", iAuth) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                    

                        



            //        port.WriteLine("결제수단별");
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));
            //        port.WriteLine("신 용 카 드(+)");
            //        port.WriteLine("승 인:" + string.Empty.PadLeft(iEmpty - reader["salec_cardauthamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cardauthamt"].ToString())) + " 원");
            //        port.WriteLine("취 소:" + string.Empty.PadLeft(iEmpty - reader["salec_cardbanamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cardbanamt"].ToString())) + " 원");
            //        port.WriteLine("합 계:" + string.Empty.PadLeft(iEmpty - reader["salec_cardamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cardamt"].ToString())) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));



            //        port.WriteLine("현금총계(A+B)(+)");
            //        port.WriteLine("승 인:" + string.Empty.PadLeft(iEmpty - reader["salec_cashauthamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashauthamt"].ToString()) + Int32.Parse(reader["salec_cashauthamt_noauth"].ToString())) + " 원");
            //        port.WriteLine("취 소:" + string.Empty.PadLeft(iEmpty - reader["salec_cashbanamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashbanamt"].ToString()) + Int32.Parse(reader["salec_cashbanamt_noauth"].ToString())) + " 원");
            //        port.WriteLine("합 계:" + string.Empty.PadLeft(iEmpty - reader["salec_cashamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashamt"].ToString()) + Int32.Parse(reader["salec_cashamt_noauth"].ToString())) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));


            //        port.WriteLine("현 금 영 수 증(A)");
            //        port.WriteLine("승 인:" + string.Empty.PadLeft(iEmpty - reader["salec_cashauthamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashauthamt"].ToString())) + " 원");
            //        port.WriteLine("취 소:" + string.Empty.PadLeft(iEmpty - reader["salec_cashbanamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashbanamt"].ToString())) + " 원");
            //        port.WriteLine("합 계:" + string.Empty.PadLeft(iEmpty - reader["salec_cashamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashamt"].ToString())) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));


            //        port.WriteLine("현 금(B)");
            //        port.WriteLine("승 인:" + string.Empty.PadLeft(iEmpty - reader["salec_cashauthamt_noauth"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashauthamt_noauth"].ToString())) + " 원");
            //        port.WriteLine("취 소:" + string.Empty.PadLeft(iEmpty - reader["salec_cashbanamt_noauth"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashbanamt_noauth"].ToString())) + " 원");
            //        port.WriteLine("합 계:" + string.Empty.PadLeft(iEmpty - reader["salec_cashamt_noauth"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_cashamt_noauth"].ToString())) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));


            //        port.WriteLine("쿠 폰 및 이벤트(-)");
            //        port.WriteLine("승 인:" + string.Empty.PadLeft(iEmpty - reader["salec_couponauthamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_couponauthamt"].ToString())) + " 원");
            //        port.WriteLine("취 소:" + string.Empty.PadLeft(iEmpty - reader["salec_couponbamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_couponbamt"].ToString())) + " 원");
            //        port.WriteLine("합 계:" + string.Empty.PadLeft(iEmpty - reader["salec_couponamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader["salec_couponamt"].ToString())) + " 원");
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //        port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    }

            //    reader.Close();

            //    //카드사별 매출 출력 안함
            //    //port.WriteLine("카 드 사 별 매 출");
            //    //port.WriteLine(string.Empty.PadLeft(iSep, '='));
            //    ////카드사별 매출
            //    //OdbcCommand oCmd2 = conn.CreateCommand();
            //    //oCmd2.CommandText = "SELECT *  " +
            //    //           " FROM end_cardc     " +
            //    //           " WHERE cardc_jum = ? AND cardc_mart_cd = ? AND  cardc_date = ? ";
            //    //oCmd2.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    //oCmd2.Parameters.Add("@salec_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    //oCmd2.Parameters.Add("@salec_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

            //    //OdbcDataReader reader2 = oCmd2.ExecuteReader();

            //    //while (reader2.Read())
            //    //{
            //    //    port.WriteLine(reader2["cardc_buysa"].ToString());
            //    //    port.WriteLine("승 인:" + string.Empty.PadLeft(iEmpty - reader2["cardc_authamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader2["cardc_authamt"].ToString())) + " 원");
            //    //    port.WriteLine("취 소:" + string.Empty.PadLeft(iEmpty - reader2["cardc_banamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader2["cardc_banamt"].ToString())) + " 원");
            //    //    port.WriteLine("합 계:" + string.Empty.PadLeft(iEmpty - reader2["cardc_amt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader2["cardc_amt"].ToString())) + " 원");
            //    //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    //}

            //    //reader2.Close();
            //    //port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    //port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    //port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));


            //    port.WriteLine("메 뉴 별 매 출");
            //    port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //    OdbcCommand oCmd3 = conn.CreateCommand();
            //    oCmd3.CommandText = "SELECT *  " +
            //               " FROM end_masterc     " +
            //               " WHERE masterc_jum = ? AND masterc_mart_cd = ? AND  masterc_date = ? ORDER BY masterc_cnt DESC ";
            //    oCmd3.Parameters.Add("@masterc_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd3.Parameters.Add("@masterc_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    oCmd3.Parameters.Add("@masterc_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

            //    OdbcDataReader reader3 = oCmd3.ExecuteReader();

            //    int i = 10;
            //    int y = 18;

            //    while (reader3.Read())
            //    {
            //        port.WriteLine(reader3["masterc_cd"].ToString() + "-" + reader3["masterc_nm"].ToString());
            //        port.WriteLine("구 매:" +
            //            string.Empty.PadLeft(i - reader3["masterc_cnt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader3["masterc_cnt"].ToString())) + " 개" +
            //            string.Empty.PadLeft(y - reader3["masterc_amt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader3["masterc_amt"].ToString())) + " 원");
            //        port.WriteLine("취 소:" +
            //            string.Empty.PadLeft(i - reader3["masterc_bancnt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader3["masterc_bancnt"].ToString())) + " 개" +
            //            string.Empty.PadLeft(y - reader3["masterc_banamt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader3["masterc_banamt"].ToString())) + " 원");
            //        port.WriteLine("합 계:" +
            //            string.Empty.PadLeft(i - reader3["masterc_totcnt"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader3["masterc_totcnt"].ToString())) + " 개" +
            //            string.Empty.PadLeft(y - reader3["masterc_total"].ToString().Length, ' ') + string.Format("{0:#,##0}", Int32.Parse(reader3["masterc_total"].ToString())) + " 원");

            //        port.WriteLine(string.Empty.PadLeft(iSep, '-'));

            //    }
            //    reader3.Close();



            //    port.WriteLine("시 재 금");
            //    port.WriteLine(string.Empty.PadLeft(iSep, '='));
            //    OdbcCommand oCmd4 = conn_local.CreateCommand();
            //    oCmd4.CommandText = "SELECT *  " +
            //               " FROM bread_kick     " +
            //               " WHERE kick_jum = ? AND kick_mart = ? AND kick_pos = ? AND  kick_date = ? ";
            //    oCmd4.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd4.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    oCmd4.Parameters.Add("@salec_mart_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
            //    oCmd4.Parameters.Add("@salec_date", SqlDbType.Date).Value = txt_Date2.Value.ToString("yyyy-MM-dd");

            //    OdbcDataReader reader4 = oCmd4.ExecuteReader();


            //    if (reader4.Read())
            //    {

            //        double dAmt = double.Parse(reader4["Kick_50Thou"].ToString()) * 50000;
            //        port.WriteLine("오만원권 : " + reader4["Kick_50Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));


            //        dAmt = double.Parse(reader4["Kick_10Thou"].ToString()) * 10000;
            //        port.WriteLine("만 원 권 : " + reader4["Kick_10Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));


            //        dAmt = double.Parse(reader4["Kick_5Thou"].ToString()) * 5000;
            //        port.WriteLine("오천원권 : " + reader4["Kick_5Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

            //        dAmt = double.Parse(reader4["Kick_1Thou"].ToString()) * 1000;
            //        port.WriteLine("천 원 권 : " + reader4["Kick_1Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

            //        dAmt = double.Parse(reader4["Kick_5Hund"].ToString()) * 500;
            //        port.WriteLine("오백원권 : " + reader4["Kick_5Hund"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

            //        dAmt = double.Parse(reader4["Kick_1Hund"].ToString()) * 100;
            //        port.WriteLine("백 원 권 : " + reader4["Kick_1Hund"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));
            //        dAmt = double.Parse(reader4["Kick_50"].ToString()) * 50;
            //        port.WriteLine("오십원권 : " + reader4["Kick_50"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

            //        dAmt = double.Parse(reader4["Kick_10"].ToString()) * 10;
            //        port.WriteLine("십 원 권 : " + reader4["Kick_10"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));



            //        port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //        port.WriteLine("합     계 : " + "".PadRight(5) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(reader4["Kick_total"].ToString())) + string.Format("{0:#,##0}", Int32.Parse(reader4["Kick_total"].ToString())));

            //    }

            //    reader.Close();



            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(clsPrintSet.Cmd_Cutting());
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));


            //}
            //catch (Exception ex)
            //{
            //    //마감은 다른 파일에 로그 저장
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    port.Close();
            //    return -1;
            //}
            //finally
            //{
                
            //    oFile.WriteFile(clsPrintSet.END_LOGFILE(), MethodBase.GetCurrentMethod().Name, "PRINT OK");
            //    if (port.IsOpen == true)
            //        port.Close();

            //}
            return 0;

        }
        #endregion

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //객수 초기화
            Panel_CloseAll();
            pan_Config.Visible = true;
            pan_Config.Dock = DockStyle.Fill;


            try
            {
                this.txt_AccCustomCnt.Text = clsPrintSet.SettingHT["ACCCNT"].ToString(); // conHt["COMP"].ToString();
                this.txt_Count.Text = clsPrintSet.SettingHT["ACCUNIT"].ToString(); // conHt["COMP"].ToString();
                txt_Screen.Text = clsPrintSet.SettingHT["SCREEN"].ToString();

                if (clsPrintSet.SettingHT["DUAL"].ToString() == "1")
                    chk_UseDual.Checked = true;
                else
                    chk_UseDual.Checked = false;


            }
             catch (Exception ex)
            {
                this.txt_AccCustomCnt.Text = "0";
                this.txt_Count.Text = "0";
                txt_Screen.Text = "0";
                chk_UseDual.Checked = false;

            }
        }

        private void btn_Init_Click(object sender, EventArgs e)
        {
            oFile.Check_PutSetting("APP", "ACCCNT","0");
            txt_AccCustomCnt.Text = "0";

        }

        private void btn_CloseConfig_Click(object sender, EventArgs e)
        {
            Panel_CloseAll();

            pan_Calendar.Visible = true;
            pan_Calendar.Dock = DockStyle.Fill;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown", "/s /f /t 30");

        }

        private void btn_SaleCRe_Click(object sender, EventArgs e)
        {
            Print_Result();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            frmStock oStock = new frmStock(oMemad,oMart );
            oStock.ShowDialog();

        }

        private void btn_SaveConfig_Click(object sender, EventArgs e)
        {   
            oFile.Check_PutSetting("APP", "ACCUNIT", txt_Count.Text);
            oFile.Check_PutSetting("VPOS", "SCREEN", txt_Screen.Text);

            if (chk_UseDual.Checked == true)
            {
                oFile.Check_PutSetting("APP", "DUAL", "1");

                if(oCustom == null)
                    Show_CustomDisplay();
            }
            else
            {
                oFile.Check_PutSetting("APP", "DUAL", "0");
                if (oCustom != null)
                {
                    oCustom.Shut_Down();
                    oCustom = null;
                }
            }
            oFile.Check_PutSetting("APP", "SOUNDCAL", comboBox_listen.Text);
        }


        private void Show_CustomDisplay()
        {

            //디스플레이폼 중복 방지
            foreach (Form  oFrm in Application.OpenForms)
            {
                if (oFrm.Name == "frmCustomDisplay")
                    return;
                    
            }

            try
            {

                Screen[] screens = Screen.AllScreens;
                int iScreenIdx = 0;
                if (screens.Length > 1)     //모니터가 복수 일때
                {

                    oCustom = new frmCustomDisplay();
                    if (int.TryParse(clsPrintSet.SettingHT["SCREEN"].ToString(), out iScreenIdx))
                    {
                        if (screens.Length < iScreenIdx - 1)
                        {
                            oCustom.Location = screens[0].Bounds.Location;  //new System.Drawing.Point(scrn.Bounds.Left, 0);
                        }
                        else
                        {
                            oCustom.Location = screens[iScreenIdx].Bounds.Location;  //new System.Drawing.Point(scrn.Bounds.Left, 0);

                        }
                    }
                    else
                    {
                        oCustom.Location = screens[1].Bounds.Location;
                    }
                    oCustom.WindowState = FormWindowState.Maximized;
                    oCustom.Show();

                    oCustom.Start_Video();
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        private void DB_MODIFY()
        {
            //try
            //{
            //    string gsSql = "CREATE TABLE IF NOT EXISTS bread_DrawOpen (" +
            //                  "DrawOpen_idx int NOT NULL AUTO_INCREMENT ," +
            //                  "DrawOpen_mem_cd varchar(5) NOT NULL ," +
            //                  "DrawOpen_mem_nm varchar(30) NOT NULL ," +
            //                  "DrawOpen_date char(20) NOT NULL ," +
            //                  "DrawOpen_time char(20) NOT NULL ," +
            //                  "PRIMARY KEY (DrawOpen_idx) " +
            //                ") ";


            //    OdbcCommand oCmd1 = conn_local.CreateCommand();
            //    oCmd1.CommandText = gsSql;
            //    oCmd1.ExecuteNonQuery();
            //}
            // catch (Exception ex)
            //{
            //    oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            //}

 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_listen_Click(object sender, EventArgs e)
        {
            clsInfoSound.Sound_ON(Application.StartupPath, Int32.Parse(comboBox_listen.Text));
        }

    


    }
}

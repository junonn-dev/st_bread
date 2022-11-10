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
using System.Runtime.InteropServices;
using System.IO.Ports;


namespace st_bread
{
    public partial class frmCancel: Form
    {
        #region DLL Import
        [DllImport("C:\\KOVAN\\VPOS_Client.dll")]
        public static extern int Kovan_Auth(
            String tcode,
            String Tid,
            String halbu,
            String tamt,
            String ori_date,
            String ori_authno,
            String tran_serial,
            String idno,
            String amt_flag,
            String tax_amt,
            String sfee_amt,
            String free_amt,
            String filler,
            byte[] rTranType,
            byte[] rErrCode,
            byte[] rCardno,
            byte[] rHalbu,
            byte[] rTamt,
            byte[] rTranDate,
            byte[] rTranTime,
            byte[] rAuthNo,
            byte[] rMerNo,
            byte[] rTranSerial,
            byte[] rIssueCard,
            byte[] rPurchaseCard,
            byte[] rSignPath,
            byte[] rMsg1,
            byte[] rMsg2,
            byte[] rMsg3,
            byte[] rMsg4,
            byte[] rFiller);

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);


        #endregion

        clsMemad oMemad = null;
        
        clsMart oMart = null;
        clsPay oPay = null;


        OdbcConnection conn = new OdbcConnection();
        OdbcConnection conn_local = new OdbcConnection();
        clsDbCon vDbCon = new clsDbCon();
        clsFile oFile = new clsFile();

        //clsRecipInfo oRecip = new clsRecipInfo();
        bool bIsReprint = false; //영수증 재발행 시 true
        bool bIsReAutCash = false; //현금영수증 재발생 시 true;

        frmCustomDisplay oCustom = null;

        public frmCancel(clsMemad o_Memad, clsMart o_Mart, frmCustomDisplay o_Custom)
        {
            InitializeComponent();
            
            oMemad = o_Memad;
            oMart = o_Mart;
            oCustom = o_Custom;
        }


        public frmCancel(clsMemad o_Memad, clsMart o_Mart, int iRe, frmCustomDisplay o_Custom)
        {
            InitializeComponent();
        
            oMemad = o_Memad;
            oMart = o_Mart;
            oCustom = o_Custom;


            if (iRe == 1)
                bIsReprint = true;
            else if (iRe == 2)
                bIsReAutCash = true;
        }


        private void btn_Pre_Click(object sender, EventArgs e)
        {
            try
            {

                ////요식업 화면으로 
                frmOrder_Type2 newForm = new frmOrder_Type2( );

                newForm.StartPosition = FormStartPosition.Manual;
                newForm.Location = new Point(100, 100);
                newForm.Show();
                Program.ac.MainForm = newForm;
                this.Close();

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        private void frmCancel_Load(object sender, EventArgs e)
        {
            try
            {

                conn = vDbCon.MysqlOdbcConnect(
                     clsPrintSet.SettingHT["IP"].ToString(),
                     clsPrintSet.SettingHT["DBNAME"].ToString(),
                     clsPrintSet.SettingHT["USER"].ToString(),
                     clsPrintSet.SettingHT["PASS"].ToString(),
                     clsPrintSet.SettingHT["PORT"].ToString()
                     );

                conn_local = vDbCon.MysqlOdbcConnect(
                        clsPrintSet.SettingHT["IP2"].ToString(),
                        clsPrintSet.SettingHT["DBNAME2"].ToString(),
                        clsPrintSet.SettingHT["USER2"].ToString(),
                        clsPrintSet.SettingHT["PASS2"].ToString(),
                        clsPrintSet.SettingHT["PORT2"].ToString()
                        );

                DateTime result = DateTime.Today;
                Date_Picker.Value = result;
                if (conn.State == ConnectionState.Open)
                {
                   

                }
                else if (conn_local.State == ConnectionState.Open)
                {
                    //Get_PayList(Date_Picker.Value.ToString("yyyy-MM-dd"));
                   
                }

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

            lst_Pay.Columns[0].Width = 180;
            lst_Pay.Columns[1].Width = 170;
            lst_Pay.Columns[2].Width = 150;
            lst_Pay.Columns[3].Width = 150;
            lst_Pay.Columns[4].Width = 150;
            lst_Pay.Columns[5].Width = 180;
            lst_Pay.Location = new Point(10, 70);
            lst_Pay.Size = new Size(1000, 571);
            

            if (bIsReprint == true)
            {

                btn_RePrint.Visible = true;
                btn_RePrint.Location = new Point(688, 654);
                btn_ReAuthCash.Visible = false;
                btn_Exit.Visible = false;
            }
            else if (bIsReAutCash == true)
            {
                btn_ReAuthCash.Visible = true;
                btn_ReAuthCash.Location = new Point(688, 654);

                btn_RePrint.Visible = false;
                btn_Exit.Visible = false;
 
            }
            else
            {
                btn_ReAuthCash.Visible = false;
                btn_RePrint.Visible = false;
                btn_Exit.Visible = true;
            }

        }

        private void Date_Picker_ValueChanged(object sender, EventArgs e)
        {
            //전표 찾아오기
            string sDate = Date_Picker.Value.ToString("yyyy-MM-dd");

            Get_PayList(sDate);
            
        }


       

        private void mnu_select_Click(object sender, EventArgs e)
        {

            if (bIsReAutCash == true)
            {

                //"6"

                Console.WriteLine(lst_Pay.SelectedItems[0].SubItems[4].Text + "-" + lst_Pay.SelectedItems[0].SubItems[6].Text);


                if (lst_Pay.SelectedItems[0].SubItems[6].Text == "1")
                {
                    frmMessage oMessage2 = new frmMessage(1);
                    oMessage2.Set_Message(10);
                    oMessage2.ShowDialog();
                    return;
                }
                if (lst_Pay.SelectedItems[0].SubItems[6].Text == "0")
                {
                    frmMessage oMessage2 = new frmMessage(1);
                    oMessage2.Set_Message(13);
                    oMessage2.ShowDialog();
                    return;
 
                }

            }
            else
            {
                if (lst_Pay.SelectedItems[0].SubItems[5].Text == "취 소")
                {
                    frmMessage oMessage2 = new frmMessage(1);
                    oMessage2.Set_Message(8);
                    oMessage2.ShowDialog();
                    return;
                }
            }

            try
            {
                lst_Pay.Visible = false;


                oPay = null; //기존 전표 초기화
                OdbcCommand oCmd = conn_local.CreateCommand();
                oCmd.CommandText = " SELECT * FROM pay" +
                                    " WHERE pay_jum  = ? AND pay_mart_cd = ? AND pay_date = ? AND pay_no = ? ";
                oCmd.Parameters.Add("@jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd.Parameters.Add("@date", SqlDbType.Char).Value = lst_Pay.SelectedItems[0].Text;
                oCmd.Parameters.Add("@no", SqlDbType.Char).Value = lst_Pay.SelectedItems[0].SubItems[2].Text;

                OdbcDataReader reader = oCmd.ExecuteReader();

                if (reader.Read())
                {

                    oPay = new clsPay();
                    oPay.sPayjum = reader["pay_jum"].ToString();
                    oPay.sPayMartcd = reader["pay_mart_cd"].ToString();

                    DateTime dt = Convert.ToDateTime(reader["pay_date"].ToString());
                    oPay.sPaydate = dt.ToString("yyyy-MM-dd");


                    oPay.sPaytime = reader["pay_time"].ToString();
                    oPay.sPayno = reader["pay_no"].ToString();
                    oPay.sPaysyn = reader["pay_syn"].ToString();
                    oPay.sPaytp = reader["pay_tp"].ToString();
                    oPay.iPaycount = Int32.Parse(reader["pay_count"].ToString());
                    oPay.dPaytotal = double.Parse(reader["pay_total"].ToString());
                    oPay.dPayvat = double.Parse(reader["pay_vat"].ToString());
                    oPay.dPayamt = double.Parse(reader["pay_amt"].ToString());
                    oPay.sPaybuysa = reader["pay_buysa"].ToString();
                    oPay.sPaycardsa = reader["pay_cardsa"].ToString();
                    oPay.sPaygetid = reader["pay_getid"].ToString();
                    oPay.sPayvan = reader["pay_van"].ToString();
                    oPay.sPaypad = reader["pay_pad"].ToString();
                    oPay.sPaymon = reader["pay_mon"].ToString();
                    oPay.sPaycartno = reader["pay_cartno"].ToString();
                    oPay.sPayagent = reader["pay_agent"].ToString();
                    oPay.sPayauth = reader["pay_auth"].ToString();
                    oPay.sPaycashno = reader["pay_cashno"].ToString();
                    oPay.dPaycard = double.Parse(reader["pay_card"].ToString());
                    oPay.dPaycash = double.Parse(reader["pay_cash"].ToString());
                    oPay.sPaytid = reader["pay_tid"].ToString();
                    oPay.sPayorder = reader["pay_order"].ToString();
                    oPay.sPaymartocd = reader["pay_mart_ocd"].ToString();
                    oPay.sPayono = reader["pay_ono"].ToString();
                    oPay.sPayodate = reader["pay_odate"].ToString();
                    oPay.sPaykind = reader["pay_kind"].ToString();

                    oPay.Set_VatAmt(double.Parse(reader["pay_total"].ToString()));


                }
                reader.Close();

                OdbcCommand oCmd2 = conn_local.CreateCommand();
                oCmd2.CommandText = " SELECT * FROM pay_gd" +
                                   " WHERE gd_jum  = ? AND gd_martcd = ? AND gd_date = ? AND gd_no = ? ";
                oCmd2.Parameters.Add("@jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd2.Parameters.Add("@mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd2.Parameters.Add("@date", SqlDbType.Char).Value = lst_Pay.SelectedItems[0].Text;
                oCmd2.Parameters.Add("@no", SqlDbType.Char).Value = lst_Pay.SelectedItems[0].SubItems[2].Text;

                OdbcDataReader readergd = oCmd2.ExecuteReader();
                lst_.Items.Clear();
                int i = 0;
                while (readergd.Read())
                {
                    i++;
                    ListViewItem lstItm = new ListViewItem();

                    lstItm.Text = i.ToString();
                    lstItm.SubItems.Add(readergd["gd_master_nm"].ToString());
                    lstItm.SubItems.Add(readergd["gd_count"].ToString());
                    lstItm.SubItems.Add(string.Format("{0:#,##0}", Int32.Parse(readergd["gd_cost"].ToString())));
                    lstItm.SubItems.Add(string.Format("{0:#,##0}", Int32.Parse(readergd["gd_total"].ToString())));
                    lstItm.SubItems.Add(readergd["gd_master_cd"].ToString());
                    lstItm.SubItems.Add(readergd["gd_gr_cd"].ToString());
                    lst_.Items.Add(lstItm);

                    //oCmd.Parameters.Add("@gd_gr_cd", SqlDbType.Char).Value = item.SubItems[7].Text;
                    //      oCmd.Parameters.Add("@gd_gr_nm", SqlDbType.Char).Value = "test";// Get_GrNM(item.SubItems[7].Text);
                    //      oCmd.Parameters.Add("@gd_master_cd", SqlDbType.Char).Value = item.SubItems[5].Text;
                    //      oCmd.Parameters.Add("@gd_master_nm", SqlDbType.Char).Value = item.SubItems[1].Text;
                    //      oCmd.Parameters.Add("@gd_mem_cd", SqlDbType.Char).Value = "0";
                    //      oCmd.Parameters.Add("@gd_count", SqlDbType.Int).Value = Int32.Parse(item.SubItems[2].Text);
                    //      oCmd.Parameters.Add("@gd_cost", SqlDbType.Int).Value = double.Parse(item.SubItems[3].Text);
                    //      oCmd.Parameters.Add("@gd_total", SqlDbType.Int).Value = double.Parse(item.SubItems[4].Text);
                }
                readergd.Close();


                oFile.Set_ResultPage(Path.Combine(Application.StartupPath, "orderResultView2.html"), lst_, oPay);
                web.Navigate(Path.Combine(Application.StartupPath, "orderResultView2.html"));

            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        private void pic_Payno_Click(object sender, EventArgs e)
        {
            string sDate = Date_Picker.Value.ToString("yyyy-MM-dd");

            Get_PayList(sDate);

        }


        //현재 날짜의 전표 목록
        private void Get_PayList(string sDate)
        {
            try
            {
                // PAYKIND 추가 시
                lst_Pay.Visible = true;
                lst_Pay.Items.Clear();
                OdbcCommand oCmd = conn_local.CreateCommand();
                oCmd.CommandText = " SELECT pay_date,pay_time,pay_no,pay_total, CASE  " +
                                    "    WHEN pay_kind = 0 THEN '신용카드'  " +
                                    "    WHEN pay_kind = 1 THEN '현금영수증' " +
                                    "    WHEN pay_kind = 2 THEN '현금' " +
                                    "    ELSE '쿠폰' " +
                                    " END AS pay_kind_str," +
                                    " CASE  " +
                                    "    WHEN pay_tp = 1 THEN CASE " + 
                                    "       WHEN pay_order = 0 THEN '확인전'  " +
                                    "        WHEN pay_order = 1 THEN '확  인' END " +
                                    "    WHEN pay_tp = 5 THEN '취 소' " +
                                    " END AS pay_order , pay_kind " +
                                    " FROM pay  " +
                                    " WHERE pay_jum  = ? AND pay_mart_cd = ? AND pay_date = ?  ORDER BY pay_no DESC ";
                oCmd.Parameters.Add("@jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd.Parameters.Add("@date", SqlDbType.Char).Value = sDate;


                OdbcDataReader reader = oCmd.ExecuteReader();

                while (reader.Read())
                {

                    ListViewItem lstItm = new ListViewItem();
                    lstItm.Text = sDate; // reader["pay_date"].ToString();
                    lstItm.SubItems.Add(reader["pay_time"].ToString());
                    lstItm.SubItems.Add(reader["pay_no"].ToString());
                    lstItm.SubItems.Add(  string.Format("{0:#,##0}", Int32.Parse(reader["pay_total"].ToString()))    );
                    lstItm.SubItems.Add(reader["pay_kind_str"].ToString());
                    lstItm.SubItems.Add(reader["pay_order"].ToString());
                    lstItm.SubItems.Add(reader["pay_kind"].ToString());

                    lst_Pay.Items.Add(lstItm);
                }
                reader.Close();

 


            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            if (oPay == null && lst_Pay.Visible == true)
            {
                frmMessage oMessage2 = new frmMessage(1);
                oMessage2.Set_Message(0);
                oMessage2.ShowDialog();
                return;
            }


            if (clsInputBox.QABox("확인", "선택 전표를 취소하시겠습니까?") == DialogResult.OK)
            {

                int iRet = 0;
                int iSerial = Int32.Parse(clsPrintSet.SettingHT["AUTH"].ToString());
                iSerial++;


                switch (Int32.Parse(oPay.sPaykind))
                {
                    case (0): //카드
                        //iRet = Cancel_Card(iSerial);

                        break;
                    case (1): //현금영수증
                        //iRet = Cancel_Cash_YES(iSerial);
                        break;

                    case (2): //현금 
                        iRet = Cancel_Save();
                        Print_Cancel();
                        break;
                    case (3): //쿠폰
                        iRet = Cancel_Save();
                        Print_Cancel();
                        break;
                }

                if (iRet == 0)
                {
                    //일련번호 저장
                    //conHt["AUTH"] = iSerial.ToString();
                    //oFile.Set_XMLVal(clsPrintSet.SETTING_FILE(), "SERIAL", "AUTH", iSerial.ToString());

                    oFile.Check_PutSetting("APP", "AUTH", iSerial.ToString());

                    frmMessage oMessage = new frmMessage(1);
                    oMessage.Set_Message(1);
                    oMessage.ShowDialog();

                    oPay = null;
                    //전표 찾아오기
                    string sDate = Date_Picker.Value.ToString("yyyy-MM-dd");

                    Get_PayList(sDate);
                }

            }


        }
        


        private int Cancel_Save()
        {
            try
            {
                //offline으로 만들고 취소 
                OdbcCommand oCmd_check = conn_local.CreateCommand();
                oCmd_check.CommandText = " UPDATE pay  SET pay_tp = 5 , pay_syn = 0 , pay_odate = ?" +
                         " WHERE pay_jum = ? " +
                            " AND pay_mart_cd = ?  " +
                            " AND pay_date = ?  " +
                            " AND pay_no = ?  ";

                oCmd_check.Parameters.Add("@pay_odate", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                oCmd_check.Parameters.Add("@pay_jum", SqlDbType.Char).Value = oPay.sPayjum;
                oCmd_check.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = oPay.sPayMartcd;
                oCmd_check.Parameters.Add("@pay_date", SqlDbType.Char).Value = oPay.sPaydate;
                oCmd_check.Parameters.Add("@pay_no", SqlDbType.Char).Value = oPay.sPayno;


                oCmd_check.ExecuteNonQuery();
            }
              catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return -1;
            }

            return 0;
        }

        ////현금영수증 취소
        //private int Cancel_Cash_YES(int iSerial)
        //{
        //    try
        //    {
        //        clsKovan_Auth oAuth = new clsKovan_Auth();

        //        oAuth.tcode = "42"; //현금영수증 취소 요청
        //        oAuth.Tid = oMart.sMart_getid; // "1450019998";

        //        //oAuth.Tid = "1450019998";

        //        //현금 소득공제용 지출증빙용 구분
        //        if(oPay.sPaymon == "00")
        //            oAuth.halbu = String.Format("{0:00}", 10);
        //        else if (oPay.sPaymon == "01")
        //            oAuth.halbu = String.Format("{0:00}", 11);



        //        oAuth.tamt = String.Format("{0:000000000}", oPay.dPaycash);
        //        oAuth.tax_amt = "000000000";
        //        oAuth.sfee_amt = "000000000";
        //        oAuth.free_amt = "000000000";
        //        oAuth.ori_date = oPay.sPaydateAuth();
        //        oAuth.ori_authno = oPay.sPayauth + String.Empty.PadRight(12 - oPay.sPayauth.Length , ' ');  
        //        oAuth.tran_serial = String.Format("{0:000000}", iSerial) + String.Empty.PadLeft(6, ' ');
        //        oAuth.idno = String.Empty.PadLeft(33, ' '); //전화 번호 vpos 에서 입력받음
        //        oAuth.amt_flag = String.Empty.PadLeft(3, ' ');
        //        oAuth.filler = String.Empty.PadLeft(21, ' ') + clsPrintSet.SDN_TP() + String.Empty.PadLeft(77, ' '); ; //'과세 결제진행

        //        clsKovan_Rep o_Rep = Card_Check(oAuth);

        //        if (o_Rep.rErrCode == "0000")
        //        {
        //            //응답승인금액으로 현재 금액으로 바꾼다.
        //            //o_Rep.rTamt = oReceip.iTotal.ToString();

        //            //판매자료 저장
        //            int iRet = Cancel_Save();

        //            if (iRet == 0)
        //            {

        //                Print_Cancel(o_Rep);

        //            }
        //            else
        //            {
        //                //저장실패 메세지
        //                frmMessage oMessage = new frmMessage(0);
        //                oMessage.Set_Message(6);
        //                oMessage.ShowDialog();
        //                return -1;

        //            }

        //        }
        //        else
        //        {
        //            //승인 에러 메세지
        //            //승인 에러 메세지
        //            frmMessage oMessage = new frmMessage(0);
        //            oMessage.Set_Message(7);
                    
        //            oMessage.ShowDialog();
        //            return -1;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
        //        return -1;
        //    }

        //    return 0;
        //}

        ////카드 취소
        //private int Cancel_Card(int iSerial)
        //{
        //    try
        //    {
        //        clsKovan_Auth oAuth = new clsKovan_Auth();

        //        oAuth.tcode = "S1"; //신용카드 취소 요청
        //        oAuth.Tid = oMart.sMart_getid; // "1450019998";

        //        //oAuth.Tid = "1450019998";
        //        oAuth.halbu = String.Format("{0:00}", 00);
        //        oAuth.tamt = String.Format("{0:000000000}", oPay.dPaycard );
        //        oAuth.tax_amt = "000000000";
        //        oAuth.sfee_amt = "000000000";
        //        oAuth.free_amt = "000000000";
        //        oAuth.ori_date = oPay.sPaydateAuth();
        //        oAuth.ori_authno = oPay.sPayauth + String.Empty.PadRight(12 - oPay.sPayauth.Length, ' ');

        //        oAuth.tran_serial = String.Format("{0:000000}", iSerial) + String.Empty.PadLeft(6, ' ');
        //        oAuth.idno = String.Empty.PadLeft(33, ' '); //전화 번호 vpos 에서 입력받음
        //        oAuth.amt_flag = String.Empty.PadLeft(3, ' ');
        //        oAuth.filler = String.Empty.PadLeft(21, ' ') + clsPrintSet.SDN_TP() + String.Empty.PadLeft(77, ' '); ; //'과세 결제진행

        //        clsKovan_Rep o_Rep = Card_Check(oAuth);

        //        if (o_Rep.rErrCode == "0000")
        //        {
        //            //응답승인금액으로 현재 금액으로 바꾼다.
        //            //o_Rep.rTamt = oReceip.iTotal.ToString();

        //            //판매자료 저장
        //            int iRet = Cancel_Save();

        //            if (iRet == 0)
        //            {
        //                Print_Cancel(o_Rep);
        //            }
        //            else
        //            {
        //                //저장실패 메세지
        //                frmMessage oMessage = new frmMessage(0);
        //                oMessage.Set_Message(6);

        //                oMessage.ShowDialog();
        //                return -1;
        //            }

        //        }
        //        else
        //        {
        //            //승인 에러 메세지
        //            //승인 에러 메세지
        //            frmMessage oMessage = new frmMessage(0);
        //            oMessage.Set_Message(7);
        //            oMessage.ShowDialog();
        //            return -1;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
        //        return -1;
        //    }

        //    return 0;
        //}

        
        private int Print_Cancel()
        {
            //SerialPort port = new SerialPort(clsPrintSet.SettingHT["COMP"].ToString(), Int32.Parse(clsPrintSet.SettingHT["SPEED"].ToString()), Parity.None, 8, StopBits.One);

            //try
            //{
            //    int iEmpty = Int32.Parse(clsPrintSet.SettingHT["EMPTY"].ToString());
            //    int iSep = Int32.Parse(clsPrintSet.SettingHT["SEP"].ToString());

            //    port.Encoding = Encoding.Default;

            //    port.Open();
                
                

            //    port.WriteLine(clsPrintSet.Cmd_Font1());
            //    port.WriteLine("취 소 영 수 증 <매장용>");
            //    port.WriteLine(clsPrintSet.Cmd_Close());

                

            //    port.WriteLine("매  장  명 : " + oMart.sMart_nm);
            //    port.WriteLine("담 당 자 : " + oMemad.memad_nm);
            //    port.WriteLine("주 문 일시 : " + oPay.sPaydate + " " + oPay.sPaytime );

            //    port.WriteLine(clsPrintSet.Cmd_Font_Items());
            //    port.WriteLine("영수증# : " + oPay.sPaydate + " " + oPay.sPayno);
            //    port.WriteLine(clsPrintSet.Cmd_Close());

            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));

            //    port.WriteLine("판매금액:".PadRight(21) + (string.Format("{0:#,##0}", oPay.dPayamt ) ).PadLeft(16));
            //    port.WriteLine("부 가 세:".PadRight(22) + (string.Format("{0:#,##0}", oPay.dPayvat ) ).PadLeft(16));
            //    port.WriteLine("합    계:".PadRight(23) + (string.Format("{0:#,##0}", oPay.dPaytotal ) ).PadLeft(16));

            //    port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //    port.WriteLine("수량     메뉴                      금액");

            //    foreach (ListViewItem item in lst_.Items)
            //    {

            //        int iPadCnt = iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text);
            //        int iPadCnt2 = 0;
            //        if (iPadCnt < 0)
            //        {
            //            iPadCnt2 = 15 + iPadCnt;
            //            iPadCnt = 0;

            //        }
            //        else
            //        {
            //            iPadCnt2 = 15;
            //        }
            //        port.WriteLine(
            //             item.SubItems[2].Text.PadRight(3) +
            //             item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
            //             "".PadRight(15 - item.SubItems[4].Text.Length) +
            //             item.SubItems[4].Text
            //             );

            //        //port.WriteLine(
            //        //       item.SubItems[2].Text.PadRight(3) +
            //        //       item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
            //        //       "".PadRight(15 - item.SubItems[4].Text.Length) +
            //        //       item.SubItems[4].Text
            //        //       );
            //    }

            //    port.WriteLine(string.Empty.PadLeft(iSep, '='));



            //    //커팅
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(clsPrintSet.Cmd_Cutting());

            //    port.WriteLine(clsPrintSet.Cmd_Font1());
            //    port.WriteLine("취 소 영 수 증 <고객용>");
            //    port.WriteLine(clsPrintSet.Cmd_Close());


            //    port.WriteLine("매  장  명 : " + oMart.sMart_nm);
            //    port.WriteLine("사업자번호 : " + oMart.sMart_comno);
            //    port.WriteLine("대 표 자명 : " + oMart.sMart_ceo);
            //    port.WriteLine("주      소 : " + oMart.sMart_address);
            //    port.WriteLine(clsPrintSet.Cmd_Font_Items());
            //    port.WriteLine("영수증# : " + oPay.sPaydate + " " + oPay.sPayno);
            //    port.WriteLine(clsPrintSet.Cmd_Close());

            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    if (oPay.sPaykind == "0" || oPay.sPaykind == "1" || oPay.sPaykind == "2")
            //    {
            //        port.WriteLine("판매금액:".PadRight(21) + (string.Format("{0:#,##0}", oPay.dPayamt)).PadLeft(16));
            //        port.WriteLine("부 가 세:".PadRight(22) + (string.Format("{0:#,##0}", oPay.dPayvat)).PadLeft(16));
            //        port.WriteLine("합    계:".PadRight(23) + (string.Format("{0:#,##0}", oPay.dPaytotal)).PadLeft(16));
            //    }

            //    port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //    switch (Int32.Parse(oPay.sPaykind))
            //    {
            //        case (0):
            //            port.WriteLine("신용 카드");

            //            port.WriteLine("가맹번호 : " + oMart.sMart_getid);

            //            port.WriteLine(o_Rep.rMsg1);

            //            port.WriteLine("승인번호 : " + oPay.sPayauth );
            //            port.WriteLine("카 드 사 : " + oPay.sPaycardsa );
            //            port.WriteLine("카드번호 : " + oPay.sPaycartno );
            //            port.WriteLine("취소일시 : " + o_Rep.rTranDate + " " + o_Rep.rTranTime );
            //            port.WriteLine("취소금액 : " + string.Format("{0:#,##0}", oPay.dPaycard ));
            //            break;
            //        case (1):
            //            port.WriteLine(oPay.sPaycardsa);
            //            port.WriteLine(o_Rep.rMsg1);

            //            port.WriteLine("처리 번호 : " + oPay.sPayauth);
            //            port.WriteLine("회원번호 : " + oPay.sPaycashno );
            //            port.WriteLine("취소일시 : " + o_Rep.rTranDate + " " + o_Rep.rTranTime);
            //            port.WriteLine(clsPrintSet.Cmd_DrawOpen());
            //            break;

            //        case (2): //현금 영수증 없이

            //            port.WriteLine("현금 ");
            //            port.WriteLine(clsPrintSet.Cmd_DrawOpen());
            //            break;
            //        case (3): //쿠폰

            //            port.WriteLine("쿠폰");
            //            break;
            //    }
            //    port.WriteLine(clsPrintSet.Cmd_Close());

            //    port.WriteLine(string.Empty.PadLeft(iSep, '='));

            //    port.WriteLine("수량     메뉴                      금액");

            //    foreach (ListViewItem item in lst_.Items)
            //    {
            //        int iPadCnt = iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text);
            //        int iPadCnt2 = 0;
            //        if (iPadCnt < 0)
            //        {
            //            iPadCnt2 = 15 + iPadCnt;
            //            iPadCnt = 0;

            //        }
            //        else
            //        {
            //            iPadCnt2 = 15;
            //        }

            //        //port.WriteLine(
            //        //        item.SubItems[2].Text.PadRight(3) +
            //        //        item.SubItems[1].Text + "".PadRight(iPadCnt) +
            //        //        "".PadRight(iPadCnt2 - item.SubItems[4].Text.Length) +
            //        //        item.SubItems[4].Text
            //        //        );
            //        if (oPay.sPaykind == "0" || oPay.sPaykind == "1" || oPay.sPaykind == "2")
            //        {
            //            port.WriteLine(
            //                   item.SubItems[2].Text.PadRight(3) +
            //                   item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
            //                   "".PadRight(15 - item.SubItems[4].Text.Length) +
            //                   item.SubItems[4].Text
            //                   );
            //        }
            //        else
            //        {
            //            port.WriteLine(
            //                      item.SubItems[2].Text.PadRight(3) +
            //                      item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
            //                      "".PadRight(15 - item.SubItems[4].Text.Length)
            //                      );
 
            //        }

            //    }

            //    Console.WriteLine("---------------------------------------");
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(clsPrintSet.Cmd_Font_Items());
            //    port.WriteLine("이용해 주셔서 감사 합니다.");
            //    port.WriteLine(clsPrintSet.Cmd_Close());

            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
            //    port.WriteLine(clsPrintSet.Cmd_Cutting());


            //}
            //catch (Exception ex)
            //{
            //    oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());


            //}
            //finally
            //{   
            //    if (port.IsOpen == true)
            //        port.Close();
            //}
            return 0;
        }

        private void lst_Pay_Click(object sender, EventArgs e)
        {
            //if (clsPrintSet.SettingHT["OS"].ToString() != "128") //요식업 포스 전용만 사용
            //    return;

            if (lst_Pay.Items.Count == 0)
                return;
            if (lst_Pay.SelectedItems.Count == 0)
                return;

            int iSelectIdx = lst_Pay.Items.IndexOf(lst_Pay.SelectedItems[0]);
            mnu_select_Click(sender, e);

        }

        private void btn_RePrint_Click(object sender, EventArgs e)
        {
            clsSavePay oSavePay = new clsSavePay();
            //영수증 출력
            //oSavePay.Print_Receip(oMart, oPay, lst_);

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int iRet = 0;
            int iSerial = Int32.Parse(clsPrintSet.SettingHT["AUTH"].ToString());
            iSerial++;

            frmCash2 oCash2 = new frmCash2();
           // oCash2.oCancel = this;
            oCash2.Size = new Size(800, 444);
            oCash2.StartPosition = FormStartPosition.CenterParent;

            //oCash2.ShowDialog(this);

            DialogResult dResult = oCash2.ShowDialog(this);


            if (dResult == DialogResult.Yes)
            {
                //Re_Auth_Cash(iSerial,"00");
            }
            else if (dResult == DialogResult.Abort)
            {
                //Re_Auth_Cash(iSerial,"01");
            }            
            else
            {
                Console.WriteLine("cancel");
            }
            oCash2.Dispose();


        }


        ////현금 영수증 재발행 시
        //private int Re_Auth_Cash(int iSerial,string sHalbu)
        //{
        //    try
        //    {

        //        clsKovan_Auth oAuth = new clsKovan_Auth();

        //        oAuth.tcode = "41"; //신용카드 취소 요청
        //        oAuth.Tid = oMart.sMart_getid; // "1450019998";

        //        //oAuth.Tid = "1450019998";
        //        oAuth.halbu = sHalbu; // String.Format("{0:00}", 00);

        //        oAuth.tamt = String.Format("{0:000000000}", oPay.dPaytotal );
        //        oAuth.tax_amt = "000000000";
        //        oAuth.sfee_amt = "000000000";
        //        oAuth.free_amt = "000000000";
        //        oAuth.ori_date = String.Empty.PadLeft(6, ' ');
        //        oAuth.ori_authno = String.Empty.PadLeft(12, ' ');

        //        oAuth.tran_serial = String.Format("{0:000000}", iSerial) + String.Empty.PadLeft(6, ' ');
        //        oAuth.idno = String.Empty.PadLeft(33, ' '); //전화 번호 vpos 에서 입력받음
        //        oAuth.amt_flag = String.Empty.PadLeft(3, ' ');
        //        oAuth.filler = String.Empty.PadLeft(21, ' ') + clsPrintSet.SDN_TP() + String.Empty.PadLeft(77, ' '); ; //'과세 결제진행

        //        clsKovan_Rep o_Rep = Card_Check(oAuth);

        //        if (o_Rep.rErrCode == "0000")
        //        {
        //            //응답승인금액으로 현재 금액으로 바꾼다.
        //            //o_Rep.rTamt = oReceip.iTotal.ToString();

        //            //판매자료 저장
        //            int iRet = ReAuth_Save(o_Rep);

        //            if (iRet == 0)
        //            {
        //                clsSavePay oSavePay = new clsSavePay();
        //                clsPay o_NewPay = ReSet_Pay(oPay.sPaydate,oPay.sPayno); //현금영수증 저장한 pay 테이블 다시 불러온다.


        //                oSavePay.Print_Receip(oMart, o_NewPay, lst_, o_Rep);
        //                //oSavePay.Print_Receip(oMart, oPay, o_Rep, lst_);

        //            }
        //            else
        //            {
        //                //저장실패 메세지
        //                frmMessage oMessage = new frmMessage(0);
        //                oMessage.Set_Message(6);

        //                oMessage.ShowDialog();
        //                return -1;
        //            }

        //        }
        //        else
        //        {
        //            //승인 에러 메세지
        //            //승인 에러 메세지
        //            frmMessage oMessage = new frmMessage(0);
        //            oMessage.Set_Message(7);
        //            oMessage.ShowDialog();
        //            return -1;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
        //        return -1;
        //    }

        //    return 0;
        //}

        //private int ReAuth_Save(clsKovan_Rep o_Rep)
        //{
        //    try
        //    {
        //        //offline으로 만들고 취소 
        //        OdbcCommand oCmd_check = conn_local.CreateCommand();
        //        oCmd_check.CommandText = " UPDATE pay  SET pay_buysa = ? , pay_cardsa = ? , pay_pad = ? , " +
        //                                " pay_mon = ? , pay_agent = ? , pay_auth = ? , pay_cashno = ? , pay_cash = ? ,pay_kind = ? " +
        //                 " WHERE pay_jum = ? " +
        //                    " AND pay_mart_cd = ?  " +
        //                    " AND pay_date = ?  " +
        //                    " AND pay_no = ?  ";




        //        oCmd_check.Parameters.Add("@pay_buysa", SqlDbType.Char).Value = o_Rep.rPurchaseCard;
        //        oCmd_check.Parameters.Add("@pay_cardsa", SqlDbType.Char).Value = o_Rep.rIssueCard;
        //        oCmd_check.Parameters.Add("@pay_pad", SqlDbType.Char).Value = o_Rep.rSignPath;
        //        oCmd_check.Parameters.Add("@pay_mon", SqlDbType.Char).Value = o_Rep.rHalbu;

        //        oCmd_check.Parameters.Add("@pay_agent", SqlDbType.Char).Value = o_Rep.rMerNo;
        //        oCmd_check.Parameters.Add("@pay_auth", SqlDbType.Char).Value = o_Rep.rAuthNo;
        //        oCmd_check.Parameters.Add("@pay_cashno", SqlDbType.Char).Value = o_Rep.rCardno; ;
        //        oCmd_check.Parameters.Add("@pay_cash", SqlDbType.Int).Value = oPay.dPaycash;
        //        oCmd_check.Parameters.Add("@pay_kind", SqlDbType.Int).Value = "1";

        //        oCmd_check.Parameters.Add("@pay_jum", SqlDbType.Char).Value = oPay.sPayjum;
        //        oCmd_check.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = oPay.sPayMartcd;
        //        oCmd_check.Parameters.Add("@pay_date", SqlDbType.Char).Value = oPay.sPaydate;
        //        oCmd_check.Parameters.Add("@pay_no", SqlDbType.Char).Value = oPay.sPayno;


        //        oCmd_check.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
        //        return -1;
        //    }

        //    return 0;
        //}


        private clsPay ReSet_Pay(string sDate,string sNo)
        {
            clsPay o_Pay = new clsPay();

            try
            {   
                OdbcCommand oCmd = conn_local.CreateCommand();
                oCmd.CommandText = " SELECT * FROM pay" +
                                    " WHERE pay_jum  = ? AND pay_mart_cd = ? AND pay_date = ? AND pay_no = ? ";
                oCmd.Parameters.Add("@jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd.Parameters.Add("@date", SqlDbType.Char).Value = sDate;
                oCmd.Parameters.Add("@no", SqlDbType.Char).Value = sNo;

                OdbcDataReader reader = oCmd.ExecuteReader();

                if (reader.Read())
                {   
                    o_Pay.sPayjum = reader["pay_jum"].ToString();
                    o_Pay.sPayMartcd = reader["pay_mart_cd"].ToString();
                    DateTime dt = Convert.ToDateTime(reader["pay_date"].ToString());
                    o_Pay.sPaydate = dt.ToString("yyyy-MM-dd");
                    o_Pay.sPaytime = reader["pay_time"].ToString();
                    o_Pay.sPayno = reader["pay_no"].ToString();
                    o_Pay.sPaysyn = reader["pay_syn"].ToString();
                    o_Pay.sPaytp = reader["pay_tp"].ToString();
                    o_Pay.iPaycount = Int32.Parse(reader["pay_count"].ToString());
                    o_Pay.dPaytotal = double.Parse(reader["pay_total"].ToString());
                    o_Pay.dPayvat = double.Parse(reader["pay_vat"].ToString());
                    o_Pay.dPayamt = double.Parse(reader["pay_amt"].ToString());
                    o_Pay.sPaybuysa = reader["pay_buysa"].ToString();
                    o_Pay.sPaycardsa = reader["pay_cardsa"].ToString();
                    o_Pay.sPaygetid = reader["pay_getid"].ToString();
                    o_Pay.sPayvan = reader["pay_van"].ToString();
                    o_Pay.sPaypad = reader["pay_pad"].ToString();
                    o_Pay.sPaymon = reader["pay_mon"].ToString();
                    o_Pay.sPaycartno = reader["pay_cartno"].ToString();
                    o_Pay.sPayagent = reader["pay_agent"].ToString();
                    o_Pay.sPayauth = reader["pay_auth"].ToString();
                    o_Pay.sPaycashno = reader["pay_cashno"].ToString();
                    o_Pay.dPaycard = double.Parse(reader["pay_card"].ToString());
                    o_Pay.dPaycash = double.Parse(reader["pay_cash"].ToString());
                    o_Pay.sPaytid = reader["pay_tid"].ToString();
                    o_Pay.sPayorder = reader["pay_order"].ToString();
                    o_Pay.sPaymartocd = reader["pay_mart_ocd"].ToString();
                    o_Pay.sPayono = reader["pay_ono"].ToString();
                    o_Pay.sPayodate = reader["pay_odate"].ToString();
                    o_Pay.sPaykind = reader["pay_kind"].ToString();

                    o_Pay.Set_VatAmt(double.Parse(reader["pay_total"].ToString()));


                }
                reader.Close();


            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
            
            return o_Pay;
 
        }

    }
}




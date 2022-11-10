using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmMain : Form, Inter_Set 
    {
        clsDataSetToXml dsXml = null;
        double dtot = 0;
        double cdtot = 0;

        Label[] lbl_Num = new Label[42];
        uscCalendar[] oCalender = new uscCalendar[42];
        List<clsSaleStat> oSaleStat = null;


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            lbl_Version.Text  = assembly.FullName
                                .Split(',')[1]
                                .Trim()
                                .Split('=')[1];


            txt_Date.Text = DateTime.Now.ToString("yyyy년MM월"); 


            #region 인터넷 상태 체크
            if (clsNetTest.isInternetOk())
            {
                lbl_Info.Text += "인터넷 정상" + Environment.NewLine;
            }
            else
            {
                lbl_Info.Text += "인터넷 연결 안됨" + Environment.NewLine;                
                return;
            }
            #endregion

            #region//db 연결 문자열 생성
            if (Get_DBSetting())
            {
                lbl_Info.Text += "연결문자열 정상" + Environment.NewLine;

            }
            else
            {   
                lbl_Info.Text += "연결 문자열 생성오류" + Environment.NewLine;                
                return;

            }
            #endregion

            #region//판매기기 정보 가져오기
            if (Get_Setting())
            {
                lbl_Info.Text += "판매기 세팅 가져옴" + Environment.NewLine;

                Get_Machine_Info(clsPos.pos_num);


                lbl_Info.Text += "가 맹 점 : " + clsPos.store_name  + Environment.NewLine;
                lbl_Info.Text += "포스번호 : " + clsPos.pos_num  + Environment.NewLine;
                lbl_Info.Text += "Van 사 : " + clsSetting.GetDescription(clsPos.van_cmpny) + Environment.NewLine;
                lbl_Info.Text += "TID : " + clsPos.tid  + Environment.NewLine;


            }
            else
            {
                lbl_Info.Text += "판매기 세팅 생성오류" + Environment.NewLine;                
                return;

            }
            #endregion
            
            #region Van 사항 체크 dll파일, 데몬실행여부

            if (clsPos.van_cmpny == clsEnum.Van_Cmpny.jtnet)
            {
                if (File.Exists(clsStaticString.VAN_FILE))
                {
                    //JTNet데몬 실행 여부 
                    using (clsJTNet oJtNet = new clsJTNet())
                    {
                        if (oJtNet.Check_Demon() == 1)
                        {
                            lbl_Info.Text += "데몬 실행 중" + Environment.NewLine;
                        }
                        else
                        {
                            lbl_Info.Text += "데몬 미 실행" + Environment.NewLine;                            
                            return;
                        }
                    }
                }
                else
                {
                    lbl_Info.Text += "밴 설정 파일 없음" + Environment.NewLine;                    
                    return;
                }
            }
            else if (clsPos.van_cmpny == clsEnum.Van_Cmpny.kovan)
            {
                if (File.Exists(clsStaticString.KO_VAN_FILE))
                {
                    // int i = clsStaticString.Serial_Open(6);
                    //int iOut = 0;
                    //clsStaticString.SetCallBackFnc_VB(iOut);
                    //Console.WriteLine(iOut);
                    //vpos 자동 실행
                    if (File.Exists(clsStaticString.KO_VAN_EXE_FILE))
                    {
                        clsSetting.Kill_Program(clsStaticString.KO_VAN_PROC);
                        ProcessStartInfo psStar = new ProcessStartInfo(clsStaticString.KO_VAN_EXE_FILE);
                        Process ps = new Process();
                        psStar.WorkingDirectory = clsStaticString.KO_VAN_DIR;
                        psStar.WindowStyle = ProcessWindowStyle.Minimized;
                        ps.StartInfo = psStar;
                        ps.Start();
                        ps.Dispose();
                    }
                }
                else
                {
                    lbl_Info.Text += "밴 설정 파일 없음" + Environment.NewLine;                   
                    return;
                }
            }

            #endregion

            lbl_Info.Text += "포스 정상 실행중..." + Environment.NewLine;

            foreach (TextBox textBox in clsSetting.GetAll(this.tbl_Money , typeof(TextBox)))
            {
                textBox.Enter += new EventHandler(TextBox_Enter);
                textBox.Leave += new EventHandler(TextBox_Leave);
                textBox.KeyDown += new KeyEventHandler(TextBox_KeyDown);
                textBox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
                textBox.TextChanged += textBox_TextChanged;
            }

            foreach (TextBox textBox in clsSetting.GetAll(this.tbl_CMoney , typeof(TextBox)))
            {
                textBox.Enter += new EventHandler(TextBox_Enter);
                textBox.Leave += new EventHandler(TextBox_Leave);
                textBox.KeyDown += new KeyEventHandler(TextBox_KeyDown);
                textBox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
                textBox.TextChanged += textBox_TextChanged;
            }

            Application.DoEvents();

            Close_All();

            Set_Calendar();

            Show_Calendar();
        }


        #region key event

        private void Check_Sum()
        {
            double dSum = 0;

            if (lbl_50Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_50Thou.Text);
            }

            if (lbl_10Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_10Thou.Text);
            }

            if (lbl_5Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_5Thou.Text);
            }

            if (lbl_1Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_1Thou.Text);
            }
            if (lbl_5Hund.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_5Hund.Text);
            }
            if (lbl_1Hund.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_1Hund.Text);
            }
            if (lbl_50.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_50.Text);
            }
            if (lbl_10.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_10.Text);
            }
            dtot = dSum;

            lbl_Tot.Text = String.Format("{0:#,##0}", dtot);


            dSum = 0;
            if (lbl_C50Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C50Thou.Text);
            }

            if (lbl_C10Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C10Thou.Text);
            }

            if (lbl_C5Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C5Thou.Text);
            }

            if (lbl_C1Thou.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C1Thou.Text);
            }
            if (lbl_C5Hund.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C5Hund.Text);
            }
            if (lbl_C1Hund.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C1Hund.Text);
            }
            if (lbl_C50.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C50.Text);
            }
            if (lbl_C10.Text.Length > 0)
            {
                dSum += clsSetting.Let_Double(lbl_C10.Text);
            }
            cdtot  = dSum;

            lbl_Ctotal.Text = String.Format("{0:#,##0}", cdtot);


        }
        void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            string sTag = txtBox.Tag.ToString();
            double dAmt = clsSetting.Let_Double(txtBox.Text );

            switch (sTag)
            {  
                case "1":                    
                    lbl_50Thou.Text = String.Format("{0:#,##0}", dAmt * 50000);
                    break;
                case "2":
                    lbl_10Thou.Text = String.Format("{0:#,##0}", dAmt * 10000);
                    break;
                case "3":
                    lbl_5Thou.Text = String.Format("{0:#,##0}", dAmt * 5000);
                    break;
                case "4":
                    lbl_1Thou.Text = String.Format("{0:#,##0}", dAmt * 1000);
                    break;
                case "5":
                    lbl_5Hund.Text = String.Format("{0:#,##0}", dAmt * 500);
                    break;
                case "6":
                    lbl_1Hund.Text = String.Format("{0:#,##0}", dAmt * 100);
                    break;
                case "7":
                    lbl_50.Text = String.Format("{0:#,##0}", dAmt * 50);
                    break;
                case "8":
                    lbl_10.Text = String.Format("{0:#,##0}", dAmt * 10);
                    break;

                case "11":
                    lbl_C50Thou.Text = String.Format("{0:#,##0}", dAmt * 50000);
                    break;
                case "12":
                    lbl_C10Thou.Text = String.Format("{0:#,##0}", dAmt * 10000);
                    break;
                case "13":
                    lbl_C5Thou.Text = String.Format("{0:#,##0}", dAmt * 5000);
                    break;
                case "14":
                    lbl_C1Thou.Text = String.Format("{0:#,##0}", dAmt * 1000);
                    break;
                case "15":
                    lbl_C5Hund.Text = String.Format("{0:#,##0}", dAmt * 500);
                    break;
                case "16":
                    lbl_C1Hund.Text = String.Format("{0:#,##0}", dAmt * 100);
                    break;
                case "17":
                    lbl_C50.Text = String.Format("{0:#,##0}", dAmt * 50);
                    break;
                case "18":
                    lbl_C10.Text = String.Format("{0:#,##0}", dAmt * 10);
                    break;
            }

            Check_Sum();
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBox.SelectAll();
            txtBox.BackColor = Color.LightBlue;
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBox.BackColor = Color.White;
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
        #endregion




        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        /// <summary>
        /// db설정 파일 읽어와서 연결문자 생성
        /// </summary>
        /// <returns></returns>
        private bool Get_DBSetting()
        {
            bool bReturn = false;
            //db연결 문자열 만든다.
            clsDataSetToXml dsXml = new clsDataSetToXml(clsStaticString.DB_FILE);
            DataSet ds = dsXml.GetXMLSet();

            //디비 관련 내용 있는지 체크
            DataTable tbl_db = ds.Tables["dbinfo"];
            if (tbl_db != null)
            {
                if (tbl_db.Rows.Count >= 1)
                {
                    //db연결 문자열 생성
                    clsDBString oDBstring = new clsDBString();
                    clsEncryption oEncryption = new clsEncryption();

                    foreach (DataRow dr in tbl_db.Rows)
                    {
#if DEBUG
                        Console.WriteLine(dr["ip"].ToString());
#endif

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
                bReturn = true;
            }
            else
            {
                bReturn = false;

            }
            ds.Dispose();
            dsXml.Dispose();


            return bReturn;
        }
        
        /// <summary>
        /// 판매기기 정보 가져오기
        /// </summary>
        /// <returns></returns>
        private bool Get_Setting()
        {
            bool bReturn = false;
            //db연결 문자열 만든다.
            clsDataSetToXml dsXml = new clsDataSetToXml(clsStaticString.SETTING_FILE);
            DataSet ds = dsXml.GetXMLSet();

            //디비 관련 내용 있는지 체크
            DataTable tbl_db = ds.Tables["appinfo"];
            if (tbl_db != null)
            {
                if (tbl_db.Rows.Count >= 1)
                {
                    foreach (DataRow dr in tbl_db.Rows)
                    {
                        clsPos.store_code = dr["store"].ToString();
                        clsPos.pos_num = dr["pos"].ToString();                        
                    }

                }
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            ds.Dispose();
            dsXml.Dispose();
            return bReturn;
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

                    clsPos.store_address1 = dr["store_address1"].ToString();
                    clsPos.store_address2 = dr["store_address2"].ToString();
                    clsPos.store_number = dr["store_number"].ToString();
                    clsPos.store_tel = dr["store_tel"].ToString();

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





        private void pic_Exit_Click(object sender, EventArgs e)
        {   
            clsSetting.Exit_Program();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //Thread_Stop();
            Program_Exit();
        }


        private void Program_Exit()
        {
            frmExit oExit = new frmExit();
            oExit.Show();
            Program.ac.MainForm = oExit;
            this.Close();

        }





        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close_All();

            pan_Setting.Visible = true;
            //pan_Setting.Dock = DockStyle.Fill;

            txt_Pos.Text = clsPos.pos_num;
            txt_Port.Text = clsPos.port;
            txt_Speed.Text = clsPos.speed;
            txt_Store.Text = clsPos.store_code;
            lbl_Store.Text = clsPos.store_name;
            txt_TID.Text = clsPos.tid;
            txt_Van.Text = clsSetting.GetDescription(clsPos.van_cmpny);


        }

        private void pic_SaleC_Click(object sender, EventArgs e)
        {

            if (!Get_Open_Date())
            {
                MessageBox.Show("오픈 정보가 없습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Close_All();
            pan_Close.Visible = true;
            //pan_Close.Dock = DockStyle.Fill;


            txt_Close_Opened.Text = clsDateTime.Get_Time(clsPosOpen.open_date).ToString("yyyy-MM-dd");
            txt_Count.Text = clsPosOpen.sale_customs.ToString();
            txt_Amount.Text = clsSetting.Let_Money(clsPosOpen.sale_amount);

            Open_Draw();
        }


        private void pic_Start_Click(object sender, EventArgs e)
        {
            Close_All();
            pan_Start.Visible = true;
            //pan_Start.Dock = DockStyle.Fill;
            txt_Current.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (Get_Open_Date())
            {
                frmOrder_Type2 newForm = new frmOrder_Type2();
                newForm.Show();
                Program.ac.MainForm = newForm;
                this.Close();
            }
            else
            {
                txt_SaleDate.Text = clsDateTime.Get_Time(clsPosOpen.open_date).ToString("yyyy-MM-dd");

                txt_50Thou.Focus();

                Open_Draw();
            }
        }


        //돈통 열기
        private void Open_Draw()
        {
            SerialPort port = new SerialPort(clsPos.port, Int32.Parse(clsPos.speed), Parity.None, 8, StopBits.One);
            try
            {
                port.Open();

                port.WriteLine(clsPrintSet.Cmd_DrawOpen());
                port.WriteLine(clsPrintSet.Cmd_Close());


            }
            catch (Exception ex)
            {
                //oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());


            }
            finally
            {
                if (port.IsOpen)
                    port.Close();

            }
        }

        private void Close_All()
        {
            pan_Setting.Visible = false;
            pan_Start.Visible = false;
            pan_Close.Visible = false;
            pan_Close_Final.Visible = false;
            //pan_Calendar.Visible = true;
        }


        #region 설정
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
                    clsPos.van_cmpny = clsSetting.GetVanEnum(sVal);
                    txt_Van.Text = sVal;
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Get_Machine_Info(txt_Pos.Text);

            txt_Pos.Text = clsPos.pos_num;
            txt_Port.Text = clsPos.port;
            txt_Speed.Text = clsPos.speed;
            txt_Store.Text = clsPos.store_code;
            lbl_Store.Text = clsPos.store_name;
            txt_TID.Text = clsPos.tid;
            txt_Van.Text = clsSetting.GetDescription(clsPos.van_cmpny);
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
        private void btn_Ban_Click(object sender, EventArgs e)
        {
            frmSerialSetting oSetting = new frmSerialSetting(this as Inter_Set, clsEnum.Select_SerialSet.van);
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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Close_All();
        }
        private void btn_Test_Click(object sender, EventArgs e)
        {
            SerialPort port = new SerialPort(clsPos.port, Int32.Parse(clsPos.speed), Parity.None, 8, StopBits.One);

            port.Encoding = Encoding.Default;

            port.Open();

            port.WriteLine("TEST PRINT");

            if (port.IsOpen == true)
                port.Close();
        }
        #endregion



        #region 판매시작

        private bool Get_Open_Date()
        {
            bool bContinue = false;

            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.get_POS_OPEN";

                //cmd_SEL.Parameters.Add("StoreCode", OracleDbType.Varchar2).Value = sTore;
                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("cur_pos", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                if (dt.Rows.Count == 0)
                {
                    clsPosOpen.pos_num = clsPos.pos_num;
                    clsPosOpen.open_date = clsSetting._Today();
                    clsPosOpen.close_date = 0;
                    clsPosOpen.sale_customs = 0;
                    clsPosOpen.sale_amount = 0;
                    clsPosOpen.FOThou = 0;
                    clsPosOpen.OOThou = 0;
                    clsPosOpen.FThou = 0;
                    clsPosOpen.OThou = 0;
                    clsPosOpen.FHund = 0;
                    clsPosOpen.OHund = 0;
                    clsPosOpen.FO = 0;
                    clsPosOpen.IO = 0;
                    clsPosOpen.dtotal = 0;

                    clsPosOpen.cash_io_in = 0;
                    clsPosOpen.cash_io_out = 0;
                    clsPosOpen.sale_Cardamount = 0;
                    clsPosOpen.sale_Cashamount = 0;

                    bContinue = false;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        clsPosOpen.pos_num = clsPos.pos_num;
                        clsPosOpen.open_date = Convert.ToInt32(dr["opened"]);
                        clsPosOpen.close_date = Convert.ToInt32(dr["closed"]);
                        clsPosOpen.sale_customs = Convert.ToInt32(dr["custom"]);
                        clsPosOpen.sale_amount = Convert.ToDouble(dr["amount"]);
                        clsPosOpen.FOThou = Convert.ToInt32(dr["fothou"]);
                        clsPosOpen.OOThou = Convert.ToInt32(dr["oothou"]);
                        clsPosOpen.FThou = Convert.ToInt32(dr["fthou"]);
                        clsPosOpen.OThou = Convert.ToInt32(dr["othou"]);
                        clsPosOpen.FHund = Convert.ToInt32(dr["fhund"]);
                        clsPosOpen.OHund = Convert.ToInt32(dr["ohund"]);
                        clsPosOpen.FO = Convert.ToInt32(dr["fo"]);
                        clsPosOpen.IO = Convert.ToInt32(dr["io"]);
                        clsPosOpen.dtotal = Convert.ToDouble(dr["dtotal"]);

                        clsPosOpen.cash_io_in = Convert.ToDouble(dr["cash_io_in"]);
                        clsPosOpen.cash_io_out = Convert.ToDouble(dr["cash_io_out"]);

                        clsPosOpen.sale_Cardamount = Convert.ToDouble(dr["card_amount"]);
                        clsPosOpen.sale_Cashamount = Convert.ToDouble(dr["cash_amount"]);


                    }
                    bContinue = true;
                }

                return bContinue;
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return bContinue;
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();

                if (dt != null)
                    dt.Dispose();
            }
 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close_All();

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //오픈정보 저장
            try
            {
                clsPosOpen.FOThou =  clsSetting.Let_Int(txt_50Thou.Text);
                clsPosOpen.OOThou = clsSetting.Let_Int(txt_10Thou.Text);
                clsPosOpen.FThou = clsSetting.Let_Int(txt_5Thou.Text);
                clsPosOpen.OThou = clsSetting.Let_Int(txt_1Thou.Text);
                clsPosOpen.FHund = clsSetting.Let_Int(txt_5Hund.Text);
                clsPosOpen.OHund = clsSetting.Let_Int(txt_1Hund.Text);
                clsPosOpen.FO = clsSetting.Let_Int(txt_50.Text);
                clsPosOpen.IO = clsSetting.Let_Int(txt_10.Text);
                clsPosOpen.dtotal = dtot;


                if (clsPosOpen.Set_Open())
                {
                    frmOrder_Type2 newForm = new frmOrder_Type2();
                    newForm.Show();
                    Program.ac.MainForm = newForm;

                    this.Close();
                }
            }
            catch(Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                MessageBox.Show("저장 실패." + "\n" + ex.Message.ToString());
            }
            finally
            {

            }
        }

        #endregion

       


        #region 마감
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                clsPosOpen.CFOThou = clsSetting.Let_Int(txt_C50Thou.Text);
                clsPosOpen.COOThou = clsSetting.Let_Int(txt_C10Thou.Text);
                clsPosOpen.CFThou = clsSetting.Let_Int(txt_C5Thou.Text);
                clsPosOpen.COThou = clsSetting.Let_Int(txt_C1Thou.Text);
                clsPosOpen.CFHund = clsSetting.Let_Int(txt_C5Hund.Text);
                clsPosOpen.COHund = clsSetting.Let_Int(txt_C1Hund.Text);
                clsPosOpen.CFO = clsSetting.Let_Int(txt_C50.Text);
                clsPosOpen.CIO = clsSetting.Let_Int(txt_C10.Text);
                clsPosOpen.Cdtotal = cdtot;
                
                clsPosOpen.close_date = clsSetting._Today();

                if (clsPosOpen.Set_Close())
                {
                    MessageBox.Show("마감완료 하였습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close_All();

                    pan_Close_Final.Visible = true;

                }
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                MessageBox.Show("저장 실패." + "\n" + ex.Message.ToString());
            }
            finally
            {

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close_All();
        }


        private void btn_Close_Exti_Click(object sender, EventArgs e)
        {
            Close_All();
        }

        #endregion



        #region 달력





        private void Set_Calendar()
        {
            int iCnt = 0;
            int iDay = 0;
            Size Sizes = new Size(100, 60);
            Size Margins = new Size(1, 1);
            Point Drawing_Start = new Point(30, 20);
            Label[] oDayof = new Label[7];

            //요일표시
            for (int j = 0; j < 7; j++)
            {
                oDayof[iDay] = new Label();
                oDayof[iDay].Size = new Size(100, 15);
                oDayof[iDay].Location = new Point(j * (Sizes.Width + Margins.Width) + 30,0);
                oDayof[iDay].TextAlign = ContentAlignment.MiddleCenter;
                oDayof[iDay].BackColor = Color.FromArgb(207, 214, 232);
                switch (j)
                {
                    case (0):
                        oDayof[iDay].ForeColor = Color.Red;
                        oDayof[iDay].Text = "일요일";
                        break;
                    case (1):
                        oDayof[iDay].ForeColor = Color.Black;
                        oDayof[iDay].Text = "월요일";
                        break;
                    case (2):
                        oDayof[iDay].ForeColor = Color.Black;
                        oDayof[iDay].Text = "화요일";
                        break;
                    case (3):
                        oDayof[iDay].ForeColor = Color.Black;
                        oDayof[iDay].Text = "수요일";
                        break;
                    case (4):
                        oDayof[iDay].ForeColor = Color.Black;
                        oDayof[iDay].Text = "목요일";
                        break;
                    case (5):
                        oDayof[iDay].ForeColor = Color.Black;
                        oDayof[iDay].Text = "금요일";
                        break;
                    case (6):
                        oDayof[iDay].ForeColor = Color.Blue;
                        oDayof[iDay].Text = "토요일";
                        break;
                }

                pan_Calc.Controls.Add(oDayof[iDay++]);
            }

            //달력 컨트롤 생성
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    oCalender[iCnt] = new uscCalendar();
                    oCalender[iCnt].Size = Sizes;
                    oCalender[iCnt].Location = new Point(j * (Sizes.Width + Margins.Width) + Drawing_Start.X, i * (Sizes.Height + Margins.Height) + Drawing_Start.Y);
                    //lbl_Num[iCnt].TextAlign = ContentAlignment.TopRight;
                    if (j == 0) this.oCalender[iCnt].Set_DateColor();
                    if (j == 6) this.oCalender[iCnt].ForeColor = Color.Blue;
                    pan_Calc.Controls.Add(this.oCalender[iCnt++]);
                }
            }

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
            }



            Set_SaleSate();

            for (int i = 0; i < iDays_a_Month[iMonth]; i++)
            {
                foreach (clsSaleStat o_Sale in oSaleStat)
                {
                    if (o_Sale.sDate == oCalender[iStart_day + i].sDate)
                    {
                        oCalender[iStart_day + i].SetAmtText = string.Format("{0:#,##0}", o_Sale.dAmt);
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

            int iStartDate = clsDateTime.StartOfDay(oCalender[0].Get_Date());
            int iDate = clsDateTime.EndOfDay(oCalender[oCalender.GetUpperBound(0)].Get_Date());


            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;

            oSaleStat = new List<clsSaleStat>();

            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Sale_Sum_Calc";
                cmd_SEL.Parameters.Add("pos", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("date1", OracleDbType.Varchar2).Value = iStartDate;
                cmd_SEL.Parameters.Add("date2", OracleDbType.Int32).Value = iDate;
                cmd_SEL.Parameters.Add("cur_sum", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {
                    clsSaleStat o_Sale = new clsSaleStat();

                    o_Sale.sDate = dr["bill_sum_date"].ToString();
                    o_Sale.dAmt = Convert.ToDouble(dr["amt"]);
                    o_Sale.iCnt = Convert.ToInt16(dr["cnt"]);
                    o_Sale.sState = "";

                    oSaleStat.Add(o_Sale);


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

        #endregion

        private void pan_Close_Final_VisibleChanged(object sender, EventArgs e)
        {
            if (pan_Close_Final.Visible)
            {
                pan_Close_Final.Dock = DockStyle.Fill;

                txt_Open_Date.Text = clsDateTime.Get_Time(clsPosOpen.open_date).ToString("yyyy-MM-dd");
                txt_Close_Count2.Text = clsPosOpen.sale_customs.ToString();
                txt_Amt2.Text = clsSetting.Let_Money(clsPosOpen.sale_amount);

                txt_Start_Amt.Text = clsSetting.Let_Money(clsPosOpen.dtotal);
                txt_Cash_Amt.Text = clsSetting.Let_Money(clsPosOpen.sale_Cashamount);

                txt_CashIO_IN.Text = clsSetting.Let_Money(clsPosOpen.cash_io_in );
                txt_CashIO_OUT.Text = clsSetting.Let_Money(clsPosOpen.cash_io_out );

                double dTemp = (clsPosOpen.dtotal + clsPosOpen.sale_Cashamount) + clsPosOpen.cash_io_in - clsPosOpen.cash_io_out;


                txt_CashSub.Text = clsSetting.Let_Money(dTemp);
                                
                txt_Close_Amt.Text = clsSetting.Let_Money(clsPosOpen.Cdtotal);
                
                txt_Ballance.Text = clsSetting.Let_Money(dTemp  - clsPosOpen.Cdtotal);
                
                txt_Card_Amt.Text = clsSetting.Let_Money(clsPosOpen.sale_Cardamount);
            }
            else
            {
                pan_Close_Final.Dock = DockStyle.None;

                foreach (TextBox textBox in clsSetting.GetAll(this.pan_Close_Final, typeof(TextBox)))
                {
                    textBox.Text = "";
                }

            }
        }

        

        private void pan_Close_VisibleChanged(object sender, EventArgs e)
        {
            if (pan_Close.Visible)
            {
                pan_Close.Dock = DockStyle.Fill;
            }
            else
            {
                pan_Close.Dock = DockStyle.None;
            }
        }

        private void pan_Setting_VisibleChanged(object sender, EventArgs e)
        {
            if (pan_Setting.Visible)
            {
                pan_Setting.Dock = DockStyle.Fill;
            }
            else
            {
                pan_Setting.Dock = DockStyle.None;
            }
        }

        private void pan_Start_VisibleChanged(object sender, EventArgs e)
        {
            if (pan_Start.Visible)
            {
                pan_Start.Dock = DockStyle.Fill;
            }
            else
            {
                pan_Start.Dock = DockStyle.None;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //마감내역프린트
            using (clsPrint oPrint = new clsPrint())
            {
                oPrint.Print_Close();
            }

        }




       


    }
}

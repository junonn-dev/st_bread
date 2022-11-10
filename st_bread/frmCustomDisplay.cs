using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection; //현재 실행중 함수명
//using System.Threading;
//using DirectShowLib;

namespace st_bread
{
    public partial class frmCustomDisplay : Form
    {
        
        double dTotal = 0; //
        clsFile oFile = new clsFile();
        static string filePath = Application.StartupPath + @"\img\" + clsPrintSet.SettingHT["MOFILE"].ToString();//gongchaAd.avi

        public Timer oTimer = new Timer();

        Boolean bIsFullScreen = false;
        int iJob = 0;
        
        public frmCustomDisplay()
        {
            InitializeComponent();
        }


        private void frmCustomDisplay_Load(object sender, EventArgs e)
        {
            oTimer.Interval = 1000;
            oTimer.Tick += new EventHandler(Timer_Tick);
            oTimer.Start();
        }


        void Timer_Tick(object sender, System.EventArgs e)
        {
           //7초 동안 아무 이벤트 없으면 화면 전체 모드로 변경
            if (iJob == 7)
            {
                Show_FullScreen();
                iJob = 0;
            }
            else
            {
                iJob++;
            }
        }



        //폼 열고 비디오 재생 시작
        public void Start_Video()
        {
            try
            {
                
                Media_Player.URL = filePath;
                Media_Player.settings.setMode("loop", true);
                if (bIsFullScreen == false)
                    bIsFullScreen = true;

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        //손님이 없을때 전체 화면
        public void Show_FullScreen()
        {
            //pan_Show.Location = new Point(0, 0);
            //pan_Show.Size = new Size(1027, 768);
            //pan_Show.BringToFront();
            try
            {
                if (bIsFullScreen == false)
                {
                    Media_Player.fullScreen = true;
                    bIsFullScreen = true;
                }
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }
        //손님 방문시 다시 기존 화면
        public void Show_FullScreen_OFF()
        {

            //pan_Show.Location = new Point(434, 0);
            //pan_Show.Size = new Size(587, 463);
            try
            {
                if (bIsFullScreen == true)
                {
                    Media_Player.fullScreen = false;
                    bIsFullScreen = false;
                    Show_Start();
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

        }

        //종료 했을때 다시 재생
        private void Media_Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if (bIsFullScreen == true)
                {
                    //Show_FullScreen();
                    if (e.newState == 3)
                    {
                        Media_Player.fullScreen = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }



        //고객이 들어오면 시작
        public void Show_Start()
        {
            try
            {
                //pan_Show.Location = new Point(434, 0);
                //pan_Show.Size = new Size(587, 463);
                iJob = 0;
                lbl_Rest.Text = "0";
                lbl_CustomPay.Text = "0";
                oTimer.Stop();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }


        public void Set_AllClear()
        {
            try
            {
                lst_Items.Items.Clear();
                iJob = 0;
                lbl_Rest.Text = "0";
                lbl_CustomPay.Text = "0";
                lbl_tot.Text = "0";

                oTimer.Start();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }


        public void Set_Start()
        {
            oTimer.Start();
        }

        //주문 화면에서 찍은 상품 표시
        public void Set_List(ListView oItems)
        {
            try
            {
                iJob = 0;
                lst_Items.Items.Clear();

                pan_Cong.Visible = false;


                foreach (ListViewItem o_item in oItems.Items)
                {
                    //상품 없음 리스트에 추가
                    ListViewItem lstItm = new ListViewItem();
                    lstItm.Text = o_item.Text;
                    lstItm.SubItems.Add(o_item.SubItems[1].Text);
                    lstItm.SubItems.Add(o_item.SubItems[2].Text);
                    lstItm.SubItems.Add(o_item.SubItems[3].Text);
                    lstItm.SubItems.Add(o_item.SubItems[4].Text);
                    lstItm.SubItems.Add(o_item.SubItems[5].Text);
                    lstItm.SubItems.Add(o_item.SubItems[6].Text);

                    lst_Items.Items.Add(lstItm);
                }
                
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
            finally 
            {
                Check_SUM();
            }
        }
        
        //받은돈 거스름돈 표시
        public void Set_Pay(string sRest,string sPay)
        {
            try
            {
                iJob = 0;
                lbl_Rest.Text = sRest;
                lbl_CustomPay.Text = sPay;
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        //전체 합계 계산
        private double Check_SUM()
        {
            try
            {
                //double dReturn = 0;
                int iItemCount = 0;
                int iSumcnt = 0;
                //Double dTotAmt = 0;

                //dTotAmt = 0;
                dTotal = 0;

                foreach (ListViewItem item in lst_Items.Items)
                {
                    dTotal = dTotal + Double.Parse(item.SubItems[5].Text);
                    iSumcnt = iSumcnt + Int32.Parse(item.SubItems[3].Text);
                    iItemCount++;
                }

                if (dTotal == 0 && lst_Items.Items.Count == 0)
                {
                    //취소 하여 금액이 0이 되면 전체 취소로 간주
                    oTimer.Start();
                }


                //iTotal = dTotAmt;
                lbl_tot.Text = string.Format("{0:#,##0}", dTotal);

                //Lst_ReOrder();
                //lbl_items.Text = iItemCount.ToString();  ///+ "총 주문 수량 : "  + iSumcnt.ToString();
                //lbl_orderitems.Text = iSumcnt.ToString();
                return dTotal;
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return 0;
            }

        }


        //이벤트 당첨 손님 표시
        public void Show_CongMsg(string sAccNum)
        {
            try
            {
                pan_Cong.Location = new Point(0, 0);
                pan_Cong.Size = new Size(1024, 768);
                pan_Cong.Visible = true;

                lbl_AccNum.Text = sAccNum + " 번째";
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

        }

       //폼 종료시
        public void Shut_Down()
        {
            try
            {
                if(bIsFullScreen == true)
                    Media_Player.fullScreen = false;


                Media_Player.Ctlcontrols.stop();
                this.Close();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
 
        }


        #region 기존 재생 함수
        //IGraphBuilder pGraphBuilder;
        //IMediaControl pMediaControl;
        //IVideoWindow pVideoWindow;
        //IMediaEvent pMediaEvent = null;
        //IBasicAudio pBasicAudio = null;
        //Boolean bIsMovie = false;

        //protected Thread m_thread;
        //protected ManualResetEvent m_shutdownEvent;
        //protected TimeSpan m_delay;

        //protected void ThreadMain()
        //{
        //    bool bSignaled = false;

        //    while (true)
        //    {
        //        Thread.Sleep(100);
        //        bSignaled = m_shutdownEvent.WaitOne(m_delay, true);
        //        if (bSignaled == true)
        //            break;

        //        string filePath = Application.StartupPath + @"\img\" + clsPrintSet.SettingHT["MOFILE"].ToString();//gongchaAd.avi

        //        Invoke(new EventHandler(delegate
        //        {
        //            SetupMovei(filePath);
        //        }));



        //    }
        //}


        //public void Turn_Movie_ON()
        //{

        //    if (bIsMovie == true)
        //    {
        //        bIsMovie = false;

        //        pMediaControl.Stop();
        //    }

        //    string filePath = Application.StartupPath + @"\img\" + clsPrintSet.SettingHT["MOFILE"].ToString();//gongchaAd.avi

        //    SetupMovei(filePath);

        //    double Length;
        //    IMediaPosition pMediaPosition = (IMediaPosition)pGraphBuilder;
        //    pMediaPosition.get_Duration(out Length);


        //    //m_delay = new TimeSpan(Int32.Parse(conHt["DAY"].ToString()), Int32.Parse(conHt["HOUR"].ToString()), Int32.Parse(conHt["MIN"].ToString()), Int32.Parse(conHt["SEC"].ToString()), Int32.Parse(conHt["MILSEC"].ToString()));
        //    m_delay = new TimeSpan(0, 0, (int)Length);
        //    ThreadStart ts = new ThreadStart(this.ThreadMain);
        //    m_shutdownEvent = new ManualResetEvent(false);
        //    m_thread = new Thread(ts);
        //    m_thread.IsBackground = true;
        //    m_thread.Start();



        //}

        //public void Turn_Movie_OFF()
        //{
        //    if (bIsMovie == true)
        //    {
        //        bIsMovie = false;

        //        pMediaControl.Stop();
        //    }
        //}

        //private void SetupMovei(string sFile)
        //{
        //    try
        //    {
        //        // web.Visible = false;
        //        pGraphBuilder = (IGraphBuilder)new FilterGraph();
        //        pMediaControl = (IMediaControl)pGraphBuilder;
        //        pMediaEvent = (IMediaEvent)pGraphBuilder;
        //        pVideoWindow = (IVideoWindow)pGraphBuilder;
        //        pBasicAudio = (IBasicAudio)pGraphBuilder;

        //        pGraphBuilder.RenderFile(sFile, null);

        //        pBasicAudio.put_Balance(0);
        //        pBasicAudio.put_Volume(Int32.Parse(clsPrintSet.SettingHT["VOL"].ToString()));


        //        pVideoWindow.put_Owner(this.pan_Show.Handle);
        //        pVideoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings);
        //        pVideoWindow.SetWindowPosition(0, 0, this.pan_Show.Width, this.pan_Show.Height);
        //        pVideoWindow.put_MessageDrain(this.pan_Show.Handle);
        //        pVideoWindow.put_Visible(OABool.True);

        //        if (pMediaControl == null)
        //        {
        //            return;
        //        }
        //        bIsMovie = true;
        //        pMediaControl.Run();
        //    }
        //    catch (Exception ex)
        //    {
        //        oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

        //    }
        //}

        #endregion





    }
}

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

using st_bread.CLASS.Van;
using st_bread.Bill;
using st_bread.Kiosk_Bill;
using Oracle.ManagedDataAccess.Client;

namespace st_bread
{
    public delegate void ClickMe(string idx);
    public delegate void ClickBtn(int idx);

    //숫자키 이벤트
    public delegate void ClickAccept(string sCnt);
    public delegate void ClickCancel();



    public partial class frmOrder_Type2 : Form,Inter_Bill,Inter_Table 
    {
        bool bModify_Mode = false;

        List<clsGr> oGr = null;
        

        List<uscGroup> oGroup = null;
        List<clsItems> oMast = null;
        List<clsKiosk_Item> lst_kiosk_items = null;

        List<clsKiosk_Bill> lst_Kiosk_bill = null; //구매한 키오스크 영수증

        clsBill oBill = null;


        clsFile oFile = new clsFile();
        string s_ID = string.Empty;

        uscGroup[] ouGroup = new uscGroup[10];
        uscMenu[] ouMenu = new uscMenu[25];
        uscButton[] ouBtn = new uscButton[10];
        uscButton[] ocBtn = new uscButton[10];

        double  dTotal = 0; //

        frmCustomDisplay oCustom = null;


        bool bCancelMode = false;


        #region DLL Import

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);


        #endregion


        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

       

        public frmOrder_Type2()
        {
            InitializeComponent();

            this.Size = new Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void frmOrder_Type2_Load(object sender, EventArgs e)
        {
            try
            {
                //인터넷 연결시만 DB에 연결 원 디비는 상품의 가격을 가져오는거 외엔 안쓴다.
                if (IsConnectedToInternet())
                {
                    Get_GrInfo();
                    Set_Gruop(0);
                    Set_MasterInfo();
                    Set_KioskItems();
                }


                //,상품이 없을때 상품 등록 메세지 표시
                if (oMast.Count == 0)
                {
                    lbl_Message.ForeColor = Color.Red;
                    lbl_Message.Text = "등록된 상품이 없습니다.";

                }

                //상품의 분류가 없을때 처리 디폴트값으로 현재 상품의 첫번째 분류 값을 쓴다.
                if (oGr.Count != 0)
                    Set_Menu(oGr[0].sGr_cd);
                else
                    Set_Menu(oMast[0].item_gr);

                //누적 고객 숫자 가져온다.
                lbl_AccCustom.Text = string.Format("{0:#,##0}", clsPosOpen.sale_customs );

                Set_Btn();

                lbl_SaleDate.Text = "영업일: " + clsDateTime.Get_Time(clsPosOpen.open_date).ToString("yyyy-MM-dd");

                lbl_User.Text = clsPos.store_name;
                lbl_PosNo.Text = "포스번호: " + clsPos.pos_num;

                //if (clsPrintSet.SettingHT["DUAL"].ToString() == "1")
                //{

                //}

                if (clsPos.isPrint)
                {
                    btn_Print.Image = Properties.Resources.btn_receipt;
                    btn_Print.Tag = "1";
                }
                else 
                {
                    btn_Print.Image = Properties.Resources.btn_receipt_no;
                    btn_Print.Tag = "2";
                }

                foreach (Button btn in clsSetting.GetAll(this.pan_Table_Btn, typeof(Button)))
                {
                    btn.Click += btn_Click;
                    btn.MouseHover += btn_MouseHover;
                    btn.MouseLeave += btn_MouseLeave;
 
                }


                Set_Table();

                Set_Panel(2);
            }
            catch (Exception ex)
            {

                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

        }

        //버튼에 마우스 떠날때
        void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackgroundImage = Properties.Resources.bg_btntype03;

            
        }

        //버튼에 마우스 올라갈때
        void btn_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackgroundImage = Properties.Resources.bg_btntype03_over;

        }


        private void frmOrder_Type2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (oCustom != null)
                oCustom.Set_AllClear();

            Clear_All();
        }



        #region Form Controls
       


        private void btn_Print_Click(object sender, EventArgs e)
        {

            if (btn_Print.Tag.ToString() == "1")
            {
                btn_Print.Image = Properties.Resources.btn_receipt_no;
                btn_Print.Tag = "2";
                clsPos.isPrint = false;
            }
            else if (btn_Print.Tag.ToString() == "2")
            {
                btn_Print.Image = Properties.Resources.btn_receipt;
                btn_Print.Tag = "1";
                clsPos.isPrint = true;
            }



        }


        //선택 이동
        private void btn_UP_Click_1(object sender, EventArgs e)
        {
            //버튼 이미지 처리
            btn_UP.Image = Properties.Resources.btn_pre_over;
            btn_Down.Image = Properties.Resources.btn_next;

            if (lst_Items.Items.Count == 0)
                return;

            try
            {

                if (lst_Items.SelectedItems.Count == 0)
                {

                    lst_Items.Items[0].Selected = true;
                    lst_Items.Select();
                }
                else
                {
                    int iSelectIdx = lst_Items.Items.IndexOf(lst_Items.SelectedItems[0]);

                    if ((iSelectIdx) > 0)
                    {
                        lst_Items.Items[iSelectIdx - 1].Selected = true;
                        lst_Items.Items[iSelectIdx - 1].EnsureVisible();
                        lst_Items.Select();


                    }
                    else
                    {
                        lst_Items.Items[lst_Items.Items.Count - 1].Selected = true;
                        lst_Items.Items[lst_Items.Items.Count - 1].EnsureVisible();
                        lst_Items.Select();

                    }


                }
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        private void btn_Down_Click_1(object sender, EventArgs e)
        {
            //버튼 이미지 처리
            btn_UP.Image = Properties.Resources.btn_pre;
            btn_Down.Image = Properties.Resources.btn_next_over;

            if (lst_Items.Items.Count == 0)
                return;

            try
            {
                if (lst_Items.SelectedItems.Count == 0)
                {
                    lst_Items.Items[0].Selected = true;
                    lst_Items.Select();
                }
                else
                {
                    int iSelectIdx = lst_Items.Items.IndexOf(lst_Items.SelectedItems[0]);

                    if ((iSelectIdx + 1) < lst_Items.Items.Count)
                    {
                        lst_Items.Items[iSelectIdx + 1].Selected = true;
                        lst_Items.Items[iSelectIdx + 1].EnsureVisible();
                        lst_Items.Select();
                    }
                    else
                    {
                        lst_Items.Items[0].Selected = true;
                        lst_Items.Items[0].EnsureVisible();
                        lst_Items.Select();

                    }
                }
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }

        }

        //그룹선택
        void SelectGroup(string sGr_cd)
        {
            if (bCancelMode)
                return;

            try
            {
                if (sGr_cd != null)
                {
                    //선택항목이미지 변경
                    for (int i = 0; i < 10; i++)
                    {
                        if (ouGroup[i].sGr_cd == sGr_cd)
                            ouGroup[i].SetPanelImg(1);
                        else
                            ouGroup[i].SetPanelImg(0);
                    }

                    Set_Menu(sGr_cd);
                }
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        //상품선택시
        void SelectMenu(string sMaster_cd)
        {
            if (bCancelMode)
                return;


            //리스트 가 0일때 이 이벤트 에 들어온다면 새로운 손님 으로 간주 한다.
            if (lst_Items.Items.Count == 0)
            {
                lbl_Rest.Text = "0";
                lbl_CustomPay.Text = "0";
                if( oCustom != null)
                    oCustom.Show_FullScreen_OFF();
            }
            if (sMaster_cd != null)
            {

                //선택항목이미지 변경
                for (int i = 0; i < 10; i++)
                {
                    if (ouMenu[i].sMaster_cd == sMaster_cd)
                        ouMenu[i].SetPanelImg(1);
                    else
                        ouMenu[i].SetPanelImg(0);
                }


                Boolean bFind = false;

                foreach (clsItems xMaster in oMast)
                {
                    if (xMaster.item_code  == sMaster_cd)
                    {
                        //리스트 돌면서 같은 상품이 있는지 체크
                        foreach (ListViewItem item in lst_Items.Items)
                        {
                            if (item.SubItems[1].Text == sMaster_cd)
                            {
                                //상품선택
                                lst_Items.Items[item.Index].Selected = true;
                                lst_Items.Items[item.Index].EnsureVisible();
                                lst_Items.Select();

                                //중복 상품 있음 수량 누적후 합계 계산
                                int iCount = Int32.Parse(item.SubItems[3].Text);
                                //double dCost = Double.Parse(item.SubItems[4].Text);

                                iCount++;
                                //double dTotal = dCost * iCount;

                                item.SubItems[3].Text = iCount.ToString();
                                
                                item.SubItems[5].Text = string.Format("{0:#,##0}", Check_Sub(item));

                                bFind = true;
                            }

                        }

                        if (bFind == false)
                        {
                            double dCost = 0;

                            //상품 없음 리스트에 추가
                            ListViewItem lstItm = new ListViewItem();
                            lstItm.Text = (lst_Items.Items.Count + 1).ToString();
                            lstItm.SubItems.Add(xMaster.item_code );
                            lstItm.SubItems.Add(xMaster.item_name );
                            lstItm.SubItems.Add("1");

                            lstItm.SubItems.Add(string.Format("{0:#,##0}", xMaster.item_cost));
                            lstItm.SubItems.Add(string.Format("{0:#,##0}", xMaster.item_cost));

                            lstItm.SubItems.Add(xMaster.item_gr );
                            lst_Items.Items.Add(lstItm);
                            lst_Items.Items[lst_Items.Items.Count - 1].Selected = true;
                            lst_Items.Items[lst_Items.Items.Count - 1].EnsureVisible();
                            lst_Items.Select();

                        }
                        Check_SUM();

                        
                        return;
                    }

                }

            }

        }

        //기능 버튼
        void SelectBtn(int iIdx)
        {
            if (bCancelMode)
            {
                clsbill_payments oPay = null;

                #region 영수증 불러온경우
                switch (iIdx)
                {
                    case (0): //결제 취소
                        if (oBill.oLstBill_payments.Count == 1)
                        {
                            
                            //지불방법이 한가지인 경우
                            switch (oBill.oLstBill_payments[0].bill_paymentskind)
                            {
                                case clsEnum.Payment_Kind.card:

                                    oPay = Auth(true, 0, oBill.oLstBill_payments[0]);
                                    if (oPay.sRepCode == "0000")
                                    {
                                        //Pay_Step(oPay);
                                        Finish();
                                    }
                                    break;
                                case clsEnum.Payment_Kind.cash:
                                    Cash_Cancel();
                                    Finish();
                                    break;
                                case clsEnum.Payment_Kind.cashwithaut:
                                    oPay = Auth(true, 1, oBill.oLstBill_payments[0]);
                                    
                                    if (oPay.sRepCode == "0000")
                                    {
                                        //Pay_Step(oPay);
                                        Finish();
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            //지불방법이 2가지 이상인경우
                            MessageBox.Show("2가지 이상 지불방법이 있는경우 취소할 수 없습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }

                        //if (oBill.bill_cardkind == "CA") //현금제외
                        //{
                            
                        //}
                        //else if (oBill.bill_cardkind == "ACA") //현금영수증
                        //{
                        //    //if (Auth(true, 1))
                        //    //{
                        //    //    Finish();
                        //    //}
                        //}
                        //else //카드 결제 취소
                        //{
                        //    //if (Auth(true, 0))
                        //    //{
                        //    //    Finish();
                        //    //} 
                        //}


                        break;
                    case (1): //재발행

                         using (clsPrint oPrint = new clsPrint())
                        {
                            oPrint.Print_Bill(oBill, true);
                        }
                        btn_CancelAll();

                        break;
                    case (2): //현금영수증

                        if (ocBtn[2].SetLabe1Text.Length > 0)
                        {
                            foreach (clsbill_payments oBillPay in oBill.oLstBill_payments)
                            {
                                if (oBillPay.bill_paymentskind == clsEnum.Payment_Kind.cash)
                                {
                                    oBill.bill_iscancel = clsEnum.bill_isCancel.reissue;
                                    oBillPay.bill_iscancel = clsEnum.bill_isCancel.reissue;


                                    oPay = Auth(false, 1, oBillPay);

                                    if (oPay.sRepCode == "0000")
                                    {
                                        oBillPay.bill_paymentamt = oPay.bill_paymentamt;
                                        oBillPay.bill_recvamt = oPay.bill_paymentamt;
                                        oBillPay.bill_restAmt = oBillPay.bill_restAmt;
                                        oBillPay.sRepCode = oPay.sRepCode;
                                        oBillPay.bill_buycmpny = oPay.bill_buycmpny;
                                        oBillPay.bill_cardcmpny = oPay.bill_cardcmpny;
                                        oBillPay.bill_cmpny = oPay.bill_cmpny;
                                        oBillPay.bill_vancmpny = oPay.bill_vancmpny;
                                        oBillPay.bill_cardnum = oPay.bill_cardnum;
                                        oBillPay.bill_authnum = oPay.bill_authnum;
                                        oBillPay.bill_tid = oPay.bill_tid;
                                        oBillPay.bill_OrgApprovalNum = oPay.bill_OrgApprovalNum;
                                        oBillPay.bill_signpath = "";
                                        oBillPay.bill_authdatetime = oPay.bill_authdatetime;
                                        oBillPay.bill_halbu = oPay.bill_halbu;

                                        oBillPay.bill_paymentskind = clsEnum.Payment_Kind.cashwithaut;
                                        oBillPay.bill_cardkind = "ACA";
                                        oBillPay.bill_iscancel = clsEnum.bill_isCancel.auth;

                                        //Pay_Step(oPay);
                                        Finish();
                                    }


                                }

                            }

                        }



                        





                            //현금영수증
                            //if (Auth(false, 1))
                            //{
                            //    oBill.bill_iscancel = clsEnum.bill_isCancel.reissue;

                            //    //oBill.bill_cardkind = "ACA";

                            //    Finish();
                            //}
                       

                        break;
                    case (4):
                        btn_CancelAll();
                        break;
                }
                #endregion
            }
            else
            {
                #region 평소모드
                switch (iIdx)
                {
                    case (0):
                        //"수량추가";
                        btn_Plus();
                        break;
                    case (1):
                        // "수량빼기";
                        btn_Minus();
                        break;

                    case (2):
                        // "수량변경";
                        btn_QtyChange();
                        break;
                    case (3):
                        // "지정취소";
                        btn_SelectCancel();
                        break;
                    case (4):
                        // "전체취소";
                        btn_CancelAll();
                        break;
                    case (5):
                        // "결제취소";
                        frmBill ofBill = new frmBill(this as Inter_Bill);
                        ofBill.ShowDialog();

                        break;
                    case (6): //쿠폰 구매
                        

                        if (oBill != null)
                        {
                            if (oBill.oLstBill_payments != null)
                            {
                                if (oBill.oLstBill_payments.Count > 0)
                                {
                                    lbl_Message.Text = "현재 결제 중입니다.";
                                }
                                else
                                {
                                    Set_Panel(1);
                                }
                            }
                            else
                            {
                                Set_Panel(1);
                            }
                        }
                        else
                        {
                            Set_Panel(1);
                        }
                        
                        break;
                    case (7): // "재발행";


                        Re_Print();
                        

                        break;
                    case (8): //입출금


                        
                        using (clsPrint oPrint = new clsPrint())
                        {
                            oPrint.Open_Drawer();
                        }

                        
                        frmCashIO oCashIO = new frmCashIO();
                        oCashIO.ShowDialog();
                        break;
                    case (9): // "환 전";
                        frmNFC oNfc = new frmNFC(this as Inter_Bill);
                        oNfc.ShowDialog();



                        
                        
                        break;
                    default:
                        //
                        break;
                }
                #endregion
            }
        }

        //종료 버튼
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                clsPos.Set_Pos_Info();

                //설정 화면으로 전환                
                frmMain newForm = new frmMain();
                newForm.Show();
                Program.ac.MainForm = newForm;
                this.Close();

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }

        private void Set_ClassToList(List<clsBill_Items> oLst)
        {
            if (oLst == null)
                return;
            if (oLst.Count == 0)
                return;

            foreach (clsBill_Items oBill_Item in oLst)
            {
                foreach (clsItems xMaster in oMast)
                {
                    if (xMaster.item_code == oBill_Item.item_code)
                    {

                        ListViewItem lstItm = new ListViewItem();
                        lstItm.Text = (lst_Items.Items.Count + 1).ToString();
                        lstItm.SubItems.Add(xMaster.item_code);
                        lstItm.SubItems.Add(xMaster.item_name);
                        //수량
                        lstItm.SubItems.Add(oBill_Item.item_qty.ToString());
                        lstItm.SubItems.Add(string.Format("{0:#,##0}", oBill_Item.item_cost));
                        //소계
                        lstItm.SubItems.Add(string.Format("{0:#,##0}", oBill_Item.dBill_Amt));
                        lstItm.SubItems.Add(xMaster.item_gr);
                        lst_Items.Items.Add(lstItm);
                        lst_Items.Items[lst_Items.Items.Count - 1].Selected = true;
                        lst_Items.Items[lst_Items.Items.Count - 1].EnsureVisible();
                        lst_Items.Select();
                        break;
                    }

                }
                Check_SUM();
            }
        }

        #endregion
        public void Send_SelectBill(string bill_date)
        {
            Clear_All();

            oBill = new clsBill(bill_date,oMast );

            //nfc결제 내역 파악
            lst_Kiosk_bill = new List<clsKiosk_Bill>();

            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_POS_NFC";
                cmd_SEL.Parameters.Add("posbill", OracleDbType.Varchar2).Value = oBill.bill_Code;                
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {
                    clsKiosk_Bill okioskbill = new clsKiosk_Bill(dr["KIOSK_BILL"].ToString());
                    if (okioskbill.bill_date != null)
                        lst_Kiosk_bill.Add(okioskbill);
                }


                //영수증 내용 화면에 표시
                //리스트 가 0일때 이 이벤트 에 들어온다면 새로운 손님 으로 간주 한다.
                if (lst_Items.Items.Count == 0)
                {
                    lbl_Rest.Text = "0";
                    lbl_CustomPay.Text = "0";
                    if (oCustom != null)
                        oCustom.Show_FullScreen_OFF();
                }

                Set_ClassToList(oBill.oLstBill_Items);

                //foreach (clsBill_Items oBill_Item in oBill.oLstBill_Items)
                //{
                //    foreach (clsItems xMaster in oMast)
                //    {
                //        if (xMaster.item_code == oBill_Item.item_code)
                //        {

                //            ListViewItem lstItm = new ListViewItem();
                //            lstItm.Text = (lst_Items.Items.Count + 1).ToString();
                //            lstItm.SubItems.Add(xMaster.item_code);
                //            lstItm.SubItems.Add(xMaster.item_name);
                //            //수량
                //            lstItm.SubItems.Add(oBill_Item.item_qty.ToString() );
                //            lstItm.SubItems.Add(string.Format("{0:#,##0}", oBill_Item.item_cost));
                //            //소계
                //            lstItm.SubItems.Add(string.Format("{0:#,##0}", oBill_Item.dBill_Amt));
                //            lstItm.SubItems.Add(xMaster.item_gr);
                //            lst_Items.Items.Add(lstItm);
                //            lst_Items.Items[lst_Items.Items.Count - 1].Selected = true;
                //            lst_Items.Items[lst_Items.Items.Count - 1].EnsureVisible();
                //            lst_Items.Select();
                //            break;
                //        }
 
                //    }
                //    Check_SUM();
                //}





                bCancelMode = true;

                pan_Cancel.Visible = true;


                ocBtn[0].SetLabe1Text = "결제취소";
                ocBtn[1].SetLabe1Text = "재발행";
                ocBtn[2].SetLabe1Text = "";
                
                foreach (clsbill_payments oPay in oBill.oLstBill_payments)
                {
                    if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cash)
                    {
                        ocBtn[2].SetLabe1Text = "현금영수증";
                        break;
                    }
                }


                ocBtn[4].SetLabe1Text = "취소";









                //if (oBill.bill_cardkind == "CA") //현금제외
                //{
                    
                //    ocBtn[1].SetLabe1Text = "현금영수증";
                //    ocBtn[2].SetLabe1Text = "재발행";
                //    ocBtn[4].SetLabe1Text = "취소";

                //}
                //else if (oBill.bill_cardkind == "ACA") //현금영수증
                //{
                //    ocBtn[0].SetLabe1Text = "결제취소";                    
                //    ocBtn[1].SetLabe1Text = "재발행";
                //    ocBtn[4].SetLabe1Text = "취소";
                //    //if (Auth(true, 1))
                //    //{
                //    //    Finish();
                //    //}
                //}
                //else
                //{
                    

                //    //영수증 화면 표시


                //    //그외 카드
                //    //결제 취소
                //    //Auth(true, 0);

                //    //Finish();
                //}


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


        public void Send_SelectBill(string pos_num,string bill_date)
        {
            Clear_All();

            oBill = new clsBill(pos_num,bill_date, oMast);

            //nfc결제 내역 파악
            lst_Kiosk_bill = new List<clsKiosk_Bill>();

            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_POS_NFC";
                cmd_SEL.Parameters.Add("posbill", OracleDbType.Varchar2).Value = oBill.bill_Code;
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {
                    clsKiosk_Bill okioskbill = new clsKiosk_Bill(dr["KIOSK_BILL"].ToString());
                    if (okioskbill.bill_date != null)
                        lst_Kiosk_bill.Add(okioskbill);
                }


                //영수증 내용 화면에 표시
                //리스트 가 0일때 이 이벤트 에 들어온다면 새로운 손님 으로 간주 한다.
                if (lst_Items.Items.Count == 0)
                {
                    lbl_Rest.Text = "0";
                    lbl_CustomPay.Text = "0";
                    if (oCustom != null)
                        oCustom.Show_FullScreen_OFF();
                }

                Set_ClassToList(oBill.oLstBill_Items);

                //foreach (clsBill_Items oBill_Item in oBill.oLstBill_Items)
                //{
                //    foreach (clsItems xMaster in oMast)
                //    {
                //        if (xMaster.item_code == oBill_Item.item_code)
                //        {

                //            ListViewItem lstItm = new ListViewItem();
                //            lstItm.Text = (lst_Items.Items.Count + 1).ToString();
                //            lstItm.SubItems.Add(xMaster.item_code);
                //            lstItm.SubItems.Add(xMaster.item_name);
                //            //수량
                //            lstItm.SubItems.Add(oBill_Item.item_qty.ToString() );
                //            lstItm.SubItems.Add(string.Format("{0:#,##0}", oBill_Item.item_cost));
                //            //소계
                //            lstItm.SubItems.Add(string.Format("{0:#,##0}", oBill_Item.dBill_Amt));
                //            lstItm.SubItems.Add(xMaster.item_gr);
                //            lst_Items.Items.Add(lstItm);
                //            lst_Items.Items[lst_Items.Items.Count - 1].Selected = true;
                //            lst_Items.Items[lst_Items.Items.Count - 1].EnsureVisible();
                //            lst_Items.Select();
                //            break;
                //        }

                //    }
                //    Check_SUM();
                //}





                bCancelMode = true;

                pan_Cancel.Visible = true;


                ocBtn[0].SetLabe1Text = "결제취소";
                ocBtn[1].SetLabe1Text = "재발행";
                ocBtn[2].SetLabe1Text = "";

                foreach (clsbill_payments oPay in oBill.oLstBill_payments)
                {
                    if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cash)
                    {
                        ocBtn[2].SetLabe1Text = "현금영수증";
                        break;
                    }
                }


                ocBtn[4].SetLabe1Text = "취소";









                //if (oBill.bill_cardkind == "CA") //현금제외
                //{

                //    ocBtn[1].SetLabe1Text = "현금영수증";
                //    ocBtn[2].SetLabe1Text = "재발행";
                //    ocBtn[4].SetLabe1Text = "취소";

                //}
                //else if (oBill.bill_cardkind == "ACA") //현금영수증
                //{
                //    ocBtn[0].SetLabe1Text = "결제취소";                    
                //    ocBtn[1].SetLabe1Text = "재발행";
                //    ocBtn[4].SetLabe1Text = "취소";
                //    //if (Auth(true, 1))
                //    //{
                //    //    Finish();
                //    //}
                //}
                //else
                //{


                //    //영수증 화면 표시


                //    //그외 카드
                //    //결제 취소
                //    //Auth(true, 0);

                //    //Finish();
                //}


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


        public void Send_NFC(string card_num)
        {

            if (lst_Kiosk_bill != null)
            {
                lbl_Message.Text = "이미 NFC 카드를 읽어 왔습니다.";
                return;
            }

            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;

            int iStartDate = clsDateTime.StartOfDay(DateTime.Now );
            int iDate = clsDateTime.EndOfDay(DateTime.Now );

            double dSum = 0;
            bool bFind = false;

            lst_Kiosk_bill = new List<clsKiosk_Bill>();

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Bill";
                cmd_SEL.Parameters.Add("startdate", OracleDbType.Varchar2).Value = iStartDate;
                cmd_SEL.Parameters.Add("enddate", OracleDbType.Varchar2).Value = iDate;
                cmd_SEL.Parameters.Add("cardnum", OracleDbType.Varchar2).Value = card_num;
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                if (dt.Rows.Count == 0)
                {
                    lbl_Message.Text = "현재 카드로 결제한 내역이 없습니다.";
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dSum += Convert.ToDouble(dr["bill_totalamt"]);

                        clsKiosk_Bill okioskbill = new clsKiosk_Bill(dr["bill_code"].ToString());
                        if(okioskbill.bill_date != null)
                            lst_Kiosk_bill.Add(okioskbill);
                    }
                    
                    //리스트 돌면서 같은 상품이 있는지 체크
                    foreach (ListViewItem item in lst_Items.Items)
                    {
                        if (item.SubItems[1].Text == "99999")
                        {
                            //상품선택
                            lst_Items.Items[item.Index].Selected = true;
                            lst_Items.Items[item.Index].EnsureVisible();
                            lst_Items.Select();
                            bFind = true;
                        }
                    }


                    if (bFind == false)
                    {
                        double dCost = 0;

                        //상품 없음 리스트에 추가
                        ListViewItem lstItm = new ListViewItem();
                        lstItm.Text = (lst_Items.Items.Count + 1).ToString();
                        lstItm.SubItems.Add("99999");
                        lstItm.SubItems.Add("NFC 카드");
                        lstItm.SubItems.Add("1");

                        lstItm.SubItems.Add(string.Format("{0:#,##0}", dSum));
                        lstItm.SubItems.Add(string.Format("{0:#,##0}", dSum));

                        lstItm.SubItems.Add("");
                        lst_Items.Items.Add(lstItm);
                        lst_Items.Items[lst_Items.Items.Count - 1].Selected = true;
                        lst_Items.Items[lst_Items.Items.Count - 1].EnsureVisible();
                        lst_Items.Select();

                    }
                    Check_SUM();


                    List<clsKiosk_Item> lst_buykioskitems = new List<clsKiosk_Item>();
                    

                    //키오스크 구매 상품 표시
                    foreach (clsKiosk_Bill okbill in lst_Kiosk_bill)
                    {
                        OracleCommand oCmd = new OracleCommand();
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.CommandText = "PROC_POS.Get_Bill_Items";
                        oCmd.Parameters.Add("machinecode", OracleDbType.Varchar2).Value = okbill.bill_machine;
                        oCmd.Parameters.Add("billdate", OracleDbType.Varchar2).Value = okbill.bill_date;                    
                        oCmd.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        oCmd.BindByName = true;

                        //이름및 로그인 정보 가져온다.
                        dt = clsDBExcute.SelectQuery(oCmd);

                        foreach (DataRow dr in dt.Rows)
                        {
                            clsKiosk_Item okItem = new clsKiosk_Item();
                            okItem.item_code = dr["BILL_ITEM_CODE"].ToString();

                            foreach (clsKiosk_Item oTemp in lst_kiosk_items)
                            {
                                if (oTemp.item_code == okItem.item_code)
                                {
                                    okItem.item_name = oTemp.item_name;
                                    okItem.item_capa = Convert.ToDouble(dr["BILL_CAPA"]);
                                    okItem.item_dg = Convert.ToDouble(dr["BILL_DG"]);
                                    okItem.item_price = Convert.ToDouble(dr["BILL_AMT"]);
                                    lst_buykioskitems.Add(okItem);
                                    break;
                                }
                               
                            }
                        }
                    }

                    if (lst_buykioskitems.Count > 0)
                    {
                        foreach (clsKiosk_Item oTemp in lst_buykioskitems)
                        {
                            ListViewItem oItem = new ListViewItem();
                            oItem.Text = oTemp.item_name;
                            oItem.SubItems.Add(clsSetting.Let_Money(oTemp.item_price));
                            lst_Kiosk.Items.Add(oItem);

                        }

                    }









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
            }


 
        }


        

        #region DB 값 이용

        private void Re_Print()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            
            int iStartDate = clsDateTime.StartOfDay(DateTime.Now );


            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Bill";

                cmd_SEL.Parameters.Add("MACHINECODE", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("startdate", OracleDbType.Varchar2).Value = iStartDate;
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);
                string bill_date = string.Empty;

                if(dt.Rows.Count  == 0)
                {
                    lbl_Message.Text = "영수증이 없습니다.";
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["rw"].ToString() == "1")
                        {
                            bill_date = dr["bill_date"].ToString();
                        }
                    }

                    oBill = new clsBill(bill_date, oMast);

                    using (clsPrint oPrint = new clsPrint())
                    {
                        oPrint.Print_Bill(oBill, true);
                    }
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

                btn_CancelAll();
            }
        }


        //매장 직원 정보
        private int Get_MemAd(string sID)
        {
            //try
            //{
            //    OdbcCommand oCmd = conn_local.CreateCommand();
            //    oCmd.CommandText = " SELECT * FROM memad  " +
            //             " WHERE memad_id = ? ";
            //    oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = sID;


            //    OdbcDataReader reader = oCmd.ExecuteReader();


            //    if (reader.Read())
            //    {
            //        oMemad = new clsMemad();
            //        oMemad.memad_jum = reader["memad_jum"].ToString();
            //        oMemad.memad_mart_cd = reader["memad_mart_cd"].ToString();
            //        oMemad.memad_id = reader["memad_id"].ToString();
            //        oMemad.memad_pw = reader["memad_pw"].ToString();
            //        oMemad.memad_nm = reader["memad_nm"].ToString();
            //        oMemad.memad_div = reader["memad_div"].ToString();
            //        oMemad.memad_leave = reader["memad_leave"].ToString();
            //        oMemad.memad_web = reader["memad_web"].ToString();


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

        

        //상품그룹정보
        private void Get_GrInfo()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            oGr = new List<clsGr>();


            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.get_POS_GR";
                
                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("cur_pos", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {
                    clsGr oTempGr = new clsGr();
                    oTempGr.sGr_pos = dr["pos"].ToString();
                    oTempGr.sGr_cd = dr["gr_code"].ToString();
                    oTempGr.sGr_nm = dr["gr_name"].ToString();
                    oGr.Add(oTempGr);
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
            }
        }

        //최초 상품 마스터 로딩
        private void Set_MasterInfo()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            oMast = new List<clsItems>();

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.get_POS_ITEMS";

                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("cur_pos", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {
                    clsItems oTemp = new clsItems();

                    oTemp.pos_num = dr["POS"].ToString();
                    oTemp.item_code  = dr["item_code"].ToString();
                    oTemp.item_name  = dr["item_name"].ToString();
                    oTemp.item_gr  = dr["item_gr"].ToString();
                    oTemp.item_seq = Convert.ToInt32(dr["POS"]);
                    oTemp.item_cost = Convert.ToDouble(dr["item_cost"]);
                    oTemp.item_vat = (clsEnum.Item_vat)Convert.ToDouble(dr["item_vat"]);

                    oMast.Add(oTemp);
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
            }

        }

        

        private void Set_KioskItems()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            lst_kiosk_items = new List<clsKiosk_Item>();

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Select_Item";
                cmd_SEL.Parameters.Add("cur_item", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {
                    clsKiosk_Item oTemp = new clsKiosk_Item();

                    
                    oTemp.item_code = dr["item_code"].ToString();
                    oTemp.item_name = dr["item_name"].ToString();
                    
                    lst_kiosk_items.Add(oTemp);
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
            }

        }

        //날짜의 마지막 영수증 찾아오기
        private int Get_PayNum()
        {
            int iReturn = 0;

            //try
            //{
            //    OdbcCommand oCmd = conn_local.CreateCommand();
            //    oCmd.CommandText = " SELECT pay_no FROM pay   " +
            //             " WHERE pay_jum = ? AND pay_mart_cd = ? AND pay_date = ?  ORDER BY pay_no DESC LIMIT 1";

            //    oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
            //    oCmd.Parameters.Add("@MART", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            //    oCmd.Parameters.Add("@DATE", SqlDbType.Char).Value = dtOpenDate.ToString("yyyy-MM-dd");

            //    OdbcDataReader reader = oCmd.ExecuteReader();

            //    if (reader.Read())
            //    {   
            //        iReturn = Int32.Parse(reader["pay_no"].ToString());
            //    }
            //    else
            //    {
            //        iReturn = 0; //오늘 첫전표
 
            //    }
            //    reader.Close();

            //}
            //catch (Exception ex)
            //{
            //    oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            //    return 0;
            //}
            return iReturn;
        }

        //매장의 누적 고객 카운트
        private int Get_AccCustoumCnt()
        {
            int iReturn = 0;

            try
            {
                iReturn = 0;

                //OdbcCommand oCmd = conn_local.CreateCommand();
                //oCmd.CommandText = " SELECT pay_customcnt FROM pay   " +
                //         " WHERE pay_jum = ? AND pay_mart_cd = ?  ORDER BY pay_customcnt DESC LIMIT 1";

                //oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
                //oCmd.Parameters.Add("@MART", SqlDbType.Char).Value = oMemad.memad_mart_cd;

                //OdbcDataReader reader = oCmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    iReturn = Int32.Parse(reader["pay_customcnt"].ToString());
                //}
                //else
                //{
                //    iReturn = 0; //오늘 첫전표

                //}
                //reader.Close();

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

                
                return 0;
            }
            return iReturn;
        }
        #endregion


        #region 함수
        //그룹 아이콘 표시
        private void Set_Gruop(int iPage) //0=초기 판넬 1=판매판넬
        {
           
            int iMenu = 0;
            int iPosition = 0;

            pan_Group.Controls.Clear();
            System.Array.Clear(ouGroup, 0, 10);
            //uscGroup[] ouGroup = new uscGroup[10];
           
            foreach (clsGr xGroup in oGr)
            {
                ouGroup[iMenu] = new uscGroup();
                ouGroup[iMenu].Size = new Size(clsOrderType2_Layer.Btn_Width(), clsOrderType2_Layer.Btn_Height() );
                ouGroup[iMenu].Location = new Point(
                    clsOrderType2_Layer.Btn_Margin() + iMenu % clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Width() + clsOrderType2_Layer.Btn_Margin()),
                    clsOrderType2_Layer.Btn_Margin() + iMenu / clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Height() + clsOrderType2_Layer.Btn_Margin())
                    );

                ouGroup[iMenu].sGr_cd = xGroup.sGr_cd;
                ouGroup[iMenu].sGr_Name = xGroup.sGr_nm;
                ouGroup[iMenu].SetLabe1Text = xGroup.sGr_nm;
                ouGroup[iMenu].ClickGroup += new ClickMe(SelectGroup);

                pan_Group.Controls.Add(ouGroup[iMenu]);

                iMenu++;
                Application.DoEvents();
            }


            for (int i = iMenu; i < 10; i++)
            {
                ouGroup[i] = new uscGroup();
                ouGroup[i].Size = new Size(clsOrderType2_Layer.Btn_Width(), clsOrderType2_Layer.Btn_Height());
                ouGroup[i].Location = new Point(
                    clsOrderType2_Layer.Btn_Margin() + i % clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Width() + clsOrderType2_Layer.Btn_Margin()),
                    clsOrderType2_Layer.Btn_Margin() + i / clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Height() + clsOrderType2_Layer.Btn_Margin())
                    );
                ouGroup[i].SetLabe1Text = "";
                ouGroup[i].ClickGroup += new ClickMe(SelectGroup);

                pan_Group.Controls.Add(ouGroup[i]);

                Application.DoEvents();
            }
        }

        //메뉴 아이콘 표시
        private void Set_Menu(string sGr_cd) //0=초기 판넬 1=판매판넬
        {   
            int iMenu = 0;
            int iPosition = 0;

            pan_Menu.Controls.Clear();
            System.Array.Clear(ouMenu, 0, 16);

            //uscMenu[] ouMenu = new uscMenu[25];
            foreach (clsItems xMaster in oMast )
            {
                if (xMaster.item_gr  == sGr_cd)
                {
                    ouMenu[iMenu] = new uscMenu();
                    ouMenu[iMenu].Size = new Size(clsOrderType2_Layer.Btn_Width_Menu(), clsOrderType2_Layer.Btn_Height_Menu());
                    ouMenu[iMenu].Location = new Point(
                        clsOrderType2_Layer.Btn_Margin() + iMenu % clsOrderType2_Layer.Btn_Column_Menu() * (clsOrderType2_Layer.Btn_Width_Menu() + clsOrderType2_Layer.Btn_Margin()),
                        clsOrderType2_Layer.Btn_Margin() + iMenu / clsOrderType2_Layer.Btn_Column_Menu() * (clsOrderType2_Layer.Btn_Height_Menu() + clsOrderType2_Layer.Btn_Margin())
                        );

                    ouMenu[iMenu].sMaster_cd = xMaster.item_code;
                    ouMenu[iMenu].sMaster_nm = xMaster.item_name;
                    ouMenu[iMenu].iMaster_cost = xMaster.item_cost;
                    ouMenu[iMenu].SetLabe1Text = xMaster.item_name;
                    ouMenu[iMenu].SetLabe2Text = string.Format("{0:#,##0}", xMaster.item_cost);  
                    ouMenu[iMenu].BackColor = Color.Aqua;
                    ouMenu[iMenu].ClickMenu += new ClickMe(SelectMenu);


                    pan_Menu.Controls.Add(ouMenu[iMenu]);

                    iMenu++;
                }
                Application.DoEvents();
            }



            for (int i = iMenu; i < 16; i++)
            {
                ouMenu[i] = new uscMenu();
                ouMenu[i].Size = new Size(clsOrderType2_Layer.Btn_Width_Menu(), clsOrderType2_Layer.Btn_Height_Menu());
                ouMenu[i].Location = new Point(
                    clsOrderType2_Layer.Btn_Margin() + i % clsOrderType2_Layer.Btn_Column_Menu() * (clsOrderType2_Layer.Btn_Width_Menu() + clsOrderType2_Layer.Btn_Margin()),
                    clsOrderType2_Layer.Btn_Margin() + i / clsOrderType2_Layer.Btn_Column_Menu() * (clsOrderType2_Layer.Btn_Height_Menu() + clsOrderType2_Layer.Btn_Margin())
                    );

                ouMenu[i].SetLabe1Text = "";
                ouMenu[i].SetLabe2Text = "";
                ouMenu[i].BackColor = Color.Aqua;
                ouMenu[i].ClickMenu += new ClickMe(SelectMenu);

                pan_Menu.Controls.Add(ouMenu[i]);

                Application.DoEvents();
            }
        }

        //버튼 동적 생성
        private void Set_Btn()
        {
            
            
            int iMenu = 0;
            int iPosition = 0;

            pan_Button.Controls.Clear();
            System.Array.Clear(ouBtn, 0, 10);

            for (int i = 0; i < 10; i++)
            {
                ouBtn[i] = new uscButton();
                ouBtn[i].Size = new Size(clsOrderType2_Layer.Btn_Width(), clsOrderType2_Layer.Btn_Height());
                ouBtn[i].Location = new Point(
                    clsOrderType2_Layer.Btn_Margin() + i % clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Width() + clsOrderType2_Layer.Btn_Margin()),
                    clsOrderType2_Layer.Btn_Margin() + i / clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Height() + clsOrderType2_Layer.Btn_Margin())
                    );

                ouBtn[i].iIdx = i;

                //메뉴 배열에 추가 후 SelectBtn 에서 기능 추가 할것
                switch (i)
                {
                    case (0):
                        ouBtn[i].SetLabe1Text = "수량추가";
                        break;
                    case (1):
                        ouBtn[i].SetLabe1Text = "수량빼기";
                        break;

                    case (2): 
                        ouBtn[i].SetLabe1Text = "수량변경";
                        break;
                    case (3):
                        ouBtn[i].SetLabe1Text = "지정취소";
                        break;
                    case (4):
                        ouBtn[i].SetLabe1Text = "전체취소";
                        break;
                    case (5):
                        ouBtn[i].SetLabe1Text = "영수증";
                        break;
                    case (6):
                        ouBtn[i].SetLabe1Text = "테이블";
                        break;
                    case (7):
                        ouBtn[i].SetLabe1Text = "재발행";
                        break;
                    case (8):
                        ouBtn[i].SetLabe1Text = "입출/환전";
                        break;
                    case (9):
                        ouBtn[i].SetLabe1Text = "NFC";
                        break;
                    default:
                        ouBtn[i].SetLabe1Text = "";
                        break;

                }

                ouBtn[i].BackColor = Color.Aqua;
                ouBtn[i].ClickButton  += new ClickBtn(SelectBtn);

                pan_Button.Controls.Add(ouBtn[i]);

                Application.DoEvents();
            }

            iMenu = 0;
            iPosition = 0;

            pan_Cancel.Controls.Clear();

            System.Array.Clear(ocBtn, 0, 10);

            for (int x = 0; x < 10; x++)
            {
                ocBtn[x] = new uscButton();
                ocBtn[x].Size = new Size(clsOrderType2_Layer.Btn_Width(), clsOrderType2_Layer.Btn_Height());
                ocBtn[x].Location = new Point(
                    clsOrderType2_Layer.Btn_Margin() + x % clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Width() + clsOrderType2_Layer.Btn_Margin()),
                    clsOrderType2_Layer.Btn_Margin() + x / clsOrderType2_Layer.Btn_Column() * (clsOrderType2_Layer.Btn_Height() + clsOrderType2_Layer.Btn_Margin())
                    );

                ocBtn[x].iIdx = x;

                //메뉴 배열에 추가 후 SelectBtn 에서 기능 추가 할것
                switch (x)
                {
                    case (0):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (1):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (2):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (3):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (4):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (5):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (6):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (7):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (8):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    case (9):
                        ocBtn[x].SetLabe1Text = "";
                        break;
                    default:
                        ocBtn[x].SetLabe1Text = "";
                        break;

                }



                ocBtn[x].BackColor = Color.Aqua;
                ocBtn[x].ClickButton += new ClickBtn(SelectBtn);

                pan_Cancel.Controls.Add(ocBtn[x]);

                Application.DoEvents();
            }
        }

        //수량변경등 함수
        //전체 취소
        private void btn_CancelAll()
        {
            if (oCustom != null)
                oCustom.Set_AllClear();

            //테이블 주문 내역있으면 삭제
            if (oSelectedTable != null)
            {
                if (MessageBox.Show("테이블 주문 내역을 삭제 하시겠습니까?", "information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (oSelectedTable.TableState == clsEnum.Table_State.table_Assem) //합쳐진 테이블이면 관련 테이블 다 지운다.
                    {
                        Cancel_Assembly();
                    }
                    else
                    {
                        oSelectedTable.TableState = clsEnum.Table_State.table;
                        oSelectedTable.Clear_Order();

                    }
                    Clear_All();
                }
            }
            else
            {
                Clear_All();
            }


            
        }

        //지정 취소
        private void btn_SelectCancel()
        {
            if (lst_Items.SelectedItems.Count == 0)
            {

            }
            else
            {
                while (lst_Items.SelectedItems.Count > 0)
                    lst_Items.Items.Remove(lst_Items.SelectedItems[0]);
            }
            
            Check_SUM();
        }

        //수량 추가
        private void btn_Plus()
        {
            if (lst_Items.Items.Count == 0)
                return;
            if (lst_Items.SelectedItems.Count == 0)
                return;

            int iSelectIdx = lst_Items.Items.IndexOf(lst_Items.SelectedItems[0]);
            int iCount = Int32.Parse(lst_Items.Items[iSelectIdx].SubItems[3].Text);
            iCount++;
            lst_Items.Items[iSelectIdx].SubItems[3].Text = iCount.ToString();
            lst_Items.Items[iSelectIdx].SubItems[5].Text = string.Format("{0:#,##0}", Check_Sub(lst_Items.Items[iSelectIdx]));
            
            Check_SUM();

        }

        //수량 빼기
        private void btn_Minus()
        {
            if (lst_Items.Items.Count == 0)
                return;
            if (lst_Items.SelectedItems.Count == 0)
                return;

            int iSelectIdx = lst_Items.Items.IndexOf(lst_Items.SelectedItems[0]);
            int iCount = Int32.Parse(lst_Items.Items[iSelectIdx].SubItems[3].Text);
            iCount--;

            if (iCount == 0)
            {
                lst_Items.Items.Remove(lst_Items.SelectedItems[0]);
            }
            else
            {

                lst_Items.Items[iSelectIdx].SubItems[3].Text = iCount.ToString();
                lst_Items.Items[iSelectIdx].SubItems[5].Text = string.Format("{0:#,##0}", Check_Sub(lst_Items.Items[iSelectIdx]));
            }
            
            Check_SUM();

        }

        //수량 변경
        private void btn_QtyChange()
        {
            if (lst_Items.Items.Count == 0)
                return;

            if (lst_Items.SelectedItems.Count == 0)
                return;

            int iSelectIdx = lst_Items.Items.IndexOf(lst_Items.SelectedItems[0]);

            uscNum oNum = new uscNum();
            oNum.SetLabe1Text = lst_Items.Items[iSelectIdx].Text + " 번 " + lst_Items.Items[iSelectIdx].SubItems[2].Text;
            oNum.SetLabe2Text = "수량을 변경 하시겠습니까? ";
            oNum.SetLabe3Text = "기존 수량 " + lst_Items.Items[iSelectIdx].SubItems[3].Text + " 개 ";             
            oNum.ClickOK += new ClickAccept(Num_Accept);
            oNum.ClickCancel += new ClickCancel(Num_Cancel);
            pan_QtyChange.Controls.Add(oNum);
            pan_QtyChange.Visible = true;
            pan_QtyChange.Location = new Point(250, 200);
            pan_QtyChange.Size = new Size(640, 371);
            pan_QtyChange.BringToFront();


        }

        //수량 변경 화면 의 버튼
        private void Num_Cancel()
        {
            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;
        }
        private void Num_Accept(string sNum)
        {
            if (sNum == "0" )
            {
                lst_Items.Items.Remove(lst_Items.SelectedItems[0]);

            }
            else
            {
                int iSelectIdx = lst_Items.Items.IndexOf(lst_Items.SelectedItems[0]);
                lst_Items.Items[iSelectIdx].SubItems[3].Text = sNum;

                lst_Items.Items[iSelectIdx].SubItems[5].Text = string.Format("{0:#,##0}", Check_Sub(lst_Items.Items[iSelectIdx]));
            }

            Check_SUM();
            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;
        }


        private double Check_Sub(ListViewItem iItem)
        {
            //중복 상품 있음 수량 누적후 합계 계산
            int iCount = Int32.Parse(iItem.SubItems[3].Text);
            double dCost = Double.Parse(iItem.SubItems[4].Text);

            double dTotal = dCost * iCount;

            return dTotal;
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

                //iTotal = dTotAmt;
                lbl_tot.Text = string.Format("{0:#,##0}", dTotal);

                Lst_ReOrder();

                if (oCustom != null)
                    oCustom.Set_List(lst_Items);

                return dTotal;
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return 0;
            }
 
        }

        //주문 리스트의 순번 재정렬
        private void Lst_ReOrder()
        {
            int iIdx = 1;
            foreach (ListViewItem item in lst_Items.Items)
            {
                item.Text = iIdx.ToString();
                iIdx++;
            }
        }


        private void Clear_All()
        {
            lbl_tot.Text = "0";
            lbl_SalePrice.Text = "0";
            lbl_CustomPay.Text = "0";
            lbl_Rest.Text = "0";

            dTotal = 0;
            dRecv = 0;
            dRest = 0;

            lst_Items.Items.Clear();
            lst_Kiosk.Items.Clear();

            lst_Kiosk_bill = null;
            oBill = null;

            if (bCancelMode)
            {
                bCancelMode = false;
                pan_Cancel.Visible = false;
            }

        }

        #endregion


        #region 결제 부분

        double dRest = 0;
        double dRecv = 0;
        clsbill_payments oHalu_Pay = null;
        

        public void Send_Recv(double _dRest, double _dRecieved,clsEnum.Payment_Kind oKind)
        {
            if (oCustom != null)
            {
                oCustom.Set_Pay(lbl_Rest.Text, lbl_CustomPay.Text);
            }

            if (oKind == clsEnum.Payment_Kind.card)
            {
                if (_dRecieved <= 49999)
                {
                    clsbill_payments _oPay = new clsbill_payments();
                    _oPay.bill_halbu = "00";
                    _oPay.bill_paymentamt = _dRecieved;


                    clsbill_payments oPay = Auth(false, 0, _oPay);
                    if (oPay.sRepCode == "0000")
                    {
                        Pay_Step(oPay);
                        //Finish();
                    }
                }
                else
                {



                    oHalu_Pay = new clsbill_payments();
                    oHalu_Pay.bill_paymentamt = _dRecieved;


                    uscNum oNum = new uscNum();

                    oNum.SetLabe1Text = "할부 기간을 정해주세요.";
                    oNum.SetLabe2Text = "1 - 12개월 ";
                    oNum.SetLabe3Text = "";

                    oNum.ClickOK += new ClickAccept(Num_Hal);
                    oNum.ClickCancel += new ClickCancel(Num_Hal_Cancel);

                    pan_QtyChange.Controls.Add(oNum);
                    pan_QtyChange.Visible = true;
                    pan_QtyChange.Location = new Point(250, 200);
                    pan_QtyChange.Size = new Size(640, 371);
                    pan_QtyChange.BringToFront();
                }



            }
            else if(oKind == clsEnum.Payment_Kind.cash)
            {


                frmCash2 oCash2 = new frmCash2();
                oCash2.oOwner = this;
                oCash2.Size = new Size(800, 444);
                oCash2.StartPosition = FormStartPosition.CenterParent;

                DialogResult dResult = oCash2.ShowDialog(this);

                if (dResult == DialogResult.Yes)
                {
                    clsbill_payments _oPay = new clsbill_payments();
                    _oPay.bill_halbu = "00";
                    _oPay.bill_paymentamt = _dRecieved;


                    clsbill_payments oPay = Auth(false, 1,_oPay );
                    if (oPay.sRepCode == "0000")
                    {
                        Pay_Step(oPay);
                        //Finish();
                    }

                }
                else if (dResult == DialogResult.Abort)
                {

                    clsbill_payments _oPay = new clsbill_payments();
                    _oPay.bill_halbu = "00";
                    _oPay.bill_paymentamt = _dRecieved;


                    clsbill_payments oPay = Auth(false, 2,_oPay );
                    if (oPay.sRepCode == "0000")
                    {
                        Pay_Step(oPay);
                        //Finish();
                    }

                }
                else if (dResult == DialogResult.No)
                {
                    clsbill_payments obillpay = new clsbill_payments();
                    obillpay.bill_paymentamt = _dRecieved;
                    obillpay.bill_recvamt = _dRecieved;
                    obillpay.bill_restAmt = _dRest;
                    obillpay.sRepCode = "0000";
                    obillpay.bill_paymentskind = clsEnum.Payment_Kind.cash;
                    obillpay.bill_buycmpny = "현금";
                    obillpay.bill_cardcmpny = "none";
                    obillpay.bill_cmpny = "none";
                    obillpay.bill_vancmpny = clsSetting.GetDescription(clsPos.van_cmpny);
                    obillpay.bill_cardnum = "****************";
                    obillpay.bill_authnum = "9999999";
                    obillpay.bill_tid = clsPos.tid;
                    obillpay.bill_OrgApprovalNum = "0000";
                    obillpay.bill_signpath = "";
                    obillpay.bill_authdatetime = clsSetting._Today();
                    obillpay.bill_cardkind = "CA";
                    obillpay.bill_iscancel = clsEnum.bill_isCancel.auth;
                    obillpay.bill_halbu = "00";

                    Pay_Step(obillpay);
                }
                else
                {

                }
                oCash2.Dispose();

            }



        }

    
        //화면내용을 클래스로 옮기기
        private List<clsBill_Items> Set_ListToClass()
        {
            List<clsBill_Items> lst_Bill_Items = new List<clsBill_Items>();
            
            foreach (ListViewItem item in lst_Items.Items)
            {
                clsBill_Items oBill_Item = new clsBill_Items();

                foreach (clsItems xMaster in oMast)
                {
                    if (oBill != null)
                    {
                        oBill_Item.bill_pos = oBill.bill_pos;
                        oBill_Item.bill_date = oBill.bill_date;
                        oBill_Item.bill_iscancel = oBill.bill_iscancel;
                    }
                    if (item.SubItems[1].Text == "99999")
                    {
                        oBill_Item.item_code = item.SubItems[1].Text;
                        oBill_Item.item_name = "NFC 카드";
                        oBill_Item.item_vat = clsEnum.Item_vat.item_duty_free;
                        oBill_Item.item_cost = double.Parse(item.SubItems[4].Text);
                        oBill_Item.item_qty = 1;
                        oBill_Item.dBill_Amt = double.Parse(item.SubItems[5].Text);
                        break;
                    }
                    else
                    {
                        if (xMaster.item_code == item.SubItems[1].Text)
                        {
                            oBill_Item.item_code = xMaster.item_code;
                            oBill_Item.item_name = xMaster.item_name;
                            oBill_Item.item_vat = xMaster.item_vat;
                            oBill_Item.item_cost = xMaster.item_cost;
                            oBill_Item.item_qty = Int32.Parse(item.SubItems[3].Text);
                            oBill_Item.dBill_Amt = double.Parse(item.SubItems[5].Text);
                            break;
                        }
                    }
                }


                if(oSelectedTable != null)
                    oBill_Item.orgtable_no = oSelectedTable.sTableNo;

                lst_Bill_Items.Add(oBill_Item);
            }

            return lst_Bill_Items;
        }

        private void Set_Bill()
        {
            oBill = new clsBill();
            oBill.bill_pos = clsPos.pos_num;
            oBill.bill_iscancel = clsEnum.bill_isCancel.auth;

            double dItemSum = 0;            
            double dDutyFree = 0;
            double dTax = 0;

            oBill.oLstBill_Items = Set_ListToClass();
            oBill.oLstBill_payments = new List<clsbill_payments>();


            foreach (clsBill_Items obill_items in oBill.oLstBill_Items)
            {
                dItemSum += obill_items.dBill_Amt;

                if (obill_items.item_vat == clsEnum.Item_vat.item_tax)
                {
                    dTax += obill_items.dItem_Vat;
                }
                else
                {
                    dDutyFree += obill_items.dBill_Amt;
                }
            }


            oBill.bill_itemcount = oBill.oLstBill_Items.Count;
            oBill.bill_totalamt = dItemSum;
            oBill.bill_vatamt = dTax;
            oBill.bill_DutyFreeAmt = dDutyFree;
            oBill.bill_TaxAmt = dTax;
            oBill.bill_paymentamt = dItemSum;
            oBill.bill_halbu = "00";
            oBill.Set_BillCode();


        }

        private void btn_Cash_Click(object sender, EventArgs e)
        {
            if (bCancelMode)
                return;

            if (lst_Items.Items.Count == 0)
            {
                lbl_Message.ForeColor = Color.Red;
                lbl_Message.Text = "주문한 상품이 없습니다.";
                return;
            }

            if(oBill == null)
                Set_Bill();
            

            frmCash2 oCash = new frmCash2(this as Inter_Bill , dTotal, clsEnum.Payment_Kind.cash);
            oCash.oOwner = this;
            oCash.Size = new Size(800, 444);
            oCash.StartPosition = FormStartPosition.CenterParent;
            //받은 현금
            oCash.ShowDialog();
        }
                
        private void btn_Card_Click(object sender, EventArgs e)
        {
            if (bCancelMode)
                return;
            
            if (lst_Items.Items.Count == 0)
            {
                lbl_Message.ForeColor = Color.Red;
                lbl_Message.Text = "주문한 상품이 없습니다.";
                return;
            }
            
            //영수증 생성 후 결제
            if (oBill == null)
                Set_Bill();


            frmCash2 oCash = new frmCash2(this as Inter_Bill, dTotal, clsEnum.Payment_Kind.card );
            oCash.oOwner = this;
            oCash.Size = new Size(800, 444);
            oCash.StartPosition = FormStartPosition.CenterParent;
            //받은 현금
            oCash.ShowDialog();

        }


        private void Num_Hal_Cancel()
        {
            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;
        }

        private void Num_Hal(string sCnt)
        {

            int ihalbu = clsSetting.Let_Int(sCnt);

            if (ihalbu > 12)
            {
                lbl_Message.Text = "할부 기간 오류";
                return;
            }


            oBill.bill_halbu = string.Format("{0:D2}", ihalbu);

            clsbill_payments _oPay = new clsbill_payments();
            _oPay.bill_halbu = string.Format("{0:D2}", ihalbu);
            _oPay.bill_paymentamt = oHalu_Pay.bill_paymentamt;

            oHalu_Pay = null;

            clsbill_payments oPay = Auth(false, 0, _oPay);
            if (oPay.sRepCode == "0000")
            {
                Pay_Step(oPay);
                //Finish();
            }

            //Auth(false,0);
            //Finish();

            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isCancel"></param>
        /// <param name="iCardCash">0-카드 1-현금 2-지출증빙</param>
        /// <returns></returns>
        private clsbill_payments  Auth( bool isCancel,int iCardCash,clsbill_payments oPay)
        {

            clsbill_payments obillpay = new clsbill_payments();

            pan_ReadyPay.Location = new Point(197, 314);
            pan_ReadyPay.Visible = true;

            if (iCardCash == 0)
            {
                lbl_PayInfo.Text = "단말기에 IC 카드를 삽입하거나 MS 카드를 읽혀 주세요.";
            }
            else if (iCardCash == 1)
            {
                lbl_PayInfo.Text = "서명패드에 고객정보를 입력 바랍니다.";
            }
            else
            {
                lbl_PayInfo.Text = "단말기에 법인지출증빙카드를 읽혀 주세요.";
            }


            try
            {
                switch (clsPos.van_cmpny)
                {
                    case clsEnum.Van_Cmpny.none:

                        if (!isCancel)
                        {
                            oBill.bill_buycmpny = "결제생략";
                            oBill.bill_cardcmpny = "none";
                            oBill.bill_cmpny = "none";
                            oBill.bill_vancmpny = clsSetting.GetDescription(clsPos.van_cmpny);
                            oBill.bill_cardnum = "****************";
                            oBill.bill_authnum = "9999999";
                            oBill.bill_tid = clsPos.tid;
                            oBill.bill_OrgApprovalNum = "0000";
                            oBill.bill_signpath = "";
                            oBill.bill_authdatetime = clsSetting._Today();
                            oBill.bill_cardkind = "EM";
                            oBill.bill_iscancel = clsEnum.bill_isCancel.auth;
                        }
                        else
                        {
                            foreach (clsBill_Items oBillItems in oBill.oLstBill_Items)
                            {
                                oBillItems.bill_iscancel = clsEnum.bill_isCancel.cancel;
                            }

                            oBill.bill_iscancel = clsEnum.bill_isCancel.cancel;

                        }
                        break;
                    case clsEnum.Van_Cmpny.jtnet:
                        #region jtnet_단말기
                        using (clsJTNet_SEQD oJtSeq = new clsJTNet_SEQD(oPay, isCancel, iCardCash))
                        {
                            clsVanRep oRep = oJtSeq.Auth_JTNet();

                            if (oRep.RepCode == "0000") //정상승인
                            {
                                if (!isCancel)
                                {
                                    obillpay.bill_paymentamt = oPay.bill_paymentamt;
                                    obillpay.bill_recvamt = oPay.bill_paymentamt;
                                    obillpay.bill_restAmt = 0;
                                    obillpay.sRepCode = oRep.RepCode;
                                    obillpay.bill_buycmpny = string.Format("{0}", oRep.RepBuyCmpny).Trim();
                                    obillpay.bill_cardcmpny = string.Format("{0}", oRep.RepCardCmpny).Trim();
                                    obillpay.bill_cmpny = oRep.RepCmpny;
                                    obillpay.bill_vancmpny = clsSetting.GetDescription(clsPos.van_cmpny);
                                    obillpay.bill_cardnum = oRep.RepCardNum.Trim();
                                    obillpay.bill_authnum = oRep.RepAuthNum.Trim();
                                    obillpay.bill_tid = clsPos.tid;
                                    obillpay.bill_OrgApprovalNum = oRep.RepOrgApprovalNum;
                                    obillpay.bill_signpath = "";
                                    obillpay.bill_authdatetime = oRep.RepAuthDateTime;
                                    obillpay.bill_halbu = oPay.bill_halbu;

                                    if (iCardCash == 1 || iCardCash == 2)
                                    {
                                        obillpay.bill_paymentskind = clsEnum.Payment_Kind.cashwithaut;
                                        oBill.bill_cardkind = "ACA";
                                    }
                                    else
                                    {
                                        obillpay.bill_paymentskind = clsEnum.Payment_Kind.card;
                                        oBill.bill_cardkind = oRep.RepCardKind;
                                    }
                                    obillpay.bill_iscancel = clsEnum.bill_isCancel.auth;
                                }
                                else
                                {
                                    obillpay.sRepCode = oRep.RepCode;

                                    foreach (clsBill_Items oBillItems in oBill.oLstBill_Items)
                                    {
                                        oBillItems.bill_iscancel = clsEnum.bill_isCancel.cancel;
                                    }

                                    foreach (clsbill_payments  oBillPay in oBill.oLstBill_payments )
                                    {
                                        oBillPay.bill_iscancel = clsEnum.bill_isCancel.cancel;
                                    }

                                    oBill.bill_iscancel = clsEnum.bill_isCancel.cancel;

                                }
                            }
                            else if (oRep.RepCode == "0064") //사용자 취소
                            {
                                string sErrCode = oRep.RepErrDesc.Substring(0, 4);

                                if (sErrCode == "8302" || sErrCode == "8205")
                                {
                                    //시간초과 다시 결제 하시려면 재결제를 선택해주세요.
                                    lbl_Message.ForeColor = Color.Red;
                                    lbl_Message.Text = "결제 하시려면 재결제를 선택해주세요.";
                                }
                                else if (sErrCode == "8208")
                                {

                                }
                                else
                                {
                                    lbl_Message.ForeColor = Color.Red;
                                    lbl_Message.Text = oRep.RepErrDesc;
                                }
                            }
                            else
                            {
                                lbl_Message.ForeColor = Color.Red;
                                lbl_Message.Text = oRep.RepErrDesc;
                            }
                        }
                        break;
                        #endregion
                    case clsEnum.Van_Cmpny.jtnet2:
                        #region jtnet
                        using (clsJTNet oJTNet = new clsJTNet(oPay, isCancel, iCardCash))
                        {
                            clsVanRep oRep = oJTNet.Auth_JTNet();

                            if (oRep.RepCode == "0000") //정상승인
                            {
                                if (!isCancel)
                                {

                                    obillpay.bill_paymentamt = oPay.bill_paymentamt;
                                    obillpay.bill_recvamt = oPay.bill_paymentamt;
                                    obillpay.bill_restAmt = 0;
                                    obillpay.sRepCode = oRep.RepCode;
                                    obillpay.bill_buycmpny = string.Format("{0}", oRep.RepBuyCmpny).Trim();
                                    obillpay.bill_cardcmpny = string.Format("{0}", oRep.RepCardCmpny).Trim();
                                    obillpay.bill_cmpny = oRep.RepCmpny;
                                    obillpay.bill_vancmpny = clsSetting.GetDescription(clsPos.van_cmpny);
                                    obillpay.bill_cardnum = oRep.RepCardNum.Trim();
                                    obillpay.bill_authnum = oRep.RepAuthNum.Trim();
                                    obillpay.bill_tid = clsPos.tid;
                                    obillpay.bill_OrgApprovalNum = oRep.RepOrgApprovalNum;
                                    obillpay.bill_signpath = "";
                                    obillpay.bill_authdatetime = oRep.RepAuthDateTime;
                                    obillpay.bill_halbu = oPay.bill_halbu;


                                    if (iCardCash == 1 || iCardCash == 2)
                                    {
                                        obillpay.bill_paymentskind = clsEnum.Payment_Kind.cashwithaut;
                                        oBill.bill_cardkind = "ACA";
                                    }
                                    else
                                    {
                                        obillpay.bill_paymentskind = clsEnum.Payment_Kind.card;
                                        oBill.bill_cardkind = oRep.RepCardKind;
                                    }
                                    obillpay.bill_iscancel = clsEnum.bill_isCancel.auth;

                                }
                                else
                                {
                                    obillpay.sRepCode = oRep.RepCode;

                                    foreach (clsBill_Items oBillItems in oBill.oLstBill_Items)
                                    {
                                        oBillItems.bill_iscancel = clsEnum.bill_isCancel.cancel;
                                    }

                                    foreach (clsbill_payments oBillPay in oBill.oLstBill_payments)
                                    {
                                        oBillPay.bill_iscancel = clsEnum.bill_isCancel.cancel;
                                    }

                                    oBill.bill_iscancel = clsEnum.bill_isCancel.cancel;

                                }

                            }
                            else if (oRep.RepCode == "0069") //사용자 취소
                            {
                                string sErrCode = oRep.RepErrDesc.Substring(0, 4);

                                if (sErrCode == "8302" || sErrCode == "8205")
                                {
                                    //시간초과 다시 결제 하시려면 재결제를 선택해주세요.
                                    lbl_Message.ForeColor = Color.Red;
                                    lbl_Message.Text = "결제 하시려면 재결제를 선택해주세요.";
                                }
                                else if (sErrCode == "8208")
                                {

                                }
                                else
                                {
                                    lbl_Message.ForeColor = Color.Red;
                                    lbl_Message.Text = oRep.RepErrDesc;
                                }
                            }
                            else
                            {
                                lbl_Message.ForeColor = Color.Red;
                                lbl_Message.Text = oRep.RepErrDesc;
                            }

                        }
                        #endregion
                        break;
                    case clsEnum.Van_Cmpny.kovan:
                        #region Kovan
                        using (clsVanKovan oKovan = new clsVanKovan(oBill, isCancel))
                        {
                            clsKovan_Rep oRep = oKovan.Approval_CreditCard();

                            if (oRep.rErrCode == "0000")
                            {
                                //날짜 형식 바꾼다.
                                if (oRep.rTranDate != null && oRep.rTranTime != null)
                                {
                                    oRep.rTranDate = String.Format("{0:  ######}", Int32.Parse(oRep.rTranDate)).Trim();
                                    oRep.rTranTime = String.Format("{0:  ######}", Int32.Parse(oRep.rTranTime)).Trim();
                                }
                                if (!isCancel)
                                {
                                    oBill.bill_buycmpny = string.Format("{0}", oRep.rPurchaseCard);
                                    oBill.bill_cardcmpny = string.Format("{0}", oRep.rIssueCard);
                                    oBill.bill_cmpny = "";
                                    oBill.bill_Posserialnum = oRep.rTranSerial;
                                    oBill.bill_vancmpny = clsSetting.GetDescription(clsPos.van_cmpny);
                                    oBill.bill_cardnum = oRep.rCardno;
                                    oBill.bill_authnum = oRep.rAuthNo;
                                    oBill.bill_tid = clsPos.tid;
                                    oBill.bill_OrgApprovalNum = "";
                                    oBill.bill_signpath = "";
                                    oBill.bill_authdatetime = clsDateTime.Set_Time(string.Format("{0}{1}", oRep.rTranDate, oRep.rTranTime));
                                    oBill.bill_cardkind = "";
                                    oBill.bill_iscancel = clsEnum.bill_isCancel.auth;
                                }
                                else
                                {
                                    foreach (clsBill_Items oBillItems in oBill.oLstBill_Items)
                                    {
                                        oBillItems.bill_iscancel = clsEnum.bill_isCancel.cancel;
                                    }

                                    oBill.bill_iscancel = clsEnum.bill_isCancel.cancel;

                                }

                            }
                            else if (oRep.rErrCode == "5007" || oRep.rErrCode == "8001" || oRep.rErrCode == "8006")  //"IC카드 처리중 강제로 카드분리"; "거래중 사용자 중단 요청"; "서명오류 취소(화면서명- 전송할 서명파일 없음)";
                            {


                            }
                            else
                            {
                                lbl_Message.ForeColor = Color.Red;
                                lbl_Message.Text = "VAN 승인 실패";
                            }

                        }
                        #endregion
                        break;
                }




                return obillpay;

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

                lbl_Message.ForeColor = Color.Red;
                lbl_Message.Text = "VAN 승인 실패";

                obillpay.sRepCode = "9999";

                return obillpay;
            }
            finally
            {
                pan_ReadyPay.Visible = false;
 
            }
        }

        private void Cash_Cancel()
        {
            foreach (clsBill_Items oBillItems in oBill.oLstBill_Items)
            {
                oBillItems.bill_iscancel = clsEnum.bill_isCancel.cancel;
            }

            foreach (clsbill_payments oBillPay in oBill.oLstBill_payments)
            {
                oBillPay.bill_iscancel = clsEnum.bill_isCancel.cancel;
            }

            oBill.bill_iscancel = clsEnum.bill_isCancel.cancel;


        }


        /// <summary>
        /// 결제된 금액들 체크
        /// </summary>
        private void Pay_Step(clsbill_payments oPay)
        {


            double dFinalTotalAmt = 0; //정상판매가총액 - 세일 총액


            foreach (clsBill_Items oScan in oBill.oLstBill_Items )
            {
                dFinalTotalAmt += oScan.dBill_Amt;
            }



            Boolean isFind = false;
            if (oBill.oLstBill_payments  == null) //첫번째 결제
            {
                oBill.oLstBill_payments = new List<clsbill_payments>();
            }
            else
            {
                //이미 있고 현금,포인트면 받은 금액 누적 한다. 카드,현금영수증,상품권 은 건마다 별도로 처리
                if (oPay.bill_paymentskind  == clsEnum.Payment_Kind.cash)
                {
                    foreach (clsbill_payments _pay in oBill.oLstBill_payments)
                    {
                        if (_pay.bill_paymentskind == oPay.bill_paymentskind )
                        {
                            _pay.bill_restAmt += oPay.bill_restAmt;
                            _pay.bill_recvamt += oPay.bill_recvamt;
                            _pay.bill_paymentamt += oPay.bill_paymentamt;
                            isFind = true;
                            break;
                        }
                    }
                }
            }



            if (!isFind) //같은 결제 방식이 없으면 신규 방식 더함
            {
                oBill.oLstBill_payments.Add(oPay);
            }


            double dRecvAmt = 0;
            double dRest = 0;
            //모든 결제 금액 파악
            foreach (clsbill_payments _Pay in oBill.oLstBill_payments)
            {
                dRecvAmt += _Pay.bill_recvamt; //받은금액 합산
                dRest += _Pay.bill_restAmt;
            }

            //사용자 실수로 합계 금액에 소수점이 들어오는 경우 떨군다.


            if (dRecvAmt >= Math.Truncate(dFinalTotalAmt))
            {
                Finish();

            }
            else
            {
                dTotal -= oPay.bill_recvamt;
            }


            using (clsPrint oPrint = new clsPrint())
            {
                oPrint.Open_Drawer();
            }

            lbl_Rest.Text = String.Format("{0:#,##0}", dRest );
            lbl_CustomPay.Text = String.Format("{0:#,##0}", dRecvAmt);


 
        }



        private void Finish()
        {
            if (oBill.bill_iscancel == clsEnum.bill_isCancel.auth)
            {
                double dRecvAmt = 0;
                double dRest = 0;
                //모든 결제 금액 파악
                foreach (clsbill_payments _Pay in oBill.oLstBill_payments)
                {
                    dRecvAmt += _Pay.bill_recvamt; //받은금액 합산

                    dRest += _Pay.bill_restAmt; ;
                }



                lbl_Rest.Text = String.Format("{0:#,##0}", dRest);
                lbl_CustomPay.Text = String.Format("{0:#,##0}", dRecvAmt);

                try
                {
                    oBill.Save_Bill();


                    if (clsPos.isPrint)
                    {
                        using (clsPrint oPrint = new clsPrint())
                        {
                            oPrint.Print_Bill(oBill, false);
                        }
                    }

                    //kiosk 판매분 결제 처리
                    if (lst_Kiosk_bill != null)
                    {
                        foreach (clsKiosk_Bill oKBill in lst_Kiosk_bill)
                        {
                            oKBill.bill_authnum = oBill.bill_authnum;
                            oKBill.bill_authdatetime = oBill.bill_authdatetime;

                            oKBill.Update_Bill(oBill);
                        }
                    }


                    foreach (clsbill_payments oPay in oBill.oLstBill_payments)
                    {
                        switch (oPay.bill_paymentskind)
                        {
                            case clsEnum.Payment_Kind.card:
                                clsPosOpen.sale_Cardamount += oBill.bill_paymentamt;
                                break;
                            case clsEnum.Payment_Kind.cash:
                                clsPosOpen.sale_Cashamount += oBill.bill_paymentamt;
                                break;
                            case clsEnum.Payment_Kind.cashwithaut:
                                clsPosOpen.sale_Cashamount += oBill.bill_paymentamt;
                                break;
                        }

                    }

                    //현판매 상태 누적
                    clsPosOpen.sale_customs++;
                    clsPosOpen.sale_amount += oBill.bill_paymentamt;


                    if (oSelectedTable != null)
                    {
                        if (oSelectedTable.TableState == clsEnum.Table_State.table_Assem) //합쳐진 테이블이면 관련 테이블 다 지운다.
                        {
                            Cancel_Assembly();
                        }
                        else
                        {
                            oSelectedTable.TableState = clsEnum.Table_State.table;
                            oSelectedTable.Clear_Order();

                            oSelectedTable = null;
                        }

                    }



                    lbl_Message.Text = "결제가 완료 되었습니다.";

                }
                catch (Exception ex)
                {
                    lbl_Message.Text = "결제시 오류 발생.";
 
                }
                
            }
            else if (oBill.bill_iscancel == clsEnum.bill_isCancel.reissue )
            {
                try
                {
                    oBill.Update_Bill(0);

                    lbl_Message.Text = "현금영수증 재발행 했습니다.";
                }
                catch (Exception ex)
                {
                    lbl_Message.Text = "재발행시 오류 발생.";
                }
            }
            else
            {

                try
                {
                    oBill.Update_Bill();
                    if (clsPos.isPrint)
                    {
                        using (clsPrint oPrint = new clsPrint())
                        {
                            oPrint.Print_Bill(oBill, false);

                            // oPrint.Print_Bill(oBill, true);

                        }
                    }



                    //현판매 상태 누적
                    clsPosOpen.sale_customs--;
                    clsPosOpen.sale_amount -= oBill.bill_paymentamt;

                    if (lst_Kiosk_bill != null)
                    {
                        foreach (clsKiosk_Bill oKBill in lst_Kiosk_bill)
                        {
                            oKBill.bill_authnum = "0";
                            oKBill.bill_authdatetime = 0;

                            oKBill.Update_Bill(oBill);
                        }
                    }


                    foreach (clsbill_payments oPay in oBill.oLstBill_payments)
                    {
                        switch (oPay.bill_paymentskind)
                        {
                            case clsEnum.Payment_Kind.card:
                                clsPosOpen.sale_Cardamount -= oBill.bill_paymentamt;
                                break;
                            case clsEnum.Payment_Kind.cash:
                                clsPosOpen.sale_Cashamount -= oBill.bill_paymentamt;
                                break;
                            case clsEnum.Payment_Kind.cashwithaut:
                                clsPosOpen.sale_Cashamount -= oBill.bill_paymentamt;
                                break;
                        }

                    }


                    lbl_Message.Text = "결제 취소 하였습니다.";
                }
                catch (Exception ex)
                {
                    lbl_Message.Text = "취소시 오류 발생.";
                }
            }



            clsPosOpen.Set_Open();
            


            //누적 고객 숫자 가져온다.
            lbl_AccCustom.Text = string.Format("{0:#,##0}", clsPosOpen.sale_customs);

            Clear_All();
            lbl_Message.ForeColor = Color.Black;
            
            if (oCustom != null)
                oCustom.Set_Start();
        }

        #endregion

        private void pan_Cancel_VisibleChanged(object sender, EventArgs e)
        {
            if (pan_Cancel.Visible)
            {
                pan_Button.Visible = false;
                pan_Cancel.Location = new Point(480, 563);


               // 480, 563

            }
            else
            {
                pan_Button.Visible = true;

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 테이블 포스 뷰 교환
        /// </summary>
        /// <param name="iKind"> 0-포스 1-테이블</param>
        private void Set_Panel(int iKind)
        {
            if (iKind == 0)
            {
                pan_Table.Visible = false;
                pan_Table.Dock = DockStyle.None;
                

                pan_Pos.Visible = true;
                pan_Pos.Dock = DockStyle.Fill;

                
                if (oSelectedTable != null)
                {
                    lbl_Message.Text = string.Format("{0} 번테이블 선택", oSelectedTable.sTableNo);

                    if (oSelectedTable.TableState == clsEnum.Table_State.table_Assem)
                    {
                        //기존 주문 상품 과 합치기
                        int iMajor = oSelectedTable.iMajorTalbe;

                        foreach (usc_Table oTempTable in oTables)
                        {
                            if (oTempTable != null)
                            {
                                if (oTempTable.sTableNo == iMajor)
                                {
                                    Set_ClassToList(oTempTable.oLstBill_Items);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        Set_ClassToList(oSelectedTable.oLstBill_Items);
                    }
                }
            }
            else
            {
                try
                {
                    if (oSelectedTable != null )
                    {
                        if (lst_Items.Items.Count == 0)
                        {
                            if (MessageBox.Show("테이블 주문 내역을 삭제 하시겠습니까?", "information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (oSelectedTable.TableState == clsEnum.Table_State.table_Assem) //합쳐진 테이블이면 관련 테이블 다 지운다.
                                {
                                    Cancel_Assembly();
                                }
                                else
                                {
                                    oSelectedTable.TableState = clsEnum.Table_State.table;
                                    oSelectedTable.Clear_Order();

                                }
                                Clear_All();
                            }
                        }
                        else
                        {
                            if (oSelectedTable != null)
                            {
                                //상품 넣는데 묶인 테이블이면 상품 잘 넣자..
                                if (oSelectedTable.TableState == clsEnum.Table_State.table_Assem)
                                {

                                    //기존 주문 상품 과 합치기
                                    int iMajor = oSelectedTable.iMajorTalbe;

                                    foreach (usc_Table oTempTable in oTables)
                                    {
                                        if (oTempTable != null)
                                        {
                                            if (oTempTable.sTableNo == iMajor)
                                            {
                                                oTempTable.oLstBill_Items = Set_ListToClass();
                                                oTempTable.Add_Table_Info();
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    oSelectedTable.TableState = clsEnum.Table_State.table_On;
                                    oSelectedTable.oLstBill_Items = Set_ListToClass();
                                    oSelectedTable.Add_Table_Info();
                                }
                            }
                        }
                    }



                    pan_Pos.Visible = false;
                    pan_Pos.Dock = DockStyle.None;

                    pan_Table.Visible = true;
                    pan_Table.Dock = DockStyle.Fill;
                    Clear_All();
                    oSelectedTable = null;
                }
                catch (Exception ex)
                {
                    lbl_Message.Text = "에러 발생";
                }
            }
        }

        #region 테이블
        usc_Table[] oTables = null;
        usc_Table oSelectedTable = null;

        List<int> lst_AssembleTable = null;




        /// <summary>
        /// 초기 테이블 세팅
        /// </summary>
        private void Set_Table()
        {

            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;

            List<clsTable> lst_tables = new List<clsTable>();

            int iDay = 0;

            oTables = new usc_Table[36];
            
            
            CoTableLayoutPanel ctlp = new CoTableLayoutPanel();
            oSelectedTable = null;
            pan_Table_Layout.Controls.Clear();

            ctlp.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.get_tables";

                cmd_SEL.Parameters.Add("posno", OracleDbType.Varchar2).Value = clsPos.pos_num;                
                cmd_SEL.Parameters.Add("cur_table", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);
                string bill_date = string.Empty;

                if (dt.Rows.Count == 0)
                {
                    
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        using(clsTable  oTable = new clsTable(
                        Convert.ToInt16(dr["table_no"]),
                        Convert.ToInt16(dr["table_x"]),
                        Convert.ToInt16(dr["table_y"])))
                        {
                            lst_tables.Add(oTable );
                        }
                    }
                }

                ctlp.Dock = DockStyle.Fill;
                ctlp.Margin = new System.Windows.Forms.Padding(1);
                ctlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                ctlp.BackColor = Color.FromArgb(207, 214, 232);

                ctlp.Controls.Clear();
                ctlp.RowStyles.Clear();
                ctlp.ColumnCount = 6;
                ctlp.RowCount = 6;

                for (int i = 0; i < 6; i++)
                {
                    RowStyle rs = new RowStyle(SizeType.Absolute, 100);
                    ctlp.RowStyles.Add(rs);
                }


                for (int i = 0; i < 6; i++)
                {
                    ColumnStyle cs = new ColumnStyle(SizeType.Absolute, 170);
                    //ColumnStyle cs = new ColumnStyle(SizeType.Percent  ,(float)16.66667 );
                    ctlp.ColumnStyles.Add(cs);
                }
                pan_Table_Layout.Controls.Add(ctlp);

                //달력 컨트롤 생성
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        oTables[iDay] = new usc_Table(this as Inter_Table);
                        oTables[iDay].AutoSize = false;
                        oTables[iDay].Dock = DockStyle.Fill;
                        ctlp.Controls.Add(oTables[iDay], j, i);

                        oTables[iDay].xPos = j;
                        oTables[iDay].yPos = i;

                        oTables[iDay].sTableNo = 0;
                        oTables[iDay].TableState = clsEnum.Table_State.empty;
                        oTables[iDay].BackgroundImage = Properties.Resources.bg_btntype02;

                        //oTables[iDay].Show_Table_Info();
                        iDay++;
                    }
                }


                foreach (clsTable otable in lst_tables)
                {
                    foreach (usc_Table ouTable in oTables)
                    {
                        if (otable.xPos == ouTable.xPos && otable.yPos == ouTable.yPos)
                        {
                            ouTable.sTableNo = otable.sTable;
                            ouTable.TableState = clsEnum.Table_State.table;
                            ouTable.Show_Table_Info();

                            if (ouTable.oLstBill_Items.Count > 0)
                                ouTable.TableState = clsEnum.Table_State.table_On;


                            break;
                        }
                    }
                }


                //수정모드이면 버튼표시
                if (bModify_Mode)
                {
                    Set_Modify_Mode(1);
                }
                else
                {
                    Set_Modify_Mode(0);
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
                ctlp.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }


        bool bAssemblyMode = false;


        void Btn_Activate(int iKInd)
        {
            foreach (Button obtn in clsSetting.GetAll(this.pan_Table_Btn, typeof(Button)))
            {
                if (obtn.Tag.ToString() == "6")
                    continue;

                if(iKInd == 0)
                    obtn.Enabled = true;
                else
                    obtn.Enabled = false;
            }

        }


        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //모든 버튼 잠군다.
            Btn_Activate(1);

            switch (btn.Tag.ToString())
            {
                case "0":
                    Set_Panel(0);
                    Btn_Activate(0);
                    break;
                case "1":
                    if (bModify_Mode)
                    {
                        btn.Text = "잠금";
                        bModify_Mode = false;
                        Set_Modify_Mode(0);

                        btn.Enabled = true;
                    }
                    else
                    {
                        bool bFind = false;
                        //사용중인 테이블검색
                        foreach (usc_Table oTable in oTables)
                        {
                            if (oTable != null)
                            {
                                if (oTable.TableState == clsEnum.Table_State.table_On)
                                {
                                    bFind = true; //사용중인 테이블 존재
                                    break;
                                }
                            }
                        }

                        if (bFind)
                        {
                            MessageBox.Show("사용중인 테이블이 있습니다.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            btn.Text = "풀림";
                            bModify_Mode = true;
                            Set_Modify_Mode(1);

                            
                        }
                    }
                    break;
                case "2": //테이블합치기
                    if (bAssemblyMode)
                    {
                        btn.Text = "테이블합치기";
                        bAssemblyMode = false;
                        Set_Modify_Mode(0);
                       

                        if(lst_AssembleTable.Count >=  2)
                            //합치기 모드 종료
                            Set_Table_Assemble();
                        else
                            Btn_Activate(0);
                    }
                    else
                    {
                        lst_AssembleTable = new List<int>();
                        btn.Text = "완료";
                        bAssemblyMode = true;
                        Set_Modify_Mode(2);
                        btn.Enabled = true;
                        
                    }
                    break;
                case "3": //나누기 
                    Set_Modify_Mode(3);

                    break;
                case "4": //테이블 이동
                    Set_Modify_Mode(4);

                    

                    break;
                case "5": //테이블 주문 취소
                    Set_Modify_Mode(6);

                    

                    break;

                case "6": // 취소
                    Set_Modify_Mode(0);
                    if (oSelectedTable != null)
                        oSelectedTable = null;

                    if (bAssemblyMode)
                    {
                        button2.Text = "테이블합치기";
                        bAssemblyMode = false;
 
                    }

                    if (bModify_Mode)
                    {
                        button1.Text = "잠금";
                        bModify_Mode = false;
                    }

                    Btn_Activate(0);

                    break;

            }

        }
        
        private void Set_Modify_Mode(int iKind )
        {
            foreach (usc_Table oTable in oTables)
            {
                if (oTable != null)
                {
                    oTable.Set_Modify(iKind);
                }
            }
        }
        
        public void Inter_TableSelected(usc_Table oTable)
        {
            oSelectedTable = oTable;
            Set_Panel(0);
        }

        public void Inter_TableFunc(usc_Table oTable,string sTag)
        {
            Console.WriteLine("x{0} y{1} tag{2}", oTable.xPos, oTable.yPos,sTag );

            switch (sTag)
            {
                case "1":
                    oSelectedTable = oTable;
                    uscNum oNum = new uscNum();
                    oNum.SetLabe1Text = "테이블 번호 지정";
                    oNum.SetLabe2Text = "테이블 번호를 지정해 주세요. ";
                    oNum.SetLabe3Text = " ";
                    oNum.ClickOK += new ClickAccept(Table_Num_Accept);
                    oNum.ClickCancel += new ClickCancel(Table_Num_Cancel);
                    pan_QtyChange.Controls.Add(oNum);
                    pan_QtyChange.Visible = true;
                    pan_QtyChange.Location = new Point(250, 200);
                    pan_QtyChange.Size = new Size(640, 371);
                    pan_QtyChange.BringToFront();

                    break;
                case "2":
                    if (MessageBox.Show("테이블 삭제 하시겠습니까?", "information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        oTable.Delete_Table();
                        Set_Table();

                    }
                    break;
                case "3": //테이블 합치기

                    lst_AssembleTable.Add(oTable.sTableNo);

                    break;
                case "33": //테이블 합치기 선택한 테이블에서 취소 누른경우
                    lst_AssembleTable.Remove(oTable.sTableNo);
                    break;

                case "4":  //합친 테이블 분리

                    oSelectedTable = oTable;

                    lst_AssembleTable = new List<int>();

                    lst_AssembleTable.AddRange(oTable.lst_AssembleTable);

                    Set_Table_DisAssemble();


                    Btn_Activate(0);

                    Set_Modify_Mode(0);


                    break;

                case "5": //이동할 테이블 선택
                    Set_Modify_Mode(0); //이동할 테이블 선택

                    oSelectedTable  = oTable;


                    Set_Modify_Mode(5); //이동할 테이블 선택
                    break;

                case "6": //이쪽으로 이동

                    List<clsBill_Items> lst_TempItem = oSelectedTable.oLstBill_Items;

                    if (oSelectedTable != null)
                    {   
                        foreach (usc_Table oTempTable in oTables)
                        {
                            if (oTempTable != null)
                            {
                                if (oTempTable.sTableNo == oTable.sTableNo)
                                {
                                    oTempTable.TableState = clsEnum.Table_State.table_On;
                                    oTempTable.oLstBill_Items = lst_TempItem;
                                    oTempTable.Add_Table_Info();

                                    oTempTable.Reloard_Table_Info();


                                    break;
                                }
                            }
                        }
                        oSelectedTable.TableState = clsEnum.Table_State.table;
                        oSelectedTable.Clear_Order();
                        oSelectedTable = null;
                    }

                    Set_Modify_Mode(0); //이동할 테이블 선택

                    Btn_Activate(0);

                    break;
                case "7":

                    if (oTable.TableState == clsEnum.Table_State.table_Assem)
                    {
                        oSelectedTable = oTable;
                        Cancel_Assembly();
                    }
                    else
                    {
                        oTable.TableState = clsEnum.Table_State.table;
                        oTable.Clear_Order();
                    }

                    Set_Modify_Mode(0); //테이블 주문 취소
                    Btn_Activate(0);

                    break;

            }

            if (lst_AssembleTable != null)
            {
                foreach (int iTable in lst_AssembleTable)
                {

                    Console.WriteLine("table no {0}", iTable);
                }
            }



        }

        private void Table_Num_Cancel()
        {
            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;



            Console.WriteLine("table cancel");
        }
        private void Table_Num_Accept(string sCnt)
        {
            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;

            int iTempTableNo = clsSetting.Let_Int(sCnt);

            bool bFind = false;
            foreach (usc_Table oTable in oTables)
            {
                if (oTable.sTableNo == iTempTableNo)
                {
                    MessageBox.Show("이미 지정된 테이블 번호 입니다..", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bFind = true;
                    break;
                }
            }


            if (!bFind) //중복된 테이블 번호가 없을때 테이블 저장 한다.
            {
                if (oSelectedTable != null)
                    //Console.WriteLine("selected x{0} y{1} new number{2}",oSelectedTable.xPos,oSelectedTable.yPos,iTempTableNo  );

                    oSelectedTable.sTableNo = iTempTableNo;
                oSelectedTable.Insert_Table();

                Set_Table();

            }



        }
        
        private void Set_Table_Assemble()
        {
            List<clsBill_Items> lst_TempItem = new List<clsBill_Items>();

            //리스트에 있는 테이블 합치고 참조 건다.
            foreach (int tableno in lst_AssembleTable)
            {
                foreach (usc_Table oTempTable in oTables)
                {
                    if (oTempTable != null)
                    {
                        if (oTempTable.sTableNo == tableno)
                        {
                            oTempTable.TableState = clsEnum.Table_State.table_Assem;
                            lst_TempItem.AddRange(oTempTable.oLstBill_Items);

                            //oTempTable.Clear_Order();

                            break;
                        }
                    }
                }
            }

            int iMajorTable = 0;


            foreach (int tableno in lst_AssembleTable)
            {
                foreach (usc_Table oTempTable in oTables)
                {
                    if (oTempTable != null)
                    {
                        Console.WriteLine("table no {0} ", oTempTable.sTableNo);

                        if (oTempTable.sTableNo == tableno)
                        {
                            oTempTable.lst_AssembleTable = lst_AssembleTable;

                            if (iMajorTable == 0)
                            {   
                                oTempTable.oLstBill_Items = lst_TempItem;
                                oTempTable.Add_Table_Info();
                                oTempTable.Reloard_Table_Info();

                                oTempTable.TableState = clsEnum.Table_State.table_Assem;
                                iMajorTable = oTempTable.sTableNo;
                                oTempTable.iMajorTalbe = iMajorTable;
                                oTempTable.Set_Assembly( );
                            }
                            else
                            {
                                oTempTable.TableState = clsEnum.Table_State.table_Assem;
                                oTempTable.iMajorTalbe = iMajorTable;
                                oTempTable.Set_Assembly();
                            }
                            break;
                        }
                    }
                }
            }

            Btn_Activate(0);
        }


        private void Cancel_Assembly()
        {
            if (oSelectedTable == null)
                return;

            foreach (int tableno in oSelectedTable.lst_AssembleTable )
            {
                foreach (usc_Table oTempTable in oTables)
                {
                    if (oTempTable != null)
                    {
                        if (oTempTable.sTableNo == tableno)
                        {
                            oTempTable.TableState = clsEnum.Table_State.table;
                            oTempTable.Clear_Order();
                            oTempTable.iMajorTalbe = 0;
                            oTempTable.Set_Assembly();
                            break;
                        }
                    }
                }
            }
        }

        private void Set_Table_DisAssemble()
        {
            //주 테이블에서 상품 리스트 분배
            List<clsBill_Items> lst_TempItem = oSelectedTable.oLstBill_Items;

            foreach (int tableno in oSelectedTable.lst_AssembleTable)
            {
                foreach (usc_Table oTempTable in oTables)
                {
                    if (oTempTable != null)
                    {
                        if (oTempTable.sTableNo == tableno)
                        {

                            oTempTable.oLstBill_Items = new List<clsBill_Items>();

                            var oTemp = lst_TempItem.Where(x => x.orgtable_no == oTempTable.sTableNo);

                            oTempTable.oLstBill_Items = oTemp.ToList();



                            if (oTempTable.oLstBill_Items.Count == 0)
                                oTempTable.TableState = clsEnum.Table_State.table;
                            else
                                oTempTable.TableState = clsEnum.Table_State.table_On;

                            oTempTable.Add_Table_Info();

                            oTempTable.iMajorTalbe = 0;
                            oTempTable.Set_Assembly();
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        private void frmOrder_Type2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.X)
            {
                button1.Visible = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                button1.Visible = false;
            }



        }

       

      
    }
}

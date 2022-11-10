using Oracle.ManagedDataAccess.Client;
using st_bread.Bill;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmBill : Form
    {
        Inter_Bill frm = null;
        public frmBill()
        {
            InitializeComponent();
        }

        public frmBill(Inter_Bill frm_)
        {
            InitializeComponent();

            frm = frm_;

        }


        private void frmBill_Load(object sender, EventArgs e)
        {
            lst_Pay.Columns.Clear();

            lst_Pay.Columns.Add("포스", 100, HorizontalAlignment.Center);
            lst_Pay.Columns.Add("판매일자", 190, HorizontalAlignment.Center );
            lst_Pay.Columns.Add("판매일시", 190, HorizontalAlignment.Center);
            lst_Pay.Columns.Add("판매금액", 180, HorizontalAlignment.Right );
            lst_Pay.Columns.Add("결제종류", 200,HorizontalAlignment.Left  );
            lst_Pay.Columns.Add("구 분", 100, HorizontalAlignment.Center);
            lst_Pay.Columns.Add("영수증번호", 0);



        }

        private void frmBill_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmBill_Shown(object sender, EventArgs e)
        {
            Load_Bill_Data(DateTime.Now );

        }


        private void Load_Bill_Data(DateTime _dt)
        {
            int iStartDate = clsDateTime.StartOfDay(_dt);
            int iDate = clsDateTime.EndOfDay(_dt);
            int iCnt = 1;
#if DEBUG
            Console.WriteLine("date1 {0}:{1} date2{2}:{3}", clsDateTime.Get_Time(iStartDate), iStartDate, clsDateTime.Get_Time(iDate), iDate);
#endif
            lst_Pay.Items.Clear();

            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataSet ds = null;

            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Bill";
                cmd_SEL.Parameters.Add("storecode", OracleDbType.Varchar2).Value = clsPos.store_code;
                cmd_SEL.Parameters.Add("startdate", OracleDbType.Int32).Value = iStartDate;
                cmd_SEL.Parameters.Add("enddate", OracleDbType.Int32).Value = iDate;                
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.Parameters.Add("cur_pay", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                ds = clsDBExcute.SelectQuery_SET(cmd_SEL);


                foreach (DataRow dr in ds.Tables[0].Rows )
                {

                    string sPos = dr["machine_code"].ToString();
                    int bill_date =  Convert.ToInt32(dr["bill_date"]);

                    ListViewItem oItem = new ListViewItem();
                    
                    DateTime dt_bill = clsDateTime.Get_Time(Convert.ToInt32(dr["bill_date"]));


                    oItem.Text = sPos;
                    oItem.SubItems.Add(dt_bill.ToString("yyyy-MM-dd"));
                    oItem.SubItems.Add(dt_bill.ToString("HH:mm:ss"));
                    oItem.SubItems.Add(clsSetting.Let_Money( Convert.ToDouble(dr["bill_paymentamt"])  ));


                    //해당영수증 자료를 ds.tablse[1]에서 찾아온다.
                    List<clsbill_payments> oLstBill_payments = new List<clsbill_payments>();

                    foreach (DataRow dr_pay in ds.Tables[1].Rows)
                    {
                        if (sPos == dr_pay["machine_code"].ToString() && bill_date == Convert.ToInt32(dr_pay["bill_date"]))
                        {
                            //해당영수증
                            clsbill_payments obill_pay = new clsbill_payments();
                            obill_pay.bill_pos = clsPos.pos_num;
                            obill_pay.bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                            obill_pay.bill_seq = Convert.ToInt32(dr_pay["bill_seq"]);
                            obill_pay.bill_paymentskind = (clsEnum.Payment_Kind)Convert.ToInt32(dr_pay["bill_paykind"]);
                            obill_pay.sRepCode = dr_pay["bill_repcode"].ToString();
                            obill_pay.bill_recvamt = Convert.ToDouble(dr_pay["bill_recvamt"]);
                            obill_pay.bill_restAmt = Convert.ToDouble(dr_pay["bill_restamt"]);
                            obill_pay.bill_paymentamt = Convert.ToDouble(dr_pay["BILL_PAYMENTAMT"]);
                            obill_pay.bill_buycmpny = dr_pay["BILL_BUYCMPNY"].ToString();
                            obill_pay.bill_cardcmpny = dr_pay["BILL_CARDCMPNY"].ToString();
                            obill_pay.bill_vancmpny = dr_pay["BILL_VANCMPNY"].ToString();
                            obill_pay.bill_signpath = dr_pay["BILL_SIGNPATH"].ToString();
                            obill_pay.bill_halbu = dr_pay["BILL_HALBU"].ToString();
                            obill_pay.bill_cardnum = dr_pay["BILL_CARDNUM"].ToString();
                            obill_pay.bill_authnum = dr_pay["BILL_AUTHNUM"].ToString();
                            obill_pay.bill_tid = dr_pay["BILL_TID"].ToString();
                            obill_pay.bill_authdatetime = Convert.ToInt32(dr_pay["BILL_AUTHDATETIME"]);
                            obill_pay.bill_serialnum = "";
                            obill_pay.bill_OrgApprovalNum = dr_pay["bill_orgapprovalnum"].ToString();
                            obill_pay.bill_cardkind = dr_pay["BILL_CARDKIND"].ToString();
                            

                            oLstBill_payments.Add(obill_pay);


                        }
                    }

                    if (oLstBill_payments.Count > 0)
                    {

                        string sPayKind = clsSetting.GetDescription(oLstBill_payments[0].bill_paymentskind);

                        if (oLstBill_payments.Count > 1)
                        {
                            sPayKind += string.Format(" 외{0}", oLstBill_payments.Count -1);
                        }


                        oItem.SubItems.Add(sPayKind);
                    }


                    if (dr["bill_iscancel"].ToString() == "1")
                    {
                        oItem.SubItems.Add("취소");
                        oItem.ForeColor = Color.Red;
                    }
                    else
                    {
                        oItem.SubItems.Add("승인");

                    }

                    
                    oItem.SubItems.Add(dr["bill_date"].ToString());




                    lst_Pay.Items.Add(oItem);
                    Application.DoEvents();

                }













                if (lst_Pay.Items.Count > 0)
                {
                    int iIdx = 1;
                    foreach (ListViewItem item in lst_Pay.Items)
                    {
                        if ((iIdx % 2) == 0)
                        {
                            item.BackColor = Color.AliceBlue;
                        }
                        iIdx++;
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

                if (ds != null)
                    ds.Dispose();
            }



        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            if (this.lst_Pay.Items.Count == 0)
                return;
            if (frm == null)
                return;

            if (lst_Pay.SelectedItems.Count == 1)
            {
                var item = lst_Pay.SelectedItems[0];

               //frm.Send_SelectBill(item.SubItems[6].Text );

                frm.Send_SelectBill(item.SubItems[0].Text,item.SubItems[6].Text);

            }

            this.Close();

        }






    }
}

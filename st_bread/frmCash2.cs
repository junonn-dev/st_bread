using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using st_bread.Bill; //현재 실행중 함수명

namespace st_bread
{
    public partial class frmCash2 : Form
    {   
        public frmOrder_Type2 oOwner;
     
        double dTAmt = 0;

        string sRecCash = string.Empty;
        double dRecCash = 0;
        Inter_Bill frm = null;
        clsEnum.Payment_Kind oKind = clsEnum.Payment_Kind.cash;

        public frmCash2()
        {
            InitializeComponent();
            pan_CashAuth.Visible = true;
            pan_CashAuth.SetBounds(0, 47, 800, 397);

        }



        public frmCash2(Inter_Bill _frm, double dTotal,clsEnum.Payment_Kind _oKind)
        {
            InitializeComponent();

            frm = _frm;

            txt_Total.Text = string.Format("{0:#,##0}", dTotal);
            dTAmt = dTotal;
            oKind = _oKind;

            txt_Recive.Text = "0";
            txt_Rest.Text = "0";
            panel2.Visible = true;
            panel2.SetBounds(0, 47, 800, 397);

        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            //oOwner = new frmOrder_Type2();

            //oOwner.bChildOk = true;
            //if (txt_Recive.Text == "")
            //{
            //    oOwner.dRecive = 0;
            //}
            //else
            //{
            //    oOwner.dRecive = Double.Parse(txt_Recive.Text);
            //}

            //if (txt_Rest.Text == "")
            //{
            //    oOwner.dRest = 0;
            //}
            //else
            //{
            //    oOwner.dRest = Double.Parse(txt_Rest.Text);
            //}

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            //oOwner.bChildOk = false;
        }

        private void frmCash2_Load(object sender, EventArgs e)
        {
           
            
        }

        private void set_Receive(string sAmt)
        {
            double dSpare = 0;
            double dAmt = double.Parse(sAmt);

            try
            {
                if (dAmt == 0)
                {
                    dSpare = 0;
                }
                else
                {
                    if (dAmt > dTAmt)
                        dSpare = dAmt - dTAmt;
                }

                txt_Recive2.Text = String.Format("{0:#,##0}", dAmt);

                txt_Rest.Text = String.Format("{0:#,##0}", dSpare);
            }
             catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
        }


        private void set_Receive(double dAmt)
        {
            double dSpare = 0;
            //double dAmt = double.Parse(iAmt);

            if (sRecCash.Length == 0)
            {
                dTempSum = dRecCash;
                txt_Recive.Text = String.Format("{0:#,##0}", dRecCash);
            }
            else
            {
                dTempSum = dRecCash + double.Parse(sRecCash);
                txt_Recive.Text = String.Format("{0:#,##0}", dRecCash);
                txt_Recive2.Text = String.Format("{0:#,##0}", double.Parse(sRecCash));
            }

            if (dTempSum == 0)
            {
                dSpare = 0;
            }
            else
            {
                if (dTempSum > dTAmt)
                    dSpare = dTempSum - dTAmt;
            }
            txt_RealRecive.Text = String.Format("{0:#,##0}", dTempSum);
            txt_Rest.Text = String.Format("{0:#,##0}", dSpare);
        }


        double dTempSum = 0;

        #region 숫자
        private void btn_1_Click(object sender, EventArgs e)
        {
            sRecCash += "1";
            set_Receive(dRecCash);
        }

        private void bnt_2_Click(object sender, EventArgs e)
        {
            sRecCash += "2";
            set_Receive(dRecCash);
        }

        private void bnt_3_Click(object sender, EventArgs e)
        {
            sRecCash += "3";
            set_Receive(dRecCash);
        }

        private void btn_4_Click(object sender, EventArgs e)
        {
            sRecCash += "4";
            set_Receive(dRecCash);
        }

        private void btn_5_Click(object sender, EventArgs e)
        {
            sRecCash += "5";
            set_Receive(dRecCash);
        }

        private void btn_6_Click(object sender, EventArgs e)
        {
            sRecCash += "6";
            set_Receive(dRecCash);
        }

        private void btn_7_Click(object sender, EventArgs e)
        {
            sRecCash += "7";
            set_Receive(dRecCash);
        }

        private void btn_8_Click(object sender, EventArgs e)
        {
            sRecCash += "8";
            set_Receive(dRecCash);
        }

        private void btn_9_Click(object sender, EventArgs e)
        {
            sRecCash += "9";
            set_Receive(dRecCash);
        }

        private void btn_0_Click(object sender, EventArgs e)
        {
            sRecCash += "0";
            set_Receive(dRecCash);
        }

        private void btn_00_Click(object sender, EventArgs e)
        {
            sRecCash += "00";
            set_Receive(dRecCash);
        }

        private void btn_000_Click(object sender, EventArgs e)
        {
            sRecCash += "000";
            set_Receive(dRecCash);
        }

        private void btn_50000_Click(object sender, EventArgs e)
        {
            dRecCash += 50000;
            set_Receive(dRecCash);
        }

        private void btn_10000_Click(object sender, EventArgs e)
        {
            dRecCash += 10000;
            set_Receive(dRecCash);
        }

        private void btn_5000_Click(object sender, EventArgs e)
        {
            dRecCash += 5000;
            set_Receive(dRecCash);
        }

        private void btn_1000_Click(object sender, EventArgs e)
        {
            dRecCash += 1000;
            set_Receive(dRecCash);
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            sRecCash = "0";
            dRecCash = 0;
            //set_Receive(sRecCash);


            set_Receive(dRecCash);
        }

        #endregion


        //받은 현금 처리
        private void btn_Enter2_Click(object sender, EventArgs e)
        {

            double dRest = clsSetting.Let_Double(txt_Rest.Text);            
            double dRecive = clsSetting.Let_Double(txt_RealRecive.Text);


            if (dRecive == 0) //받은돈이 없다면 결제 금액 전부 낸걸로 본다.
                dRecive = dTAmt;


            if (frm != null)
                frm.Send_Recv(dRest,dRecive,oKind  );

            this.Close();

           // this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private void btn_Close_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        //현금영수증 처리
        private void btn_No_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
           // this.Close();
        }

        private void btn_Company_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
           // this.Close();
        }

        private void btn_Person_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            //this.Close();
        }
    }
}

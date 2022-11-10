using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmCashIO : Form
    {   
        string sRecCash = string.Empty;
        double dRecCash = 0;
        double dTempSum = 0;

        public frmCashIO()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //입금
            Save_CashIO(0);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //출금
            Save_CashIO(1);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }




        private void Save_CashIO(int iJob)
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.insert_cashio";
                
                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("open_date", OracleDbType.Varchar2).Value = clsPosOpen.open_date;
                cmd_SEL.Parameters.Add("iodate", OracleDbType.Varchar2).Value = clsSetting._Today();
                cmd_SEL.Parameters.Add("ioamt", OracleDbType.Varchar2).Value = dTempSum;
                cmd_SEL.Parameters.Add("iotype", OracleDbType.Varchar2).Value = iJob;

                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);

                if (iJob == 0)
                {
                    clsPosOpen.cash_io_in += dTempSum;
                }
                else
                {
                    clsPosOpen.cash_io_out += dTempSum;
                }

                clsPosOpen.Set_Open();

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





        private void set_Receive(double dAmt)
        {
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
            
            txt_RealRecive.Text = String.Format("{0:#,##0}", dTempSum);
            //txt_Rest.Text = String.Format("{0:#,##0}", dSpare);
        }
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


    }
}

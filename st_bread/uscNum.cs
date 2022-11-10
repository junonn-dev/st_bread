using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class uscNum : UserControl
    {

        //public delegate void ClickAccept(string sCnt);
        //public delegate void ClickCancel();

        public event ClickAccept ClickOK;
        public event ClickCancel ClickCancel;


        public uscNum()
        {
            InitializeComponent();
        }

        private void uscNum_Load(object sender, EventArgs e)
        {
            txt_AuthNum.Select();
        }

        public string SetLabe1Text
        {
            set { label1.Text = value; }
        }

        public string SetLabe2Text
        {
            set { label2.Text = value; }
        }
        public string SetLabe3Text
        {
            set { label5.Text = value; }
        }
       

        #region 숫자키

        private void btn_One_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "1";
        }

        private void btn_Two_Click(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "2";
        }

        private void btn_Three_Click(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "3";
        }

        private void btn_Four_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "4";
        }

        private void btn_Five_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "5";
        }

        private void btn_Six_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "6";
        }

        private void btn_Seven_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "7";
        }

        private void btn_Eight_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "8";
        }

        private void bnt_Nine_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "9";
        }

        private void btn_DelOne_Click(object sender, EventArgs e)
        {
            if (txt_AuthNum.Text.Length > 0)
                txt_AuthNum.Text = txt_AuthNum.Text.Substring(0, txt_AuthNum.Text.Length - 1);
        }

        private void btn_Zero_Click_1(object sender, EventArgs e)
        {
            txt_AuthNum.Text += "0";
        }

        private void btn_Delall_Click(object sender, EventArgs e)
        {
            txt_AuthNum.Text = "";
        }

        #endregion

       //ok 버튼 라벨이나 패널 클릭시 작동
        private void label3_Click(object sender, EventArgs e)
        {
            if (ClickOK != null)

                ClickOK(txt_AuthNum.Text);
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (ClickOK != null)

                ClickOK(txt_AuthNum.Text);

        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (ClickCancel != null)

                ClickCancel();

        }

      

        private void panel2_Click(object sender, EventArgs e)
        {
            if (ClickCancel != null)

                ClickCancel();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ClickOK != null)

                ClickOK(txt_AuthNum.Text);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ClickCancel != null)

                ClickCancel();

        }

       


    }
}

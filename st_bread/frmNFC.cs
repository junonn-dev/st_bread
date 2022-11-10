using st_bread.Bill;
using st_bread.CLASS.Van;
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
    public partial class frmNFC : Form
    {
        Inter_Bill frm = null;
        clsBill oBill = null;


        public frmNFC()
        {
            InitializeComponent();
        }

        public frmNFC(Inter_Bill _frm)
        {
            InitializeComponent();
            frm = _frm;
        }



        private void frmNFC_Load(object sender, EventArgs e)
        {
            lbl_Info.Text = "NFC 카드를 리더기에 가까이 데주세요.";
            txt_NFC.Focus();

        }

        private void frmNFC_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmNFC_Shown(object sender, EventArgs e)
        {

        }

        private void frmNFC_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_NFC.Focused)
            {

                if (e.KeyCode == Keys.Enter)
                {
                    lbl_Info.Text = "카드를 정상적으로 인식했습니다.";

                    if (frm != null)
                    {
                        frm.Send_NFC(txt_NFC.Text);
                        this.Close();
                    }
                }
            }

            lbl_Info.Text = "카드를 다시 인식해주세요.";
            txt_NFC.Focus();

        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

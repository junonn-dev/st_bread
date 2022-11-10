using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.Odbc;
using System.Data.OleDb;

namespace st_bread
{
    public partial class uscCalendar : UserControl
    {

        private DateTime dCalDate;
        private string stDate;
        public string sDate {
            set
            {
                stDate = value;
                dCalDate = clsSetting.Let_DateTime(stDate);
            }
            get
            {
                return stDate;
            }
        }


        public uscCalendar()
        {
            InitializeComponent();
        }


        public string SetDateText
        {
            set 
            { 
                lbl_Date.Text = value;
                lbl_Amt.Text = "";
                lbl_Cnt.Text = "";
                lbl_IsClose.Text = "";
                
            }
        }


        public void Set_DateColor()
        {
            lbl_Date.ForeColor = Color.Red;
        }


        public string SetAmtText
        {
            set { lbl_Amt.Text = value; }
        }

        public string SetCntText
        {
            set { lbl_Cnt.Text = value; }
        }

        public string SetIsCloseText
        {
            set
            {
                if (value == "0") lbl_IsClose.Text = "미 마 감";
                else if (value == "1") lbl_IsClose.Text = "마 감";
                else lbl_IsClose.Text = "";

            }
        }

        public void SetVisible()
        {
            if (sDate == "")
            {
                foreach (Control x in this.Controls)
                {
                    x.Visible = false;
                }
            }
            else if (lbl_Amt.Text == "")
            {
                label1.Visible = false;
                label2.Visible = false;
 
            }
        }


        public DateTime Get_Date()
        {
            return dCalDate;
        }



    }
}

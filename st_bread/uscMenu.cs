using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace st_bread
{
    public partial class uscMenu : UserControl
    {

        public int iMaster_seq { set; get; }
        public string sMaster_jum { set; get; }
        public string sMaster_mart_cd { set; get; }
        public string sMaster_gr_cd { set; get; }
        public string sMaster_cd { set; get; }
        public string sMaster_nm { set; get; }
        public string sMaster_img { set; get; }
        public double iMaster_cost { set; get; }
        public string sMaster_date { set; get; }
        public string sMaster_time { set; get; }
        public string sMaster_rank { set; get; }
        public string sMaster_best { set; get; }
        public string sMaster_leave { set; get; }

        public event ClickMe ClickMenu;

        public uscMenu()
        {
            InitializeComponent();
        }

        
        public string SetLabe1Text
        {
            set { label1.Text = value; }
        }


        public string SetLabe2Text
        {
            set { label2.Text = value; }
        }

        public void SetPanelImg(int iSelected)
        {
            if (iSelected == 1)
                this.BackgroundImage = Properties.Resources.bg_btntype02_over;
            else
                this.BackgroundImage = Properties.Resources.bg_btntype02;
        }

        private void uscMenu_Click(object sender, EventArgs e)
        {
            if (ClickMenu != null)

                ClickMenu(sMaster_cd);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (ClickMenu != null)

                ClickMenu(sMaster_cd);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (ClickMenu != null)

                ClickMenu(sMaster_cd);
        }

      
    }
}

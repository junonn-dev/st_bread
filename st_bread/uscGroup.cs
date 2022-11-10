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
    public partial class uscGroup : UserControl
    {   
        public int iMas_seq { set; get; }
        public string sMas_mart_seq { set; get; }
        public string sGr_cd { set; get; }
        public string sGr_Name { set; get; }
        
        public event ClickMe ClickGroup;

        public uscGroup()
        {
            InitializeComponent();
        }

        public string SetLabe1Text
        {
            set { label1.Text = value; }
            get { return label1.Text; }
        }

        public void  SetPanelImg(int iSelected)
        {
            if (iSelected == 1)
                this.BackgroundImage = Properties.Resources.bg_btntype01_over;
            else
                this.BackgroundImage = Properties.Resources.bg_btntype01;
        }

      

        private void uscGroup_Click(object sender, EventArgs e)
        {
            if (ClickGroup != null)
                ClickGroup(sGr_cd);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (ClickGroup != null)
                ClickGroup(sGr_cd);
        }


    }
}

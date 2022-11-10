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
    public partial class uscButton : UserControl
    {
        public event ClickBtn ClickButton;
        public int iIdx { set; get; }

        public uscButton()
        {
            InitializeComponent();
        }

        public string SetLabe1Text
        {
            set { label1.Text = value; }
            get { return label1.Text; }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void uscButton_Click(object sender, EventArgs e)
        {
            if (ClickButton != null)
                ClickButton(iIdx);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (ClickButton != null)
                ClickButton(iIdx);

        }

       
    }
}

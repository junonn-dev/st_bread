using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmExit : Form
    {
        [DllImport("advapi32.dll")]
        public static extern void InitiateSystemShutdown(string lpMachineName, string lpMessage, int dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown);


        public frmExit()
        {
            InitializeComponent();
        }

        private void frmExit_Load(object sender, EventArgs e)
        {
            foreach (Button btn in clsSetting.GetAll(this, typeof(Button)))
            {
                btn.MouseHover += btn_MouseHover;
            }

        }

        private void btn_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            pan_index.Height = btn.Height;
            pan_index.Top = btn.Top;
        }

        private void frmExit_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                case Keys.D1:
                    Exit_Program();
                    break;
                case Keys.NumPad2:
                case Keys.D2:
                    Shutdown_Window(false);
                    break;
                case Keys.NumPad3:
                case Keys.D3:
                    Shutdown_Window(true);
                    break;
                case Keys.Escape:
                    Cancel_Exit();
                    this.Close();
                    break;
            }

        }




        #region 컨트롤 모서리 둥글게 처리
        private int radius = 20;
        [DefaultValue(20)]
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                this.RecreateRegion();
            }
        }
        private GraphicsPath GetRoundRectagle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius,
                        radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
        private void RecreateRegion()
        {
            var bounds = new Rectangle(this.ClientRectangle.Location, this.ClientRectangle.Size);
            bounds.Inflate(-1, -1);
            this.Region = new Region(GetRoundRectagle(bounds, this.Radius));
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.RecreateRegion();
        }
        #endregion



        private void Exit_Program()
        {
            pan_index.Height = btn_ProgExit.Height;
            pan_index.Top = btn_ProgExit.Top;
            lbl_Message.Text = "프로그램을 종료 합니다.";
            Application.Exit();
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }


        private void Shutdown_Window(bool reStart)
        {
            if (reStart == true)
            {
                pan_index.Height = btn_WinRestart.Height;
                pan_index.Top = btn_WinRestart.Top;
                lbl_Message.Text = "윈도우를 재시작 합니다.";
            }
            else
            {
                pan_index.Height = btn_WinExit.Height;
                pan_index.Top = btn_WinExit.Top;
                lbl_Message.Text = "윈도우를 종료 합니다.";
            }

            InitiateSystemShutdown("\\\\127.0.0.1", "종료 합니다.", 5, false, reStart);
        }

        private void Cancel_Exit()
        {
            pan_index.Height = btn_Cancel.Height;
            pan_index.Top = btn_Cancel.Top;
            lbl_Message.Text = "시작화면으로 갑니다.";
            frmMain oMain = new frmMain();
            oMain.Show();
            Program.ac.MainForm = oMain;
        }

        private void btn_ProgExit_Click(object sender, EventArgs e)
        {
            Exit_Program();
        }

        private void btn_WinExit_Click(object sender, EventArgs e)
        {
            Shutdown_Window(false);
        }

        private void btn_WinRestart_Click(object sender, EventArgs e)
        {
            Shutdown_Window(true);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Cancel_Exit();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace st_bread
{
    public class clsForm_Extends : Form 
    {
        
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint   | ControlStyles.UserPaint  , true);

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= clsNativeMethods.WS_EX_COMPOSITED;
                return cp;
            }
        }

        public void BeginUpdate()
        {
            clsNativeMethods.SendMessage(this.Handle, clsNativeMethods.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        public void EndUpdate()
        {
            clsNativeMethods.SendMessage(this.Handle, clsNativeMethods.WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            Parent.Invalidate(true);
        }
    }
}

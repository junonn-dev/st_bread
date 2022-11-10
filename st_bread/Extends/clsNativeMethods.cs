using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace st_bread
{
    class clsNativeMethods
    {
        public static int WM_SETREDRAW = 0x000B; //uint WM_SETREDRAW
        public static int WS_EX_COMPOSITED = 0x02000000;
        public static int WS_EX_TRANSPARENT = 0x00000020;


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); //UInt32 Msg
    }
}

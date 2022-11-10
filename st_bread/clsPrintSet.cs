using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    class clsPrintSet
    {

        public static Hashtable SettingHT;

        private string GS = Convert.ToString((char)29);
        private string ESC = Convert.ToString((char)27);

        public static string Cmd_Cutting()
        {
            return Convert.ToString((char)27) + "@" + Convert.ToString((char)29) + "V" + (char)1;
        }

        public static string Cmd_Font1()
        {
            return Convert.ToString((char)27) + "!a";
        }

               
        public static string Cmd_Font_Items()
        {
            return Convert.ToString((char)27) + "!" + Convert.ToString((char)14);
        }

        public static string Cmd_Font_Count()
        {
            return Convert.ToString((char)27) + "!r";
        }

        public static string Cmd_DrawOpen()
        {
            return Convert.ToString((char)27) + Convert.ToString((char)112) +  Convert.ToString((char)0);
        }


        public static string Cmd_Close()
        {
            return Convert.ToString((char)27) + "!";
        }

        public static string SDN_TP()
        {
            return "S1";
        }


        public static string SETTING_FILE()
        {
            return Application.StartupPath + @"\Settings.xml";
        }
       

        public static string IMG_CONFIG()
        {
            return Application.StartupPath + @"\img\confi";
        }

        public static string END_LOGFILE()
        {
            return Application.StartupPath + @"\Log\End_Log.txt";
        }


        



    }



}

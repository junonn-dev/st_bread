
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace st_bread
{
    class clsStaticString
    {
        public static string DB_FILE = "http://121.254.162.107/alpha_office/config/db_config.xml"; //Path.Combine(clsSetting.CON_DIR(), "db_config.xml");    
        public static string Img_FTP_URL = "ftp://121.254.162.107/Img";
        public static string Img_URL = "http://121.254.162.107/img";

        public static string Img_FTP_USER = "junonn";
        public static string Img_FTP_PASS = "jd890bls";
        

        public static string DOC_SETTING_FILE = Path.Combine(clsSetting.CON_DIR(), "DockPanel.config");        
        public static string SETTING_FILE = Path.Combine(clsSetting.CON_DIR(), "config.xml");
        public static string USER_FILE = Path.Combine(clsSetting.CON_DIR(), "user.xml");
        public static string MENU_FILE = Path.Combine(clsSetting.CON_DIR(), "menu.xml");
        public static string GRID_FILE = Path.Combine(clsSetting.CON_DIR(), "grid.xml");
        public static string AREA_FILE = Path.Combine(clsSetting.CON_DIR(), "area_data.xml");

        public static string VAN_FILE = Path.Combine(Application.StartupPath, "JTPosSeqDmDll.dll");
        public static string KO_VAN_FILE = @"C:\KOVAN\VPOS_Client.dll";
        public static string KO_VAN_EXE_FILE = @"C:\KOVAN\AutoUpdate.exe";
        public static string KO_VAN_DIR = @"C:\KOVAN";
        public static string KO_VAN_PROC = "VPos_Connector";
        public static int g_iPOSseq = 1;




        public static string APP_Name = "Alpha_Pos";

        public static string DB_Str = string.Empty;
        public static string DB_SerStr = string.Empty;

        
        public static string Str_sub_sum = "소 계";
        public static string Str_sum = "총 계";

        

    }
}

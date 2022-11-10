using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        /// 

        public static ApplicationContext ac = new ApplicationContext();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Environment.SetEnvironmentVariable("PATH", Environment.CurrentDirectory + "\\ODP\\", EnvironmentVariableTarget.Process);

            String nlsLang = Environment.GetEnvironmentVariable("NLS_LANG");
            if (String.IsNullOrEmpty(nlsLang))//KOREAN_KOREA.KO16MSWIN949 , OR string.Empty
                Environment.SetEnvironmentVariable("NLS_LANG", "KOREAN_KOREA.KO16MSWIN949");

            bool isSettingCheck = true;
            
            if (File.Exists(clsStaticString.SETTING_FILE))
            {
                clsDataSetToXml dsXml = new clsDataSetToXml(clsStaticString.SETTING_FILE);
                DataSet ds = dsXml.GetXMLSet();

                //업체코드 기기 코드 있는지 체크
                DataTable tbl_set = ds.Tables["appinfo"];
                if (tbl_set != null)
                {
                    if (tbl_set.Rows.Count == 0)
                    {
                        isSettingCheck = false;
                    }
                }
                dsXml.Dispose();
            }
            else
            {
                isSettingCheck = false;
            }

            if (isSettingCheck)
            {
                frmMain startForm = new frmMain();
                ac.MainForm = startForm;
            }
            else
            {
                frmConfig startForm = new frmConfig();
                ac.MainForm = startForm;
            }


            Application.Run(ac);

        }



       

    }
}

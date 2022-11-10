
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using System.Data.OleDb;
using System.Net;
using System.Threading;
using System.Reflection; //현재 실행중 함수명
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;

namespace st_bread
{
    class clsSContoller
    {

        [DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
        //window 표시 상수
        private const int SW_SHOWNORMAL = 1;
        private const int SW_MAXIMIZE = 3;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;



        clsFile oFile = new clsFile();

        private string sServiceNm = string.Empty;
        private string sPath = string.Empty;

        public clsSContoller(string sStr,string s_Path)
        {
            sServiceNm = sStr;
            sPath = s_Path; 
        }
        public clsSContoller(string s_Path)
        {   
            sPath = s_Path;
        }
        /// <summary>
        /// stser 컨트롤 시작 없으면 설치 후 시작
        /// </summary>
        public  void Service_Start()
        {
            try
            {
                ServiceController oControl = new ServiceController(sServiceNm);


                if (oControl.Status != ServiceControllerStatus.Running)
                {
                    oControl.Start();
                }
            }
            catch (Exception ex)
            {
                Cmd_Run();

            }
        }
        /// <summary>
        /// stser  종료
        /// </summary>
        public void Service_Stop()
        {
            try
            {
                ServiceController oControl = new ServiceController(sServiceNm);


                if (oControl.Status == ServiceControllerStatus.Running)
                {
                    oControl.Stop();
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }
        }
        /// <summary>
        /// stser이 설치가 되지 않았을경우 서비스 설치
        /// </summary>
        private void Cmd_Run()
        {
            try
            {
                ProcessStartInfo oProinfo = new ProcessStartInfo();
                Process oInstallPro = new Process();
                Process oStartPro = new Process();

                //프로세스 시작점 
                oProinfo.FileName = @"cmd";
                oProinfo.Verb = "runas";
                oProinfo.CreateNoWindow = false;
                oProinfo.UseShellExecute = false;

                oProinfo.RedirectStandardOutput = true;
                oProinfo.RedirectStandardInput = true;
                oProinfo.RedirectStandardError = true;

                oInstallPro.StartInfo = oProinfo;
                oInstallPro.Start();

                //서비스 등록
                oInstallPro.StandardInput.Write(@"cd /" + Environment.NewLine);
                oInstallPro.StandardInput.Write(@"cd Windows" + Environment.NewLine);
                oInstallPro.StandardInput.Write(@"cd System32" + Environment.NewLine);
                oInstallPro.StandardInput.Write(@"sc create " + sServiceNm + " binpath= " + sPath + @"\ST_Service.exe start= auto" + Environment.NewLine);

                oInstallPro.StandardInput.Close();
                string resultvalue = oInstallPro.StandardOutput.ReadToEnd();
                oInstallPro.WaitForExit();
                oInstallPro.Close();
                #if DEBUG
                Console.WriteLine(resultvalue);
                #endif
                if (resultvalue.Contains("성공"))
                {
                    oStartPro.StartInfo = oProinfo;
                    oStartPro.Start();
                    //성공 메세지 뜨면 프로스세 시작 한다.
                    oStartPro.StandardInput.Write(@"cd /" + Environment.NewLine);
                    oStartPro.StandardInput.Write(@"cd Windows" + Environment.NewLine);
                    oStartPro.StandardInput.Write(@"cd System32" + Environment.NewLine);
                    oStartPro.StandardInput.Write(@"sc start " + sServiceNm + Environment.NewLine);
                    oStartPro.StandardInput.Close();
                    string resultvalue2 = oStartPro.StandardOutput.ReadToEnd();
                    oStartPro.WaitForExit();
                    oStartPro.Close();
                    #if DEBUG
                    Console.WriteLine(resultvalue2);
                    #endif
                }



            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }




        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsInfoSound
    {

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);


        /// <summary>
        /// 안내 음성 출력
        /// </summary>
        /// <param name="AppPath">실행파일 경로</param>
        /// <param name="iSoundIdx">음성 파일 인덱스</param>
        public static void Sound_ON(string AppPath, int iSoundIdx)
        {
            string sSound = string.Empty;

            switch (iSoundIdx)
            {
                case (1): //당첨 안내
                    sSound = AppPath + @"\MP3\m1.wav";
                    break;
                case (2):
                    sSound = AppPath + @"\MP3\m2.wav";
                    break;
                case (3):
                    sSound = AppPath + @"\MP3\m3.wav";
                    break;
                case (4):
                    sSound = AppPath + @"\MP3\m4.wav";
                    break;
                case (5):
                    sSound = AppPath + @"\MP3\m5.wav";
                    break;
            }

            Console.WriteLine("aa2 : " + sSound);

            //당첨 안내 음성
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);

            mciSendString("open \"" + sSound + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile", null, 0, IntPtr.Zero);

        }
    }
}

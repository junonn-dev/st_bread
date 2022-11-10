using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace st_bread
{
    class clsNetTest
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);


        public static bool isInternetOk()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        //ms에서 제공 하는 web주소 연결 체크할때 응답없음이 걸린다.
        public static bool IsInternetConnected()
        {
            const string NCSI_TEST_URL = "http://www.msftncsi.com/ncsi.txt";
            const string NCSI_TEST_RESULT = "Microsoft NCSI";
            const string NCSI_DNS = "dns.msftncsi.com";
            const string NCSI_DNS_IP_ADDRESS = "131.107.255.255";

            try
            {
                // Check NCSI test link
                WebClient webClient = new WebClient();
                string result = webClient.DownloadString(NCSI_TEST_URL);
                if (result != NCSI_TEST_RESULT)
                {
                    return false;
                }

                // Check NCSI DNS IP
                var dnsHost = Dns.GetHostEntry(NCSI_DNS);
                if (dnsHost.AddressList.Count() < 0 || dnsHost.AddressList[0].ToString() != NCSI_DNS_IP_ADDRESS)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {                
                return false;
            }

            return true;
        }


    }
}

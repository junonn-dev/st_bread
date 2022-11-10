using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsMachine_Info : IDisposable
    {
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //해제 할때 할일
                }
                disposed = true;

            }

        }

        ~clsMachine_Info()
        {
            Dispose(false);
        }
        /// <summary>
        /// 내부 아이피 가져오기
        /// </summary>
        public string Client_IP
        {
            get
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ClientIP = host.AddressList[i].ToString();
                    }
                }
                return ClientIP;
            }
        }

        /// <summary>
        /// 외부 아이피 가져오기
        /// </summary>
        public string Public_IP
        {
            get
            {
                string externalip = new WebClient().DownloadString("http://ipinfo.io/ip").Trim();

                if (String.IsNullOrWhiteSpace(externalip))
                {
                    externalip = string.Empty;
                }

                return externalip;
            }
        }

        /// <summary>
        /// PC 이름 가져오기
        /// </summary>
        /// <param name="ipAdress">아이피 V4</param>
        /// <returns>PC 이름</returns>
        public string GetMachineNameFromIPAddress(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);

                machineName = hostEntry.HostName;
            }
            catch (Exception ex)
            {
                // Machine not found...
            }
            return machineName;
        }
    }
}

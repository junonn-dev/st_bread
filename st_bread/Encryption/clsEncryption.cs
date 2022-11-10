using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsEncryption : IDisposable
    {
        string sRealPass = string.Empty; //복호화 한 암호 저장
        private const string KEY_DES = "abcdefgh"; //복호화 키값
        private bool isDispose = false;



        ~clsEncryption()
        {
            if (!isDispose)
            {
                Dispose();
        
            }
        }

        public void Dispose()
        {
            isDispose = true;
            GC.SuppressFinalize(this);
        }


        #region MD5
        /// <summary>
        /// MD5 암호화 Mac Address 암호화에 사용
        /// </summary>
        /// <param name="str">Mac address</param>
        /// <returns>암호화 문자열</returns>
        public string MD5HashFunc(string str)
        {
            StringBuilder MD5Str = new StringBuilder();
            byte[] byteArr = Encoding.ASCII.GetBytes(str);
            byte[] resultArr = (new MD5CryptoServiceProvider()).ComputeHash(byteArr);

            //for (int cnti = 1; cnti < resultArr.Length; cnti++) (2010.06.27)
            for (int cnti = 0; cnti < resultArr.Length; cnti++)
            {
                MD5Str.Append(resultArr[cnti].ToString("X2"));
            }
            return MD5Str.ToString();
        }
        #endregion


        #region SHA256
        /// <summary>
        /// sha256 암호화 
        /// </summary>
        /// <param name="phrase">암호화할 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public string EncryptSHA256_EUCKR(string phrase)
        {
            int euckrCodepage = 51949;
            Encoding encoder = Encoding.GetEncoding(euckrCodepage);

            SHA256CryptoServiceProvider sha256hasher = new SHA256CryptoServiceProvider();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));

            string hashString = string.Empty;

            foreach (byte x in hashedDataBytes)
            {
                hashString += String.Format("{0:x2}", x);
            }

            //원본 문자열 : abcdefghijklmno
            //암호화 된 문자열 : NDFjNzc2MGM1MGVmZGU5OWJmNTc0ZWQ4ZmZmYzdhNmRkMzQwNWQ1NDZkM2RhOTI5YjIxNGM4OTQ1YWNmOGE5Nw==


            return Convert.ToBase64String(encoder.GetBytes(hashString));
        }
        #endregion


        #region DES암복호화

        public string Set_Key(string sKey)
        {
            int i = 0;
            string sTemp = "12345678";
            i = 8 - sKey.Length;

            if (i < 0)
            {
                return sKey.Substring(0, 8);
            }
            else
            {
                return sKey + (sTemp.Substring(0, i));
            }
        }


        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="inStr">암호 문자열</param>
        /// <param name="desKey">암호 key값</param>
        /// <returns>암호문</returns>
        public string DESEncrypt(string inStr)
        {
            return DesEncrypt(inStr);
        }

        /// <summary>
        /// 복호와
        /// </summary>
        /// <param name="inStr">복호화 할 문자열</param>
        /// <param name="desKey">key값</param>
        /// <returns>해독된 문자열</returns>
        public string DESDecrypt(string inStr, string desKey) // 복호화
        {
            return DesDecrypt(inStr, desKey);
        }
        public string DESDecrypt(string inStr) // 복호화
        {
            return DesDecrypt(inStr, KEY_DES);
        }


        /// <summary>
        /// 암호화 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string DesEncrypt(string str)
        {

            string key = KEY_DES;
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }

            //소스 문자열
            byte[] btSrc = ConvertStringToByteArray(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desencrypt = des.CreateEncryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desencrypt,
             CryptoStreamMode.Write);

            cs.Write(btSrc, 0, btSrc.Length);
            cs.FlushFinalBlock();


            byte[] btEncData = ms.ToArray();

            return (ConvertByteArrayToStringB(btEncData));
        }//end of func DesEncrypt


        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string DesDecrypt(string str, string key)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }


            byte[] btEncData = ConvertStringToByteArrayB(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desdecrypt = des.CreateDecryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desdecrypt,
             CryptoStreamMode.Write);

            cs.Write(btEncData, 0, btEncData.Length);

            cs.FlushFinalBlock();

            byte[] btSrc = ms.ToArray();


            return (ConvertByteArrayToString(btSrc));

        }//end of func DesDecrypt

        //문자열->유니코드 바이트 배열
        private static Byte[] ConvertStringToByteArray(String s)
        {
            return (new UnicodeEncoding()).GetBytes(s);
        }

        //유니코드 바이트 배열->문자열
        private static string ConvertByteArrayToString(byte[] b)
        {
            return (new UnicodeEncoding()).GetString(b, 0, b.Length);
        }

        //문자열->안시 바이트 배열
        private static Byte[] ConvertStringToByteArrayA(String s)
        {
            return (new ASCIIEncoding()).GetBytes(s);
        }

        //안시 바이트 배열->문자열
        private static string ConvertByteArrayToStringA(byte[] b)
        {
            return (new ASCIIEncoding()).GetString(b, 0, b.Length);
        }

        //문자열->Base64 바이트 배열
        private static Byte[] ConvertStringToByteArrayB(String s)
        {
            return Convert.FromBase64String(s);
        }

        //Base64 바이트 배열->문자열
        private static string ConvertByteArrayToStringB(byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        #endregion //DES암복호화

    }
}

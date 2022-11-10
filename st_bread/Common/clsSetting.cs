using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography;
using System.Globalization;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Diagnostics;

namespace st_bread
{
    class clsSetting
    {   
        
        public static int iSelectedCount { get; set; }

        public static string Now_Date()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string Now_Time()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        public static string Now_DateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd") + " " +  DateTime.Now.ToString("HH:mm:ss");
        }

        public static int _Today()
        {
            return clsDateTime.Set_Time(DateTime.Now);
        }


        #region 프로그램 디렉토리

        private static string Get_Dir(string sDir)
        {
            try
            {

                if (Directory.Exists(sDir) == false)
                {
                    Directory.CreateDirectory(sDir);
                }
                return sDir;
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
        }

        public static string BIN_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "bin");
            return Get_Dir(sDir);
        }

        public static string CON_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "config");
            return Get_Dir(sDir);
           
        }

        public static string IMG_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "img");
            return Get_Dir(sDir);
            
        }

        public static string LOG_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "log");
            return Get_Dir(sDir);
        }

        public static string ERR_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "errlog");
            return Get_Dir(sDir);
            
        }

        public static string SERIAL_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "serial");
            return Get_Dir(sDir);
            
        }
        
        public static string INFO_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "info");
            return Get_Dir(sDir);
            
        }
        
        public static string BACK_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "backup");
            return Get_Dir(sDir);
        }

        public static string BILL_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "bill");
            return Get_Dir(sDir);
            
        }

        public static string MOV_DIR()
        {
            string sDir = Path.Combine(Application.StartupPath, "mov");
            return Get_Dir(sDir);
        }

        #endregion




        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        /// <summary>
        /// 버튼 누른 효과 나타 내기 위해 타이머로 돌린다.
        /// </summary>
        /// <param name="MS">millesecon</param>
        /// <returns></returns>
        public static DateTime Delay(Control oCtrl, int MS, int iDirect)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            int iX = oCtrl.Location.X, iY = oCtrl.Location.Y;
            int iWid = oCtrl.Width, iHei = oCtrl.Height;
            int i = 2;

            while (AfterWards >= ThisMoment)
            {
                if (iDirect == 0)
                {
                    oCtrl.Location = new Point(iX + i, iY + i);
                    oCtrl.Size = new Size(iWid - i, iHei - i);
                }
                else
                {
                    oCtrl.Location = new Point(iX - i, iY - i);
                    oCtrl.Size = new Size(iWid + i, iHei + i);

                }

                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        public static DateTime Delay( int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

         

            while (AfterWards >= ThisMoment)
            {
                Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }


        #region XML 사용
        ///XML 설정 쓰기
        ///
        //setting xml 에 값을 넣기 위해 사용 기존 값 변경 혹은 신규값 추가
        //public void Check_PutSetting(string sNode, string sKey, string sval)
        //{
        //    if (SettingHT[sKey] != null)
        //    {
        //        SettingHT[sKey] = sval;
        //        Set_XMLVal(SETTING_FILE(), sNode, sKey, sval);
        //    }
        //    else
        //    {
        //        Set_XMLVal(clsPrintSet.SETTING_FILE(), sNode, sKey, sval);
        //        clsPrintSet.SettingHT.Add(sKey, sval);
        //    }
        //}


        //해쉬테이블이용 XML 가져오기
        public Hashtable ReadXml(string sFile)
        {
            try
            {
                string sKey, sValue;
                Hashtable ht = new Hashtable();
                XmlTextReader xtr = new XmlTextReader(sFile);

                while (xtr.Read())
                {
                    if (xtr.NodeType == XmlNodeType.Element)
                    {
                        sKey = xtr.LocalName;
                        xtr.Read();

                        if (xtr.NodeType == XmlNodeType.Text)
                        {
                            sValue = xtr.Value;
                            ht.Add(sKey, sValue);
                        }

                        else
                            continue;
                    }

                }//while

                xtr.Close();
                return ht;

            }
            catch (FileNotFoundException ex)
            {

                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

                //xml파일을 찾지 못할경우 default값이 들어있는 해쉬테이블을 리턴한다.

                Hashtable ht = new Hashtable();
                ht.Add("COL5", "0");
                ht.Add("DBNAME2", "ft2");
                ht.Add("DAY", "0");
                ht.Add("SubHeight", "60");
                ht.Add("MIN", "0");
                ht.Add("COL3", "180");
                ht.Add("IP", "seumtech.kr");
                ht.Add("PASS2", "8jV9fgREukMsa3rsnqMZvw==");
                ht.Add("IMG", "http://ft.seumtech.kr/ft/img/sample/");
                ht.Add("PORT2", "3307");
                ht.Add("IP2", "127.0.0.1");
                ht.Add("PASS", "AcFag8gyirdSMNWcbC+cTPAD04F+xWnz");
                ht.Add("COL2", "100");
                ht.Add("ORDER", "0");
                ht.Add("COL4", "200");
                ht.Add("AUTH", "0");
                ht.Add("DBNAME", "ft2");
                ht.Add("MainHeight", "320");
                ht.Add("COL1", "300");
                ht.Add("SEC", "30");
                ht.Add("PORT", "3309");
                ht.Add("COL7", "0");
                ht.Add("USER", "buyerfgd");
                ht.Add("COL0", "0");
                ht.Add("SubMargin", "10");
                ht.Add("MainWidth", "300");
                ht.Add("MILSEC", "0");
                ht.Add("COL6", "100");
                ht.Add("Display", "http://ft.seumtech.kr/ft/manu/main.asp?");
                ht.Add("HOUR", "0");
                ht.Add("SubWidth", "180");
                ht.Add("MainMargin", "10");
                ht.Add("USER2", "root");

                // WriteErrLog(MethodBase.GetCurrentMethod().Name, e.Message.ToString());
                return ht;
            }
        }

        
        #endregion



        public static double Set_Amt(double dAmt, clsEnum.Item_vat vat)
        {   
            double dReturn = 0;
            switch (vat)
            {
                case clsEnum.Item_vat.item_duty_free: //면제                                 
                    dReturn = dAmt;
                    break;                
                case clsEnum.Item_vat.item_tax: //과세 별도
                    dReturn = Math.Round(dAmt / 10 , 2);
                    break;
                
                default:                    
                    dReturn = dAmt;
                    break;
            }
            return dReturn;
        }

        public static double Set_Amt(double dAmt, string vat)
        {
            double dReturn = 0;
            switch (vat)
            {
                case "dutyfree": //면제                                 
                    dReturn = dAmt;
                    break;
                case "dutyfree2": //영세율

                    dReturn = dAmt;
                    break;
                case "tax": //과세 별도
                    dReturn = Math.Round(dAmt / 10, 2);
                    break;
                case "tax_include": //과세 포함
                    dReturn = Math.Round(dAmt / 10, 2);
                    break;
                default:
                    dReturn = dAmt;
                    break;
            }
            return dReturn;
        }





        private static string Make_Digit(string sBarCode)
        {
            var chars = sBarCode.ToCharArray();
            int isum = 0;
            int iStart = chars.Length - 1;
            for (int i = iStart; i >= 0; i--)
            {

                isum += clsSetting.Let_Int(chars[i].ToString()) * 3;
                i--;
                if (i < 0)
                {
                    break;
                }
                else
                {
                    isum += clsSetting.Let_Int(chars[i].ToString());
                }
            }
            int checksum_digit = 10 - (isum % 10);
            if (checksum_digit == 10)
                checksum_digit = 0;

            return sBarCode + checksum_digit.ToString();
        }

        /// <summary>
        ///  bool타입을 int로 변환 0-false 1-true
        /// </summary>
        /// <param name="bStr"></param>
        /// <returns></returns>
        public static int Let_Int(bool bStr)
        {
            int iReturn = 0;

            if (bStr == null)
                return iReturn;

            if (bStr.ToString().Length == 0)
            {
                return iReturn;
            }

            if (bStr == true)
            {
                iReturn = 1;
            }
            else
            {
                iReturn = 0;
            }
            return iReturn;
        }
        /// <summary>
        /// 문자열을 bool타입으로 변환 0-false 1-true
        /// </summary>
        /// <param name="sStr"></param>
        /// <returns></returns>
        public static bool Let_Boolean(string sStr)
        {
            bool bReturn = false;

            if (sStr == null)
                return bReturn;

            if (sStr.Length == 0)
            {
                return bReturn;
            }
            if (sStr == string.Empty)
            {
                return bReturn;
            }

            if (sStr == "1")
                bReturn = true;


            return bReturn;
        }


        public static string Let_Money(double dAmt)
        {
            return String.Format("{0:#,##0}", dAmt);
        }
        public static string Let_Money(int dAmt)
        {
            return String.Format("{0:#,##0}", dAmt);
        }
        public static string Let_Money(string sAmt)
        {
            return String.Format("{0:#,##0}", sAmt);
        }

        public static Double Let_Percent(double dUpper,double dLower)
        {
            if (dUpper > 0 && dLower > 0)
                return Math.Round(dUpper / dLower  * 100, 2);
            else
                return 0;
        }


        public static Double Let_Double(string sStr)
        {
            double dReturn = 0;

            if (sStr == null)
                return dReturn;

            if (sStr.Length == 0)
            {
                return dReturn;
            }


            if (sStr == string.Empty)
            {
                return dReturn;
            }

            double dTemp = 0;

            if (Double.TryParse(sStr, out dTemp))
            {
                dReturn = dTemp;
            }
            else
            {
                return dReturn;
            }
            return dReturn;
        }

        public static int Let_Int(string sStr)
        {
            int iReturn = 0;

            if (sStr == null)
                return iReturn;

            if (sStr.Length == 0)
            {
                return iReturn;
            }


            if (sStr == string.Empty)
            {
                return iReturn;
            }

            int iTemp = 0;

            if (Int32.TryParse(sStr, out iTemp))
            {
                iReturn = iTemp;
            }
            else
            {
                return iReturn;
            }
            return iReturn;
        }


        public static int Let_Int(char c)
        {
            return (int)(c - '0');
        }

        
        public static string Let_String(string sStr)
        {
            if (sStr == null)
                return string.Empty;

            if (sStr.Length == 0)
                return string.Empty;
            else
                return sStr;

        }


        public static string Let_CardKind(string str)
        {
            string sReturn = "";
            switch (str)
            {
                case "CK":
                    sReturn = "신용카드";
                    break;
                case "CH":
                    sReturn = "체크카드";
                    break;
                case "GK":
                    sReturn = "기프트카드";
                    break;
                case "UP":
                    sReturn = "중국은련";
                    break;
                case "NT":
                    sReturn = "BC/NH면세유";
                    break;
                case "OL":
                    sReturn = "유가보조카드";
                    break;
                case "CP":
                    sReturn = "원카드";
                    break;
                case "CA":
                    sReturn = "현금";
                    break;
                case "ACA":
                    sReturn = "현금영수증";
                    break;
                default:
                    sReturn = "불명";
                    break;
            }

            return sReturn;
        }

        /// <summary>
        /// 나눗셈 하기
        /// </summary>
        /// <param name="dVal"></param>
        /// <param name="dBottom"></param>
        /// <returns></returns>
        public static double Let_Squd(double dVal, int dBottom)
        {
            double dTemp = 0;
            
            
            if (dVal == 0 || dBottom == 0)
            {
                return 0;
            }

            dTemp = dVal / dBottom;

            dTemp = Math.Round(dTemp / 1.1, 0); //Math.Ceiling(dTemp);

            return dTemp;

        }

        public static double Let_Division(double dVal, int dBottom)
        {
            double dTemp = 0;


            if (dVal == 0 || dBottom == 0)
            {
                return 0;
            }

            dTemp = dVal / dBottom;

            dTemp = Math.Round(dTemp , 2); //Math.Ceiling(dTemp);

            return dTemp;

        }
        
        public static DateTime Let_DateTime(string sStr)
        {

            if (sStr.Length == 0)
                return DateTime.Now;

            DateTime dT = new DateTime();

            //if (sStr.Length == 6) //190707 포맷인경우
            //{
            //    sStr = string.Format("{0} {1} {2}", sStr.Substring(0, 2), sStr.Substring(2, 2), sStr.Substring(4, 2));
            //}

            if (sStr.Length == 12)
            {
                dT = DateTime.ParseExact(sStr, "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (sStr.Length == 10)
            {
                dT = DateTime.ParseExact(sStr , "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (sStr.Length == 6)
            {
                dT = DateTime.ParseExact(string.Format("{0}000000", sStr), "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (sStr.Length == 8)
            {
                dT = DateTime.ParseExact(string.Format("{0}000000", sStr), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                dT = DateTime.Now;
            }

            //DateTime.TryParse(sStr, null, System.Globalization.DateTimeStyles.AssumeLocal, out dT);


            //dT = DateTime.ParseExact(sStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            return dT;

        }

        public static String Let_DoubleToString(double dVal)
        {
            return string.Format("{0:#,##0.00}", dVal);
        }

        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }



        public static int Get_CapaPerAmt(double Capa, double ct)
        {
            double num1 = 100.0;
            int num2 = 0;
            if (Capa != 0.0)
            {
                double num3 = Capa / num1;
                num2 = (int)((double)ct * num3) / 10 * 10;
            }
            return num2;
        }


        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }





        public static StringBuilder Set_CSSHeader()
        {
            StringBuilder oStrB = new StringBuilder();
            oStrB.AppendLine(" <!doctype html> ");
            oStrB.AppendLine(" <html lang='en'> ");
            oStrB.AppendLine("  <head> ");
            oStrB.AppendLine(" <style type='text/css'> ");
            oStrB.AppendLine(" @import url('http://fonts.googleapis.com/earlyaccess/nanumgothic.css');  ");
            oStrB.AppendLine(" /*<![CDATA[*/ ");
            oStrB.AppendLine(" body { ");
            oStrB.AppendLine(" 	font-family:'NanumGothic', '나눔고딕','NanumGothicWeb', '맑은 고딕', 'Malgun Gothic', Dotum; } ");
            oStrB.AppendLine(" .orderView  { ");
            oStrB.AppendLine(" 	margin-top:15px; ");
            oStrB.AppendLine(" 	border:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	border-collapse: collapse; ");
            oStrB.AppendLine(" 	border-left-style:none; ");
            oStrB.AppendLine(" 	border-right-style:none;	 ");
            oStrB.AppendLine(" 	line-height: 2; ");
            oStrB.AppendLine(" 	width:100%; ");
            oStrB.AppendLine(" 	text-align:center;} ");
            oStrB.AppendLine(" .orderView caption { ");
            oStrB.AppendLine(" 	font-size:2em; ");
            oStrB.AppendLine(" 	padding-bottom: 10px;} ");
            oStrB.AppendLine(" .orderView thead th{ ");
            oStrB.AppendLine(" 	border-top:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	border-bottom:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	border-left:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	font-size:1.5em; ");
            oStrB.AppendLine(" 	font-weight:bold; ");
            oStrB.AppendLine(" 	color: rgb(172,33,32); ");
            oStrB.AppendLine(" 	background: rgb(2444, 245, 226);} ");
            oStrB.AppendLine(" .orderView thead th:first-child { ");
            oStrB.AppendLine(" 	border-left: 0px !important;} ");
            oStrB.AppendLine(" .orderView tbody td{ ");
            oStrB.AppendLine(" 	border-top:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	border-bottom:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	border-left:1px solid rgb(224, 225, 211); ");
            oStrB.AppendLine(" 	font-size:1.5em;	 ");
            oStrB.AppendLine(" 	padding-bottom:1px; ");
            oStrB.AppendLine(" 	font-weight: normal;} ");
            oStrB.AppendLine(" .orderView tbody td:first-child { ");
            oStrB.AppendLine(" 	border-left: 0px !important;} ");
            oStrB.AppendLine(" .orderView tbody tr:nth-child(2n) { ");
            oStrB.AppendLine(" 	background:#f2f2f2;} ");
            oStrB.AppendLine(" .orderView tfoot tr th{ ");
            oStrB.AppendLine(" 	font-size:1.5em; ");
            oStrB.AppendLine(" 	text-align:left; ");
            oStrB.AppendLine(" 	padding: 30px 0px 0px 30px;} ");
            oStrB.AppendLine(" .orderView tfoot tr td{ ");
            oStrB.AppendLine(" 	padding-top: 30px; ");
            oStrB.AppendLine(" 	font-size:2em; ");
            oStrB.AppendLine(" 	padding: 30px 60px 0px 0px;	} ");

            oStrB.AppendLine(" th {background-color: #094b9c; font-size:1.5em; color:#ffffff} ");

            oStrB.AppendLine(" .txt_l {text-align:left !important;} ");
            oStrB.AppendLine(" .pl10 {padding-left: 10px !important;} ");
            oStrB.AppendLine(" /*]]>*/ ");
            oStrB.AppendLine("  </style>  ");
            oStrB.AppendLine("  </head> ");
            oStrB.AppendLine("  <body> ");


            return oStrB;
        }
        
        public static System.Boolean IsNumeric (System.Object Expression)
        {
            if(Expression == null || Expression is DateTime)
                return false;

            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try 
            {
                if(Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                    return true;
                } 
            catch {} // just dismiss errors but return false
                return false;
        }

        /// <summary>
        /// 현재 odbc 커맨드의 파라메터 값 숫자만큼 ? 표시 만들기
        /// </summary>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public static string Get_ParameterSymbol(int iCount)
        {
            string sParameterSymbols = "(";

            for (int i = 1; i <= iCount; i++)
            {
                if (i == iCount)
                {

                    sParameterSymbols += "?";
                }
                else
                {
                    sParameterSymbols += "?,";
                }
            }

            sParameterSymbols += ")";

            return sParameterSymbols;
        }

        /// <summary>
        /// cell의 value가 nulㅣ 이거나 아무 글자도 없는경우 확인
        /// </summary>
        /// <param name="Grid">datagridview의 cell</param>
        /// <returns>true-값정상 false-null이거나 빈문자</returns>
        public static bool isOkValue(object Grid)
        {
            bool bReturn = false;
            if (Grid != null)
            {   
                if (Grid.ToString().Length > 0)
                {
                    bReturn = true;  
                }
            }
            return bReturn;
        }
        public static bool isOkValue_Grid(object Grid)
        {
            bool bReturn = false;
            if (Grid != null)
            {
                bReturn = true;             
            }
            return bReturn;
        }






        /// <summary>
        /// enum 값 넣기
        /// </summary>
        /// <param name="en">enum</param>
        /// <returns>description 값</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                //해당 text 값을 배열로 추출해 옵니다.
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }


        public static clsEnum.Van_Cmpny GetVanEnum(string str)
        {
            clsEnum.Van_Cmpny oVan = clsEnum.Van_Cmpny.none;

            int iVal = 0;

            var enumlist = Enum.GetValues(typeof(clsEnum.Van_Cmpny))
                .Cast<clsEnum.Van_Cmpny>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                    value
                })
            .OrderBy(item => item.value)
            .ToList();

            foreach (var data in enumlist)
            {
                if (data.Description == str)
                {
                    iVal = (int)data.value;

                    oVan = (clsEnum.Van_Cmpny)iVal;
                    break;
 
                }

            }

            return oVan;
        }



        public static void List_AlternateRows(ListView oLst)
        {
            //짝수 컬럼 색 변경
            int iIdx = 1;
            foreach (ListViewItem item in oLst.Items)
            {
                if ((iIdx % 2) == 0)
                    item.BackColor = Color.AliceBlue;
                iIdx++;
            }
        }
         public static void List_AlternateRowsInit(ListView oLst)
        {
            //짝수 컬럼 색 초기화
            int iIdx = 1;
            foreach (ListViewItem item in oLst.Items)
            {
                if ((iIdx % 2) == 0)
                    item.BackColor = Color.White;
                iIdx++;
            }

        }


       

        /// <summary>
        /// 전화번호 입력란에 숫자 방지
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string TelNumberTextBox(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToInt32(Keys.Back) || (e.KeyChar == '-') || e.KeyChar == Convert.ToInt32(Keys.Enter))
            {
                //e.Handled = true;
                return "";
            }
            else
            {
                e.Handled = true;
                return "숫자와 - 만 입력할수 있습니다.";
            }
        }

        /// <summary>
        /// 숫자만 입력 받기
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string OnlyNumberTextBox(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToInt32(Keys.Back) || e.KeyChar == Convert.ToInt32(Keys.Enter) ||e.KeyChar == 46 )
            {
                //e.Handled = true;
                return "";
            }
            else
            {
                e.Handled = true;
                return "숫자만 입력할수 있습니다.";
            }
        }


        /// <summary>
        /// 사업자 번호 체크
        /// </summary>
        /// <param name="businessCode"></param>
        /// <returns></returns>
        public static bool businessNumberCheck(string businessCode)
        {
            Boolean isCompanyNumberCheck = true;

            String companyNumber = businessCode.Trim().Replace("-", "").Replace(" ", "");
            String checkRule = "137137135"; // 사업자등록번호 체크 형식

            int step1 = 0, step2 = 0, step3 = 0, step4 = 0, step5 = 0, step6 = 0, step7 = 0;

            if (companyNumber.Length == 10 && Regex.IsMatch(companyNumber, @"^[0-9]+$"))
            {
                for (int i = 0; i < 7; i++)
                {

                    step1 = step1 + (int.Parse(companyNumber.Substring(i, 1)) * int.Parse(checkRule.Substring(i, 1)));

                }

                step2 = step1 % 10;
                step3 = (int.Parse(companyNumber.Substring(7, 1)) * int.Parse(checkRule.Substring(7, 1))) % 10;
                step4 = int.Parse(companyNumber.Substring(8, 1)) * int.Parse(checkRule.Substring(8, 1));
                step5 = (int)Math.Round((step4 / 10 - 0.5), MidpointRounding.AwayFromZero);
                if (step5 < 0)

                    step5 = 0;

                step6 = step4 - (step5 * 10);
                step7 = (10 - ((step2 + step3 + step5 + step6) % 10)) % 10;

                if (int.Parse(companyNumber.Substring(9, 1)) != step7)

                    isCompanyNumberCheck = false;//실패(맞지 않은 번호)

            }
            else
            {

                isCompanyNumberCheck = false;
            }
            return isCompanyNumberCheck;
        }


        /// <summary>
        /// 주민번호 번호 체크
        /// </summary>
        /// <param name="RRN"></param>
        /// <returns></returns>
        public static bool socialNumberCheck(string RRN)
        {

            //공백 제거

            RRN = RRN.Replace(" ", "");

            //문자 '-' 제거

            RRN = RRN.Replace("-", "");

            //주민등록번호가 13자리인가?

            if (RRN.Length != 13)
            {

                return false;

            }



            int sum = 0;

            for (int i = 0; i < RRN.Length - 1; i++)
            {

                char c = RRN[i];

                //숫자로 이루어져 있는가?

                if (!char.IsNumber(c))
                {

                    return false;

                }

                else
                {

                    if (i < RRN.Length)
                    {

                        //지정된 숫자로 각 자리를 나눈 후 더한다.

                        sum += int.Parse(c.ToString()) * ((i % 8) + 2);

                    }

                }

            }

            // 검증코드와 결과 값이 같은가?

            if (!((((11 - (sum % 11)) % 10).ToString()) == ((RRN[RRN.Length - 1]).ToString())))
            {

                return false;

            }

            return true;

        }

        /// <summary>
        /// 모든 폼의 판넬등 컨트롤 초기화
        /// </summary>
        /// <param name="oCtr"></param>
        public static void Clear_Control(Control oCtr)
        {
            foreach (TextBox textBox in clsSetting.GetAll(oCtr, typeof(TextBox)))
            {
                textBox.Text = string.Empty;
                textBox.DataBindings.Clear();
            }
            foreach (CheckBox chk in clsSetting.GetAll(oCtr, typeof(CheckBox)))
            {
                chk.Checked = false;
            }

            foreach (ComboBox ComBox in clsSetting.GetAll(oCtr, typeof(ComboBox)))
            {
                ComBox.SelectedValue = 0;
            }
            foreach (MaskedTextBox textBox in clsSetting.GetAll(oCtr, typeof(MaskedTextBox)))
            {
                textBox.Text = string.Empty;
                textBox.DataBindings.Clear();
            }
        }
        public static void Clear_Control(Control oCtr,bool oBool)
        {
            foreach (TextBox textBox in clsSetting.GetAll(oCtr, typeof(TextBox)))
            {
                textBox.Text = string.Empty;
                textBox.DataBindings.Clear();
            }
            foreach (CheckBox chk in clsSetting.GetAll(oCtr, typeof(CheckBox)))
            {
                chk.Checked = false;
            }

            foreach (MaskedTextBox textBox in clsSetting.GetAll(oCtr, typeof(MaskedTextBox)))
            {
                textBox.Text = string.Empty;
                textBox.DataBindings.Clear();
            }
        }


       


        /// <summary>
        /// 현재 바코드가 유효한지 검사.
        /// </summary>
        /// <param name="sBarCode"></param>
        /// <returns></returns>
        public static bool Check_Digit(string sBarCode)
        {
            if (sBarCode.Trim().Length == 0)
                return false;

            string sTempCode = sBarCode.Substring(0, sBarCode.Length-1 );
            var chars = sTempCode.ToCharArray();
            int isum = 0;
            string sCheckVal = sBarCode.Substring(sBarCode.Length - 1, 1);

            int iStart = chars.Length - 1;
            for (int i = iStart; i >= 0; i--)
            {

                isum += clsSetting.Let_Int(chars[i].ToString()) * 3;
                i--;
                if (i < 0)
                {
                    break;
                }
                else
                {
                    isum += clsSetting.Let_Int(chars[i].ToString());
                }
            }



            int checksum_digit = 10 - (isum % 10);
            if (checksum_digit == 10)
                checksum_digit = 0;

            if (checksum_digit.ToString() == sCheckVal)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int Kill_Program(string sExeName)
        {

            try
            {
                // Get the name of the On screen keyboard
                string processName = System.IO.Path.GetFileNameWithoutExtension(sExeName);

                // Check whether the application is not running 
                var query = from process in Process.GetProcesses()
                            where process.ProcessName == processName
                            select process;

                var keyboardProcess = query.FirstOrDefault();

                // launch it if it doesn't exist
                if (keyboardProcess != null)
                {
                    keyboardProcess.Kill();
                    keyboardProcess.Dispose();
                }

                return 0;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }

        public static double Set_Sale_Price(double dCost, double dPercent)
        {
            return dCost - Math.Round(dCost * (dPercent / 100), 0); ;

        }


        public static void Exit_Program()
        {
            try
            {
                Application.Exit();
                //Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
 
            }
        }


    }
}

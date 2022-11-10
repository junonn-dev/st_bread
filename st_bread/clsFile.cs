using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection; //현재 자신의 함수 이름 가져올때 사용
using System.Xml;
using System.Collections;
using System.Windows.Forms;

namespace st_bread
{
    class clsFile
    {
        private StreamWriter swWrite;
        private string CurrentPath = Environment.CurrentDirectory;
       // private string CurrentPath = string.Empty;
        
        private string sLogDir = string.Empty;

        public clsFile()
        {
            //CurrentPath = Get_AppPath();
            sLogDir = CurrentPath + "\\Log";

            if (Directory.Exists(sLogDir) == false)
            {
                try
                {
                    Directory.CreateDirectory(sLogDir);
                }
                catch (Exception ex)
                {
                   
                }
            }

        }

        public int WriteFile(string sFile, string sClass, string sArg)
        {
            int iReturn = 0;
            try
            {
                swWrite = new StreamWriter(new FileStream(sFile, FileMode.Append, FileAccess.Write));
                swWrite.WriteLine("[" + DateTime.Now.ToString() + "] " + "\t" + sClass + "\t" + sArg);
                swWrite.Flush();
                swWrite.Close();

            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), 1);
                iReturn = -1;
            }
            return iReturn;
        }

        public int WriteLog(string sClass, string sArg)
        {
            int iReturn = 0;
            string sFile = string.Empty;
            //string sFile = System.DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string sPrevClass = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Name.ToString() + "::" + sClass; //로그 발생 클래스 명 가져오기

            sFile = Chk_LogFile();
            iReturn = WriteFile(sFile, sPrevClass, sArg);

            return iReturn;
        }

        public int WriteErrLog(string sClass, string sArg)
        {
            int iReturn = 0;
            string sFile = "Err_" + System.DateTime.Today.ToString("yyyyMMdd") + ".txt";
            string sPrevClass = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Name.ToString() + "::" + sClass; //로그 발생 클래스 명 가져오기

            sFile = sLogDir + "\\" + sFile;

            iReturn = WriteFile(sFile, sPrevClass, sArg);

            return iReturn;
        }


        public int WriteErrLog(string sClass, string sArg, int i)
        {
            int iReturn = 0;
            string sLogDir = CurrentPath + "\\Log";
            string sFile = "Err_" + System.DateTime.Today.ToString("yyyyMMdd") + ".txt";

            sFile = sLogDir + "\\" + sFile;
            try
            {
                swWrite = new StreamWriter(new FileStream(sFile, FileMode.Append, FileAccess.Write));
                swWrite.WriteLine("[" + DateTime.Now.ToString() + "] " + sClass + " ->> " + sArg);
                swWrite.Flush();
                swWrite.Close();
            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                iReturn = -1;
            }
            return iReturn;
        }

        //로그 파일 사이즈 체크 후 파일명 리턴 한다.
        public string Chk_LogFile()
        {
            string sReturn = string.Empty;
            string sFile = string.Empty;
            
            string sDate = System.DateTime.Today.ToString("yyyyMMdd");
            string newFilePath = Path.Combine(sLogDir, sDate + ".txt");
            int count = 1;

            try
            {
                
                while (File.Exists(newFilePath))
                {
                    string temporaryFileName;
                    FileInfo oFinfo = new FileInfo(newFilePath);
                    if (oFinfo.Length > 1048576)//1048576
                    {
                         temporaryFileName = string.Format("{0} ({1})", sDate, count++);
                         newFilePath = Path.Combine(sLogDir, temporaryFileName + ".txt");
                    }
                    else
                    {
                         temporaryFileName =  Path.GetFileNameWithoutExtension(newFilePath);
                         newFilePath = Path.Combine(sLogDir, temporaryFileName + ".txt");
                         break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }
            return newFilePath;
        }





        ///XML 설정 쓰기
        ///
        //setting xml 에 값을 넣기 위해 사용 기존 값 변경 혹은 신규값 추가
        public void Check_PutSetting(string sNode,string sKey, string sval)
        {

            if (clsPrintSet.SettingHT[sKey] != null)
            {
                clsPrintSet.SettingHT[sKey] = sval;
                Set_XMLVal(clsPrintSet.SETTING_FILE(), sNode, sKey, sval);
            }
            else
            {
                Set_XMLVal(clsPrintSet.SETTING_FILE(), sNode, sKey, sval);
                clsPrintSet.SettingHT.Add(sKey, sval);
            }
        }



        // Setting XML 저장
        private bool bSaveXML()
        {
            string sFilePath = "D:/";
 
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                XmlWriter xmlWriter = XmlWriter.Create(sFilePath + "/Settings.xml"); // 저장할 xml 파일명
                xmlWriter.WriteStartDocument();
 
                xmlWriter.WriteStartElement("root");
                xmlWriter.WriteElementString("PRODUCT", "갤럭시"); // 앞이 node, 뒤는 value
                xmlWriter.WriteElementString("WORKER", "삼손");
                xmlWriter.WriteEndDocument();
 
                xmlWriter.Flush(); // xml 파일을 쓴다.
                xmlWriter.Close(); // 반드시 닫아줍시다.
 
                return true;
            }
            catch (Exception ex)
            {
                //WriteLog(ex.Message);
                return false;
            }
        }

        //읽기
        // xml 파일 열기
        private bool bLoadXML()
        {
            bool bFlag;
            string sFilePath = "D:/";
 
            try
            {
                if (File.Exists(sFilePath + "/Settings.xml"))   //  xml 파일 존재 유무 검사
                {
                    XmlTextReader xmlReadData = new XmlTextReader(sFilePath + "/Settings.xml");    //  xml 파일 열기
 
                    while (xmlReadData.Read())
                    {
                        if (xmlReadData.NodeType == XmlNodeType.Element)
                        {
                            switch (xmlReadData.Name.ToUpper().Trim())
                            {
                                //case "PRODUCT": sProductNm = xmlReadData.ReadString().ToString().Trim(); break; // node name이 PRODUCT 일 때
                                //case "WORKER": sWorkerNm = xmlReadData.ReadString().ToString().Trim(); break;
                            }
                        }
                    }
                    xmlReadData.Close();
                }
                else // xml 파일이 존재 하지 않을 때
                {
                    // 디폴트값으로 xml 파일을 기록해야하므로 저장 함수로 보낸다.
                    bSaveXML();
                }
          
                return true;
            }
            catch (Exception ex)
            {
                //WriteLog(ex.Message);
                return false;
            }
        }


        //해쉬테이블이용 XML 가져오기
        public Hashtable ReadXml(string sFile)
        {

            try
            {
                string sKey, sValue;
                Hashtable ht = new Hashtable();
                XmlTextReader xtr = new XmlTextReader(sFile );

                while (xtr.Read())
                {

                    if (xtr.NodeType == XmlNodeType.Element)
                    {
                        sKey = xtr.LocalName;

                        xtr.Read();
                        
#if DEBUG
                        Console.WriteLine(sKey);
#endif

                        if (xtr.NodeType == XmlNodeType.Text)
                        {
                            sValue = xtr.Value;

                            ht.Add(sKey, sValue);
                            
#if DEBUG
                            Console.WriteLine(sValue);
#endif
                        }

                        else
                            continue;

                    }

                }//while

                xtr.Close();
                return ht;

            }
            catch (FileNotFoundException e)
            {

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
                WriteErrLog(MethodBase.GetCurrentMethod().Name, e.Message.ToString());
                return ht;

            }

        }


        //attribute 접근 form 의 컨트롤 배열에 사용
        public Hashtable ReadLayerXml(string sFile,string sElement)
        {
            try
            {
                string sKey = string.Empty; 
                string sValue;
                Hashtable ht = new Hashtable();                
                XmlDocument oDoc = new XmlDocument();

                XmlReaderSettings xSetting = new XmlReaderSettings();
                xSetting.IgnoreComments = true;
                xSetting.IgnoreWhitespace = true;
                
                oDoc.Load(sFile);
                XmlNodeList oNode = oDoc.DocumentElement.SelectNodes(sElement);

                foreach (XmlNode xNode in oNode)
                {
                    XmlNodeList cNodeList = xNode.ChildNodes;

                    foreach (XmlNode cNode in cNodeList)
                    {
                        if (cNode.Name.CompareTo("CTRL") == 0 && cNode.NodeType == XmlNodeType.Element)
                        {
                            
#if DEBUG
                            Console.WriteLine("1- " + cNode.Name);
                            
#endif
                            foreach (XmlAttribute attribute in cNode.Attributes)
                            {
                                if (attribute.Name == "name")
                                    sKey = attribute.Value;
                                else if (attribute.Name == "x")
                                {
                                    ht.Add(sKey + attribute.Name, attribute.Value);
                                    
                                }
                                else if (attribute.Name == "y")
                                {
                                    ht.Add(sKey + attribute.Name, attribute.Value);
                                    
                                }
                                else if (attribute.Name == "w")
                                {
                                    ht.Add(sKey + attribute.Name, attribute.Value);
                                    
                                }
                                else if (attribute.Name == "h")
                                {
                                    ht.Add(sKey + attribute.Name, attribute.Value);
                                    
                                }
                                else if (attribute.Name == "img")
                                {
                                    ht.Add(sKey + attribute.Name, attribute.Value);
                                    
                                }


                            }
                        }
                    }
                }
                
                
                return ht;

            }
            catch (FileNotFoundException e)
            {
                //xml파일을 찾지 못할경우 빈 ht리턴 한다.
                Hashtable ht = new Hashtable();
                
                WriteErrLog(MethodBase.GetCurrentMethod().Name, e.Message.ToString());
                return ht;

            }

        }


        //특정 노드값 추가 삭제
        public int Set_XMLVal(string sXml,string First,string sELements, string sVal)
        {
            try
            {
                Console.WriteLine("aa1 : " + " " + sXml + " " + First + " " + sELements + " " + sVal);

                XmlDocument doc = new XmlDocument();
                doc.Load(sXml);

                // 첫노드를 잡아주고 하위 노드를 선택한다
                XmlNode FristNode = doc.DocumentElement;
                XmlElement SubNode = (XmlElement)FristNode.SelectSingleNode(First);

                XmlNode DeleteNode = SubNode.SelectSingleNode(sELements);

                if (DeleteNode != null)
                    SubNode.RemoveChild(DeleteNode);

                SubNode.AppendChild(CreateNode(doc, sELements, sVal));

                doc.Save(sXml);
            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return -1;
            }


            return 0;
        }

        private XmlNode CreateNode(XmlDocument doc, string p, string sVal)
        {
            XmlNode node = doc.CreateElement(string.Empty, p, string.Empty);
            node.InnerXml = sVal;
            return node;
        }

        //특정 노드값 읽어오기
        public string Get_XMLVal(string sXml, string First, string sELements)
        {
            string sVal = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(sXml);

                // 첫노드를 잡아주고 하위 노드를 선택한다
                XmlNode FristNode = doc.DocumentElement;
                XmlElement SubNode = (XmlElement)FristNode.SelectSingleNode(First);

                sVal = SubNode.SelectSingleNode(sELements).InnerText;

            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return "-1";
            }


            return sVal;
        }

        /// <summary>
        /// 취소 화면에서 전표 선택시 영수증 상세 사항 표시 할때 사용
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="o_List"></param>
        /// <param name="o_Pay"></param>
        /// <returns></returns>
        public int Set_ResultPage(string sFile,ListView o_List, clsPay o_Pay )
        {
            try
            {
                //html
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
                oStrB.AppendLine(" .txt_l {text-align:left !important;} ");
                oStrB.AppendLine(" .pl10 {padding-left: 10px !important;} ");
                oStrB.AppendLine(" /*]]>*/ ");
                oStrB.AppendLine("  </style>  ");
                oStrB.AppendLine("  </head> ");
                oStrB.AppendLine("  <body> ");

                if (o_Pay != null)
                {

                    string sPayKind = string.Empty;
                    string sPayCardNO = string.Empty;
                    // PAYKIND 추가 시
                    switch (Int32.Parse(o_Pay.sPaykind) )
                    {
                        case (0):
                            sPayKind = "신용카드";
                            sPayCardNO = o_Pay.sPaycartno;
                            break;
                        case (1):
                            sPayKind = "현금영수증";
                            sPayCardNO = o_Pay.sPaycashno;
                            break;

                        case (2): //현금 영수증 없이
                            sPayKind = "현 금";
                            break;
                        case (3): //쿠폰
                            sPayKind = "쿠 폰";
                            break;
                    }


                    oStrB.AppendLine(" <table class='orderView' summary='결제내역입니다.'> ");
                    oStrB.AppendLine("<caption><strong>결제내역</strong></caption>     ");
                    oStrB.AppendLine("<colgroup> ");
                    oStrB.AppendLine("<col style='width:200px'/> ");
                    oStrB.AppendLine("<col style='width:200px'/> ");
                    oStrB.AppendLine("<col style='width:200px;'/> ");
                    oStrB.AppendLine("<col style='width:200px;'/> ");
                    oStrB.AppendLine("</colgroup>               <tbody>              <tr> ");

                    oStrB.AppendLine("<tr><td>영 수 증</td> <td  align='center'><strong> " + "".PadRight(2) + o_Pay.sPayno + "</strong></td>");

                    oStrB.AppendLine("<td>결제방법</td> <td>" + sPayKind + "</td> </tr>");

                    if (o_Pay.sPaykind == "0" || o_Pay.sPaykind == "1")
                    {
                        oStrB.AppendLine("<tr><td>카 드 사</td> <td>" + o_Pay.sPaycardsa + "</td> <td>카드번호</td> <td>" + sPayCardNO + "</td></tr>");
                        oStrB.AppendLine("<tr><td>승인번호</td> <td colspan='3' align='left'>" + o_Pay.sPayauth + "</td></tr>");
                    }

                    oStrB.AppendLine("<tr><td>승인일자</td> <td colspan='3' align='left'>" + o_Pay.sPaydate + " " + o_Pay.sPaytime + "  </td></tr>");
                    
                    oStrB.AppendLine(" </tbody> ");
                    oStrB.AppendLine(" </table> ");

                    oStrB.AppendLine(" </br> ");
                    oStrB.AppendLine(" </br> ");


                    //colspan='3'

                }

                oStrB.AppendLine("  <table class='orderView' summary='주문내역입니다.'> ");
                oStrB.AppendLine("   <caption><strong>주문내역</strong></caption> ");
                oStrB.AppendLine("    <colgroup> ");
                oStrB.AppendLine("     <col style='width:100px'/> ");
                oStrB.AppendLine("     <col /> ");
                oStrB.AppendLine("     <col style='width:100px'/>                   ");
                oStrB.AppendLine("     <col style='width:200px;'/> ");
                oStrB.AppendLine("     <col style='width:200px;'/> ");
                oStrB.AppendLine("   </colgroup> ");
                oStrB.AppendLine("    <thead> ");
                oStrB.AppendLine("     <tr> ");
                oStrB.AppendLine(" 	 <th>번호</th> ");
                oStrB.AppendLine(" 	 <th>주문제품</th> ");
                oStrB.AppendLine(" 	 <th>수량</th> ");
                oStrB.AppendLine(" 	 <th>단가</th> ");
                oStrB.AppendLine(" 	 <th>총금액</th> ");
                oStrB.AppendLine(" 	</tr> ");
                oStrB.AppendLine("    </thead> ");
                oStrB.AppendLine("    <tbody> ");


                int i = 1;
                foreach (ListViewItem item in o_List.Items)
                {
                    oStrB.AppendLine(" 	<tr> ");
                    oStrB.AppendLine(" 	<td>" + i + "</td> ");
                    oStrB.AppendLine(" 	<td class='txt_l pl10'>" + item.SubItems[1].Text + "</td> ");
                    oStrB.AppendLine(" 	<td>" + item.SubItems[2].Text + "</td> ");
                    oStrB.AppendLine(" 	<td align='right'>" + item.SubItems[3].Text + "</td> ");
                    oStrB.AppendLine(" 	<td align='right'><strong>" + item.SubItems[4].Text + "</strong></td> ");
                    oStrB.AppendLine(" 	</tr> ");
                    i++;
                }
                oStrB.AppendLine("  </tbody> ");

                if (o_Pay != null)
                {
                    oStrB.AppendLine(" <tfoot> ");
                    oStrB.AppendLine("<tr> ");
                    oStrB.AppendLine("<th colspan='4'> ");
                    oStrB.AppendLine("취소 금액 ");
                    oStrB.AppendLine("</th> ");
                    oStrB.AppendLine("<td style='text-align:right'>" + string.Format("{0:#,##0}", o_Pay.dPaytotal) + "</td> ");
                    oStrB.AppendLine("</tr> ");
                    oStrB.AppendLine("</tfoot> ");
                }

                oStrB.AppendLine("   </table> ");


              

               
                oStrB.AppendLine("  </body> ");
                oStrB.AppendLine(" </html> ");

                if (File.Exists(sFile))
                    File.Delete(sFile);

                StreamWriter swWrite = new StreamWriter(sFile, true, Encoding.GetEncoding("euc-kr"));
                
#if DEBUG
                Console.WriteLine(swWrite.Encoding.ToString());
#endif
                swWrite.WriteLine(oStrB);
                swWrite.Flush();
                swWrite.Close();
                //browser.BackColor = Color.Black;
            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return -1;
 
            }
            return 0;
        }


        //listview 에서 총액 가져온다.
        public int Check_SUM(ListView lst_)
        {
            try
            {
                //double dReturn = 0;
                int iItemCount = 0;
                int iSumcnt = 0;
                double dTotAmt = 0;

                dTotAmt = 0;

                foreach (ListViewItem item in lst_.Items)
                {
                    dTotAmt = dTotAmt + double.Parse(item.SubItems[4].Text);
                    iSumcnt = iSumcnt + Int32.Parse(item.SubItems[2].Text);
                    iItemCount++;
                }

                //lbl_total.Text = string.Format("{0:#,##0}", dTotAmt);
                //lbl_items.Text = iItemCount.ToString();  ///+ "총 주문 수량 : "  + iSumcnt.ToString();
                //lbl_orderitems.Text = iSumcnt.ToString();
                return Int32.Parse(dTotAmt.ToString());
            }
            catch (Exception ex)
            {
                WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                return 0;
            }

        }
        
    }
}

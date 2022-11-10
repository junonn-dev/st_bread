using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsPrint : IDisposable
    {
        private bool disposed = false;

        private SerialPort oPort = null;
        private string sComPort = string.Empty;
        private Parity oParity = Parity.None;
        private int iBit = 8;
        private StopBits oStopBits = StopBits.One;
        private bool bRePrint = false;
        private bool bIsForceCancel = false; //강제 취소 여부 강제 취소 라면 true 이고 결제 취소 안내 메세지 표시함


        private int iCharCount = 48;


        #region Byte 모음
        /// <summary>NUL</summary>
        static string NUL = Convert.ToString((char)0);
        /// <summary>SOH</summary>
        static string SOH = Convert.ToString((char)1);
        /// <summary>STX</summary>
        static string STX = Convert.ToString((char)2);
        /// <summary>ETX</summary>
        static string ETX = Convert.ToString((char)3);
        /// <summary>EOT</summary>
        static string EOT = Convert.ToString((char)4);
        /// <summary>ENQ</summary>
        static string ENQ = Convert.ToString((char)5);
        /// <summary>ACK</summary>
        static string ACK = Convert.ToString((char)6);
        /// <summary>BEL</summary>
        static string BEL = Convert.ToString((char)7);
        /// <summary>BS</summary>
        static string BS = Convert.ToString((char)8);
        /// <summary>TAB</summary>
        static string TAB = Convert.ToString((char)9);
        /// <summary>VT</summary>
        static string VT = Convert.ToString((char)11);
        /// <summary>FF</summary>
        static string FF = Convert.ToString((char)12);
        /// <summary>CR</summary>
        static string CR = Convert.ToString((char)13);
        /// <summary>SO</summary>
        static string SO = Convert.ToString((char)14);
        /// <summary>SI</summary>
        static string SI = Convert.ToString((char)15);
        /// <summary>DLE</summary>
        static string DLE = Convert.ToString((char)16);
        /// <summary>DC1</summary>
        static string DC1 = Convert.ToString((char)17);
        /// <summary>DC2</summary>
        static string DC2 = Convert.ToString((char)18);
        /// <summary>DC3</summary>
        static string DC3 = Convert.ToString((char)19);
        /// <summary>DC4</summary>
        static string DC4 = Convert.ToString((char)20);
        /// <summary>NAK</summary>
        static string NAK = Convert.ToString((char)21);
        /// <summary>SYN</summary>
        static string SYN = Convert.ToString((char)22);
        /// <summary>ETB</summary>
        static string ETB = Convert.ToString((char)23);
        /// <summary>CAN</summary>
        static string CAN = Convert.ToString((char)24);
        /// <summary>EM</summary>
        static string EM = Convert.ToString((char)25);
        /// <summary>SUB</summary>
        static string SUB = Convert.ToString((char)26);
        /// <summary>ESC</summary>
        static string ESC = Convert.ToString((char)27);
        /// <summary>FS</summary>
        static string FS = Convert.ToString((char)28);
        /// <summary>GS</summary>
        static string GS = Convert.ToString((char)29);
        /// <summary>RS</summary>
        static string RS = Convert.ToString((char)30);
        /// <summary>US</summary>
        static string US = Convert.ToString((char)31);
        /// <summary>Space</summary>
        static string Space = Convert.ToString((char)32);
        #endregion

        #region 기능 커맨드 모음
        /// <summary> 프린터 초기화</summary>
        private string InitializePrinter = ESC + "@";

        /// <summary>ASCII LF</summary>
        private string NewLine = Convert.ToString((char)10);

        /// <summary>
        /// 라인피드
        /// </summary>
        /// <param name="val">라인피드시킬 줄 수</param>
        /// <returns>변환된 문자열</returns>
        private string LineFeed(int val)
        {
            return ESC + "d" + DecimalToCharString(val);
        }
        /// <summary>볼드 On</summary>
        private string BoldOn = ESC + "E" + DecimalToCharString(1);

        /// <summary>볼드 Off</summary>
        private string BoldOff = ESC + "E" + DecimalToCharString(0);

        /// <summary>언더라인 On</summary>
        private string UnderlineOn = ESC + "-" + DecimalToCharString(1);

        /// <summary>언더라인 Off</summary>
        private string UnderlineOff = ESC + "-" + DecimalToCharString(0);

        /// <summary>흑백반전 On</summary>
        private string ReverseOn = GS + "B" + DecimalToCharString(1);

        /// <summary>흑백반전 Off</summary>
        private string ReverseOff = GS + "B" + DecimalToCharString(0);

        /// <summary>좌측정렬</summary>
        private string AlignLeft = ESC + "a" + DecimalToCharString(0);

        /// <summary>가운데정렬</summary>
        private string AlignCenter = ESC + "a" + DecimalToCharString(1);

        /// <summary>우측정렬</summary>
        private string AlignRight = ESC + "a" + DecimalToCharString(2);

        /// <summary>커트</summary>
        private string Cut = GS + "V" + DecimalToCharString(1);

        /// <summary>돈통 열기</summary>
        private string OPEN = ESC + DecimalToCharString(112) + DecimalToCharString(0);

        /// <summary> 바코드 높이 1<= n <= 255 </summary>
        private string SetBarcodeHeight = GS + "h" + DecimalToCharString(70);
        /// <summary> 바코드 너비 2<= n <= 6 </summary>
        private string SetBarcodeWidth = GS + "w" + DecimalToCharString(3);
        /// <summary> code93 바코드 출력 </summary>
        private string BarCodeStart = GS + "k" + DecimalToCharString(72) + DecimalToCharString(14);  //code93 
        /// <summary> ean13바코드 출력 </summary>
        private string BarCodeStart_EAN13 = GS + "k" + DecimalToCharString(67) + DecimalToCharString(13);  //ean13
        /// <summary> code128 바코드 출력 </summary>
        private string BarCodeStart_CODE128 = GS + "k" + DecimalToCharString(73) + DecimalToCharString(14);  //code128 

        /// <summary> 프린트에 저장된 이미지 출력 </summary>
        private string PrintNVImage = FS + "p" + DecimalToCharString(1) + DecimalToCharString(0);  //code128 
        #endregion 기능 커맨드 모음 끝

        /// <summary>
        /// Decimal을 캐릭터 변환 후 스트링을 반환 합니다.
        /// </summary>
        /// <param name="val">커맨드 숫자</param>
        /// <returns>변환된 문자열</returns>
        private static string DecimalToCharString(decimal val)
        {
            string result = "";

            try
            {
                result = Convert.ToString((char)val);
            }
            catch { }

            return result;
        }

        private string LineSeperate()
        {
            string sReturn = string.Empty;

            if (iCharCount  == 48)
            {
                sReturn = "------------------------------------------------";
            }
            else
            {
                sReturn = "------------------------------------------";
            }

            return sReturn;
        }

        /// <summary>
        /// 폰트 크기 결정
        /// </summary>
        /// <param name="width"> 1 ~ 10</param>
        /// <param name="height">1 ~ 10</param>
        /// <returns></returns>
        private string ConvertFontSize(int width, int height)
        {
            string result = "0";
            int _w, _h;

            //가로변환
            if (width == 1)
                _w = 0;
            else if (width == 2)
                _w = 16;
            else if (width == 3)
                _w = 32;
            else if (width == 4)
                _w = 48;
            else if (width == 5)
                _w = 64;
            else if (width == 6)
                _w = 80;
            else if (width == 7)
                _w = 96;
            else if (width == 8)
                _w = 112;
            else _w = 0;

            //세로변환
            if (height == 1)
                _h = 0;
            else if (height == 2)
                _h = 1;
            else if (height == 3)
                _h = 2;
            else if (height == 4)
                _h = 3;
            else if (height == 5)
                _h = 4;
            else if (height == 6)
                _h = 5;
            else if (height == 7)
                _h = 6;
            else if (height == 8)
                _h = 7;
            else _h = 0;

            //가로x세로
            int sum = _w + _h;
            result = GS + "!" + DecimalToCharString(sum);

            return result;
        }

        /// <summary>
        /// 바코드 출력
        /// </summary>
        /// <param name="barcode">바코드</param>
        /// <returns>바코드 포함 명령어</returns>
        private string BarcodeString(string barcode)
        {
            string s = SetBarcodeHeight;
            s += SetBarcodeWidth;
            s += string.Format("{0}{1}", BarCodeStart_EAN13, barcode);
            return s;
        }

        public clsPrint()
        {
            try
            {
                oPort = new SerialPort();
                oPort.PortName = clsPos.port;
                oPort.BaudRate = clsSetting.Let_Int( clsPos.speed);
                oPort.Parity = oParity;
                oPort.DataBits = iBit;
                oPort.StopBits = oStopBits;
                oPort.Encoding = Encoding.Default;
                oPort.Open();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }
        }




        ~clsPrint()
        {
            if (oPort.IsOpen)
                oPort.Close();

            Dispose(false);
        }

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
                }
                disposed = true;
            }
        }

        /// <summary>
        /// 상품 판매 후 결제 영수증 출력
        /// </summary>
        /// <param name="oBill">영수증 정보</param>
        /// <param name="oLstItems">상품 정보</param>
        /// <param name="oLstPay">결제 정보</param>
        /// <returns></returns>
        public int Print_Bill(clsBill oBill)
        {
            int iReturn = 0;
            if (!oPort.IsOpen)
            {
                iReturn = -1;
                return iReturn;
            }

            try
            {
                oPort.Write(OPEN);// 돈통 열기

                Print_Mart(oBill);

                Print_Upper();

                Print_ItemList(oBill.oLstBill_Items );
                Print_Summary(oBill);

                Print_Payments(oBill);


                
                oPort.Write(AlignCenter);
                oPort.Write(BarcodeString(oBill.bill_Code));
                oPort.WriteLine(oBill.bill_Code);
                oPort.WriteLine("==========================================");


                oPort.Write(AlignLeft);
                Print_BottomInfo();
                oPort.WriteLine(NewLine);
                oPort.WriteLine(NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                iReturn = -1;

            }
            finally
            {
                oPort.Write(InitializePrinter);
                oPort.Write(Cut);

                if (oPort.IsOpen)
                    oPort.Close();
            }

            return iReturn;

        }

        /// <summary>
        /// 영수증 재발행 시만 들어오는곳 위와 차이점은 rePrint 위 코드 수정시 같이 수정 하던 없애던 나중에 결정 할것
        /// </summary>
        /// <param name="oBill"></param>
        /// <param name="oLstItems"></param>
        /// <param name="oLstPay"></param>
        /// <param name="rePrint"></param>
        /// <returns></returns>
        public int Print_Bill(clsBill oBill, bool rePrint)
        {

#if DEBUG
            foreach (clsBill_Items oItem in oBill.oLstBill_Items )
            {
                string sItemName = string.Empty;
                //저울 상품은 상품명 뒤에 사이즈 표시 한다.

                if (oItem.item_vat == clsEnum.Item_vat.item_duty_free)
                    sItemName = "* " + oItem.item_name;
                else
                    sItemName = oItem.item_name;

                Console.WriteLine (string.Format("{0,-30}", sItemName));

                Console.WriteLine 
                (
                    String.Format("{0,-14}{1,12}{2,6}{3,16}",
                                    oItem.item_code,
                                    String.Format("{0:#,##0}", oItem.item_cost),
                                    String.Format("{0:#,##0}", oItem.item_qty),
                                    String.Format("{0:#,##0}", oItem.dBill_Amt)
                                    )
                );
            }

            Debug_Print_Payments(oBill);

#endif


            int iReturn = 0;
            if (!oPort.IsOpen)
            {
                iReturn = -1;
                return iReturn;
            }

            //영수증 재발행 여부 영수증 문구에 재발행 영수증이란것만 쓰는 기능을 함
            if (rePrint)
                bRePrint = true;

            Print_Bill(oBill);
            return 0;
        }





        private void Debug_Print_Payments(clsBill oBill)
        {

            int iForm = 0;
            if (iCharCount == 48)
                iForm = 32;
            else
                iForm = 26;

            double dCash = 0;
            double dCashPay = 0;
            double dRest = 0;

            //현금만 따로 모아서 거스름돈 표시
            foreach (clsbill_payments oPay in oBill.oLstBill_payments)
            {
                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cash || oPay.bill_paymentskind == clsEnum.Payment_Kind.cashwithaut)
                {
                    dCash += oPay.bill_recvamt;
                    dCashPay += oPay.bill_paymentamt;
                    dRest += oPay.bill_restAmt;
                }
            }

            if (dCash > 0)
            {
                Console.WriteLine("현금");

                Console.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "받은금액 : ", String.Format("{0:#,##0}", dCash)));
                Console.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "거스름돈 : ", String.Format("{0:#,##0}", dRest)));
            }
            Console.WriteLine(LineSeperate());




            //결제방법별 결제 결과 표시
            foreach (clsbill_payments oPay in oBill.oLstBill_payments)
            {
                string sKind = string.Empty;
                switch (oPay.bill_paymentskind)
                {
                    case clsEnum.Payment_Kind.cash:
                        sKind = "현금";
                        break;
                    case clsEnum.Payment_Kind.cashwithaut:
                        sKind = "현금영수증";
                        break;
                    case clsEnum.Payment_Kind.card:
                        sKind = "신용카드";
                        break;
                }


                Console.WriteLine(sKind);

                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cash || oPay.bill_paymentskind == clsEnum.Payment_Kind.cashwithaut)
                {
                    //현금 과 현금영수증일때
                    if (oPay.bill_recvamt > 0)
                    {
                        string sRecv = string.Empty;
                        string sRest = string.Empty;

                        if (oPay.bill_iscancel == clsEnum.bill_isCancel.cancel)
                        {
                            sRecv = String.Format("{0,-12}{1," + iForm + "}", "반품금액 : ", String.Format("{0:#,##0}", oPay.bill_recvamt));
                            sRest = "";
                        }
                        else
                        {
                            sRecv = String.Format("{0,-12}{1," + iForm + "}", "받은금액 : ", String.Format("{0:#,##0}", oPay.bill_recvamt));
                            //sRest = String.Format("{0,-12}{1," + iForm + "}", "거스름돈 : ", String.Format("{0:#,##0}", (oPay.bill_recvamt - oPay.bill_paymentamt)));
                        }

                        Console.WriteLine(sRecv);
                        //oPort.WriteLine(sRest);
                    }
                }

                //카드 표시
                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.card)
                {
                    string sCard = string.Empty;
                    string sAuth = string.Empty;
                    string sDate = string.Empty;
                    string sAmt = string.Empty;

                    Console.WriteLine(String.Format("{0,-8}{1}", "카드  종류:", oPay.bill_cardcmpny));
                    Console.WriteLine(String.Format("{0,-7}{1}", "전표매입사:", oPay.bill_buycmpny));
                    if (oPay.bill_halbu.Length > 0 && oPay.bill_halbu != "00")
                    {
                        Console.WriteLine(String.Format("{0,-8} {1} 개월", "할      부:", oPay.bill_halbu));
                    }
                    sCard = String.Format("{0,-8}{1}", "카드  번호:", oPay.bill_cardnum);
                    sAuth = String.Format("{0,-8}{1}", "승인  번호:", oPay.bill_authnum);
                    sDate = String.Format("{0,-8}{1}", "승인  일시:", clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyyy-MM-dd HH:mm:ss"));
                    sAmt = String.Format("{0,-12}{1," + iForm + "}", "승인  금액:", String.Format("{0:#,##0}", oPay.bill_paymentamt));

                    Console.WriteLine(sCard);
                    Console.WriteLine(sAuth);
                    Console.WriteLine(sDate);

                    
                    Console.WriteLine(sAmt);
                }

                //현금영수증 표시
                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cashwithaut)
                {
                    string sCard = string.Empty;
                    string sAuth = string.Empty;
                    string sDate = string.Empty;
                    string sAmt = string.Empty;

                    sCard = String.Format("{0,-8}{1}", "회원번호:", oPay.bill_cardnum);
                    sAuth = String.Format("{0,-8}{1}", "처리번호:", oPay.bill_authnum);
                    sDate = String.Format("{0,-8}{1}", "거래일시:", clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyyy-MM-dd HH:mm:ss"));
                    sAmt = String.Format("{0,-12}{1," + iForm + "}", "승인금액:", String.Format("{0:#,##0}", oPay.bill_paymentamt));

                    Console.WriteLine(sCard);
                    Console.WriteLine(sAuth);
                    Console.WriteLine(sDate);

                    Console.WriteLine(sAmt);


                }


                Console.WriteLine(LineSeperate());
            }

        }


















        

        /// <summary>
        /// 매장 상호 이미지 등 출력 
        /// </summary>
        private void Print_Mart(clsBill oBill)
        {

            int iForm = 19;
            if (iCharCount == 48)
                iForm = 19;
            else
                iForm = 13;
            

            oPort.Write(AlignCenter);
            oPort.Write(ConvertFontSize(2, 2));
            oPort.Write(BoldOn);
            oPort.WriteLine(clsPos.store_name );
            oPort.Write(BoldOff);

            oPort.Write(AlignLeft);
            oPort.Write(ConvertFontSize(1, 1));

            oPort.WriteLine(String.Format("사업자번호 : {0}", clsPos.store_number ));
            oPort.WriteLine(String.Format("전화  번호 : {0}", clsPos.store_tel ));

            oPort.WriteLine("주소");
            oPort.WriteLine(String.Format("{0} {1}", clsPos.store_address1 ,clsPos.store_address2 ));
            //oPort.WriteLine(String.Format("{0}", "용봉동 1414-2"));

            oPort.Write(NewLine);
            //if (clsSetting.oLst_PrintInfo != null)
            //{
            //    foreach (clsPrintInfo oPrintInfo in clsSetting.oLst_PrintInfo)
            //    {
            //        if (oPrintInfo.info_Kind == "0") //상단 메세지
            //            oPort.WriteLine(String.Format("{0}", oPrintInfo.info_Message));
            //    }
            //}

            oPort.Write(AlignCenter);




            if (bRePrint)
                oPort.WriteLine("[재발행 전표]");
            if (oBill.bill_iscancel == clsEnum.bill_isCancel.cancel)
            {
                oPort.WriteLine("[취소 전표]");
            }
            else
            {
                oPort.WriteLine("[정상 매출 전표]");
            }

            oPort.Write(AlignLeft);

            oPort.WriteLine(String.Format("영수증 : {0} ", oBill.bill_date ));
            //oPort.WriteLine(String.Format("계산원 : {0}", oBill.bill_user + " " + oBill.bill_user_nm));
            oPort.WriteLine(String.Format("[등록] {0,-15}   {1," + iForm + "}", clsDateTime.Get_Time(oBill.bill_date).ToString("yyyy-MM-dd HH:mm:ss"), "POS : " + oBill.bill_pos));
        }




        /// <summary>
        /// 상품 리스트 표시
        /// </summary>
        /// <param name="oLstItems"></param>
        private void Print_ItemList(List<clsBill_Items > oLstItems)
        {
            int[] iForm;
            if (iCharCount == 48)
                iForm = new int[] { 6, 7, 11, 12, 15, 16, 35 };
            else
                iForm = new int[] { 4, 5, 9, 10, 13, 14, 29 };


            oPort.Write(AlignLeft);
            oPort.WriteLine(LineSeperate());


            oPort.WriteLine
                    (
                        String.Format("{0,-6}{1," + iForm[4] + "}{2," + iForm[1] + "}{3," + iForm[2] + "}",
                                                "상 품 명",
                                                "단 가",
                                                "수 량",
                                                "금 액"
                                                )
                    );

            oPort.WriteLine(LineSeperate());

            foreach (clsBill_Items oItem in oLstItems)
            {
                string sItemName = string.Empty;
                //저울 상품은 상품명 뒤에 사이즈 표시 한다.

                if (oItem.item_vat == clsEnum.Item_vat.item_duty_free)
                    sItemName = "* " + oItem.item_name;
                else
                    sItemName = oItem.item_name;



                oPort.WriteLine(string.Format("{0,-30}", sItemName));

                oPort.WriteLine
                (
                    String.Format("{0,-14}{1," + iForm[3] + "}{2," + iForm[0] + "}{3," + iForm[5] + "}",
                                    oItem.item_code ,
                                    String.Format("{0:#,##0}", oItem.item_cost ),
                                    String.Format("{0:#,##0}", oItem.item_qty ),
                                    String.Format("{0:#,##0}", oItem.dBill_Amt )
                                    )
                );
            }



        }




        /// <summary>
        /// 총 합계 금액 등을 표시
        /// </summary>
        /// <param name="oBill"></param>
        private void Print_Summary(clsBill oBill)
        {
            int[] iForm;
            if (iCharCount == 48)
                iForm = new int[] { 35, 32, 6, 15, 16, 33 };
            else
                iForm = new int[] { 29, 26, 5, 13, 14, 27 };


            string sSum = string.Empty;
            string sSale = string.Empty;
            string sTotal = string.Empty;
            string sVAT = string.Empty;
            string sVAT2 = string.Empty;
            string sVatAmt = string.Empty;

            sSum = String.Format("{0,-11}{1," + iForm[0] + "}", "합    계 : ", String.Format("{0:#,##0}", (oBill.bill_totalamt )));
            
            sTotal = String.Format("{0,-12}{1," + iForm[1] + "}", "결제금액 : ", String.Format("{0:#,##0}", oBill.bill_totalamt));
            sVAT = String.Format("{0,-" + iForm[2] + "}{1," + iForm[3] + "} {2,-" + iForm[2] + "}{3," + iForm[4] + "}", "면세:", String.Format("{0:#,##0}", oBill.bill_DutyFreeAmt), "과세:", String.Format("{0:#,##0}", oBill.bill_TaxAmt));            
            sVatAmt = String.Format("{0,-12}{1," + iForm[5] + "}", "부 가 세 : ", String.Format("{0:#,##0}", oBill.bill_vatamt));


            oPort.WriteLine(LineSeperate());
            oPort.Write(BoldOn);
            oPort.WriteLine(sSum);
            oPort.WriteLine(sSale);
            oPort.WriteLine(sTotal);
            oPort.Write(BoldOff);


            oPort.WriteLine(LineSeperate());

            oPort.WriteLine(sVAT);
            oPort.WriteLine(sVAT2);
            oPort.WriteLine(sVatAmt);
            oPort.WriteLine(LineSeperate());







        }
        /// <summary>
        /// 총 합계 금액 등을 표시
        /// </summary>
        /// <param name="oBill"></param>
        private void Print_Summary_En(clsBill oBill)
        {


            string sSum = string.Empty;
            string sSale = string.Empty;
            string sTotal = string.Empty;
            string sVAT = string.Empty;
            string sVAT2 = string.Empty;
            string sVatAmt = string.Empty;

            sSum = Let_String("Total :", String.Format("{0:#,##0}", (oBill.bill_totalamt )));
            
            sTotal = Let_String("Pay Amount :", String.Format("{0:#,##0}", oBill.bill_totalamt));

            sVAT = Let_String("Tax Free :", String.Format("{0:#,##0}", oBill.bill_DutyFreeAmt));
            sVAT2 = Let_String("Tax :", String.Format("{0:#,##0}", oBill.bill_TaxAmt));

            sVatAmt = Let_String("VAT :", String.Format("{0:#,##0}", oBill.bill_vatamt)); //String.Format("{0,-12}{1,33}", "V A T : ", String.Format("{0:#,##0}", oBill.bill_vatamt));


            oPort.WriteLine(LineSeperate());


            oPort.Write(BoldOn);
            oPort.WriteLine(sSum);
            oPort.WriteLine(sSale);
            oPort.WriteLine(sTotal);
            oPort.Write(BoldOff);

            oPort.WriteLine(LineSeperate());
            oPort.WriteLine(sVAT);
            oPort.WriteLine(sVAT2);
            oPort.WriteLine(sVatAmt);
            oPort.WriteLine(LineSeperate());
        }

        private void Print_Payments(clsBill oBill)
        {

            int iForm = 0;
            if (iCharCount == 48)
                iForm = 32;
            else
                iForm = 26;

            double dCash = 0;
            double dCashPay = 0;
            double dRest = 0;
            //현금만 따로 모아서 거스름돈 표시
            foreach (clsbill_payments oPay in oBill.oLstBill_payments )
            {
                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cash || oPay.bill_paymentskind == clsEnum.Payment_Kind.cashwithaut)
                {
                    dCash += oPay.bill_recvamt;
                    dCashPay += oPay.bill_paymentamt;

                    dRest += oPay.bill_restAmt;

                }
            }

            if (dCash > 0)
            {
                oPort.WriteLine("현금");

                oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "받은금액 : ", String.Format("{0:#,##0}", dCash)));
                oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "거스름돈 : ", String.Format("{0:#,##0}", dRest)));
            }
            oPort.WriteLine(LineSeperate());




            //결제방법별 결제 결과 표시
            foreach (clsbill_payments oPay in oBill.oLstBill_payments)
            {
                string sKind = string.Empty;
                switch (oPay.bill_paymentskind)
                {
                    case clsEnum.Payment_Kind.cash:
                        sKind = "현금";
                        break;
                    case clsEnum.Payment_Kind.cashwithaut:
                        sKind = "현금영수증";
                        break;
                    case clsEnum.Payment_Kind.card:
                        sKind = "신용카드";
                        break;
                }


                oPort.WriteLine(sKind);

                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cash || oPay.bill_paymentskind == clsEnum.Payment_Kind.cashwithaut)
                {
                    //현금 과 현금영수증일때
                    if (oPay.bill_recvamt > 0)
                    {
                        string sRecv = string.Empty;
                        string sRest = string.Empty;

                        if (oPay.bill_iscancel  == clsEnum.bill_isCancel.cancel)
                        {
                            sRecv = String.Format("{0,-12}{1," + iForm + "}", "반품금액 : ", String.Format("{0:#,##0}", oPay.bill_recvamt));
                            sRest = "";
                        }
                        else
                        {
                            sRecv = String.Format("{0,-12}{1," + iForm + "}", "받은금액 : ", String.Format("{0:#,##0}", oPay.bill_recvamt));
                            //sRest = String.Format("{0,-12}{1," + iForm + "}", "거스름돈 : ", String.Format("{0:#,##0}", (oPay.bill_recvamt - oPay.bill_paymentamt)));
                        }

                        oPort.WriteLine(sRecv);
                        //oPort.WriteLine(sRest);
                    }
                }

                //카드 표시
                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.card)
                {
                    string sCard = string.Empty;
                    string sAuth = string.Empty;
                    string sDate = string.Empty;
                    string sAmt = string.Empty;

                    oPort.WriteLine(String.Format("{0,-8}{1}", "카드  종류:", oPay.bill_cardcmpny));
                    oPort.WriteLine(String.Format("{0,-7}{1}", "전표매입사:", oPay.bill_buycmpny));
                    if (oPay.bill_halbu.Length > 0 && oPay.bill_halbu != "00")
                    {
                        oPort.WriteLine(String.Format("{0,-8} {1} 개월", "할      부:", oPay.bill_halbu));
                    }
                    sCard = String.Format("{0,-8}{1}", "카드  번호:", oPay.bill_cardnum);
                    sAuth = String.Format("{0,-8}{1}", "승인  번호:", oPay.bill_authnum);
                    sDate = String.Format("{0,-8}{1}", "승인  일시:", clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyyy-MM-dd HH:mm:ss"));
                    sAmt = String.Format("{0,-12}{1," + iForm + "}", "승인  금액:", String.Format("{0:#,##0}", oPay.bill_paymentamt));

                    oPort.WriteLine(sCard);
                    oPort.WriteLine(sAuth);
                    oPort.WriteLine(sDate);

                    oPort.Write(BoldOn);
                    oPort.WriteLine(sAmt);
                    oPort.Write(BoldOff);
                }

                //현금영수증 표시
                if (oPay.bill_paymentskind == clsEnum.Payment_Kind.cashwithaut)
                {
                    string sCard = string.Empty;
                    string sAuth = string.Empty;
                    string sDate = string.Empty;
                    string sAmt = string.Empty;

                    sCard = String.Format("{0,-8}{1}", "회원번호:", oPay.bill_cardnum);
                    sAuth = String.Format("{0,-8}{1}", "처리번호:", oPay.bill_authnum);
                    sDate = String.Format("{0,-8}{1}", "거래일시:", clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyyy-MM-dd HH:mm:ss"));
                    sAmt = String.Format("{0,-12}{1," + iForm + "}", "승인금액:", String.Format("{0:#,##0}", oPay.bill_paymentamt));

                    oPort.WriteLine(sCard);
                    oPort.WriteLine(sAuth);
                    oPort.WriteLine(sDate);

                    oPort.Write(BoldOn);
                    oPort.WriteLine(sAmt);
                    oPort.Write(BoldOff);


                    //oPort.WriteLine("전화: 국번없이 126-1-1");
                    //oPort.WriteLine("http://현금영수증.kr");
                }


                oPort.WriteLine(LineSeperate());
            }



            //string sKind = string.Empty;


            //clsEnum.Payment_Kind oPayKine = clsEnum.Payment_Kind.cash;


            //if (oBill.bill_cardkind == "CA") //현금
            //{
            //    sKind = "현 금";
            //    oPayKine = clsEnum.Payment_Kind.cash;

            //}
            //else if (oBill.bill_cardkind == "ACA") //현금영수증
            //{
            //    sKind = oBill.bill_buycmpny;
            //    oPayKine = clsEnum.Payment_Kind.cashwithaut;
            //}
            //else
            //{
            //    sKind = "신용카드";
            //    oPayKine = clsEnum.Payment_Kind.card;
            //}


            //oPort.WriteLine(sKind);

            //if (oPayKine == clsEnum.Payment_Kind.cash || oPayKine == clsEnum.Payment_Kind.cashwithaut)
            //{
            //    string sRecv = string.Empty;
            //    string sRest = string.Empty;

            //    if (oBill.bill_iscancel == clsEnum.bill_isCancel.cancel)
            //    {
            //        sRecv = String.Format("{0,-12}{1," + iForm + "}", "반품금액 : ", String.Format("{0:#,##0}", oBill.bill_paymentamt));
            //        sRest = "";
            //    }
            //    else
            //    {
            //        sRecv = String.Format("{0,-12}{1," + iForm + "}", "받은금액 : ", String.Format("{0:#,##0}", oBill.bill_paymentamt));
            //        //sRest = String.Format("{0,-12}{1," + iForm + "}", "거스름돈 : ", String.Format("{0:#,##0}", (oPay.bill_recvamt - oPay.bill_paymentamt)));
            //    }

            //    oPort.WriteLine(sRecv);
            //}



            ////카드 표시
            //if (oPayKine == clsEnum.Payment_Kind.card)
            //{
            //    string sCard = string.Empty;
            //    string sAuth = string.Empty;
            //    string sDate = string.Empty;
            //    string sAmt = string.Empty;

            //    oPort.WriteLine(String.Format("{0,-8}{1}", "카드  종류:", oBill.bill_cardcmpny));
            //    oPort.WriteLine(String.Format("{0,-7}{1}", "전표매입사:", oBill.bill_buycmpny));
            //    if ( oBill.bill_halbu != "00")
            //    {
            //        oPort.WriteLine(String.Format("{0,-8} {1} 개월", "할      부:", oBill.bill_halbu));
            //    }
            //    sCard = String.Format("{0,-8}{1}", "카드  번호:", oBill.bill_cardnum);
            //    sAuth = String.Format("{0,-8}{1}", "승인  번호:", oBill.bill_authnum);
            //    sDate = String.Format("{0,-8}{1}", "승인  일시:", clsDateTime.Get_Time( oBill.bill_authdatetime).ToString("yyyy-MM-dd HH:mm:ss"));
            //    sAmt = String.Format("{0,-12}{1," + iForm + "}", "승인  금액:", String.Format("{0:#,##0}", oBill.bill_paymentamt));

            //    oPort.WriteLine(sCard);
            //    oPort.WriteLine(sAuth);
            //    oPort.WriteLine(sDate);

            //    oPort.Write(BoldOn);
            //    oPort.WriteLine(sAmt);
            //    oPort.Write(BoldOff);
            //}

            ////현금영수증 표시
            //if (oPayKine == clsEnum.Payment_Kind.cashwithaut)
            //{
            //    string sCard = string.Empty;
            //    string sAuth = string.Empty;
            //    string sDate = string.Empty;
            //    string sAmt = string.Empty;

            //    sCard = String.Format("{0,-8}{1}", "회원번호:", oBill.bill_cardnum);
            //    sAuth = String.Format("{0,-8}{1}", "처리번호:", oBill.bill_authnum);
            //    sDate = String.Format("{0,-8}{1}", "거래일시:", clsDateTime.Get_Time(oBill.bill_authdatetime).ToString("yyyy-MM-dd HH:mm:ss"));
            //    sAmt = String.Format("{0,-12}{1," + iForm + "}", "승인금액:", String.Format("{0:#,##0}", oBill.bill_paymentamt));

            //    oPort.WriteLine(sCard);
            //    oPort.WriteLine(sAuth);
            //    oPort.WriteLine(sDate);

            //    oPort.Write(BoldOn);
            //    oPort.WriteLine(sAmt);
            //    oPort.Write(BoldOff);


            //    oPort.WriteLine("전화: 국번없이 126-1");
            //    oPort.WriteLine("http://현금영수증.kr");
            //}


            //oPort.WriteLine(LineSeperate());
        }


        private void Print_MemberInfo(clsBill o_Bill)
        {
            //int[] iForm;
            //if (iCharCount == 48)
            //    iForm = new int[] { 28, 30, 32 };
            //else
            //    iForm = new int[] { 22, 24, 26 };


            //if (o_Bill.bill_member != clsSetting.Let_String(string.Empty))
            //{
            //    string sCard = string.Empty;
            //    string sPoint = string.Empty;
            //    string sPrePoint = string.Empty;

            //    if (clsPosInfo.oLang == clsEnum.Option_Lang.ko_Kr)
            //    {
            //        sCard = String.Format("{0,-12}{1," + iForm[2] + "}", o_Bill.bill_member, o_Bill.bill_member_name);
            //        sPoint = String.Format("{0,-12}{1," + iForm[0] + "}", "금회적립 포인트 :", String.Format("{0:#,##0}", o_Bill.bill_member_point));
            //        sPrePoint = String.Format("{0,-12}{1," + iForm[1] + "}", "잔  여 포 인 트 :", String.Format("{0:#,##0}", o_Bill.bill_member_prepoint));
            //    }
            //    else
            //    {
            //        sCard = String.Format("{0,-12}{1," + iForm[2] + "}", o_Bill.bill_member, o_Bill.bill_member_name);
            //        sPoint = String.Format("{0,-12}{1," + iForm[0] + "}", "Saved Point :", String.Format("{0:#,##0}", o_Bill.bill_member_point));
            //        sPrePoint = String.Format("{0,-12}{1," + iForm[1] + "}", "Total Point :", String.Format("{0:#,##0}", o_Bill.bill_member_prepoint));
            //    }
            //    oPort.WriteLine(sCard);
            //    oPort.WriteLine(sPoint);
            //    oPort.WriteLine(sPrePoint);


            //}
        }



        private void Print_Cod(clsBill o_Bill)
        {
            //int[] iForm;
            //if (iCharCount == 48)
            //    iForm = new int[] { 20, 30 };
            //else
            //    iForm = new int[] { 26, 36 };

            ////배달 전표 인 경우 회원 주소 기재
            //if (o_Bill.bill_Kind != clsEnum.bill_Kind.normal)
            //{
            //    oPort.Write(BoldOn);
            //    oPort.WriteLine("배달 전표");


            //    oPort.Write(AlignCenter);

            //    string sBillNum = o_Bill.bill_codNum.ToString(); // String.Format("{0,-15}{1," + iForm2[0] + "}", "배달 전표 번호 :", o_Bill.bill_codNum);
            //    oPort.WriteLine(sBillNum);

            //    oPort.Write(AlignLeft);
            //    oPort.Write(BoldOff);

            //    if (o_Bill.bill_member != clsSetting.Let_String(string.Empty))
            //    {
            //        string sQuery = "SELECT * FROM os_member.member " +
            //                      " WHERE member_code = '" + o_Bill.bill_member + "' ";
            //        DataTable dt = clsQuery_Excute.SelectQuery(sQuery, clsEnum.DB_Kind.server);

            //        foreach (DataRow row in dt.Rows)
            //        {
            //            string saddr = String.Format("{0,-6}{1,-" + iForm[1] + "}", "주 소:", row["member_address"].ToString());
            //            oPort.WriteLine(saddr);
            //        }
            //        dt.Dispose();
            //    }
            //    oPort.WriteLine(LineSeperate());
            //}

        }

        private void Print_Upper()
        {
            //if (clsSetting.oLst_PrintInfo != null)
            //{
            //    foreach (clsPrintInfo oPrintInfo in clsSetting.oLst_PrintInfo)
            //    {
            //        if (oPrintInfo.info_Kind == "0") //하단 메세지
            //            oPort.WriteLine(String.Format("{0}", oPrintInfo.info_Message));
            //    }
            //}
        }
        private void Print_BottomInfo()
        {
            oPort.WriteLine(String.Format("{0}", "이용해 주셔서 감사 합니다."));
                


            //if (clsSetting.oLst_PrintInfo != null)
            //{
            //    foreach (clsPrintInfo oPrintInfo in clsSetting.oLst_PrintInfo)
            //    {
                    
            //    }
            //}

            //if (bIsForceCancel)
            //{
            //    oPort.WriteLine("취소 영수증 이지만 결제 취소가");
            //    oPort.WriteLine("정상적이지 않을 수 있습니다.");
            //    oPort.WriteLine("Tel : 031-790-1985 로 문의 주세요.");

            //}

        }

        private void Print_Logo()
        {
            oPort.Write(PrintNVImage);

        }


        private void Print_ImageFromPC(string sFile)
        {
            if (File.Exists(sFile))
            {
                byte[] img = GetImageToByte(sFile, 500); //(val, 500);
                oPort.Write(img, 0, img.Length);
                oPort.Write(InitializePrinter);
            }

        }

        private void Print_Cut()
        {
            oPort.Write(NewLine);
            oPort.Write(InitializePrinter);
            oPort.Write(Cut);
        }


        public void Open_Drawer()
        {
            if (oPort.IsOpen)
            {
                //oPort.Write("");
                oPort.Write(OPEN);// 돈통 열기
                oPort.Write(OPEN);// 돈통 열기
                oPort.Close();
            }
        }

        public int Test_Print()
        {

            try
            {



                clsBill oBill = new clsBill();
                oBill.bill_totalamt = 1550000;
                
                oBill.bill_DutyFreeAmt = 150000;
                oBill.bill_TaxAmt = 1350000;


                for (int i = 0; i < 10; i++)
                {
                    oPort.WriteLine(" ".PadLeft(i ) + "테스트테스트");
                }


                string sRecv = String.Format("{0,-12}{1," + 10 + "}", "테스트금액 : ", String.Format("{0:#,##0}", oBill.bill_paymentamt));
                oPort.WriteLine(sRecv );


                //oPort.Write(PrintNVImage);




                byte[] obyte2 = Encoding.UTF8.GetBytes("가나다라마바사아" + " ".PadLeft(19) + "100,000");
                oPort.WriteLine("가나다라마바사아" + " ".PadLeft(19) + "100,0000");

                byte[] obyte = Encoding.UTF8.GetBytes("가나다라마바사아자차카파타하가나다라마바사");
                oPort.WriteLine("가나다라마바사아자차카파타하가나다라마바사");

                oPort.Write(NewLine);
                oPort.Write(NewLine);
                oPort.Write(NewLine);
                oPort.Write(NewLine);

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
            }
            finally
            {

                oPort.Write(InitializePrinter);
                oPort.Write(Cut);

                if (oPort.IsOpen)
                    oPort.Close();


            }

            return 0;

        }


        private string Let_String(string a, string b)
        {
            string str = string.Empty;

            int i = a.Length;
            int y = b.Length;

            int sum = i + y;
            int spare = 42 - sum;

            str = string.Format("{0,-" + i.ToString() + "}" + string.Empty.PadLeft(spare) + "{1," + y.ToString() + "}", a, b);


            return str;

        }

        private string Let_StringHN(string a, string b)
        {
            //한줄 64byte

            byte[] obyte = Encoding.UTF8.GetBytes(a);
            byte[] obyte2 = Encoding.UTF8.GetBytes(b);

            string str = string.Empty;

            int i = obyte.Length;
            int y = obyte2.Length;

            int sum = i + y;
            int spare = 64 - sum;

            str = string.Format("{0,-" + i.ToString() + "}" + string.Empty.PadLeft(spare) + "{1," + y.ToString() + "}", a, b);


            return str;

        }



        /// <summary>
        /// 이미지를 바이트 배열로 반환 합니다.
        /// </summary>
        /// <param name="LogoPath">이미지 경로</param>
        /// <param name="printWidth">이미지 출력 가로길이</param>
        /// <returns></returns>
        public byte[] GetImageToByte(string LogoPath, int printWidth)
        {
            List<byte> byteList = new List<byte>();
            if (!File.Exists(LogoPath))
                return null;
            BitmapData data = GetBitmapData(LogoPath, printWidth);
            BitArray dots = data.Dots;
            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            //byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            //byteList.Add(Convert.ToByte('@'));
            byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            byteList.Add(Convert.ToByte('3'));
            byteList.Add((byte)24);
            while (offset < data.Height)
            {
                byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
                byteList.Add(Convert.ToByte('*'));
                byteList.Add((byte)33);
                byteList.Add(width[0]);
                byteList.Add(width[1]);

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            int i = (y * data.Width) + x;

                            bool v = false;
                            if (i < dots.Length)
                                v = dots[i];

                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }
                        byteList.Add(slice);
                    }
                }
                offset += 24;
                byteList.Add(Convert.ToByte(0x0A));
            }
            byteList.Add(Convert.ToByte(0x1B));
            byteList.Add(Convert.ToByte('3'));
            byteList.Add((byte)30);
            return byteList.ToArray();
        }

        private BitmapData GetBitmapData(string bmpFileName, int width)
        {
            using (var bitmap = (Bitmap)Bitmap.FromFile(bmpFileName))
            {
                var threshold = 127;
                var index = 0;
                double multiplier = width; // 이미지 width조정
                double scale = (double)(multiplier / (double)bitmap.Width);
                int xheight = (int)(bitmap.Height * scale);
                int xwidth = (int)(bitmap.Width * scale);
                var dimensions = xwidth * xheight;
                var dots = new BitArray(dimensions);

                for (var y = 0; y < xheight; y++)
                {
                    for (var x = 0; x < xwidth; x++)
                    {
                        var _x = (int)(x / scale);
                        var _y = (int)(y / scale);
                        var color = bitmap.GetPixel(_x, _y);
                        var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                        dots[index] = (luminance < threshold);
                        index++;
                    }
                }

                return new BitmapData()
                {
                    Dots = dots,
                    Height = (int)(bitmap.Height * scale),
                    Width = (int)(bitmap.Width * scale)
                };
            }
        }

        private class BitmapData
        {
            public BitArray Dots
            {
                get;
                set;
            }

            public int Height
            {
                get;
                set;
            }

            public int Width
            {
                get;
                set;
            }
        }


        public void Print_Close()
        {
            int iForm = 0;
            if (iCharCount == 48)
                iForm = 32;
            else
                iForm = 26;

            
            oPort.Write(InitializePrinter);


            oPort.WriteLine(String.Format("[오픈] {0,-15}", clsDateTime.Get_Time(clsPosOpen.open_date ).ToString("yyyy-MM-dd HH:mm:ss")));
            oPort.WriteLine(String.Format("[마감] {0,-15}", clsDateTime.Get_Time(clsPosOpen.close_date ).ToString("yyyy-MM-dd HH:mm:ss")));

            oPort.Write(AlignLeft);
            oPort.WriteLine(LineSeperate());

            #region 시재금
            oPort.WriteLine(string.Format("{0,-30}", "시작금액"));
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오만원권",
                                clsPosOpen.FOThou,
                                String.Format("{0:#,##0}", clsPosOpen.FOThou * 50000)
                                )
            );

            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일만원권",
                                clsPosOpen.OOThou,
                                String.Format("{0:#,##0}", clsPosOpen.OOThou * 10000)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오천원권",
                                clsPosOpen.FThou,
                                String.Format("{0:#,##0}", clsPosOpen.FThou * 5000)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일천원권",
                                clsPosOpen.OThou,
                                String.Format("{0:#,##0}", clsPosOpen.OThou  * 1000)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오백원권",
                                clsPosOpen.FHund,
                                String.Format("{0:#,##0}", clsPosOpen.FHund  * 500)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일백원권",
                                clsPosOpen.OHund,
                                String.Format("{0:#,##0}", clsPosOpen.OHund  * 100)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오십원권",
                                clsPosOpen.FO,
                                String.Format("{0:#,##0}", clsPosOpen.FO  * 50)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일십원권",
                                clsPosOpen.IO,
                                String.Format("{0:#,##0}", clsPosOpen.IO  * 10)
                                )
            );

            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "시재금합 : ", String.Format("{0:#,##0}", clsPosOpen.dtotal )));
            #endregion

            oPort.WriteLine(LineSeperate());
            //매출 
            oPort.WriteLine(string.Format("{0,-30}", "매출"));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "객    수 : ", String.Format("{0:#,##0}", clsPosOpen.sale_customs )));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "현금매출 : ", String.Format("{0:#,##0}", clsPosOpen.sale_Cashamount )));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "카드매출 : ", String.Format("{0:#,##0}", clsPosOpen.sale_Cardamount )));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "전체매출 : ", String.Format("{0:#,##0}", clsPosOpen.sale_amount )));

            oPort.WriteLine(LineSeperate());
            
            oPort.WriteLine(string.Format("{0,-30}", "현금입출금"));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "현금입금 : ", String.Format("{0:#,##0}", clsPosOpen.cash_io_in)));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "현금출금 : ", String.Format("{0:#,##0}", clsPosOpen.cash_io_out)));
            
            oPort.WriteLine(LineSeperate());


            double dTemp = (clsPosOpen.dtotal + clsPosOpen.sale_Cashamount) + clsPosOpen.cash_io_in - clsPosOpen.cash_io_out;

            #region 마감금액
            oPort.WriteLine(string.Format("{0,-30}", "마감금액"));
            oPort.WriteLine
            (
                String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오만원권",
                                clsPosOpen.CFOThou,                                
                                String.Format("{0:#,##0}", clsPosOpen.CFOThou * 50000)
                                )
            );

            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일만원권",
                                clsPosOpen.COOThou,
                                String.Format("{0:#,##0}", clsPosOpen.COOThou * 10000)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오천원권",
                                clsPosOpen.CFThou,
                                String.Format("{0:#,##0}", clsPosOpen.CFThou * 5000)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일천원권",
                                clsPosOpen.COThou,
                                String.Format("{0:#,##0}", clsPosOpen.COThou * 1000)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오백원권",
                                clsPosOpen.CFHund,
                                String.Format("{0:#,##0}", clsPosOpen.CFHund * 500)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일백원권",
                                clsPosOpen.COHund,
                                String.Format("{0:#,##0}", clsPosOpen.COHund * 100)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "오십원권",
                                clsPosOpen.CFO,
                                String.Format("{0:#,##0}", clsPosOpen.CFO * 50)
                                )
            );
            oPort.WriteLine
            (
                 String.Format("{0,-11}{1}{2," + iForm + "}",
                                "일십원권",
                                clsPosOpen.CIO,
                                String.Format("{0:#,##0}", clsPosOpen.CIO * 10)
                                )
            );

            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "마감금합 : ", String.Format("{0:#,##0}", clsPosOpen.Cdtotal)));
            #endregion


            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "현금소계 : ", String.Format("{0:#,##0}", dTemp)));
            oPort.WriteLine(String.Format("{0,-12}{1," + iForm + "}", "현금체크 : ", String.Format("{0:#,##0}", dTemp - clsPosOpen.Cdtotal)));

            oPort.WriteLine(LineSeperate());

            oPort.WriteLine(NewLine);
            oPort.WriteLine(NewLine);

            oPort.Write(InitializePrinter);
            oPort.Write(Cut);

            if (oPort.IsOpen)
                oPort.Close();
 
        }




    }
}

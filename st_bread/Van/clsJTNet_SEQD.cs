using st_bread.CLASS.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsJTNet_SEQD : IDisposable
    {
        
        #region Define DLL functions
        [DllImport("JTPayPosSEQD.dll")]
        private static extern int FN_TPAYPOS_SERIAL_SET(
            int serialPortNo,
            int serialBaud);

        [DllImport("JTPayPosSEQD.dll")]
        private static extern int FN_TPAYPOS_AUTH_SERIAL(
            int jobKind,
            int detailKind,
            byte[] input,
            int inputLen,
            [MarshalAs(UnmanagedType.LPStr)]StringBuilder output);

        [DllImport("JTPayPosSEQD.dll")]
        private static extern int FN_TPAYPOS_WINSOCK_SET(
            [MarshalAs(UnmanagedType.LPStr)]string IP,
            [MarshalAs(UnmanagedType.LPStr)]string port);

        [DllImport("JTPayPosSEQD.dll")]
        private static extern int FN_TPAYPOS_AUTH_SOCKET(
            int jobKind,
            int detailKind,
            byte[] input,
            int inputLen,
            [MarshalAs(UnmanagedType.LPStr)]StringBuilder output);
        #endregion

        #region Define command type codes and return codes of methods
        enum DLLResult
        {
            Fail = 0,
            Success = 1
        }

        enum JobKind
        {
            Auth = 0,
            Function = 1,
            Print = 2
        }

        enum AuthKind
        {
            NoPrint = 0,
            Print = 1
        }

        enum FunctionKind
        {
            DeviceInfo = 0,
            PreviousTRRequest = 1,
            KeyChange = 2,
            Sign = 3,
            Customer = 4
        }

        enum PrintKind
        {
            FreeForm = 0,
            Reprint = 1
        }
        #endregion



        private bool disposed = false;
        
        clsBill oBill = null;
        bool isCancel = false;
        int iCardCash = 0;
        clsbill_payments oPay = null;



        public clsJTNet_SEQD(clsBill _oBill,bool _isCancel,int iCArdCash_ )
        {
            oBill = _oBill;
            isCancel = _isCancel;
            iCardCash = iCArdCash_;
        }


        public clsJTNet_SEQD(clsbill_payments _oPay, bool _isCancel, int iCArdCash_)
        {   
            isCancel = _isCancel;
            iCardCash = iCArdCash_;
            oPay = _oPay;
        }





        public clsVanRep Auth_JTNet()
        {
            int result;

            byte[] request = new byte[1024];
            request.Initialize();

            StringBuilder response = new StringBuilder(1024);
            response.Clear();


            if (iCardCash == 0)
            {
                request = MakeCreditAuth();
            }
            else
            {
                request = MakeCashAuth();
            }

            Console.WriteLine(Encoding.GetEncoding(949).GetString(request));
            
            result = FN_TPAYPOS_AUTH_SERIAL(0, 0, request, request.Length, response);

            //Console.WriteLine(response.ToString(0, response.Length));
            

            
            return SetResData(response, result);
        }







        public clsJTNet_SEQD()
        {
            
        }

        ~clsJTNet_SEQD()
        {
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



        private byte[] MakeCreditAuth()
        {
            string temp = string.Empty;

            if (isCancel)             // 전문종류
            {
                temp = "1050"; //신용취소
            }
            else
            {
                temp = "1010";//신용승인
            }

            temp += string.Empty.PadLeft(2);                      // 카드종류 및 AID 값
            temp += string.Format("POS140054360");                           // POS 거래 고유번호
            temp += "JPTP150800015CB5";                             // POS 일련번호


            temp += "0";                                      // 단말기 거래 구분 0-일반 1-앱카드 2-면세유


            temp += oPay.bill_paymentamt.ToString().PadLeft(9, '0');                // 거래금액
            temp += "0".PadLeft(9, '0');             // 세금
            temp += "0".PadLeft(9, '0');             // 봉사료            
            temp += oPay.bill_halbu.PadLeft(2, '0');           // 할부개월
            temp += "0".PadLeft(9, '0');           // 비과세금액

            //승인일자 _ 승인번호 _ 고유번호
            if (isCancel)
            {
                temp += clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyMMdd").Substring(0, 6);
                temp += oPay.bill_authnum.PadRight(12);
                temp += oPay.bill_OrgApprovalNum.PadRight(12);
            }
            else
            {
                temp += string.Empty.PadLeft(6);
                temp += string.Empty.PadLeft(12);
                temp += string.Empty.PadLeft(12);
            }


            temp += string.Empty.PadLeft(2);                        // 결제코드
            temp += string.Empty.PadLeft(24);                            // 주유정보


            //옵션정보(32) 13
            temp += string.Format("C{0}" ,clsPos.tid).PadRight(11);  // 옵션정보

            if (oPay.bill_paymentamt  <= 49999)
            {

                temp += "NS";
            }
            else
            {
                temp += string.Empty.PadLeft(2);
            }

            temp += string.Empty.PadLeft(19);


            temp += string.Empty.PadLeft(16);                     // 부가정보1
            temp += string.Empty.PadLeft(32);                     // 부가정보2
            temp += string.Empty.PadLeft(128);                    // 부가정보3

            byte[] result = Encoding.GetEncoding(949).GetBytes(temp);

            return result;
        }



        private byte[] MakeCashAuth()
        {
            string temp = string.Empty;

            if (isCancel)             // 전문종류
            {
                temp = "5050"; //현금취소
            }
            else
            {
                temp = "5010";//현금승인
            }

            temp += string.Format("POS140054360");                               // POS 거래 고유번호

            if (iCardCash == 1) //소비자
            {
                temp += "0";                                        // 거래자 구분
            }
            else if (iCardCash == 2) //사업자
            {
                temp += "1";
            }


            temp += oPay.bill_paymentamt.ToString().PadLeft(9, '0');                  // 거래금액
            temp += "0".PadLeft(9, '0');               // 세금
            temp += "0".PadLeft(9, '0');           // 봉사료
            temp += "0".PadLeft(9, '0');             // 비과세 금액

            if (isCancel )
            {
                temp += clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyMMdd").Substring(0, 6);
                temp += oPay.bill_authnum.PadRight(12, ' ');
                temp += "3";                                        // 취소사유
            }
            else
            {
                temp += string.Empty.PadLeft(6);
                temp += string.Empty.PadLeft(12);
                temp += string.Empty.PadLeft(1);
            }


            temp += string.Format("C{0}", clsPos.tid).PadRight(11);  // 옵션정보            
            temp += string.Empty.PadLeft(21);  //"01027011436".PadRight(20 - "01027011436".Length, ' ');
            //temp += string.Empty.PadLeft(1);
            temp += string.Empty.PadLeft(16);                       // 부가정보1
            temp += string.Empty.PadLeft(32);                       // 부가정보2
            temp += string.Empty.PadLeft(128);                      // 부가정보3

            byte[] result = Encoding.GetEncoding(949).GetBytes(temp);

            return result;
        }




        private clsVanRep SetResData(StringBuilder str, int lResult)
        {
            clsVanRep oRep = new clsVanRep();


            byte[] tmpStr = System.Text.Encoding.Default.GetBytes(str.ToString());

            int iPos = 0;


            if (iCardCash == 0)
            {

                #region 카드
                if (lResult == 0) //오류
                {
                    iPos += 1;

                    //응답코드
                    byte[] rRepCode = new byte[4];
                    Array.Copy(tmpStr, iPos, rRepCode, 0, 4);
                    oRep.RepCode = Encoding.Default.GetString(rRepCode);
                    iPos += 4;

                    //응답메세지
                    byte[] rRepDesc = new byte[64];
                    Array.Copy(tmpStr, iPos, rRepDesc, 0, 64);
                    oRep.RepErrDesc = Encoding.Default.GetString(rRepDesc).Trim();
                }
                else if (lResult == -1) //"함수 승인 요청 오류(환경 설정)"
                {
                    oRep.RepCode = "9999";
                    oRep.RepErrDesc = "함수 승인 요청 오류(환경 설정)";

                }
                else//정상승인
                {

                    //byte[] rTranType = new byte[1];
                    //Array.Copy(tmpStr, iPos, rTranType, 0, 1);                
                    //Console.WriteLine("구분:{0}", Encoding.Default.GetString(rTranType));
                    iPos += 1;

                    //byte[] rTranLenght = new byte[4];
                    //Array.Copy(tmpStr, iPos, rTranLenght, 0, 4);
                    //Console.WriteLine("길이:{0}", Encoding.Default.GetString(rTranLenght));
                    iPos += 4;

                    //전문종류
                    byte[] rTranKind = new byte[4];
                    Array.Copy(tmpStr, iPos, rTranKind, 0, 4);
                    oRep.RepType = Encoding.Default.GetString(rTranKind);
                    iPos += 4;


                    //byte[] rPosNum = new byte[12];
                    //Array.Copy(tmpStr, iPos, rPosNum, 0, 12);
                    //Console.WriteLine("pos거래고유번호:{0}", Encoding.Default.GetString(rPosNum));
                    iPos += 12;

                    //터미널 ID
                    byte[] rTerminalID = new byte[10];
                    Array.Copy(tmpStr, iPos, rTerminalID, 0, 10);
                    oRep.terminalID = Encoding.Default.GetString(rTerminalID);
                    iPos += 10;

                    //응답코드
                    byte[] rRepCode = new byte[4];
                    Array.Copy(tmpStr, iPos, rRepCode, 0, 4);
                    oRep.RepCode = Encoding.Default.GetString(rRepCode);
                    iPos += 4;


                    //byte[] rRepDesc = new byte[64];
                    //Array.Copy(tmpStr, iPos, rRepDesc, 0, 64);
                    //Console.WriteLine("응답메세지:{0}", Encoding.Default.GetString(rRepDesc));
                    iPos += 64;

                    //승인번호
                    byte[] rAuthNum = new byte[12];
                    Array.Copy(tmpStr, iPos, rAuthNum, 0, 12);
                    oRep.RepAuthNum = Encoding.Default.GetString(rAuthNum).Trim();
                    iPos += 12;

                    //거래승인일시

                    byte[] rAuthDate = new byte[12];
                    Array.Copy(tmpStr, iPos, rAuthDate, 0, 12);
                    oRep.RepAuthDateTime = clsDateTime.Set_Time(Encoding.Default.GetString(rAuthDate));
                    iPos += 12;

                    //거래고유번호
                    byte[] rAuthOrgNum = new byte[12];
                    Array.Copy(tmpStr, iPos, rAuthOrgNum, 0, 12);
                    oRep.RepOrgApprovalNum = Encoding.Default.GetString(rAuthOrgNum).Trim();
                    iPos += 12;

                    //byte[] rAmt = new byte[9];
                    //Array.Copy(tmpStr, iPos, rAmt, 0, 9);
                    //Console.WriteLine("결제금액:{0}", Encoding.Default.GetString(rAmt));
                    iPos += 9;


                    //매입사코드
                    byte[] rCardCode = new byte[4];
                    Array.Copy(tmpStr, iPos, rCardCode, 0, 4);
                    oRep.RepCardCmpny_Code = Encoding.Default.GetString(rCardCode);
                    iPos += 4;

                    //매입사
                    byte[] rCard = new byte[20];
                    Array.Copy(tmpStr, iPos, rCard, 0, 20);
                    oRep.RepCardCmpny = Encoding.Default.GetString(rCard).Trim();
                    iPos += 20;


                    //발급사코드
                    byte[] rIssueCode = new byte[4];
                    Array.Copy(tmpStr, iPos, rIssueCode, 0, 4);
                    oRep.RepBuyCmpny_Code = Encoding.Default.GetString(rIssueCode);
                    iPos += 4;

                    //발급사
                    byte[] rIssue = new byte[20];
                    Array.Copy(tmpStr, iPos, rIssue, 0, 20);
                    oRep.RepBuyCmpny = Encoding.Default.GetString(rIssue).Trim();
                    iPos += 20;



                    //가맹점번호
                    byte[] rStore = new byte[15];
                    Array.Copy(tmpStr, iPos, rStore, 0, 15);
                    oRep.RepCmpny = Encoding.Default.GetString(rStore);
                    iPos += 15;


                    //byte[] rCardKind = new byte[1];
                    //Array.Copy(tmpStr, iPos, rCardKind, 0, 1);
                    //Console.WriteLine("매입구분:{0}", Encoding.Default.GetString(rCardKind));
                    iPos += 1;


                    //카드구분
                    byte[] rCardType = new byte[2];
                    Array.Copy(tmpStr, iPos, rCardType, 0, 2);
                    oRep.RepCardKind = Encoding.Default.GetString(rCardType);
                    iPos += 2;

                    //byte[] rEmtpy1 = new byte[9];
                    //Array.Copy(tmpStr, iPos, rEmtpy1, 0, 9);
                    //Console.WriteLine("예비금액1:{0}", Encoding.Default.GetString(rEmtpy1));
                    iPos += 9;


                    //byte[] rEmtpy2 = new byte[9];
                    //Array.Copy(tmpStr, iPos, rEmtpy2, 0, 9);
                    //Console.WriteLine("예비금액2:{0}", Encoding.Default.GetString(rEmtpy2));
                    iPos += 9;

                    //byte[] rEmtpy3 = new byte[9];
                    //Array.Copy(tmpStr, iPos, rEmtpy3, 0, 9);
                    //Console.WriteLine("예비금액3:{0}", Encoding.Default.GetString(rEmtpy3));
                    iPos += 9;

                    //byte[] rEmtpy4 = new byte[9];
                    //Array.Copy(tmpStr, iPos, rEmtpy4, 0, 9);
                    //Console.WriteLine("예비금액4:{0}", Encoding.Default.GetString(rEmtpy4));
                    iPos += 9;

                    //byte[] rEmtpy5 = new byte[9];
                    //Array.Copy(tmpStr, iPos, rEmtpy5, 0, 9);
                    //Console.WriteLine("예비금액5:{0}", Encoding.Default.GetString(rEmtpy5));
                    iPos += 9;


                    //byte[] rPoint1 = new byte[60];
                    //Array.Copy(tmpStr, iPos, rPoint1, 0, 60);
                    //Console.WriteLine("포인트정보1:{0}", Encoding.Default.GetString(rPoint1));
                    iPos += 60;

                    //byte[] rPoint2 = new byte[60];
                    //Array.Copy(tmpStr, iPos, rPoint2, 0, 60);
                    //Console.WriteLine("포인트정보2:{0}", Encoding.Default.GetString(rPoint2));
                    iPos += 60;

                    //카드번호
                    byte[] rTrack2data = new byte[20];
                    Array.Copy(tmpStr, iPos, rTrack2data, 0, 20);
                    oRep.RepCardNum = Encoding.Default.GetString(rTrack2data).Trim();
                    iPos += 20;


                    //byte[] rPrint = new byte[1];
                    //Array.Copy(tmpStr, iPos, rPrint, 0, 1);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rPrint));
                    iPos += 1;

                    //byte[] rETC = new byte[15];
                    //Array.Copy(tmpStr, iPos, rETC, 0, 15);
                    //Console.WriteLine("부가정보:{0}", Encoding.Default.GetString(rETC));
                    //iPos += 15;

                    //byte[] rReserved = new byte[32];
                    //Array.Copy(tmpStr, iPos, rReserved, 0, 32);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rReserved));
                    //iPos += 32;


                    //byte[] rMsg1 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rMsg1, 0, 28);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rMsg1));
                    //iPos += 28;


                    //byte[] rMsg2 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rMsg2, 0, 28);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rMsg2));
                    //iPos += 28;

                    //byte[] rMsg3 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rMsg3, 0, 28);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rMsg3));
                    //iPos += 28;
                    //byte[] rMsg4 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rMsg4, 0, 28);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rMsg4));
                    //iPos += 28;
                    //byte[] rMsg5 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rMsg5, 0, 28);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rMsg5));
                    //iPos += 28;
                    //byte[] rMsg6 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rMsg6, 0, 28);
                    //Console.WriteLine("rReserved:{0}", Encoding.Default.GetString(rMsg6));
                    //iPos += 28;
                }
                #endregion

            }
            else
            {
                #region 현금영수증
                if (lResult == 0) //오류
                {
                    iPos += 1;
                    //응답코드
                    byte[] rRepCode = new byte[4];
                    Array.Copy(tmpStr, iPos, rRepCode, 0, 4);
                    oRep.RepCode = Encoding.Default.GetString(rRepCode);
                    iPos += 4;

                    //응답메세지
                    byte[] rRepDesc = new byte[64];
                    Array.Copy(tmpStr, iPos, rRepDesc, 0, 64);
                    oRep.RepErrDesc = Encoding.Default.GetString(rRepDesc).Trim();
                }
                else if (lResult == -1) //"함수 승인 요청 오류(환경 설정)"
                {
                    oRep.RepCode = "9999";
                    oRep.RepErrDesc = "함수 승인 요청 오류(환경 설정)";

                }
                else//정상승인
                {

                    //byte[] rTranType = new byte[1];
                    //Array.Copy(tmpStr, iPos, rTranType, 0, 1);                
                    //Console.WriteLine("구분:{0}", Encoding.Default.GetString(rTranType));
                    iPos += 1;

                    //byte[] rTranLenght = new byte[4];
                    //Array.Copy(tmpStr, iPos, rTranLenght, 0, 4);
                    //Console.WriteLine("길이:{0}", Encoding.Default.GetString(rTranLenght));
                    iPos += 4;

                    //전문종류
                    byte[] rTranKind = new byte[4];
                    Array.Copy(tmpStr, iPos, rTranKind, 0, 4);
                    oRep.RepType = Encoding.Default.GetString(rTranKind);
                    iPos += 4;


                    //byte[] rPosNum = new byte[12];
                    //Array.Copy(tmpStr, iPos, rPosNum, 0, 12);
                    //Console.WriteLine("pos거래고유번호:{0}", Encoding.Default.GetString(rPosNum));
                    iPos += 12;

                    //터미널 ID
                    byte[] rTerminalID = new byte[10];
                    Array.Copy(tmpStr, iPos, rTerminalID, 0, 10);
                    oRep.terminalID = Encoding.Default.GetString(rTerminalID);
                    iPos += 10;

                    //응답코드
                    byte[] rRepCode = new byte[4];
                    Array.Copy(tmpStr, iPos, rRepCode, 0, 4);
                    oRep.RepCode = Encoding.Default.GetString(rRepCode);
                    iPos += 4;


                    //byte[] rRepDesc = new byte[64];
                    //Array.Copy(tmpStr, iPos, rRepDesc, 0, 64);
                    //Console.WriteLine("응답메세지:{0}", Encoding.Default.GetString(rRepDesc));
                    iPos += 64;

                    //승인번호
                    byte[] rAuthNum = new byte[12];
                    Array.Copy(tmpStr, iPos, rAuthNum, 0, 12);
                    oRep.RepAuthNum = Encoding.Default.GetString(rAuthNum).Trim();
                    iPos += 12;

                    //거래승인일시
                    byte[] rAuthDate = new byte[12];
                    Array.Copy(tmpStr, iPos, rAuthDate, 0, 12);
                    oRep.RepAuthDateTime = clsDateTime.Set_Time(Encoding.Default.GetString(rAuthDate));
                    iPos += 12;

                    //거래고유번호
                    byte[] rAuthOrgNum = new byte[12];
                    Array.Copy(tmpStr, iPos, rAuthOrgNum, 0, 12);
                    oRep.RepOrgApprovalNum = Encoding.Default.GetString(rAuthOrgNum).Trim();
                    iPos += 12;



                    byte[] rAmt = new byte[9];
                    Array.Copy(tmpStr, iPos, rAmt, 0, 9);

                    oRep.RepAuthAmt = clsSetting.Let_Double( Encoding.Default.GetString(rAmt).Trim());
                    
                    //Console.WriteLine("결제금액:{0}", Encoding.Default.GetString(rAmt));
                    iPos += 9;

                    //전표타이틀
                    //소비자:현금(소득공제), 사업자:현금(지출증빙) 거래취소 : 현금결제취소
                    byte[] rIssueCode = new byte[20];
                    Array.Copy(tmpStr, iPos, rIssueCode, 0, 20);
                    oRep.RepBuyCmpny = Encoding.Default.GetString(rIssueCode);
                    iPos += 20;

                    iPos += 9;
                    iPos += 9;
                    iPos += 9;
                    iPos += 60;

                    //카드번호
                    byte[] rTrack2data = new byte[20];
                    Array.Copy(tmpStr, iPos, rTrack2data, 0, 20);
                    oRep.RepCardNum = Encoding.Default.GetString(rTrack2data).Trim();
                    iPos += 20;



                    //byte[] rPrint = new byte[16];
                    //Array.Copy(tmpStr, iPos, rPrint, 0, 16);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rPrint));
                    //iPos += 16;

                    //byte[] rtest1 = new byte[32];
                    //Array.Copy(tmpStr, iPos, rtest1, 0, 32);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest1));
                    //iPos += 32;

                    //byte[] rtest2 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rtest2, 0, 28);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest2));
                    //iPos += 28;


                    //byte[] rtest3 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rtest3, 0, 28);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest3));
                    //iPos += 28;

                    //byte[] rtest4 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rtest4, 0, 28);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest4));
                    //iPos += 28;

                    //byte[] rtest5 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rtest5, 0, 28);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest5));
                    //iPos += 28;

                    //byte[] rtest6 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rtest6, 0, 28);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest6));
                    //iPos += 28;

                    //byte[] rtest7 = new byte[28];
                    //Array.Copy(tmpStr, iPos, rtest7, 0, 28);
                    //Console.WriteLine("전표출력유무:{0}", Encoding.Default.GetString(rtest7));
                    //iPos += 28;





                }
                #endregion
            }


            return oRep;
        }



    }
}

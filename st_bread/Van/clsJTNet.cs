using st_bread.CLASS.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace st_bread
{
    class clsJTNet: IDisposable
    {
        
        #region Define DLL functions
        [DllImport("JTPosSeqDmDll.dll")]
        
        private static extern int FN_JTDMABLITY(
            byte[] _pReqBuf,
            int _lReqBufLen,
             byte[] _pResBuf,
            //[MarshalAs(UnmanagedType.LPStr)]StringBuilder _pResBuf,
            int _lMinTimeOut
        );

        [DllImport("JTPosSeqDmDll.dll")]
        private static extern int FN_JTDMAUTHREQ(
           byte[] _pReqBuf,
           int _lReqBufLen,
           byte[] _pResBuf,
            //[MarshalAs(UnmanagedType.LPStr)]StringBuilder _pResBuf,
           int _lMinTimeOut
       );
        #endregion

        private bool disposed = false;
        
        clsBill oBill = null;
        bool isCancel = false;
        int iCardCash = 0;

        clsbill_payments oPay = null;



        public clsJTNet(clsBill _oBill,bool _isCancel,int iCArdCash_ )
        {
            oBill = _oBill;
            isCancel = _isCancel;
            iCardCash = iCArdCash_;
        }



        public clsJTNet(clsbill_payments _oPay, bool _isCancel, int iCArdCash_)
        {   
            isCancel = _isCancel;
            iCardCash = iCArdCash_;
            oPay = _oPay;
        }





        public clsJTNet()
        {
            
        }

        ~clsJTNet()
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


        public clsVanRep Auth_JTNet()
        {
            long lResult;
            byte[] bReq = new byte[2048];
            byte[] bRes = new byte[2048];
            bReq.Initialize();
            bRes.Initialize();

            if (iCardCash == 0)
                bReq = SetAuthData();
            else
                bReq = SetAuthData_Cash();

            
            clsVanRep oRep = new clsVanRep();

            //#if DEBUG
            //oRep.RepCode = "0000";
            //if (oRep.RepCode == "0000")
            //{
            //    oRep.RepAuthNum = "94550915    ";

            //    oRep.RepAuthDateTime = oBill.bill_date;
            //    oRep.RepOrgApprovalNum = "924000172458";
            //    oRep.RepCmpny = "732110112      ";
            //    oRep.RepCardCmpny_Code = "0100";
            //    oRep.RepCardCmpny = "비씨";
            //    oRep.RepBuyCmpny_Code = "0100";
            //    oRep.RepBuyCmpny = "테스트카드";
            //    oRep.RepCardKind = "CK";
            //    //거래구분
            //    oRep.RepCardNum = "490220**********    ";
            //}
            //else
            //{
            //    oRep.RepErrDesc = "Test 승인 실패";
            //}

            //#else
            if (bReq != null)
                lResult = FN_JTDMAUTHREQ(bReq, bReq.Length, bRes, 1);
            else
                lResult = -1;

            oRep = SetResData(bRes, lResult);
            //#endif

            return oRep;   
        }


        public long Check_Demon()
        {
            long lResult;
            byte[] bReq = new byte[2048];
            byte[] bRes = new byte[2048];
            bReq.Initialize();
            bRes.Initialize();


            string strReq = "DC";
            char CR = (char)0x0d;
            strReq += CR.ToString();

            bReq = Encoding.GetEncoding(949).GetBytes(strReq);

            if (bReq != null)
                lResult = FN_JTDMABLITY(bReq, bReq.Length, bRes, 1);
            else
                lResult = -1;

            return lResult;
        }


        private byte[] Check_CardIN()
        {
            string strReq = "RC";
            char CR = (char)0x0d;

            strReq += CR.ToString();
            return Encoding.GetEncoding(949).GetBytes(strReq);
        }



        private byte[] SetAuthData_Cash()
        {
            StringBuilder strReq = new StringBuilder(2048);
            strReq.Clear();

            //헤더 정보
            strReq = SetHeaderData();


            //현금영수증
            //WCC
            //strReq.Append("K");
            strReq.Append((char)0x20);

            //Track2Data
            //카드번호입력후
            //strReq.Append(string.Format("{0:D2}", TB_CARD.Text.Length) + TB_CARD.Text.PadRight(100 - 2, ' '));
            strReq.Append(string.Empty.PadRight(100));

            //거래금약
            strReq.Append(oPay.bill_paymentamt.ToString().PadLeft(9, '0'));
            //세금
            strReq.Append("0".PadLeft(9, '0'));
            //봉사료
            strReq.Append("0".PadLeft(9, '0'));

            //거래구분자
            if(iCardCash == 1)            
                strReq.Append("0");
            else
                strReq.Append("1");


            if (isCancel)
            {
                //취소                     
                strReq.Append(clsDateTime.Get_Time(oPay.bill_authdatetime  ).ToString("yyMMdd").Substring(0, 6).PadRight(6));
                strReq.Append(oPay.bill_authnum.PadRight(12));
                strReq.Append("1");


            }
            else
            {
                strReq.Append(string.Empty.PadRight(6));
                strReq.Append(string.Empty.PadRight(12));
                strReq.Append(string.Empty.PadRight(1));
            }
            //비과세 금액
            strReq.Append(string.Format("{0:D9}", 0));

            //하위사업자번호
            strReq.Append(string.Empty.PadRight(10));

            //부가정보 1
            strReq.Append(string.Empty.PadRight(16));
            //부가정보 2
            strReq.Append(string.Empty.PadRight(32));
            //Reserved
            strReq.Append(string.Empty.PadRight(128));
            //CR
            strReq.Append((char)0x0d);

            return Encoding.ASCII.GetBytes(strReq.ToString());
        }


        private byte[] SetAuthData()
        {
            StringBuilder strReq = new StringBuilder(2048);
            strReq.Clear();

            //헤더 정보
            strReq = SetHeaderData();


            //신용 승인 _ 취소
            //WCC
            strReq.Append((char)0x20); //리더기

            strReq.Append(string.Empty.PadRight(100));

            //할부개월
            strReq.Append(oPay.bill_halbu );

            //거래금약
            strReq.Append(oPay.bill_paymentamt.ToString().PadLeft(9, '0'));
            //세금
            strReq.Append("0".PadLeft(9, '0'));
            //봉사료
            strReq.Append("0".PadLeft(9, '0'));

            //통화코드
            strReq.Append("KRW");

            //승인일자 _ 승인번호 _ 고유번호
            if (isCancel )
            {
                //취소                     
                strReq.Append(clsDateTime.Get_Time(oPay.bill_authdatetime).ToString("yyMMdd").Substring(0, 6).PadRight(6));
                strReq.Append(oPay.bill_authnum.PadRight(12));
                strReq.Append(oPay.bill_OrgApprovalNum.PadRight(12));
            }
            else
            {
                strReq.Append(string.Empty.PadRight(6));
                strReq.Append(string.Empty.PadRight(12));
                strReq.Append(string.Empty.PadRight(12));               
            }

            //폴백 정보
            strReq.Append(string.Empty.PadRight(3));
            //결제코드
            strReq.Append(string.Empty.PadRight(2));
            //서비스코드
            strReq.Append(string.Empty.PadRight(8));
            //비과세 금액
            strReq.Append(string.Format("{0:D9}", 0));
            //비밀번호
            strReq.Append(string.Empty.PadRight(18));
            //주유정보
            strReq.Append(string.Empty.PadRight(24));
            //하위사업자번호
            strReq.Append(string.Empty.PadRight(10));
            //POS시리얼 번호
            strReq.Append("JTPOSDM16011E278");
            //부가정보 1
            strReq.Append(string.Empty.PadRight(32));
            //부가정보 2
            strReq.Append(string.Empty.PadRight(128));
            //Reserved
            strReq.Append(string.Empty.PadRight(64));
            //서명처리
            if (isCancel )
            {
                strReq.Append("N");
            }
            else
            {
                if (oPay.bill_paymentamt <= 49999)
                {
                    strReq.Append("N");
                }
                else
                {
                    strReq.Append("Y");
                }
                //strReq.Append("R"); //재사용

                
            }
            //CR
            strReq.Append((char)0x0d);

                    
            //else if (가맹점다운)
            //{
            //    //장치구분
            //    strReq.Append("P");
            //    //일련번호체크
            //    strReq.Append("P");
            //    //사업자번호
            //    strReq.Append("1078155843");
            //    //FILLER
            //    strReq.Append(string.Empty.PadRight(16));
            //    //CR
            //    strReq.Append((char)0x0d);
            //}

            return Encoding.ASCII.GetBytes(strReq.ToString());
        }
        
        private StringBuilder SetHeaderData()
        {
            StringBuilder _Header = new StringBuilder(38);
            string strTime = DateTime.Now.ToString("yyMMddhhmmss");
            string strPOS = DateTime.Now.ToString("yyMMddhh") + string.Format("{0:D4}", clsStaticString.g_iPOSseq);

            //전문 종류
            if (iCardCash == 0)
            {
                if (isCancel)
                {
                    _Header.Append("1050");
                }
                else
                {
                    _Header.Append("1010");

                }
            }
            else
            {
                if (isCancel)
                {
                    _Header.Append("5050");
                }
                else
                {
                    _Header.Append("5010");

                }
 
            }










            //else if (가매정다운이냐?)
            //{
            //    _Header.Append("8071");
            //}
            //TID
            _Header.Append(clsPos.tid  );
            //전문 관리 번호
            _Header.Append(strPOS);
            //전문 생성 일자
            _Header.Append(strTime);

            clsStaticString.g_iPOSseq++;

            return _Header;
        }

        private clsVanRep SetResData(byte[] _strRes,long lResult)
        {
            string strTemp = Encoding.Default.GetString(_strRes);
            clsVanRep oRep = new clsVanRep();
            
            byte[] rPacketType = new byte[1];
            byte[] rErrCode = new byte[4]; //응답코드 0000 승인
            byte[] rAuthNo = new byte[12];  //승인메세지
            byte[] rAuthDate = new byte[12];
            byte[] rAuthSerial = new byte[12];
            byte[] rErrMsg = new byte[64]; //거절 메세지
            byte[] rMerNo = new byte[15]; //가맹점번호
            byte[] rIssueCode = new byte[4]; //발급사 코드
            byte[] rIssueName = new byte[20]; //발급사명
            byte[] rPurchaseCode = new byte[4]; //매입사코드 
            byte[] rPurchaseName = new byte[20]; //매입사명
            byte[] rTranKind = new byte[2]; //'CK':신용, 'CH':체크, 'GK':기프트카드, 'UP':중국은련
            //'NT':BC/NH면세유, 'OL':유가보조카드, 'CP':원카드
            byte[] rCardno = new byte[20];


            Array.Copy(_strRes, 99, rPacketType, 0, 1);
            if (Encoding.Default.GetString(rPacketType) == "E")
                return oRep;

            if (lResult == 0) //오류
            {
                Array.Copy(_strRes, 0, rErrCode, 0, 4);
                oRep.RepCode = Encoding.Default.GetString(rErrCode);

                Array.Copy(_strRes, 4, rErrMsg, 0, 64);
                oRep.RepErrDesc  = Encoding.Default.GetString(rErrMsg);


            }
            else if (lResult == -1) //"함수 승인 요청 오류(환경 설정)"
            {
                oRep.RepCode = "9999";
                oRep.RepErrDesc = "함수 승인 요청 오류(환경 설정)";
                
            }
            else//정상승인
            {
                int iPos = 100;
                if (isCancel )
                {
                    Array.Copy(_strRes, iPos, rErrCode, 0, 4);
                    oRep.RepCode = Encoding.Default.GetString(rErrCode);
                }
                else 
                {
                    Array.Copy(_strRes, iPos, rErrCode, 0, 4);
                    oRep.RepCode = Encoding.Default.GetString(rErrCode);

                    if (oRep.RepCode == "0000")
                    {
                        iPos += 4;
                        Array.Copy(_strRes, iPos, rAuthNo, 0, 12);
                        oRep.RepAuthNum = Encoding.Default.GetString(rAuthNo);
                        iPos += 12;
                        Array.Copy(_strRes, iPos, rAuthDate, 0, 12);
                        oRep.RepAuthDateTime = clsDateTime.Set_Time(Encoding.Default.GetString(rAuthDate));
                        iPos += 12;
                        Array.Copy(_strRes, iPos, rAuthSerial, 0, 12);
                        oRep.RepOrgApprovalNum = Encoding.Default.GetString(rAuthSerial);
                        iPos += 12;
                        Array.Copy(_strRes, iPos, rMerNo, 0, 15);
                        oRep.RepCmpny = Encoding.Default.GetString(rMerNo);
                        iPos += 15;
                        Array.Copy(_strRes, iPos, rIssueCode, 0, 4);
                        oRep.RepCardCmpny_Code = Encoding.Default.GetString(rIssueCode);
                        iPos += 4;
                        Array.Copy(_strRes, iPos, rIssueName, 0, 20);
                        oRep.RepCardCmpny = Encoding.Default.GetString(rIssueName);
                        iPos += 20;
                        Array.Copy(_strRes, iPos, rPurchaseCode, 0, 4);
                        oRep.RepBuyCmpny_Code = Encoding.Default.GetString(rPurchaseCode);
                        iPos += 4;
                        Array.Copy(_strRes, iPos, rPurchaseName, 0, 20);
                        oRep.RepBuyCmpny = Encoding.Default.GetString(rPurchaseName);
                        iPos += 20;
                        Array.Copy(_strRes, iPos, rTranKind, 0, 2);
                        oRep.RepCardKind = Encoding.Default.GetString(rTranKind);
                        iPos = 530;
                        Array.Copy(_strRes, iPos, rCardno, 0, 20);
                        //거래구분
                        oRep.RepCardNum = Encoding.Default.GetString(rCardno);

                    }
                    else
                    {
                        iPos += 4;
                        Array.Copy(_strRes, iPos, rErrMsg, 0, 36);
                        oRep.RepErrDesc = Encoding.Default.GetString(rErrMsg);
                    }
                }
            }
            return oRep;
        }




    }
}

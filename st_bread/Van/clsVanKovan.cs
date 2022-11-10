using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace st_bread.CLASS.Van
{
    class clsVanKovan : IDisposable
    {
        private bool disposed = false;

        [DllImport(@"C:\KOVAN\VPOS_Client.dll")]
        private static extern int Kovan_Auth(
            String tcode,
            String Tid,
            String halbu,
            String tamt,
            String ori_date,
            String ori_authno,
            String tran_serial,
            String idno,
            String amt_flag,
            String tax_amt,
            String sfee_amt,
            String free_amt,
            String filler,
            byte[] rTranType,
            byte[] rErrCode,
            byte[] rCardno,
            byte[] rHalbu,
            byte[] rTamt,
            byte[] rTranDate,
            byte[] rTranTime,
            byte[] rAuthNo,
            byte[] rMerNo,
            byte[] rTranSerial,
            byte[] rIssueCard,
            byte[] rPurchaseCard,
            byte[] rSignPath,
            byte[] rMsg1,
            byte[] rMsg2,
            byte[] rMsg3,
            byte[] rMsg4,
            byte[] rFiller);

       

        clsBill oBill = null;
        bool isCancel = false;

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

        ~clsVanKovan()
        {
            Dispose(false);
        }


        public clsVanKovan()
        {
            
        }
        public clsVanKovan(clsBill _oBill, bool _isCancel)
        {
            oBill = _oBill;
            isCancel = _isCancel;
        }







        public clsKovan_Rep Approval_CreditCard()
        {
            int iRet = 0;
            //결제 응답
            byte[] rTranType = new byte[4];
            byte[] rErrCode = new byte[4];
            byte[] rCardno = new byte[18];
            byte[] rHalbu = new byte[2];
            byte[] rTamt = new byte[9];
            byte[] rTranDate = new byte[6];
            byte[] rTranTime = new byte[6];
            byte[] rAuthNo = new byte[12];
            byte[] rMerNo = new byte[15];
            byte[] rTranSerial = new byte[12];
            byte[] rIssueCard = new byte[30];
            byte[] rPurchaseCard = new byte[30];
            byte[] rSignPath = new byte[50];
            byte[] rMsg1 = new byte[100];
            byte[] rMsg2 = new byte[100];
            byte[] rMsg3 = new byte[100];
            byte[] rMsg4 = new byte[100];
            byte[] rFiller = new byte[102];
            clsKovan_Rep oRep = null;

            Random r = new Random();
            int iTran_Serial =  r.Next(1,999999);

            try
            {
                if (!isCancel) //승인전문
                {
                    iRet = Kovan_Auth(
                            "S0",
                            clsPos.tid,
                            oBill.bill_halbu,
                            String.Format("{0:000000000}",oBill.bill_paymentamt ),
                            String.Empty.PadLeft(6, ' '),
                            String.Empty.PadLeft(12, ' '),
                            String.Format("{0:000000}", iTran_Serial) + String.Empty.PadLeft(6, ' '),
                            String.Empty.PadLeft(33, ' '),
                            String.Empty.PadLeft(3, ' '),
                            "000000000",
                            "000000000",
                            "000000000",
                            String.Empty.PadLeft(21, ' ') + "S1" + String.Empty.PadLeft(77, ' '),
                            rTranType,
                            rErrCode,
                            rCardno,
                            rHalbu,
                            rTamt,
                            rTranDate,
                            rTranTime,
                            rAuthNo,
                            rMerNo,
                            rTranSerial,
                            rIssueCard,
                            rPurchaseCard,
                            rSignPath,
                            rMsg1,
                            rMsg2,
                            rMsg3,
                            rMsg4,
                            rFiller);

#if DEBUG
                    Console.WriteLine(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                        "S0",
                        clsPos.pos_num ,
                            oBill.bill_halbu,
                            String.Format("{0:000000000}", oBill.bill_paymentamt),
                            String.Empty.PadLeft(6, ' '),
                            String.Empty.PadLeft(12, ' '),
                            String.Format("{0:000000}", iTran_Serial) + String.Empty.PadLeft(6, ' '),
                            String.Empty.PadLeft(33, ' '),
                            String.Empty.PadLeft(3, ' '),
                            "000000000",
                            "000000000",
                            "000000000",
                            String.Empty.PadLeft(21, ' ') + "S1" + String.Empty.PadLeft(77, ' ')
                        ));
#endif


                }
                else if (isCancel) //취소 전문
                {
                    iRet = Kovan_Auth(
                            "S1",
                           clsPos.tid,
                            oBill.bill_halbu,
                            String.Format("{0:000000000}", oBill.bill_paymentamt ),
                            clsDateTime.Get_Time(oBill.bill_authdatetime).ToString("yyMMdd").Substring(0, 6).PadRight(6),  //YYMMDD
                            oBill.bill_authnum + String.Empty.PadRight(12 - oBill.bill_authnum.Length , ' '),
                            String.Format("{0:000000}", iTran_Serial) + String.Empty.PadLeft(6, ' '),
                            String.Empty.PadLeft(33, ' '),
                            String.Empty.PadLeft(3, ' '),
                            "000000000",
                            "000000000",
                            "000000000",
                            String.Empty.PadLeft(21, ' ') + "S1" + String.Empty.PadLeft(77, ' '),
                            rTranType,
                            rErrCode,
                            rCardno,
                            rHalbu,
                            rTamt,
                            rTranDate,
                            rTranTime,
                            rAuthNo,
                            rMerNo,
                            rTranSerial,
                            rIssueCard,
                            rPurchaseCard,
                            rSignPath,
                            rMsg1,
                            rMsg2,
                            rMsg3,
                            rMsg4,
                            rFiller);

                }


                oRep = new clsKovan_Rep();

                oRep.rTranType = Encoding.Default.GetString(rTranType).Trim();
                oRep.rErrCode = Encoding.Default.GetString(rErrCode).Trim();
                oRep.rCardno = Encoding.Default.GetString(rCardno).Trim();
                oRep.rHalbu = Encoding.Default.GetString(rHalbu).Trim();
                oRep.rTamt = Encoding.Default.GetString(rTamt).Trim();
                oRep.rAuthNo = Encoding.Default.GetString(rAuthNo).Trim();
                oRep.rMerNo = Encoding.Default.GetString(rMerNo).Trim();
                oRep.rTranDate = Encoding.Default.GetString(rTranDate).Trim();
                oRep.rTranTime = Encoding.Default.GetString(rTranTime).Trim();
                oRep.rTranSerial = Encoding.Default.GetString(rTranSerial).Trim();
                oRep.rIssueCard = Encoding.Default.GetString(rIssueCard).Trim();
                oRep.rPurchaseCard = Encoding.Default.GetString(rPurchaseCard).Trim();
                oRep.rSignPath = Encoding.Default.GetString(rSignPath).Trim();
                oRep.rMsg1 = Encoding.Default.GetString(rMsg1).Trim();
                oRep.rMsg2 = Encoding.Default.GetString(rMsg2).Trim();
                oRep.rMsg3 = Encoding.Default.GetString(rMsg3).Trim();
                oRep.rMsg4 = Encoding.Default.GetString(rMsg4).Trim();
                oRep.rFiller = Encoding.Default.GetString(rFiller).Trim();
                oRep.rErr_Msg = Err_Descript(oRep.rErrCode);
                
                return oRep;
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;                
            }
            finally
            {
                oRep.Dispose();
            }


        }

        private string Err_Descript(string rc)
        {
            string sReturn = string.Empty;

            switch (rc)
            {
                case "0000":
                    sReturn = "정상";
                    break;
                case "4001":
                    sReturn = "리더기 포트 미설정";
                    break;
                case "4002":
                    sReturn = "무결성점검 실패";
                    break;
                case "4003":
                    sReturn = "거래요청시 TID가 vpos 설정한 TID와 다름";
                    break;
                case "4004":
                    sReturn = "포트 열기 실패";
                    break;
                case "4005":
                    sReturn = "vpos에 설정한 시리얼번호와 리더기 시리얼번호가 다름";
                    break;

                case "4006":
                    sReturn = "승인서버 IP 형식이 맞지 않음";
                    break;
                case "4007":
                    sReturn = "승인서버 포트 미설정";
                    break;
                 case "5001":
                    sReturn = "IC 리딩 실패";
                    break;
                case "5002":
                    sReturn = "거래금액이 없음";
                    break;
                case "5003":
                    sReturn = "IC 카드 인식불가";
                    break;
                case "5004":
                    sReturn = "IC 카드 삽입되어 있음";
                    break;
                case "5005":
                    sReturn = "거래 flow에서 상황에 맞지않는 명령이 호출";
                    break;
                case "5006":
                    sReturn = "IC카드인데 MS리딩이 발생";
                    break;
                 case "5007":
                    sReturn = "IC카드 처리중 강제로 카드분리";
                    break;
                case "5008":
                    sReturn = "기타오류";
                    break;
                case "5009":
                    sReturn = "기타오류(정의되지 않은 오류)";
                    break;
                case "5010":
                    sReturn = "은련카드를 일반거래요청";
                    break;
                case "5011":
                    sReturn = "리더기가 카드구분(IC,MS) 판단하지 못함";
                    break;
                case "5012":
                    sReturn = "pin 미입력";
                    break;
                case "5013":
                    sReturn = "리더기 연결이 끊어짐";
                    break;

                case "5015":
                    sReturn = "리더기 명령 기타응답";
                    break;
                case "5016":
                    sReturn = "은련카드 AID 선택오류";
                    break;
                case "5017":
                    sReturn = "현금영수증 고객정보 미입력";
                    break;
                case "5018":
                    sReturn = "App카드 길이오류";
                    break;
                case "7001":
                    sReturn = "DLL 파일이 없음";
                    break;
                case "7002":
                    sReturn = "DLL 명령어가 없음";
                    break;
                case "7003":
                    sReturn = "pin 암호화 오류";
                    break;
                case "8324":
                    sReturn = "거래거절(승인서버응답)";
                    break;
                case "8325":
                    sReturn = "거래거절(이미 취소된 취소요청 거절 승인서버 응답)";
                    break;
                case "8555":
                    sReturn = "거래거절(승인서버응답이 공백)";
                    break;

                case "8899":
                    sReturn = "거래거절(승인서버응답이 기타)";
                    break;

                case "8001":
                    sReturn = "거래중 사용자 중단 요청";
                    break;
                case "8002":
                    sReturn = "거래중 망취소(리더기판단)";
                    break;
                case "8003":
                    sReturn = "거래중 망취소(응답 미수신)";
                    break;
                case "8004":
                    sReturn = "거래거절(승인서버 기타응답)";
                    break;
                case "8005":
                    sReturn = "pin거래 MS불가";
                    break;
                case "8006":
                    sReturn = "서명오류 취소(화면서명- 전송할 서명파일 없음)";
                    break;
                case "9999":
                    sReturn = "기타";
                    break;
                default:
                    sReturn = "기타";
                    break;


            }
            return sReturn;
        }


    }
}

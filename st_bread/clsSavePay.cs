using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;
using System.Reflection; //현재 실행중 함수명
using System.IO.Ports;

namespace st_bread
{
    class clsSavePay
    {
        
        OdbcConnection conn_local = new OdbcConnection();
        clsDbCon vDbCon = new clsDbCon();

        public clsSavePay()
        {


            conn_local = vDbCon.MysqlOdbcConnect(
                           clsPrintSet.SettingHT["IP2"].ToString(),
                           clsPrintSet.SettingHT["DBNAME2"].ToString(),
                           clsPrintSet.SettingHT["USER2"].ToString(),
                           clsPrintSet.SettingHT["PASS2"].ToString(),
                           clsPrintSet.SettingHT["PORT2"].ToString()
                           );
        }

        //키오스크와 요식업 포스의 listview의 항목이 달라 iListType으로 결정 0-키오스크 1-요식업
        //0-키오스크
        //0     1       2   3   4       5       6   7
        //번호 상품명 수량 단가 금액 상품코드 취소 그룹코드
        //1-요식업
        //0     1       2       3   4     5       6
        //번호 상품코드 상품명 수량 단가 금액  그룹코드

        
        //public int Save_Record(clsMemad o_Memad, clsMart o_Mart, clsKovan_Rep o_Rep, clsRecipInfo o_Recip, ListView o_Lst,int iListType)
        //{
        //    int idxNo = 0;
        //    int idxCd = 0;
        //    int idxNm = 0;
        //    int idxQty = 0;
        //    int idxDg = 0;
        //    int idxTot = 0;
        //    int idxGr = 0;
            
        //    if (iListType == 0) //키오스크일때
        //    { 
        //        idxNo = 0;
        //        idxCd = 5;
        //        idxNm = 1;
        //        idxQty = 2;
        //        idxDg = 3;
        //        idxTot = 4;
        //        idxGr = 7;
        //    }
        //    else if(iListType == 1) //요식업 포스일때
        //    {
        //        idxNo = 0;
        //        idxCd = 1;
        //        idxNm = 2;
        //        idxQty = 3;
        //        idxDg = 4;
        //        idxTot = 5;
        //        idxGr = 6;
        //    }

        //    int iReturn = 0;
        //    try
        //    {

        //        if (conn_local.State == ConnectionState.Open)
        //        {
        //            int iCount = 0;

        //            #region 주문 내역 저장
        //            foreach (ListViewItem item in o_Lst.Items)
        //            {
        //                iCount++;
        //                DateTime dt = new DateTime();
        //                string gsSql = string.Empty;

        //                gsSql = "INSERT INTO pay_gd " +
        //                        "    (" +
        //                        "    gd_jum," +
        //                        "    gd_martcd, " +
        //                        "    gd_pos_cd, " +
        //                        "    gd_date, " +
        //                        "    gd_time, " +
        //                        "    gd_no, " +
        //                        "    gd_tp, " +
        //                        "    gd_seq, " +
        //                        "    gd_gr_cd, " +
        //                        "    gd_gr_nm, " +
        //                        "    gd_master_cd, " +
        //                        "    gd_master_nm, " +
        //                        "    gd_mem_cd, " +
        //                        "    gd_count, " +
        //                        "    gd_cost," +
        //                        "    gd_total" +
        //                        "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";


        //                OdbcCommand oCmd = conn_local.CreateCommand();
        //                oCmd.CommandText = gsSql;

        //                oCmd.Parameters.Add("@gd_jum", SqlDbType.Char).Value = o_Memad.memad_jum;
        //                oCmd.Parameters.Add("@gd_martcd", SqlDbType.Char).Value = o_Mart.sMart_cd;
        //                oCmd.Parameters.Add("@gd_martcd", SqlDbType.Char).Value = o_Recip.sPosNO;
        //                oCmd.Parameters.Add("@gd_date", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
        //                oCmd.Parameters.Add("@gd_time", SqlDbType.Char).Value = DateTime.Now.ToString("HH:mm:ss");

        //                oCmd.Parameters.Add("@gd_no", SqlDbType.Int).Value = o_Recip.iRecipNum;
        //                oCmd.Parameters.Add("@gd_tp", SqlDbType.Char).Value = 1;
        //                oCmd.Parameters.Add("@gd_seq", SqlDbType.Int).Value = iCount;
        //                oCmd.Parameters.Add("@gd_gr_cd", SqlDbType.Char).Value = item.SubItems[idxGr].Text;
        //                oCmd.Parameters.Add("@gd_gr_nm", SqlDbType.Char).Value = "test";// Get_GrNM(item.SubItems[7].Text);
        //                oCmd.Parameters.Add("@gd_master_cd", SqlDbType.Char).Value = item.SubItems[idxCd].Text;
        //                oCmd.Parameters.Add("@gd_master_nm", SqlDbType.Char).Value = item.SubItems[idxNm].Text;
        //                oCmd.Parameters.Add("@gd_mem_cd", SqlDbType.Char).Value = "0";
        //                oCmd.Parameters.Add("@gd_count", SqlDbType.Int).Value = Int32.Parse(item.SubItems[idxQty].Text);
        //                oCmd.Parameters.Add("@gd_cost", SqlDbType.Int).Value = double.Parse(item.SubItems[idxDg].Text);
        //                oCmd.Parameters.Add("@gd_total", SqlDbType.Int).Value = double.Parse(item.SubItems[idxTot].Text);
        //                oCmd.ExecuteNonQuery();
        //            }
        //            #endregion

        //            #region 영수증 정보 저장
        //            {
        //                string gsSql = string.Empty;

        //                //카드 부분
        //                string sCard = string.Empty;
        //                int iAuthAmt = 0;

        //                string sCash = string.Empty;
        //                int iCashAuthAmt = 0;


        //                gsSql = "INSERT INTO pay " +
        //                        "    (" +
        //                        "    pay_jum," +
        //                        "    pay_mart_cd, " +   //푸드트럭코드
        //                        "    pay_pos_cd, " + //포스 번호
        //                        "    pay_date, " +
        //                        "    pay_time, " +
        //                        "    pay_no, " +    //주문순번(하루 기준으로 1로 시작해야됨)
        //                        "    pay_syn, " +    //온라인,오프라인
        //                        "    pay_tp, " +     //1결제 5취소
        //                        "    pay_mem_cd, " +   //회원 코드
        //                        "    pay_count, " +    //상품가지수
        //                        "    pay_total, " +    //총합(결제 총금액)
        //                        "    pay_vat, " +    //부가세
        //                        "    pay_amt, " +    //부가세 제외 총금액
        //                        "    pay_buysa, " +    //매입사
        //                        "    pay_cardsa," +    //카드사
        //                        "    pay_getid," +    //신용카드 결제
        //                        "    pay_van," +       //밴
        //                        "    pay_pad," +       //사인패드
        //                        "    pay_mon," +      //할부
        //                        "    pay_cartno," +     //카드번호
        //                        "    pay_agent," +     //가맹점번호
        //                        "    pay_auth," +      //승인번호
        //                        "    pay_cashno," +     //현금영수증
        //                        "    pay_card," +      //카드 금액
        //                        "    pay_cash," +      //현금 금액
        //                        "    pay_tid," +       //다날 결제
        //                        "    pay_order," +      //주방 주문확인 0:확인전 1: 확인
        //                        "    pay_mart_ocd," +    //여기서부터 취소
        //                        "    pay_ono," +
        //                        "    pay_odate," +
        //                        "    pay_kind," +
        //                        "    pay_customcnt" +
        //                        "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";



        //                switch (o_Recip.iCashCard)
        //                {
        //                    case (0): //카드

        //                        sCard = o_Rep.rCardno;
        //                        iAuthAmt = Int32.Parse(o_Rep.rTamt);

        //                        sCash = string.Empty;
        //                        iCashAuthAmt = 0;

        //                        break;
        //                    case (1): //현금영수증
        //                        sCard = string.Empty;
        //                        iAuthAmt = 0;

        //                        sCash = o_Rep.rCardno;
        //                        iCashAuthAmt = Int32.Parse(o_Rep.rTamt);
        //                        break;

        //                    case (2): //현금 
        //                        sCard = string.Empty;
        //                        iAuthAmt = 0;

        //                        sCash = "현금";
        //                        iCashAuthAmt = Int32.Parse(o_Rep.rTamt);
        //                        break;

        //                    case (3): //쿠폰
        //                        sCard = string.Empty;
        //                        iAuthAmt = 0;

        //                        sCash = "쿠폰";
        //                        iCashAuthAmt = Int32.Parse(o_Rep.rTamt);
        //                        break;
        //                }


        //                OdbcCommand oCmd = conn_local.CreateCommand();
        //                oCmd.CommandText = gsSql;

        //                oCmd.Parameters.Add("@pay_jum", SqlDbType.Char).Value = o_Memad.memad_jum;
        //                oCmd.Parameters.Add("@pay_mart_cd", SqlDbType.Char).Value = o_Mart.sMart_cd;
        //                oCmd.Parameters.Add("@pay_pos_cd", SqlDbType.Char).Value = o_Recip.sPosNO;
        //                oCmd.Parameters.Add("@pay_date", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
        //                oCmd.Parameters.Add("@pay_time", SqlDbType.Char).Value = DateTime.Now.ToString("HH:mm:ss");
        //                oCmd.Parameters.Add("@pay_no", SqlDbType.Int).Value = o_Recip.iRecipNum;
        //                oCmd.Parameters.Add("@pay_syn", SqlDbType.Char).Value = 0;
        //                oCmd.Parameters.Add("@pay_tp", SqlDbType.Char).Value = 1;
        //                oCmd.Parameters.Add("@pay_mem_cd", SqlDbType.Char).Value = o_Memad.memad_id;
        //                oCmd.Parameters.Add("@pay_count", SqlDbType.Int).Value = o_Lst.Items.Count;
        //                oCmd.Parameters.Add("@pay_total", SqlDbType.Int).Value = o_Recip.iTotal;
        //                oCmd.Parameters.Add("@pay_vat", SqlDbType.Int).Value = o_Recip.iVat;
        //                oCmd.Parameters.Add("@pay_amt", SqlDbType.Int).Value = o_Recip.iPan;
        //                oCmd.Parameters.Add("@pay_buysa", SqlDbType.Char).Value = o_Rep.rPurchaseCard;
        //                oCmd.Parameters.Add("@pay_cardsa", SqlDbType.Char).Value = o_Rep.rIssueCard;
        //                oCmd.Parameters.Add("@pay_getid", SqlDbType.Char).Value = "";
        //                oCmd.Parameters.Add("@pay_van", SqlDbType.Char).Value = "KOVAN";
        //                oCmd.Parameters.Add("@pay_pad", SqlDbType.Char).Value = o_Rep.rSignPath;
        //                oCmd.Parameters.Add("@pay_mon", SqlDbType.Char).Value = o_Rep.rHalbu;
        //                oCmd.Parameters.Add("@pay_cartno", SqlDbType.Char).Value = sCard;
        //                oCmd.Parameters.Add("@pay_agent", SqlDbType.Char).Value = o_Rep.rMerNo;
        //                oCmd.Parameters.Add("@pay_auth", SqlDbType.Char).Value = o_Rep.rAuthNo;
        //                oCmd.Parameters.Add("@pay_cashno", SqlDbType.Char).Value = sCash;
        //                oCmd.Parameters.Add("@pay_card", SqlDbType.Int).Value = iAuthAmt;
        //                oCmd.Parameters.Add("@pay_cash", SqlDbType.Int).Value = iCashAuthAmt;
        //                oCmd.Parameters.Add("@pay_tid", SqlDbType.Char).Value = o_Mart.sMart_getid;
        //                oCmd.Parameters.Add("@pay_order", SqlDbType.Char).Value = 0;
        //                oCmd.Parameters.Add("@pay_mart_ocd", SqlDbType.Char).Value = "";
        //                oCmd.Parameters.Add("@pay_ono", SqlDbType.Int).Value = 0;
        //                oCmd.Parameters.Add("@pay_odate", SqlDbType.Date).Value = "1900-01-01";
        //                oCmd.Parameters.Add("@pay_kind", SqlDbType.Date).Value = o_Recip.iCashCard;
        //                oCmd.Parameters.Add("@pay_customcnt", SqlDbType.Date).Value = o_Recip.iAccCustomNum;
        //                oCmd.ExecuteNonQuery();

        //            }
        //            #endregion

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
        //        iReturn = -1;
        //    }
        //    finally
        //    {
        //        //결제 완료 되면 번호 증가
        //        //conHt["ORDER"] = o_Recip.iRecipNum.ToString();
        //        //oFile.Set_XMLVal(clsPrintSet.SETTING_FILE(), "SERIAL", "ORDER", o_Recip.iRecipNum.ToString());

        //        //conHt["AUTH"] = o_Recip.iRecipNum.ToString();
        //        //oFile.Set_XMLVal(clsPrintSet.SETTING_FILE(), "SERIAL", "AUTH", o_Recip.iSerialNum.ToString());


        //        //oFile.Check_PutSetting("SERIAL", "ORDER", o_Recip.iRecipNum.ToString());
        //        oFile.Check_PutSetting("APP", "AUTH", o_Recip.iSerialNum.ToString());
        //        oFile.Check_PutSetting("APP", "ACCCNT", o_Recip.iAccCustomNum.ToString());

        //    }


        //    return iReturn;

        //}

       

//        //영수증 재발행시 사용
//        public void Print_Receip(clsMart oMart, clsPay oPay, ListView lst_ ,clsKovan_Rep o_Rep = null )
//        {
//            SerialPort port = new SerialPort(clsPrintSet.SettingHT["COMP"].ToString(), Int32.Parse(clsPrintSet.SettingHT["SPEED"].ToString()), Parity.None, 8, StopBits.One);
//            try
//            {
                
//                    int iEmpty = Int32.Parse(clsPrintSet.SettingHT["EMPTY"].ToString());
//                    int iSep = Int32.Parse(clsPrintSet.SettingHT["SEP"].ToString());
//                    string sPayKind = string.Empty;

//                    port.Encoding = Encoding.Default;

//                    port.Open();

//                    switch (oPay.sPaykind)
//                    {
//                        case ("0"):
//                            sPayKind = "신용카드";
//                            break;
//                        case ("1"):
//                            sPayKind = "현 금";
//                            port.WriteLine(clsPrintSet.Cmd_DrawOpen());
//                            break;

//                        case ("2"): //현금 영수증 없이
//                            sPayKind = "현 금";
//                            port.WriteLine(clsPrintSet.Cmd_DrawOpen());
//                            break;
//                        case ("3"): //쿠폰 공짜 손님
//                            sPayKind = "쿠폰사용";
//                            break;
//                    }


//                    port.WriteLine(clsPrintSet.Cmd_Close());

                    


//                    port.WriteLine(clsPrintSet.Cmd_Font1());
//                    port.WriteLine(string.Empty.PadLeft(5, ' ') + "영 수 증[재발행]" + string.Empty.PadLeft(20, ' '));

//                    port.WriteLine(clsPrintSet.Cmd_Close());


//                    port.WriteLine("매  장  명 : " + oMart.sMart_nm);
//                    port.WriteLine("사업자번호 : " + oMart.sMart_comno);
//                    port.WriteLine("대 표 자명 : " + oMart.sMart_ceo);
//                    port.WriteLine("주      소 : " + oMart.sMart_address);

//                    port.WriteLine("영수증# : " + oPay.sPaydateAuth() + " " + oPay.sPaycashno);
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));

//                    if (oPay.sPaykind == "0" || oPay.sPaykind == "1" || oPay.sPaykind == "2")
//                    {
//                        port.WriteLine("판매금액:".PadRight(21) + (string.Format("{0:#,##0}", oPay.iPan)).PadLeft(16));
//                        port.WriteLine("부 가 세:".PadRight(22) + (string.Format("{0:#,##0}", oPay.iVat)).PadLeft(16));
//                        port.WriteLine("합    계:".PadRight(23) + (string.Format("{0:#,##0}", oPay.iTotal)).PadLeft(16));
//                    }

//                    port.WriteLine(string.Empty.PadLeft(iSep, '='));

//                    switch (oPay.sPaykind)
//                    {
//                        case ("0"):
//                            port.WriteLine("신용 카드" + oPay.sPaybuysa );
//                            port.WriteLine("가맹번호 : " + oMart.sMart_getid);
//                            port.WriteLine("승인번호 : " + oPay.sPayauth);
//                            port.WriteLine("카 드 사 : " + oPay.sPaycardsa);
//                            port.WriteLine("카드번호 : " + oPay.sPaycartno);
//                            port.WriteLine("승인일시 : " + oPay.sPayAuthDateTime());
//                            port.WriteLine("승인금액 : " + string.Format("{0:#,##0}", oPay.iTotal));
//                            break;
//                        case ("1"):

//                            if (o_Rep != null)//현금영수증 재 발행시 
//                            {
//                                port.WriteLine(o_Rep.rIssueCard);
//                                port.WriteLine("처리번호 : " + o_Rep.rAuthNo);
//                                port.WriteLine("회원번호 : " + o_Rep.rCardno);
//                                port.WriteLine("거래일시 : " + o_Rep.rTranDate + " " + o_Rep.rTranTime ); //DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")

//                                port.WriteLine(o_Rep.rMsg1.Replace(" ", ""));
//                                port.WriteLine(o_Rep.rMsg2.Replace(" ", ""));
//                                port.WriteLine(o_Rep.rMsg3.Replace(" ", ""));
//                                port.WriteLine(o_Rep.rMsg4.Replace(" ", ""));

//                            }
//                            else
//                            {
//                                port.WriteLine(oPay.sPaycardsa);
//                                port.WriteLine("처리 번호 : " + oPay.sPayauth);
//                                port.WriteLine("회원번호 : " + oPay.sPaycashno);
//                                port.WriteLine("거래일시 : " + oPay.sPaydate  + " " + oPay.sPaytime ); //oRep.rTranDate + " " + oRep.rTranTime);
                                
//                                port.WriteLine("금    액 : " + string.Format("{0:#,##0}", oPay.iTotal));
//                            }




//                            break;

//                        case ("2"): //현금 영수증 없이

//                            port.WriteLine("현금 ");

//                            port.WriteLine("금    액 : " + string.Format("{0:#,##0}", oPay.iTotal));

//                            port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                            break;
//                        case ("3"): //쿠폰 사용

//                            port.WriteLine("쿠폰사용");
//                            port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                            break;
//                        case ("4"): //이벤트 당첨

//                            port.WriteLine("이벤트 당첨");
//                            port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                            break;
//                    }

//                    port.WriteLine(string.Empty.PadLeft(iSep, '='));
//                    port.WriteLine("수량     메뉴                      금액");

//                    foreach (ListViewItem item in lst_.Items)
//                    {
//                        if (oPay.sPaykind == "0" || oPay.sPaykind == "1" || oPay.sPaykind == "2")
//                        {
//                            port.WriteLine(
//                                    item.SubItems[2].Text.PadRight(3) +
//                                    item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
//                                    "".PadRight(15 - item.SubItems[3].Text.Length) +
//                                    item.SubItems[3].Text
//                                    );
//                        }
//                        else
//                        {
//                            port.WriteLine(
//                                      item.SubItems[2].Text.PadRight(3) +
//                                      item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
//                                      "".PadRight(15 - item.SubItems[3].Text.Length)
//                                      );
 
//                        }

//                        #if DEBUG
//                        Console.WriteLine(
//                            item.SubItems[2].Text.PadRight(3) +
//                                item.SubItems[1].Text + "".PadRight(iEmpty - Encoding.Default.GetByteCount(item.SubItems[1].Text)) +
//                                "".PadRight(15 - item.SubItems[3].Text.Length) +
//                                item.SubItems[3].Text
//                                );
//#endif


//                    }

                    
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(clsPrintSet.Cmd_Font_Items());
//                    port.WriteLine("이용해 주셔서 감사 합니다.");
//                    port.WriteLine(clsPrintSet.Cmd_Close());

//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
//                    port.WriteLine(clsPrintSet.Cmd_Cutting());
                    
                   

//            }
//            catch (Exception ex)
//            {
//                oFile.WriteErrLog(MethodBase.GetCurrentMethod().Name, ex.Message.ToString());


//            }
//            finally
//            {
//                if (port.IsOpen)
//                    port.Close();

//            }
//        }

    }
}

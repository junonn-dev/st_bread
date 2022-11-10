using Oracle.ManagedDataAccess.Client;
using st_bread.Kiosk_Bill;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace st_bread
{
    /// <summary>
    /// 영수증 상세 현황
    /// </summary>
    public class clsKiosk_Bill : IDisposable
    {
        private bool disposed = false;

        double dTotalAmt = 0, _dVat = 0, dExvatamt = 0;

        public string bill_machine { set; get; } //기기 번호
                
        public int bill_num { set; get; } //영수증 번호
        public int  bill_date { set; get; }  //영수증 날짜
        
        public clsEnum.bill_isCancel bill_iscancel { set; get; } //취소 여부

        public int bill_itemcount { set; get; } //구매한 상품 갯수

        public double bill_totalamt { set; get; } //총금액
        
        /// <summary>
        /// 총 구매 원가
        /// </summary>
        public double bill_totalct { set; get; }

        public double bill_saleAmt { set; get; } //할인 금액

        public double bill_DutyFreeAmt { set; get; } //면세 합        
        public double bill_TaxAmt { set; get; } //과세 합
        
        public double bill_vatamt 
        {
            set
            {
                _dVat = Set_Amt(value);
            }
            get
            {
                return _dVat;
            }

        } //부과세
        

        public int bill_orgdatetime { set; get; } //원전표 생성일시        
        public string bill_orguser { set; get; } //원 전표  담당자
        public string bill_orguser_nm { set; get; } //원 전표 담당자
        public string bill_orgpos { set; get; } //원 전표 포스 번호
        public string bill_orgopennum { set; get; } //원 전표 오픈 번호
        public int bill_orgbillnum { set; get; } //원 전표 오픈 번호     
   

        public string bill_Code { set; get; } //코드 값

        /// <summary>
        /// 승인금액
        /// </summary>
        public double bill_paymentamt { set; get; } //승인금액
        
        /// <summary>
        /// 가맹점코드
        /// </summary>
        public string bill_cmpny { get; set; }
        /// <summary>
        /// 시리얼번호
        /// </summary>
        public string bill_Posserialnum { set; get; }
        /// <summary>
        /// 매입사
        /// </summary>
        public string bill_buycmpny { set; get; }
        /// <summary>
        /// 카드사
        /// </summary>
        public string bill_cardcmpny { set; get; }
        /// <summary>
        /// 밴사
        /// </summary>
        public string bill_vancmpny { set; get; }
        /// <summary>
        /// 사인
        /// </summary>
        public string bill_signpath { set; get; }
        /// <summary>
        /// 할부
        /// </summary>
        public string bill_halbu { set; get; }
        /// <summary>
        /// 카드번호
        /// </summary>
        public string bill_cardnum { set; get; }

        /// <summary>
        /// 카드구분 2byte
        /// JTnet기준
        /// 'CK':신용, 'CH':체크, 'GK':기프트카드, 'UP':중국은련
        /// 'NT':BC/NH면세유, 'OL':유가보조카드, 'CP':원카드
        /// </summary>
        public string bill_cardkind { get; set; }


        /// <summary>
        /// 승인번호
        /// </summary>
        public string bill_authnum { set; get; }        
        /// <summary>
        /// 티아이디
        /// </summary>
        public string bill_tid { set; get; }
        /// <summary>
        /// 승인일시
        /// </summary>
        public int bill_authdatetime { set; get; }
        
        /// <summary>
        /// 승인고유번호
        /// </summary>
        public string bill_OrgApprovalNum { set; get; }


        public string bill_send_cmd { get; set; }
        public string bill_recv { get; set; }


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

        public clsKiosk_Bill()
        {
            bill_num = 0;
            bill_date = clsSetting._Today();
        }


        public clsKiosk_Bill(string sBill_Code)
        {
            Get_Bill(sBill_Code);

        }






        ~clsKiosk_Bill()
        {
            Dispose(false);
        }

        public double Set_Amt(Double dAmt)
        {
            double dTemp = Math.Round(dAmt / 1.1, 0);
            return dTemp;
        }


        private int Get_Bill(string sBill_Code)
        {
            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataSet ds = null;
            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Bill";                
                cmd_SEL.Parameters.Add("billcode", OracleDbType.Varchar2 ).Value = sBill_Code;
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                ds = clsDBExcute.SelectQuery_SET(cmd_SEL);

                //영수증 테이블
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bill_machine = dr["MACHINE_CODE"].ToString();
                    bill_num = Convert.ToInt32(dr["BILL_NUM"]);
                    bill_date = Convert.ToInt32(dr["BILL_DATE"]);
                    bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    bill_itemcount = Convert.ToInt32(dr["BILL_ITEMCOUNT"]);
                    bill_totalamt = Convert.ToDouble(dr["BILL_TOTALAMT"]);
                    bill_totalct = Convert.ToDouble(dr["BILL_TOTALCT"]);
                    bill_saleAmt = Convert.ToDouble(dr["BILL_SALEAMT"]);
                    bill_DutyFreeAmt = Convert.ToDouble(dr["BILL_DUTYFREEAMT"]);
                    bill_TaxAmt = Convert.ToDouble(dr["BILL_TAXAMT"]);
                    bill_vatamt = Convert.ToDouble(dr["BILL_VATAMT"]);
                    bill_Code = dr["BILL_CODE"].ToString();
                    bill_paymentamt = Convert.ToDouble(dr["BILL_PAYMENTAMT"]);
                    bill_buycmpny = dr["BILL_BUYCMPNY"].ToString();
                    bill_cardcmpny = dr["BILL_CARDCMPNY"].ToString();
                    bill_vancmpny = dr["BILL_VANCMPNY"].ToString();
                    bill_signpath = dr["BILL_SIGNPATH"].ToString();
                    bill_halbu = dr["BILL_HALBU"].ToString();
                    bill_cardnum = dr["BILL_CARDNUM"].ToString();
                    bill_authnum = dr["BILL_AUTHNUM"].ToString();
                    bill_tid = dr["BILL_TID"].ToString();
                    bill_authdatetime = Convert.ToInt32(dr["BILL_AUTHDATE"]);
                    bill_Posserialnum = dr["BILL_POSSERIALNUM"].ToString();
                    bill_OrgApprovalNum = dr["BILL_AUTHORGNUM"].ToString();
                    bill_cardkind = dr["BILL_CARDKIND"].ToString();
                    bill_cmpny = dr["BILL_CMPNY"].ToString();

                }
                return 0;
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (cmd_SEL != null)
                    cmd_SEL.Dispose();

                if (ds != null)
                    ds.Dispose();
            }
        }

        public int Update_Bill(clsBill oBill)
        {
            OracleCommand oCmd_Bill_Item = null;



            if (bill_authnum == null)
            {
                bill_authnum = "9999999";
                bill_authdatetime = clsSetting._Today();
            }


            try
            {
                oCmd_Bill_Item = new OracleCommand();
                oCmd_Bill_Item.CommandType = CommandType.StoredProcedure;
                oCmd_Bill_Item.CommandText = "PROC_POS.UpdateBill";

                oCmd_Bill_Item.Parameters.Add("MACHINECODE", OracleDbType.Varchar2 ).Value = bill_machine;
                oCmd_Bill_Item.Parameters.Add("BILLDATE", OracleDbType.Int32).Value = bill_date;
                oCmd_Bill_Item.Parameters.Add("billauth", OracleDbType.Varchar2 ).Value = bill_authnum;
                oCmd_Bill_Item.Parameters.Add("billauthdate", OracleDbType.Int32).Value = bill_authdatetime;

                if (oBill.bill_iscancel == clsEnum.bill_isCancel.auth)
                {
                    oCmd_Bill_Item.Parameters.Add("posbill", OracleDbType.Varchar2).Value = oBill.bill_Code;
                    oCmd_Bill_Item.Parameters.Add("nfc", OracleDbType.Varchar2).Value = bill_cardnum;
                    oCmd_Bill_Item.Parameters.Add("kioskbill", OracleDbType.Varchar2).Value = bill_Code;
                }

                oCmd_Bill_Item.BindByName = true;

                clsDBExcute.ExcuteQuery(oCmd_Bill_Item);

                return 1;
            }
            catch (OracleException oEx)
            {
                //에러 발생시 xml로 저장
                //Set_BillClass();

                ArgumentException argEx = new ArgumentException(oEx.Message.ToString());
                throw argEx;
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (oCmd_Bill_Item != null)
                    oCmd_Bill_Item.Dispose();

            }
        }

    }
}

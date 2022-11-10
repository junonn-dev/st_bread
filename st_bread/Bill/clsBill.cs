using Oracle.ManagedDataAccess.Client;
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
    public class clsBill : IDisposable
    {
        private bool disposed = false;

        double dTotalAmt = 0, _dVat = 0, dExvatamt = 0;

        public string bill_pos { set; get; } //기기 번호                        
        public int  bill_date { set; get; }  //영수증 날짜
        
        public clsEnum.bill_isCancel bill_iscancel { set; get; } //취소 여부

        public int bill_itemcount { set; get; } //구매한 상품 갯수

        public double bill_totalamt { set; get; } //총금액
        
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

        public List<clsBill_Items> oLstBill_Items = null;

        public List<clsbill_payments> oLstBill_payments = null;


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

        public clsBill()
        {   
            bill_date = clsSetting._Today();
        }


        public clsBill(string sBill_Date,List<clsItems> lst_Items)
        {
            Get_Bill(sBill_Date,lst_Items );

        }

        public clsBill(string sPos, string sBill_Date, List<clsItems> lst_Items)
        {
            Get_Bill(sPos,sBill_Date, lst_Items);
        }


        public clsBill(int billnum)
        {
            //bill_num = 0;
            //Get_Bill(billnum.ToString());
        }


        ~clsBill()
        {
            Dispose(false);
        }

        public double Set_Amt(Double dAmt)
        {
            double dTemp = Math.Round(dAmt / 1.1, 0);
            return dTemp;
        }


        public void Set_BillCode()
        {
            string sTemp = "";

            DateTime dati = DateTime.Now;
            sTemp += dati.ToString("yyMMdd");
            sTemp += clsPos.pos_num;

            string sDateTemp = clsSetting._Today().ToString();

            sTemp += sDateTemp.Substring(sDateTemp.Length - 4);


            var chars = sTemp.ToCharArray();
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

            bill_Code = sTemp + checksum_digit.ToString();

        }

        private bool BillCode_Check(string sFullCode)
        {
            if (sFullCode.Length < 13)
                return false;

            string sTempCode = sFullCode.Substring(0, sFullCode.Length - 1);
            var chars = sTempCode.ToCharArray();
            int isum = 0;
            string sCheckVal = sFullCode.Substring(sFullCode.Length - 1, 1);

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

        public int Save_Bill()
        {
            List<OracleCommand> oLstCmd = new List<OracleCommand>();

            try
            {
                if (oLstBill_Items != null)
                {
                    int iSEQ = 1;

                    foreach (clsBill_Items oBillItem in oLstBill_Items)
                    {
                        OracleCommand oCmd_Bill_Item = new OracleCommand();
                        oCmd_Bill_Item.CommandType = CommandType.StoredProcedure;
                        oCmd_Bill_Item.CommandText = "PROC_POS.Insert_Bill_Item";
                        oCmd_Bill_Item.Parameters.Add("MACHINECODE", OracleDbType.Varchar2).Value = clsPos.pos_num;                        
                        oCmd_Bill_Item.Parameters.Add("BILLDATE", OracleDbType.Int32).Value = bill_date;
                        oCmd_Bill_Item.Parameters.Add("BILLISCANCEL", OracleDbType.Int32).Value = (int)bill_iscancel;
                        oCmd_Bill_Item.Parameters.Add("BILLSLOT", OracleDbType.Varchar2).Value = 0;
                        oCmd_Bill_Item.Parameters.Add("BILLITEM_CODE", OracleDbType.Varchar2).Value = oBillItem.item_code;
                        oCmd_Bill_Item.Parameters.Add("BILLITEM_SEQ", OracleDbType.Int32).Value = iSEQ++;
                        oCmd_Bill_Item.Parameters.Add("BILLCMPNY", OracleDbType.Varchar2).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLPCNUM", OracleDbType.Varchar2).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLPC_SEQ", OracleDbType.Varchar2).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLCAPA", OracleDbType.Double).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLCST", OracleDbType.Double).Value = oBillItem.item_cost;
                        oCmd_Bill_Item.Parameters.Add("BILLDG", OracleDbType.Double).Value = oBillItem.item_cost;
                        oCmd_Bill_Item.Parameters.Add("BILLPRODUCT_DATE", OracleDbType.Int32).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLIN_DATE", OracleDbType.Int32).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLEXPIRE_DATE", OracleDbType.Int32).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLOBJECT_CODE", OracleDbType.Varchar2).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLFULL_CODE", OracleDbType.Varchar2).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLIS_SALE", OracleDbType.Int32).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLCOST", OracleDbType.Double).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLPRICE", OracleDbType.Double).Value = oBillItem.dBill_Amt;
                        oCmd_Bill_Item.Parameters.Add("BILLSALE_COST", OracleDbType.Double).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLSALE_PRICE", OracleDbType.Double).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("BILLVAT", OracleDbType.Double).Value = oBillItem.dItem_Vat;
                        oCmd_Bill_Item.Parameters.Add("BILLAMT", OracleDbType.Double).Value = oBillItem.dBill_Amt;
                        oCmd_Bill_Item.Parameters.Add("billletstate", OracleDbType.Int32).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("billgradenm", OracleDbType.Int32).Value = "0";
                        oCmd_Bill_Item.Parameters.Add("billqty", OracleDbType.Int32).Value = oBillItem.item_qty;
                        oCmd_Bill_Item.BindByName = true;
                        oLstCmd.Add(oCmd_Bill_Item);
                    }
                }
                else
                {

                }


                if (oLstBill_payments != null)
                {
                    int iSEQ1 = 1;

                    foreach (clsbill_payments oBillpay in oLstBill_payments )
                    {
                        OracleCommand oCmd_Bill_pay = new OracleCommand();
                        oCmd_Bill_pay.CommandType = CommandType.StoredProcedure;
                        oCmd_Bill_pay.CommandText = "PROC_POS.Insert_Bill_Pay";
                        oCmd_Bill_pay.Parameters.Add("MACHINECODE", OracleDbType.Varchar2).Value = clsPos.pos_num;
                        oCmd_Bill_pay.Parameters.Add("BILLDATE", OracleDbType.Int32).Value = bill_date;
                        oCmd_Bill_pay.Parameters.Add("BILLISCANCEL", OracleDbType.Int32).Value = (int)bill_iscancel;
                        oCmd_Bill_pay.Parameters.Add("BILLSEQ", OracleDbType.Int32).Value = iSEQ1++;
                        oCmd_Bill_pay.Parameters.Add("BILLPAYKIND", OracleDbType.Int32).Value = (int)oBillpay.bill_paymentskind;
                        oCmd_Bill_pay.Parameters.Add("BILLREPCODE", OracleDbType.Varchar2).Value = oBillpay.sRepCode;
                        oCmd_Bill_pay.Parameters.Add("BILLRECVAMT", OracleDbType.Double).Value = oBillpay.bill_recvamt;
                        oCmd_Bill_pay.Parameters.Add("BILLRESTAMT", OracleDbType.Double).Value = oBillpay.bill_restAmt;
                        oCmd_Bill_pay.Parameters.Add("BILLPAYMENTAMT", OracleDbType.Double).Value = oBillpay.bill_paymentamt;

                        oCmd_Bill_pay.Parameters.Add("BILLSERIALNUM", OracleDbType.Varchar2).Value = "";
                        
                        oCmd_Bill_pay.Parameters.Add("BILLBUYCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_buycmpny;
                        oCmd_Bill_pay.Parameters.Add("BILLCARDCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_cardcmpny;
                        oCmd_Bill_pay.Parameters.Add("BILLVANCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_vancmpny;
                        oCmd_Bill_pay.Parameters.Add("BILLSIGNPATH", OracleDbType.Varchar2).Value = oBillpay.bill_signpath;

                        
                        oCmd_Bill_pay.Parameters.Add("BILLHALBU", OracleDbType.Varchar2).Value = oBillpay.bill_halbu == null ? "00" : oBillpay.bill_halbu;
                        oCmd_Bill_pay.Parameters.Add("BILLCARDNUM", OracleDbType.Varchar2).Value = oBillpay.bill_cardnum;
                        oCmd_Bill_pay.Parameters.Add("BILLAUTHNUM", OracleDbType.Varchar2).Value = oBillpay.bill_authnum;
                        oCmd_Bill_pay.Parameters.Add("BILLVANID", OracleDbType.Varchar2).Value = clsPos.tid;
                        oCmd_Bill_pay.Parameters.Add("BILLTID", OracleDbType.Varchar2).Value = clsPos.tid; 
                        oCmd_Bill_pay.Parameters.Add("BILLAUTHDATETIME", OracleDbType.Int32 ).Value = oBillpay.bill_authdatetime;
                        oCmd_Bill_pay.Parameters.Add("BILLCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_cmpny;
                        oCmd_Bill_pay.Parameters.Add("BILLORGAPPROVALNUM", OracleDbType.Varchar2).Value = oBillpay.bill_OrgApprovalNum;
                        oCmd_Bill_pay.Parameters.Add("BILLCARDKIND", OracleDbType.Varchar2).Value = oBillpay.bill_cardkind;
                        oCmd_Bill_pay.BindByName = true;
                        oLstCmd.Add(oCmd_Bill_pay);
                    }
                }
                else
                {

                }


                OracleCommand oCmd_Bill = new OracleCommand();
                oCmd_Bill.CommandType = CommandType.StoredProcedure;
                oCmd_Bill.CommandText = "PROC_POS.Insert_Bill";
                oCmd_Bill.Parameters.Add("MACHINECODE", OracleDbType.Varchar2).Value = clsPos.pos_num;
                
                oCmd_Bill.Parameters.Add("BILLDATE", OracleDbType.Int32).Value = bill_date;
                oCmd_Bill.Parameters.Add("BILLISCANCEL", OracleDbType.Int32).Value = (int)bill_iscancel;
                oCmd_Bill.Parameters.Add("BILLITEMCOUNT", OracleDbType.Int32).Value = bill_itemcount;
                oCmd_Bill.Parameters.Add("BILLTOTALAMT", OracleDbType.Double).Value = bill_totalamt;
                oCmd_Bill.Parameters.Add("BILLDUTYFREEAMT", OracleDbType.Double).Value = bill_DutyFreeAmt;
                oCmd_Bill.Parameters.Add("BILLTAXAMT", OracleDbType.Double).Value = bill_TaxAmt;
                oCmd_Bill.Parameters.Add("BILLVATAMT", OracleDbType.Double).Value = bill_vatamt;
                oCmd_Bill.Parameters.Add("BILLCODE", OracleDbType.Varchar2).Value = bill_Code;
                oCmd_Bill.Parameters.Add("BILLPAYMENTAMT", OracleDbType.Double).Value = bill_paymentamt;
                oCmd_Bill.Parameters.Add("BILLBUYCMPNY", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLCARDCMPNY", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLVANCMPNY", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLSIGNPATH", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLHALBU", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLCARDNUM", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLAUTHNUM", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLTID", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLAUTHDATE", OracleDbType.Int32).Value = 0;
                oCmd_Bill.Parameters.Add("BILLPOSSERIALNUM", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLAUTHORGNUM", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLCARDKIND", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("BILLCMPNY", OracleDbType.Varchar2).Value = "";

                oCmd_Bill.Parameters.Add("billsendcmd", OracleDbType.Varchar2).Value = "";
                oCmd_Bill.Parameters.Add("billrecv", OracleDbType.Varchar2).Value = "";

                oCmd_Bill.BindByName = true;
                oLstCmd.Add(oCmd_Bill);


                if (oLstCmd.Count > 0)
                {
                    clsDBExcute.ExcuteQuery(oLstCmd);

                }
                return 1;
            }
            catch (OracleException oEx)
            {
                //에러 발생시 xml로 저장
                Set_BillClass();

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
                
            }
        }

        private int Get_Bill(string billdate, List<clsItems> lst_Items)
        {
            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataSet ds = null;
            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Bill";
                cmd_SEL.Parameters.Add("MachineCode", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("finddate", OracleDbType.Int32).Value = billdate;
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.Parameters.Add("cur_billitem", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                cmd_SEL.Parameters.Add("cur_billpay", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                ds = clsDBExcute.SelectQuery_SET(cmd_SEL);


                //영수증 테이블
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //bill_machine = dr["MACHINE_CODE"].ToString();
                    //bill_num = Convert.ToInt32(dr["BILL_NUM"]);
                    bill_date = Convert.ToInt32(dr["BILL_DATE"]);
                    bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    bill_itemcount = Convert.ToInt32(dr["BILL_ITEMCOUNT"]);
                    bill_totalamt = Convert.ToDouble(dr["BILL_TOTALAMT"]);
                    //bill_totalct = Convert.ToDouble(dr["BILL_TOTALCT"]);
                    //bill_saleAmt = Convert.ToDouble(dr["BILL_SALEAMT"]);
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


                oLstBill_Items = new List<clsBill_Items>();
                //oLstBill_payments = new List<clsbill_payments>();
                //상품테이블
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    clsBill_Items oBillItem = new clsBill_Items();
                    oBillItem.bill_pos = clsPos.pos_num;
                    oBillItem.bill_date = Convert.ToInt32(dr["BILL_DATE"]);
                    oBillItem.bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    oBillItem.item_code = dr["BILL_ITEM_CODE"].ToString();

                    if (oBillItem.item_code == "99999")
                    {
                        oBillItem.item_vat = clsEnum.Item_vat.item_duty_free;
                        oBillItem.item_name = "NFC 카드";

                    }
                    else
                    {
                        foreach (clsItems xMaster in lst_Items)
                        {
                            if (xMaster.item_code == oBillItem.item_code)
                            {
                                oBillItem.item_vat = xMaster.item_vat;
                                oBillItem.item_name = xMaster.item_name;
                                break;
                            }
                        }
                    }

                    oBillItem.item_seq = Convert.ToInt32(dr["BILL_ITEM_SEQ"]);
                    oBillItem.item_cost = Convert.ToDouble(dr["BILL_DG"]);
                    oBillItem.dBill_Amt = Convert.ToDouble(dr["BILL_AMT"]);
                    oBillItem.item_qty = Convert.ToInt32(dr["BILL_QTY"]);
                    oLstBill_Items.Add(oBillItem);
                }





                oLstBill_payments  = new List<clsbill_payments>();
                //상품테이블
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    clsbill_payments obill_pay = new clsbill_payments ();


                    obill_pay.bill_pos = clsPos.pos_num;
                    obill_pay.bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    obill_pay.bill_seq = Convert.ToInt32(dr["bill_seq"]);
                    obill_pay.bill_paymentskind  = (clsEnum.Payment_Kind  )Convert.ToInt32(dr["bill_paykind"]);

                    

                    obill_pay.sRepCode  = dr["bill_repcode"].ToString();

                    obill_pay.bill_recvamt = Convert.ToDouble(dr["bill_recvamt"]);
                    obill_pay.bill_restAmt  = Convert.ToDouble(dr["bill_restamt"]);

                    obill_pay.bill_paymentamt = Convert.ToDouble(dr["BILL_PAYMENTAMT"]);

                    obill_pay.bill_buycmpny = dr["BILL_BUYCMPNY"].ToString();
                    obill_pay.bill_cardcmpny = dr["BILL_CARDCMPNY"].ToString();
                    obill_pay.bill_vancmpny = dr["BILL_VANCMPNY"].ToString();
                    obill_pay.bill_signpath = dr["BILL_SIGNPATH"].ToString();
                    obill_pay.bill_halbu = dr["BILL_HALBU"].ToString();
                    obill_pay.bill_cardnum = dr["BILL_CARDNUM"].ToString();
                    obill_pay.bill_authnum = dr["BILL_AUTHNUM"].ToString();
                    obill_pay.bill_tid = dr["BILL_TID"].ToString();
                    obill_pay.bill_authdatetime = Convert.ToInt32(dr["BILL_AUTHDATETIME"]);
                    obill_pay.bill_serialnum = "";
                    obill_pay.bill_OrgApprovalNum = dr["bill_orgapprovalnum"].ToString();
                    obill_pay.bill_cardkind = dr["BILL_CARDKIND"].ToString();
                    //obill_pay.bill_cmpny = dr["BILL_CMPNY"].ToString();

                    oLstBill_payments.Add(obill_pay);

                   
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

        private int Get_Bill(string sPos,string billdate, List<clsItems> lst_Items)
        {
            //디비에서 정보가져온다.
            OracleCommand cmd_SEL = new OracleCommand();
            DataSet ds = null;
            try
            {
                //거래처 명 가져온다.                    
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_Bill";
                cmd_SEL.Parameters.Add("MachineCode", OracleDbType.Varchar2).Value = sPos;
                cmd_SEL.Parameters.Add("finddate", OracleDbType.Int32).Value = billdate;
                cmd_SEL.Parameters.Add("cur_bill", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.Parameters.Add("cur_billitem", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                cmd_SEL.Parameters.Add("cur_billpay", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                ds = clsDBExcute.SelectQuery_SET(cmd_SEL);


                //영수증 테이블
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //bill_machine = dr["MACHINE_CODE"].ToString();
                    //bill_num = Convert.ToInt32(dr["BILL_NUM"]);
                    bill_pos = sPos;
                    bill_date = Convert.ToInt32(dr["BILL_DATE"]);
                    bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    bill_itemcount = Convert.ToInt32(dr["BILL_ITEMCOUNT"]);
                    bill_totalamt = Convert.ToDouble(dr["BILL_TOTALAMT"]);
                    //bill_totalct = Convert.ToDouble(dr["BILL_TOTALCT"]);
                    //bill_saleAmt = Convert.ToDouble(dr["BILL_SALEAMT"]);
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


                oLstBill_Items = new List<clsBill_Items>();
                //oLstBill_payments = new List<clsbill_payments>();
                //상품테이블
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    clsBill_Items oBillItem = new clsBill_Items();
                    oBillItem.bill_pos = sPos;
                    oBillItem.bill_date = Convert.ToInt32(dr["BILL_DATE"]);
                    oBillItem.bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    oBillItem.item_code = dr["BILL_ITEM_CODE"].ToString();

                    if (oBillItem.item_code == "99999")
                    {
                        oBillItem.item_vat = clsEnum.Item_vat.item_duty_free;
                        oBillItem.item_name = "NFC 카드";

                    }
                    else
                    {
                        foreach (clsItems xMaster in lst_Items)
                        {
                            if (xMaster.item_code == oBillItem.item_code)
                            {
                                oBillItem.item_vat = xMaster.item_vat;
                                oBillItem.item_name = xMaster.item_name;
                                break;
                            }
                        }
                    }

                    oBillItem.item_seq = Convert.ToInt32(dr["BILL_ITEM_SEQ"]);
                    oBillItem.item_cost = Convert.ToDouble(dr["BILL_DG"]);
                    oBillItem.dBill_Amt = Convert.ToDouble(dr["BILL_AMT"]);
                    oBillItem.item_qty = Convert.ToInt32(dr["BILL_QTY"]);
                    oLstBill_Items.Add(oBillItem);
                }





                oLstBill_payments = new List<clsbill_payments>();
                //상품테이블
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    clsbill_payments obill_pay = new clsbill_payments();


                    obill_pay.bill_pos = sPos;
                    obill_pay.bill_iscancel = (clsEnum.bill_isCancel)Convert.ToInt32(dr["BILL_ISCANCEL"]);
                    obill_pay.bill_seq = Convert.ToInt32(dr["bill_seq"]);
                    obill_pay.bill_paymentskind = (clsEnum.Payment_Kind)Convert.ToInt32(dr["bill_paykind"]);



                    obill_pay.sRepCode = dr["bill_repcode"].ToString();

                    obill_pay.bill_recvamt = Convert.ToDouble(dr["bill_recvamt"]);
                    obill_pay.bill_restAmt = Convert.ToDouble(dr["bill_restamt"]);

                    obill_pay.bill_paymentamt = Convert.ToDouble(dr["BILL_PAYMENTAMT"]);

                    obill_pay.bill_buycmpny = dr["BILL_BUYCMPNY"].ToString();
                    obill_pay.bill_cardcmpny = dr["BILL_CARDCMPNY"].ToString();
                    obill_pay.bill_vancmpny = dr["BILL_VANCMPNY"].ToString();
                    obill_pay.bill_signpath = dr["BILL_SIGNPATH"].ToString();
                    obill_pay.bill_halbu = dr["BILL_HALBU"].ToString();
                    obill_pay.bill_cardnum = dr["BILL_CARDNUM"].ToString();
                    obill_pay.bill_authnum = dr["BILL_AUTHNUM"].ToString();
                    obill_pay.bill_tid = dr["BILL_TID"].ToString();
                    obill_pay.bill_authdatetime = Convert.ToInt32(dr["BILL_AUTHDATETIME"]);
                    obill_pay.bill_serialnum = "";
                    obill_pay.bill_OrgApprovalNum = dr["bill_orgapprovalnum"].ToString();
                    obill_pay.bill_cardkind = dr["BILL_CARDKIND"].ToString();
                    //obill_pay.bill_cmpny = dr["BILL_CMPNY"].ToString();

                    oLstBill_payments.Add(obill_pay);


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



        public int Update_Bill()
        {
            OracleCommand oCmd_Bill_Item = null;
            try
            {
                oCmd_Bill_Item = new OracleCommand();
                oCmd_Bill_Item.CommandType = CommandType.StoredProcedure;
                oCmd_Bill_Item.CommandText = "PROC_POS.Set_Bill_Cancel";
                oCmd_Bill_Item.Parameters.Add("MACHINECODE", OracleDbType.Varchar2).Value = clsPos.pos_num;
                oCmd_Bill_Item.Parameters.Add("BILLDATE", OracleDbType.Int32).Value = bill_date;
                oCmd_Bill_Item.Parameters.Add("BILLISCANCEL", OracleDbType.Int32).Value = (int)bill_iscancel;
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


        public int Update_Bill(int i)
        {
            
            List<OracleCommand> oLstCmd = new List<OracleCommand>();
            try
            {

                foreach (clsbill_payments oBillpay in oLstBill_payments)
                {
                    OracleCommand oCmd_Bill_pay = new OracleCommand();
                    oCmd_Bill_pay.CommandType = CommandType.StoredProcedure;
                    oCmd_Bill_pay.CommandText = "PROC_POS.Update_Bill_Pay";
                    oCmd_Bill_pay.Parameters.Add("MACHINECODE", OracleDbType.Varchar2).Value = clsPos.pos_num;
                    oCmd_Bill_pay.Parameters.Add("BILLDATE", OracleDbType.Int32).Value = bill_date;
                    oCmd_Bill_pay.Parameters.Add("BILLISCANCEL", OracleDbType.Int32).Value = (int)bill_iscancel;
                    oCmd_Bill_pay.Parameters.Add("BILLSEQ", OracleDbType.Int32).Value = oBillpay.bill_seq;
                    oCmd_Bill_pay.Parameters.Add("BILLPAYKIND", OracleDbType.Int32).Value = (int)oBillpay.bill_paymentskind;
                    oCmd_Bill_pay.Parameters.Add("BILLREPCODE", OracleDbType.Varchar2).Value = oBillpay.sRepCode;
                    oCmd_Bill_pay.Parameters.Add("BILLRECVAMT", OracleDbType.Double).Value = oBillpay.bill_recvamt;
                    oCmd_Bill_pay.Parameters.Add("BILLRESTAMT", OracleDbType.Double).Value = oBillpay.bill_restAmt;
                    oCmd_Bill_pay.Parameters.Add("BILLPAYMENTAMT", OracleDbType.Double).Value = oBillpay.bill_paymentamt;
                    oCmd_Bill_pay.Parameters.Add("BILLSERIALNUM", OracleDbType.Varchar2).Value = "";
                    oCmd_Bill_pay.Parameters.Add("BILLBUYCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_buycmpny;
                    oCmd_Bill_pay.Parameters.Add("BILLCARDCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_cardcmpny;
                    oCmd_Bill_pay.Parameters.Add("BILLVANCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_vancmpny;
                    oCmd_Bill_pay.Parameters.Add("BILLSIGNPATH", OracleDbType.Varchar2).Value = oBillpay.bill_signpath;
                    oCmd_Bill_pay.Parameters.Add("BILLHALBU", OracleDbType.Varchar2).Value = oBillpay.bill_halbu == null ? "00" : oBillpay.bill_halbu;
                    oCmd_Bill_pay.Parameters.Add("BILLCARDNUM", OracleDbType.Varchar2).Value = oBillpay.bill_cardnum;
                    oCmd_Bill_pay.Parameters.Add("BILLAUTHNUM", OracleDbType.Varchar2).Value = oBillpay.bill_authnum;
                    oCmd_Bill_pay.Parameters.Add("BILLVANID", OracleDbType.Varchar2).Value = clsPos.tid;
                    oCmd_Bill_pay.Parameters.Add("BILLTID", OracleDbType.Varchar2).Value = clsPos.tid;
                    oCmd_Bill_pay.Parameters.Add("BILLAUTHDATETIME", OracleDbType.Int32).Value = oBillpay.bill_authdatetime;
                    oCmd_Bill_pay.Parameters.Add("BILLCMPNY", OracleDbType.Varchar2).Value = oBillpay.bill_cmpny;
                    oCmd_Bill_pay.Parameters.Add("BILLORGAPPROVALNUM", OracleDbType.Varchar2).Value = oBillpay.bill_OrgApprovalNum;
                    oCmd_Bill_pay.Parameters.Add("BILLCARDKIND", OracleDbType.Varchar2).Value = oBillpay.bill_cardkind;
                    oCmd_Bill_pay.BindByName = true;
                    oLstCmd.Add(oCmd_Bill_pay);
                }


                if (oLstCmd.Count > 0)
                    clsDBExcute.ExcuteQuery(oLstCmd);





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
                if (oLstCmd != null)
                    oLstCmd = null;

            }
        }


        /// <summary>
        /// 영수증 내역 xml로 백업 나중에 불러오던 머하던 사용할 계획
        /// </summary>
        private void Set_BillClass()
        {

            XmlSerializer x = null;
            XmlSerializer ix = null;

            try
            {                
                string sFileName = bill_Code;
                //영수증 백업할 디렉토리 생성
                string sDir = Path.Combine(clsSetting.BILL_DIR(), sFileName);
                

                //영수증 파일로 임시 저장
                x = new XmlSerializer(this.GetType());
                using (FileStream fs = new FileStream(Path.Combine(sDir, sFileName + ".xml"), FileMode.Create))
                {
                    x.Serialize(fs, this);
                }

                //구매 상품 저장
                ix = new XmlSerializer(this.oLstBill_Items.GetType());
                using (FileStream fs3 = new FileStream(Path.Combine(sDir, sFileName + "_items.xml"), FileMode.Create))
                {
                    ix.Serialize(fs3, this.oLstBill_Items);
                }
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                x = null;
                ix = null;
            }
        }
    }
}

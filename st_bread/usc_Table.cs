using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace st_bread
{
    public partial class usc_Table : UserControl
    {
        Inter_Table frm = null;

        private clsEnum.Table_State tblstate = clsEnum.Table_State.empty;

        public clsEnum.Table_State TableState 
        {
            set 
            {
                tblstate = value;

                if (tblstate == clsEnum.Table_State.table || tblstate == clsEnum.Table_State.empty)
                {
                    pan_state.BackgroundImage = null;
                }
                else
                {
                    pan_state.BackgroundImage = Properties.Resources.person_color;
 
                }

            }
            get
            {
                return tblstate;
            }
        }




        public int sTableNo { get; set; }        
        public double dAmt { get; set; }
        public int order_no { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }

        public List<clsBill_Items> oLstBill_Items { get; set; }
        public int iMajorTalbe { get; set; }
        public  List<int> lst_AssembleTable  { get; set; }



        public usc_Table(Inter_Table _frm)
        {
            InitializeComponent();
            lbl_TableNo.Text = "";
            lbl_Amt.Text = "";
            frm = _frm;
        }






        public void Show_Table_Info()
        {
            lbl_TableNo.Text = string.Format("{0} 번", sTableNo);


            if (tblstate == clsEnum.Table_State.empty)
            {
                this.BackgroundImage = Properties.Resources.bg_btntype02;
            }
            else
            {
                this.BackgroundImage = Properties.Resources.bg_btntype01;

                
            }

            double dAmt = Get_Table_Items();

            if (dAmt > 0)
            {   
                //TableState = clsEnum.Table_State.table_On;
                lbl_Amt.Text = clsSetting.Let_Money(dAmt);
            }
        }

        public void Add_Table_Info()
        {   
            if (oLstBill_Items != null)
            {
                if (oLstBill_Items.Count > 0)
                {
                    //TableState = clsEnum.Table_State.table_On;
                    lbl_Amt.Text = clsSetting.Let_Money(Insert_Table_Items());
                }
            }
        }

        public void Reloard_Table_Info()
        {
            double dAmt = Get_Table_Items();

            if (dAmt > 0)
            {
                //TableState = clsEnum.Table_State.table_On;
                lbl_Amt.Text = clsSetting.Let_Money(dAmt);
            }
        }


        //결제완료 
        public void Clear_Order()
        {
            if (oLstBill_Items != null)
            {
                if (oLstBill_Items.Count > 0)
                {
                    Delete_Table_Items();
                }
            }

            lbl_Amt.Text = "";
            //TableState = clsEnum.Table_State.table;
        }


        


        public void Set_Assembly()
        {
            if (iMajorTalbe == 0)
            {   
                lbl_TableNo.Text = string.Format("{0} 번", sTableNo);
            }
            else
            {
                

                if (sTableNo == iMajorTalbe)
                {
                    lbl_TableNo.Text = string.Format("{0}번 통합됨", sTableNo, iMajorTalbe);

                }
                else
                {
                    lbl_TableNo.Text = string.Format("{0}번-{1}번 통합됨", sTableNo, iMajorTalbe);


                    if (oLstBill_Items != null)
                    {
                        if (oLstBill_Items.Count > 0)
                        {
                            Delete_Table_Items();
                        }
                    }

                    lbl_Amt.Text = "";

                }
            }


            
        }



        /// <summary>
        /// 테이블 조정 모드
        /// </summary>
        /// <param name="iKind"> 0- 기본모드 1-수정모드 2-테이블 합치기 모드 3-테이블 분리 모드</param>
        public void Set_Modify(int iKind )
        {
            btn_Func.ForeColor = Color.Black;
            switch (iKind)
            {
                case 0:
                    lbl_Amt.Visible = true;
                    btn_Func.Visible = false;
                    lbl_Amt.Dock = DockStyle.Bottom;

                    break;
                case 1:
                        lbl_Amt.Visible = false;
                        btn_Func.Visible = true;
                        btn_Func.Location = new Point(45, 36);


                        if (TableState == clsEnum.Table_State.empty)
                        {
                            btn_Func.Text = "번호지정";
                            btn_Func.Tag = "1";

                        }
                        else
                        {
                            btn_Func.Text = "빈테이블";
                            btn_Func.Tag = "2";
                        }

                    break;
                case 2:

                    if (TableState != clsEnum.Table_State.empty)
                    {
                        lbl_Amt.Visible = false;
                        btn_Func.Visible = true;
                        btn_Func.Location = new Point(45, 36);

                        btn_Func.Text = "선택";
                        btn_Func.Tag = "3";
                    }
                    break;
                case 3:


                    if (TableState == clsEnum.Table_State.table_Assem)
                    {
                        if (iMajorTalbe == sTableNo)
                        {
                            lbl_Amt.Visible = false;
                            btn_Func.Visible = true;
                            btn_Func.Location = new Point(45, 36);

                            btn_Func.Text = "분리";
                            btn_Func.Tag = "4";
                        }
                    }




                    //if (TableState != clsEnum.Table_State.empty  )
                    //{
                    //    if (TableState == clsEnum.Table_State.table_Assem)
                    //    {
                    //        lbl_Amt.Visible = false;
                    //        btn_Func.Visible = true;
                    //        btn_Func.Dock = DockStyle.Bottom;

                    //        btn_Func.Text = "나누기";
                    //        btn_Func.Tag = "4";
                    //    }
                    //}
                    break;


                case 4:
                    if (TableState != clsEnum.Table_State.empty)
                    {
                        if (TableState == clsEnum.Table_State.table_On )
                        {
                            lbl_Amt.Visible = false;
                            btn_Func.Visible = true;
                            btn_Func.Location = new Point(45, 36);

                            btn_Func.Text = "선택";
                            btn_Func.Tag = "5";
                        }
                    }
                    break;
                case 5:
                    if (TableState != clsEnum.Table_State.empty)
                    {
                        if (TableState == clsEnum.Table_State.table  )
                        {
                            lbl_Amt.Visible = false;
                            btn_Func.Visible = true;
                            btn_Func.Location = new Point(45, 36);

                            btn_Func.Text = "선택";
                            btn_Func.Tag = "6";
                        }
                    }
                    break;
                case 6: //테이블 주문 취소
                    if (TableState != clsEnum.Table_State.empty)
                    {
                        if (TableState == clsEnum.Table_State.table_On || TableState == clsEnum.Table_State.table_Assem )
                        {
                            lbl_Amt.Visible = false;
                            btn_Func.Visible = true;
                            btn_Func.Location = new Point(45, 36);

                            btn_Func.Text = "선택";
                            btn_Func.Tag = "7";
                        }
                    }
                    break;
            }
        }


        private void lbl_Amt_Click(object sender, EventArgs e)
        {
            if (frm != null)
                frm.Inter_TableSelected(this);

        }

        private void btn_Func_Click(object sender, EventArgs e)
        {

            string sTag = btn_Func.Tag.ToString();

            if (sTag == "7")
            {
                Clear_Order();
            }


            if (sTag == "3") //테이블 합치기 선택
            {
                btn_Func.ForeColor = Color.Red;
                btn_Func.Text = "취소";
                btn_Func.Tag = "33";
            }
            else if(sTag == "33") //취소 에서 다시 선택으로
            {
                btn_Func.ForeColor = Color.Black;
                btn_Func.Text = "선택";
                btn_Func.Tag = "3";
            }




            if (frm != null)
                frm.Inter_TableFunc(this, sTag);

        }


        public void Insert_Table()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.insert_table";

                cmd_SEL.Parameters.Add("posno", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                cmd_SEL.Parameters.Add("tableno", OracleDbType.Varchar2).Value = sTableNo;
                cmd_SEL.Parameters.Add("xpos", OracleDbType.Int32 ).Value = xPos;
                cmd_SEL.Parameters.Add("ypos", OracleDbType.Int32).Value = yPos;
                
                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);

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
            }
 
        }

        public void Delete_Table()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.delete_tables";

                cmd_SEL.Parameters.Add("posno", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                cmd_SEL.Parameters.Add("tableno", OracleDbType.Varchar2).Value = sTableNo;

                cmd_SEL.BindByName = true;
                clsDBExcute.ExcuteQuery(cmd_SEL);

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
            }

        }



        private double  Insert_Table_Items()
        {   
            List<OracleCommand> lst_Cmd = new List<OracleCommand>();

            OracleCommand oCmd = new OracleCommand();
            double dItemSum = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PROC_POS.delete_table_items";
                oCmd.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                oCmd.Parameters.Add("tablenum", OracleDbType.Varchar2).Value = sTableNo;
                oCmd.BindByName = true;
                clsDBExcute.ExcuteQuery(oCmd);


                int iSeq = 1;
                foreach (clsBill_Items oItem in oLstBill_Items)
                {
                    dItemSum += oItem.dBill_Amt;

                    OracleCommand cmd_SEL = new OracleCommand();
                    cmd_SEL.CommandType = CommandType.StoredProcedure;
                    cmd_SEL.CommandText = "PROC_POS.insert_table_items";
                    cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                    cmd_SEL.Parameters.Add("tablenum", OracleDbType.Varchar2).Value = sTableNo;
                    cmd_SEL.Parameters.Add("seq", OracleDbType.Int32 ).Value = iSeq++;
                    cmd_SEL.Parameters.Add("itemcode", OracleDbType.Varchar2).Value = oItem.item_code;

                    cmd_SEL.Parameters.Add("itemname", OracleDbType.Varchar2).Value = oItem.item_name;
                    cmd_SEL.Parameters.Add("itemcost", OracleDbType.Double ).Value = oItem.item_cost;
                    cmd_SEL.Parameters.Add("itemqty", OracleDbType.Int32 ).Value = oItem.item_qty;
                    cmd_SEL.Parameters.Add("itemvat", OracleDbType.Int16 ).Value = (int)oItem.item_vat;
                    cmd_SEL.Parameters.Add("itemamt", OracleDbType.Double ).Value = oItem.dBill_Amt;

                    cmd_SEL.Parameters.Add("orgtable", OracleDbType.Varchar2).Value = oItem.orgtable_no;

                    
                    cmd_SEL.BindByName = true;
                    lst_Cmd.Add(cmd_SEL);
 
                }

                clsDBExcute.ExcuteQuery(lst_Cmd);

                return dItemSum;
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (oCmd != null)
                    oCmd.Dispose();
                
            }
 
        }


        private void Delete_Table_Items()
        {
            OracleCommand oCmd = new OracleCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PROC_POS.delete_table_items";
                oCmd.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPosOpen.pos_num;
                oCmd.Parameters.Add("tablenum", OracleDbType.Varchar2).Value = sTableNo;
                oCmd.BindByName = true;
                clsDBExcute.ExcuteQuery(oCmd);

                oLstBill_Items.Clear();

            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (oCmd != null)
                    oCmd.Dispose();
            }
        }


        private double Get_Table_Items()
        {
            OracleCommand cmd_SEL = new OracleCommand();
            DataTable dt = null;
            oLstBill_Items = new List<clsBill_Items>();
            double dItemSum = 0;

            try
            {
                cmd_SEL.CommandType = CommandType.StoredProcedure;
                cmd_SEL.CommandText = "PROC_POS.Get_table_items";

                cmd_SEL.Parameters.Add("posnum", OracleDbType.Varchar2).Value = clsPos.pos_num;
                cmd_SEL.Parameters.Add("tablenum", OracleDbType.Varchar2).Value = sTableNo;
                cmd_SEL.Parameters.Add("cur_table", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd_SEL.BindByName = true;

                //이름및 로그인 정보 가져온다.
                dt = clsDBExcute.SelectQuery(cmd_SEL);

                foreach (DataRow dr in dt.Rows)
                {

                    clsBill_Items oItem = new clsBill_Items();
                    oItem.item_code = dr["item_code"].ToString();
                    oItem.item_name  = dr["item_name"].ToString();
                    oItem.item_cost = Convert.ToDouble(dr["item_cost"]);
                    oItem.item_qty = Convert.ToInt32(dr["item_qty"]);
                    oItem.item_vat = (clsEnum.Item_vat)Convert.ToInt32(dr["item_vat"]);
                    oItem.dBill_Amt = Convert.ToDouble(dr["item_amt"]);
                    oItem.orgtable_no = Convert.ToInt32(dr["item_org_table"]);

                    dItemSum += oItem.dBill_Amt;

                    oLstBill_Items.Add(oItem);
                }

                return dItemSum;
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
            }
        }


    }
}

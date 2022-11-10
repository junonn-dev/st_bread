using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using System.Data.OleDb;
using System.Net;
using System.Threading;
using System.Reflection; //현재 실행중 함수명
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Drawing.Printing;

namespace st_bread
{
    public partial class frmStock : Form
    {
        clsMemad oMemad = null;
        OdbcConnection conn = new OdbcConnection();
        OdbcConnection conn_local = new OdbcConnection();
        clsDbCon vDbCon = new clsDbCon();
        clsFile oFile = new clsFile();
        clsMart oMart = new clsMart();

        public frmStock(clsMemad o_Memad, clsMart o_Mart)
        {
            InitializeComponent();
            oMemad = o_Memad;
            oMart = o_Mart;
        }



        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmStock_Load(object sender, EventArgs e)
        {
            txt_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");

            conn = vDbCon.MysqlOdbcConnect(
                clsPrintSet.SettingHT["IP"].ToString(),
                clsPrintSet.SettingHT["DBNAME"].ToString(),
                clsPrintSet.SettingHT["USER"].ToString(),
                clsPrintSet.SettingHT["PASS"].ToString(),
                clsPrintSet.SettingHT["PORT"].ToString()
                );


            conn_local = vDbCon.MysqlOdbcConnect(
                    clsPrintSet.SettingHT["IP2"].ToString(),
                    clsPrintSet.SettingHT["DBNAME2"].ToString(),
                    clsPrintSet.SettingHT["USER2"].ToString(),
                    clsPrintSet.SettingHT["PASS2"].ToString(),
                    clsPrintSet.SettingHT["PORT2"].ToString()
                    );



            if (conn.State == ConnectionState.Open )
            {
                Set_MasterToList();
                
            }
            else
            {
                frmMessage oMessage = new frmMessage(0);
                oMessage.Set_Message(12);
                oMessage.ShowDialog();

            }
            pan_QtyChange.Visible = false;
        }

        private void Set_MasterToList()
        {

            //list view column
            // 0    1   2   3   4
            //번호 상품코드 상품명 수량 그룹코드

            lst_View.Items.Clear();

            try
            {
                int i = 1;
                OdbcCommand oCmd = conn.CreateCommand();
                oCmd.CommandText = " SELECT master_cd,master_nm,master_gr_cd, " +
                        " IFNULL(( SELECT stock_cnt FROM  bread_stock WHERE stock_jum = master_jum AND stock_mart_cd = master_mart_cd AND stock_date = ? AND stock_master_cd = master_cd)  ,0) AS stock_cnt " + 
                        " FROM master  " +
                         " WHERE master_jum = ? AND master_mart_cd = ? AND master_leave = '0' ";

                oCmd.Parameters.Add("@stock_date", SqlDbType.Char).Value = txt_Date.Text;
                oCmd.Parameters.Add("@JUM", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@ID", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                OdbcDataReader reader = oCmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem lstItm = new ListViewItem();
                    lstItm.Text = i++.ToString();
                    
                    lstItm.SubItems.Add(reader["master_cd"].ToString());
                    lstItm.SubItems.Add(reader["master_nm"].ToString());
                    lstItm.SubItems.Add(reader["stock_cnt"].ToString());
                    lstItm.SubItems.Add(reader["master_gr_cd"].ToString());

                    lst_View.Items.Add(lstItm);
                    Application.DoEvents();
                    

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                
            }
        }

        private void lst_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (lst_View.Items.Count == 0)
            //    return;
            //if (lst_View.SelectedItems.Count == 0)
            //    return;


            //uscNum oNum = new uscNum();

            //oNum.SetLabe1Text = lst_View.SelectedItems[0].Text + " 번 " + lst_View.SelectedItems[0].SubItems[2].Text;
            //oNum.SetLabe2Text = "수량을 기재해 주세요. ";
            //oNum.SetLabe3Text = "기존 수량 " + lst_View.SelectedItems[0].SubItems[3].Text + " 개 ";


            //oNum.ClickOK += new ClickAccept(Num_Accept);
            //oNum.ClickCancel += new ClickCancel(Num_Cancel);


            //pan_QtyChange.Controls.Add(oNum);
            //pan_QtyChange.Visible = true;
            //pan_QtyChange.Location = new Point(250, 200);
            //pan_QtyChange.Size = new Size(640, 371);
            //pan_QtyChange.BringToFront();




            //Console.WriteLine(lst_View.SelectedItems[0].SubItems[2].Text);
            //txt_CD.Text = lst_View.SelectedItems[0].SubItems[1].Text;
            //txt_Nm.Text = lst_View.SelectedItems[0].SubItems[2].Text;
            ////txt_Count.Text = "0";
            //txt_Count.SelectAll();


        }


        private void Num_Cancel()
        {
            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;

            if (lst_View.SelectedItems.Count == 0)
                return;
            lst_View.SelectedItems[0].Selected = true;
            lst_View.SelectedItems[0].EnsureVisible();
            lst_View.Select();
        }


        private void Num_Accept(string sNum)
        {
            int iOldCnt = 0;
            int iNewCnt = 0;

            if (lst_View.SelectedItems[0].SubItems[3].Text.Length == 0)
            {
                 iOldCnt = 0;
            }
            else
            {
                 iOldCnt = Int32.Parse(lst_View.SelectedItems[0].SubItems[3].Text);
            }


            if (sNum.Length == 0)
            {
                iNewCnt = 0;
            }
            else
            {
                iNewCnt = Int32.Parse(sNum);
            }

            
            int iTotalCnt = iOldCnt + iNewCnt;

            if (lst_View.SelectedItems.Count == 0)
                return;

            lst_View.SelectedItems[0].SubItems[3].Text = iTotalCnt.ToString();

            pan_QtyChange.Controls.Clear();
            pan_QtyChange.Visible = false;


            lst_View.SelectedItems[0].Selected = true;
            lst_View.SelectedItems[0].EnsureVisible();
            lst_View.Select();
        }

        private void lst_View_Click(object sender, EventArgs e)
        {
            if (lst_View.Items.Count == 0)
                return;
            if (lst_View.SelectedItems.Count == 0)
                return;
            
            uscNum oNum = new uscNum();

            oNum.SetLabe1Text = lst_View.SelectedItems[0].Text + " 번 " + lst_View.SelectedItems[0].SubItems[2].Text;
            oNum.SetLabe2Text = "수량을 기재해 주세요. ";
            oNum.SetLabe3Text = "기존 수량 " + lst_View.SelectedItems[0].SubItems[3].Text + " 개 ";
            
            oNum.ClickOK += new ClickAccept(Num_Accept);
            oNum.ClickCancel += new ClickCancel(Num_Cancel);
            
            pan_QtyChange.Controls.Add(oNum);
            pan_QtyChange.Visible = true;
            pan_QtyChange.Location = new Point(250, 200);
            pan_QtyChange.Size = new Size(640, 371);
            pan_QtyChange.BringToFront();
            
            lst_View.SelectedItems[0].Selected = true;
            lst_View.SelectedItems[0].EnsureVisible();
            lst_View.Select();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

            //기존 매장과 날짜에 있는 자료 지운다.,
            //기존 마감 자료 삭제
            string sSql3 = "DELETE  " +
                       " FROM bread_stock     " +
                       " WHERE stock_jum = ? AND stock_mart_cd = ? AND stock_date = ?";

            OdbcCommand oCmd_DEL = conn.CreateCommand();
            oCmd_DEL.CommandText = sSql3;

            oCmd_DEL.Parameters.Add("@stock_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            oCmd_DEL.Parameters.Add("@stock_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            oCmd_DEL.Parameters.Add("@stock_date", SqlDbType.Date).Value = txt_Date.Text;
            oCmd_DEL.ExecuteNonQuery();

            //다시 저장
            foreach (ListViewItem oItem in lst_View.Items)
            {
                string sSql = "INSERT INTO bread_stock " +
                            "    (" +
                            "    stock_jum," +
                            "    stock_mart_cd, " +
                            "    stock_date, " +
                            "    stock_mem_cd, " +
                            "    stock_seq, " +
                            "    stock_gr_cd, " +
                            "    stock_master_cd, " +
                            "    stock_master_nm, " +
                            "    stock_cnt " +
                            "    )  VALUES (?,?,?,?,?,?,?,?,?); ";


                OdbcCommand oCmd_Ser = conn.CreateCommand();
                oCmd_Ser.CommandText = sSql;

                oCmd_Ser.Parameters.Add("@stock_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd_Ser.Parameters.Add("@stock_mart_cd", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd_Ser.Parameters.Add("@stock_date", SqlDbType.Char).Value = txt_Date.Text;
                oCmd_Ser.Parameters.Add("@stock_mem_cd", SqlDbType.Char).Value = oMemad.memad_id;
                oCmd_Ser.Parameters.Add("@stock_seq", SqlDbType.Char).Value = oItem.Text;
                oCmd_Ser.Parameters.Add("@stock_gr_cd", SqlDbType.Char).Value = oItem.SubItems[4].Text;
                oCmd_Ser.Parameters.Add("@stock_master_cd", SqlDbType.Char).Value = oItem.SubItems[1].Text;
                oCmd_Ser.Parameters.Add("@stock_master_nm", SqlDbType.Char).Value = oItem.SubItems[2].Text;
                oCmd_Ser.Parameters.Add("@stock_cnt", SqlDbType.Char).Value = oItem.SubItems[3].Text;
                oCmd_Ser.ExecuteNonQuery();
 
            }


            frmMessage oMessage = new frmMessage(1);
            oMessage.Set_Message(11);
            oMessage.ShowDialog();

            Set_MasterToList();
        }


        
    }
}

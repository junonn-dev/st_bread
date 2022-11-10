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
    public partial class frmStartSale : Form
    {
        clsMemad oMemad = null;
        OdbcConnection conn = new OdbcConnection();
        OdbcConnection conn_local = new OdbcConnection();
        clsDbCon vDbCon = new clsDbCon();
        clsFile oFile = new clsFile();
        clsMart oMart = new clsMart();
        double dtot = 0;


        public frmStartSale(clsMemad o_Memad, clsMart o_Mart)
        {
            InitializeComponent();

            oMemad = o_Memad;
            oMart = o_Mart;
        }


        private void frmStartSale_Load(object sender, EventArgs e)
        {
            txt_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");

            conn_local = vDbCon.MysqlOdbcConnect(
                clsPrintSet.SettingHT["IP2"].ToString(),
                clsPrintSet.SettingHT["DBNAME2"].ToString(),
                clsPrintSet.SettingHT["USER2"].ToString(),
                clsPrintSet.SettingHT["PASS2"].ToString(),
                clsPrintSet.SettingHT["PORT2"].ToString()
                );

            Set_KickState();
        }


        private void btn_Exit_Click(object sender, EventArgs e)
        {



            this.Close();
        }











        #region key event

        private void Check_Sum()
        {
            double dSum = 0;

            if (lbl_50Thou.Text.Length > 0)
            {
                dSum += double.Parse(lbl_50Thou.Text);
            }

            if (lbl_10Thou.Text.Length > 0)
            {
                dSum += double.Parse(lbl_10Thou.Text);
            }

            if (lbl_5Thou.Text.Length > 0)
            {
                dSum += double.Parse(lbl_5Thou.Text);
            }

            if (lbl_1Thou.Text.Length > 0)
            {
                dSum += double.Parse(lbl_1Thou.Text);
            }
            if (lbl_5Hund.Text.Length > 0)
            {
                dSum += double.Parse(lbl_5Hund.Text);
            }
            if (lbl_1Hund.Text.Length > 0)
            {
                dSum += double.Parse(lbl_1Hund.Text);
            }
            if (lbl_50.Text.Length > 0)
            {
                dSum += double.Parse(lbl_50.Text);
            }
            if (lbl_10.Text.Length > 0)
            {
                dSum += double.Parse(lbl_10.Text);
            }
            dtot = dSum;

            lbl_Tot.Text = String.Format("{0:#,##0}", dSum);


        }
        private void txt_50Thou_TextChanged(object sender, EventArgs e)
        {
            if (txt_50Thou.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_50Thou.Text) * 50000;
                lbl_50Thou.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }
        }
        private void txt_10Thou_TextChanged(object sender, EventArgs e)
        {
            if (txt_10Thou.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_10Thou.Text) * 10000;
                lbl_10Thou.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }
        }

        private void txt_5Thou_TextChanged(object sender, EventArgs e)
        {
            if (txt_5Thou.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_5Thou.Text) * 5000;
                lbl_5Thou.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }

        }

        private void txt_1Thou_TextChanged(object sender, EventArgs e)
        {
            if (txt_1Thou.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_1Thou.Text) * 1000;
                lbl_1Thou.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }
        }

        private void txt_5Hund_TextChanged(object sender, EventArgs e)
        {
            if (txt_5Hund.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_5Hund.Text) * 500;
                lbl_5Hund.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }

        }

        private void txt_1Hund_TextChanged(object sender, EventArgs e)
        {
            if (txt_1Hund.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_1Hund.Text) * 100;
                lbl_1Hund.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }

        }

        private void txt_50_TextChanged(object sender, EventArgs e)
        {
            if (txt_50.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_50.Text) * 50;
                lbl_50.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }

        }

        private void txt_10_TextChanged(object sender, EventArgs e)
        {
            if (txt_10.Text.Length > 0)
            {
                double dAmt = double.Parse(txt_10.Text) * 10;
                lbl_10.Text = String.Format("{0:#,##0}", dAmt);
                Check_Sum();
            }


        }

        private void txt_50Thou_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }





        private void txt_10Thou_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        private void txt_5Thou_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        private void txt_1Thou_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        private void txt_5Hund_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        private void txt_1Hund_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        private void txt_50_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        private void txt_10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }   
        }

        #endregion

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //이미 있는지 체크
            OdbcCommand oReadCmd = conn_local.CreateCommand();
            oReadCmd.CommandText = " SELECT * FROM bread_Kick  " +
                        " WHERE kick_jum = ? AND kick_mart = ? AND kick_pos = ? AND kick_date = ?";

            oReadCmd.Parameters.Add("@kick_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            oReadCmd.Parameters.Add("@kick_mart", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            oReadCmd.Parameters.Add("@kick_pos", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
            oReadCmd.Parameters.Add("@kick_date", SqlDbType.Char).Value = txt_Date.Text;

            OdbcDataReader reader = oReadCmd.ExecuteReader();
            if (reader.Read())
            { }
            else
            {


                string gsSql = "REPLACE INTO bread_Kick " +
                                     "    (" +
                                     "    kick_jum," +
                                     "    kick_mart," +
                                     "    kick_Pos, " +
                                     "    kick_date, " +
                                     "    kick_50Thou, " +
                                     "    Kick_10Thou, " +
                                     "    Kick_5Thou, " +
                                     "    Kick_1Thou, " +
                                     "    Kick_5Hund, " +
                                     "    Kick_1Hund, " +
                                     "    Kick_50, " +
                                     "    Kick_10, " +
                                     "    Kick_total, " +
                                     "    Kick_RealDate, " +
                                     "    Kick_SaleState" +
                                     "    )  VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); ";


                OdbcCommand oCmd = conn_local.CreateCommand();
                oCmd.CommandText = gsSql;

                oCmd.Parameters.Add("@gd_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@kick_mart", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd.Parameters.Add("@kick_Pos", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
                oCmd.Parameters.Add("@kick_date", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                oCmd.Parameters.Add("@kick_50Thou", SqlDbType.Int).Value = txt_50Thou.Text == "" ? "0" : txt_50Thou.Text;
                oCmd.Parameters.Add("@Kick_10Thou", SqlDbType.Int).Value = txt_10Thou.Text == "" ? "0" : txt_10Thou.Text;
                oCmd.Parameters.Add("@Kick_5Thou", SqlDbType.Int).Value = txt_5Thou.Text == "" ? "0" : txt_5Thou.Text;
                oCmd.Parameters.Add("@Kick_1Thou", SqlDbType.Int).Value = txt_1Thou.Text == "" ? "0" : txt_1Thou.Text;
                oCmd.Parameters.Add("@Kick_5Hund", SqlDbType.Int).Value = txt_5Hund.Text == "" ? "0" : txt_5Hund.Text;
                oCmd.Parameters.Add("@Kick_1Hund", SqlDbType.Int).Value = txt_1Hund.Text == "" ? "0" : txt_1Hund.Text;
                oCmd.Parameters.Add("@Kick_50", SqlDbType.Int).Value = txt_50.Text == "" ? "0" : txt_50.Text;
                oCmd.Parameters.Add("@Kick_10", SqlDbType.Int).Value = txt_10.Text == "" ? "0" : txt_10.Text;
                oCmd.Parameters.Add("@Kick_total", SqlDbType.Int).Value = dtot;

                oCmd.Parameters.Add("@Kick_RealDate", SqlDbType.Date).Value = txt_Date.Text;
                oCmd.Parameters.Add("@Kick_SaleState", SqlDbType.Int).Value = 0;

                oCmd.ExecuteNonQuery();
            }

            reader.Close();


            Print_KickState();

            this.Close();

        }


        private void Set_KickState()
        {

            OdbcCommand oCmd = conn_local.CreateCommand();
            oCmd.CommandText = " SELECT * FROM bread_Kick  " +
                        " WHERE kick_jum = ? AND kick_mart = ? AND kick_pos = ? AND kick_date = ?";



            oCmd.Parameters.Add("@kick_jum", SqlDbType.Char).Value = oMemad.memad_jum;
            oCmd.Parameters.Add("@kick_mart", SqlDbType.Char).Value = oMemad.memad_mart_cd;
            oCmd.Parameters.Add("@kick_pos", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
            oCmd.Parameters.Add("@kick_date", SqlDbType.Char).Value = txt_Date.Text;
            

            OdbcDataReader reader = oCmd.ExecuteReader();
            if (reader.Read())
            {
                txt_50Thou.Text = reader["kick_50Thou"].ToString();
                txt_10Thou.Text = reader["Kick_10Thou"].ToString();
                txt_5Thou.Text = reader["Kick_5Thou"].ToString();
                txt_1Thou.Text = reader["Kick_1Thou"].ToString();
                txt_5Hund.Text = reader["Kick_5Hund"].ToString();
                txt_1Hund.Text = reader["Kick_1Hund"].ToString();
                txt_50.Text = reader["Kick_50"].ToString();
                txt_10.Text = reader["Kick_10"].ToString();

            }
            else
            {
            
            }
            reader.Close();

            Check_Sum();
 
        }


        private void Print_KickState()
        {
            SerialPort port = new SerialPort(clsPrintSet.SettingHT["COMP"].ToString(), Int32.Parse(clsPrintSet.SettingHT["SPEED"].ToString()), Parity.None, 8, StopBits.One);

            int iEmpty = Int32.Parse(clsPrintSet.SettingHT["EMPTY"].ToString());
            int iSep = Int32.Parse(clsPrintSet.SettingHT["SEP"].ToString());
            //string COMMAND = ESC + "@" + GS + "V" + (char)1;
            try
            {

                port.Encoding = Encoding.Default;
                port.Open();

                port.WriteLine(clsPrintSet.Cmd_DrawOpen());
                port.WriteLine(clsPrintSet.Cmd_Close());


                port.WriteLine(clsPrintSet.Cmd_Font1());
                port.WriteLine(string.Empty.PadLeft(5, ' ') + "시 재 금" + string.Empty.PadLeft(20, ' '));

                
                port.WriteLine("매  장  명 : " + oMart.sMart_nm);
                port.WriteLine("영업 일시 : " + txt_Date.Text);
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));

                port.WriteLine("담 당 자 : " + oMemad.memad_nm);

                port.WriteLine(clsPrintSet.Cmd_Close());

                OdbcCommand oCmd = conn_local.CreateCommand();
                oCmd.CommandText = "SELECT *  " +
                           " FROM bread_kick     " +
                           " WHERE kick_jum = ? AND kick_mart = ? AND kick_pos = ? AND  kick_date = ? ";
                oCmd.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_jum;
                oCmd.Parameters.Add("@salec_jum", SqlDbType.Char).Value = oMemad.memad_mart_cd;
                oCmd.Parameters.Add("@salec_mart_cd", SqlDbType.Char).Value = clsPrintSet.SettingHT["POSNO"].ToString();
                oCmd.Parameters.Add("@salec_date", SqlDbType.Date).Value = txt_Date.Text;

                OdbcDataReader reader = oCmd.ExecuteReader();


                if (reader.Read())
                {  

                    double dAmt = double.Parse(reader["Kick_50Thou"].ToString()) * 50000;
                    port.WriteLine("오만원권 : " + reader["Kick_50Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) +  String.Format("{0:#,##0}", dAmt));


                    dAmt = double.Parse(reader["Kick_10Thou"].ToString()) * 10000;
                    port.WriteLine("만 원 권 : " + reader["Kick_10Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));


                    dAmt = double.Parse(reader["Kick_5Thou"].ToString()) * 5000;
                    port.WriteLine("오천원권 : " + reader["Kick_5Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

                    dAmt = double.Parse(reader["Kick_1Thou"].ToString()) * 1000;
                    port.WriteLine("천 원 권 : " + reader["Kick_1Thou"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

                    dAmt = double.Parse(reader["Kick_5Hund"].ToString()) * 500;
                    port.WriteLine("오백원권 : " + reader["Kick_5Hund"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

                    dAmt = double.Parse(reader["Kick_1Hund"].ToString()) * 100;
                    port.WriteLine("백 원 권 : " + reader["Kick_1Hund"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));
                    dAmt = double.Parse(reader["Kick_50"].ToString()) * 50;
                    port.WriteLine("오십원권 : " + reader["Kick_50"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));

                    dAmt = double.Parse(reader["Kick_10"].ToString()) * 10;
                    port.WriteLine("십 원 권 : " + reader["Kick_10"].ToString().PadRight(3) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(dAmt.ToString())) + String.Format("{0:#,##0}", dAmt));



                    port.WriteLine(string.Empty.PadLeft(iSep, '='));

                    port.WriteLine("합     계 : " + "".PadRight(5) + "".PadRight(iEmpty - Encoding.Default.GetByteCount(reader["Kick_total"].ToString())) + string.Format("{0:#,##0}", Int32.Parse(reader["Kick_total"].ToString())));

                }

                reader.Close();

                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(clsPrintSet.Cmd_Cutting());
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));
                port.WriteLine(string.Empty.PadLeft(iEmpty, ' '));

            }
            catch (Exception ex)
            {
                clsLog.WriteErrLog(MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());

            }
            finally
            {
                if (port.IsOpen == true)
                    port.Close();
            }

 
        }

      
    }
}

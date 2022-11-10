using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsDBExcute
    {



        public static int ExcuteQuery(OracleCommand oCmd)
        {
            clsOracleConnector OraCon = new clsOracleConnector();
            OracleConnection oCon = OraCon.DB_Connect();
            
            OracleTransaction oTran = oCon.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                if (oCon.State == System.Data.ConnectionState.Open)
                {
                    oCmd.Connection = oCon;
                    oCmd.ExecuteNonQuery();
                    oTran.Commit();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                oTran.Rollback();
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (OraCon != null)
                    OraCon.Dispose();

                if (oCon != null)
                {
                    oCon.Close();
                    oCon.Dispose();
                }
                if (oTran != null)
                    oTran.Dispose();
            }
        }


        public static DataTable SelectQuery(OracleCommand oCmd)
        {
            clsOracleConnector OraCon = new clsOracleConnector();
            OracleConnection oCon = null;

            OracleDataAdapter oraAdapter = new OracleDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                oCon = OraCon.DB_Connect();

                if (oCon.State == System.Data.ConnectionState.Open)
                {

                    oCmd.Connection = oCon;

                    oraAdapter = new OracleDataAdapter(oCmd);

                    oraAdapter.Fill(dt);
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (OraCon != null)
                    OraCon.Dispose();

                if (oCon != null)
                {
                    oCon.Close();
                    oCon.Dispose();
                }
            }
        }

        public static DataSet SelectQuery_SET(OracleCommand oCmd)
        {
            clsOracleConnector OraCon = new clsOracleConnector();
            OracleConnection oCon = null;

            OracleDataAdapter oraAdapter = new OracleDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                oCon = OraCon.DB_Connect();

                if (oCon.State == System.Data.ConnectionState.Open)
                {

                    oCmd.Connection = oCon;

                    oraAdapter = new OracleDataAdapter(oCmd);

                    oraAdapter.Fill(ds);
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (OraCon != null)
                    OraCon.Dispose();

                if (oCon != null)
                {
                    oCon.Close();
                    oCon.Dispose();
                }
            }
        }

        public static int ExcuteQuery(List<OracleCommand> oLstCmd)
        {
            clsOracleConnector OraCon = new clsOracleConnector();
            OracleConnection oCon = OraCon.DB_Connect();

            OracleTransaction oTran = oCon.BeginTransaction(IsolationLevel.ReadCommitted );

            try
            {
                if (oCon.State == System.Data.ConnectionState.Open)
                {

                    foreach (OracleCommand oCmd in oLstCmd)
                    {
                        if (oCmd != null)
                        {
                            oCmd.Connection = oCon;
                            oCmd.Transaction = oTran;
                            oCmd.ExecuteNonQuery();
                        }
                    }

                    oTran.Commit();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                oTran.Rollback();
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (OraCon != null)
                    OraCon.Dispose();
                
                if (oLstCmd != null)
                    oLstCmd = null;

                if (oCon != null)
                {
                    oCon.Close();
                    oCon.Dispose();
                }

                if (oTran != null)
                    oTran.Dispose();
            }
        }


        public static int ExcuteScalar(OracleCommand oCmd,int outPoint)
        {
            clsOracleConnector OraCon = new clsOracleConnector();
            OracleConnection oCon = OraCon.DB_Connect();

            OracleTransaction oTran = oCon.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                if (oCon.State == System.Data.ConnectionState.Open)
                {
                    oCmd.Connection = oCon;
                    oCmd.ExecuteNonQuery();

                    //Console.WriteLine(oCmd.Parameters["out_seq"].Value);

                    int iRet = clsSetting.Let_Int( oCmd.Parameters[outPoint].Value.ToString());
                    oTran.Commit();
                    return iRet;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                oTran.Rollback();
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {
                if (OraCon != null)
                    OraCon.Dispose();

                if (oCon != null)
                {
                    oCon.Close();
                    oCon.Dispose();
                }
                if (oTran != null)
                    oTran.Dispose();
            }
        }

    }
}

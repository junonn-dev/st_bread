using System;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace st_bread
{
    public class clsDBUtil
    {

        public static string toString(DataRow dr ,string sColumn,string sDefaultValue)
        {
            string sResult = sDefaultValue;

            if(dr.Table.Columns.Contains(sColumn))
            {
                object value = dr[sColumn];
                if((value is DBNull) == false && value != null  )
                {
                    sResult = value.ToString();
                }
            }
            sResult = sResult.Trim();
            return sResult;
        }

        public static string toString(DataRow row, string sColumnName)
        {
            return toString(row, sColumnName, string.Empty);
        }

        public static int toInteger(DataRow row, string sColumnName, int nDefaultValue)
        {
            int nResult = nDefaultValue;
            if (row.Table.Columns.Contains(sColumnName))
            {
                object value = row[sColumnName];
                if ((value is DBNull) == false && value != null)
                {
                    string sValue = value.ToString();
                    if (int.TryParse(sValue, out nResult) == false)
                        nResult = nDefaultValue;
                }
            }
            return nResult;
        }
        public static int toInteger(DataRow row, string sColumnName)
        {
            return toInteger(row, sColumnName, 0);
        }

        public static double toDouble(DataRow row, string sColumnName, double dDefaultValue)
        {
            double dResult = dDefaultValue;
            if (row.Table.Columns.Contains(sColumnName))
            {
                object value = row[sColumnName];
                if ((value is DBNull) == false && value != null)
                {
                    string sValue = value.ToString();
                    if (double.TryParse(sValue, out dResult) == false)
                        dResult = dDefaultValue;
                }
            }
            return dResult;
        }
        public static double toDouble(DataRow row, string sColumnName)
        {
            return toDouble(row, sColumnName, 0);
        }

        public static DateTime toDateTime(DataRow row, string sColumnName, DateTime defaultTime)
        {
            DateTime result = defaultTime;

            if (row.Table.Columns.Contains(sColumnName))
            {
                object value = row[sColumnName];
                if ((value is DBNull) == false && value != null)
                {
                    try
                    {
                        result = (DateTime)value;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            }
            return result;
        }
        public static DateTime toDateTime(DataRow row, string sColumnName)
        {
            return toDateTime(row, sColumnName, DateTime.MinValue);
        }



        public static string toString(OracleCommand oCmd, string sColumn, string sDefaultValue)
        {
            string sResult = sDefaultValue;

            if (oCmd.Parameters.Contains(sColumn))
            {
                object value = oCmd.Parameters[sColumn].Value;

                if ((value is DBNull) == false && value != null)
                {
                    sResult = value.ToString();
                }
            }
            sResult = sResult.Trim();
            return sResult;
        }
        public static string toString(OracleCommand oCmd, string sColumnName)
        {
            return toString(oCmd, sColumnName, string.Empty);
        }

        public static int toInteger(OracleCommand oCmd, string sColumnName, int nDefaultValue)
        {
            int nResult = nDefaultValue;
            if (oCmd.Parameters.Contains(sColumnName))
            {
                object value = oCmd.Parameters[sColumnName].Value;

                if ((value is DBNull) == false && value != null)
                {
                    string sValue = value.ToString();
                    if (int.TryParse(sValue, out nResult) == false)
                        nResult = nDefaultValue;
                }
            }
            return nResult;
        }
        public static int toInteger(OracleCommand oCmd, string sColumnName)
        {
            return toInteger(oCmd, sColumnName, 0);
        }


        public static double toDouble(OracleCommand oCmd, string sColumnName, double dDefaultValue)
        {
            double dResult = dDefaultValue;
            if (oCmd.Parameters.Contains(sColumnName))
            {
                object value = oCmd.Parameters[sColumnName].Value;
                if ((value is DBNull) == false && value != null)
                {
                    string sValue = value.ToString();
                    if (double.TryParse(sValue, out dResult) == false)
                        dResult = dDefaultValue;
                }
            }
            return dResult;
        }
        public static double toDouble(OracleCommand oCmd, string sColumnName)
        {
            return toDouble(oCmd, sColumnName, 0);
        }

    }
}

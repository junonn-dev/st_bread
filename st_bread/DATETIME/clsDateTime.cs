using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    class clsDateTime
    {
        /// <summary>
        /// 시작 날짜 및 시간 1970년 1월 1일 00:00:00
        /// </summary>
        static DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// 날짜 를 받아오면 숫자로 변환
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        public static int Set_Time(DateTime _dt)
        {
            TimeSpan dateDiff = _dt - dt;

            double TotalSecond = Math.Truncate(dateDiff.TotalSeconds);

            return clsSetting.Let_Int(TotalSecond.ToString());

        }

        /// <summary>
        /// 날짜를 문자로 받아오면 날짜형으로 변환 후 숫자로 변환
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public static int Set_Time(string sDate)
        {
            DateTime _dt;

            if (sDate.Length == 12)
            {
                _dt = DateTime.ParseExact(sDate, "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (sDate.Length == 6)
            {
                _dt = DateTime.ParseExact(string.Format("{0}000000",sDate), "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (sDate.Length == 8)
            {
                _dt = DateTime.ParseExact(string.Format("{0}000000", sDate), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                _dt = DateTime.Now;
            }


            //DateTime _dt = DateTime.Parse(sDate);

            TimeSpan dateDiff = _dt - dt;

            double TotalSecond = Math.Truncate(dateDiff.TotalSeconds);

            return clsSetting.Let_Int(TotalSecond.ToString());
        }
        
        /// <summary>
        /// 숫자를 날짜형으로 변환
        /// </summary>
        /// <param name="Interval"></param>
        /// <returns></returns>
        public static DateTime Get_Time(int Interval)
        {
            return dt.AddSeconds(Interval);
        }
        
        /// <summary>
        /// 숫자가 문자로 된경우 보통 DB에서 받아오는 경우 사용
        /// </summary>
        /// <param name="Interval"></param>
        /// <returns></returns>
        public static DateTime Get_Time(string  Interval)
        {
            int t = clsSetting.Let_Int(Interval);
            return dt.AddSeconds(t);

        }
        


        /// <summary>
        /// 두 날짜 사이의 간격이 몇일인지 표시 
        /// </summary>
        /// <param name="iInterval"></param>
        /// <returns></returns>
        public static string Get_DayInterval(int iInterval)
        {
            if (iInterval < 0)
                return "0";

            string sDays = string.Empty;

            


            double dInterval = Convert.ToDouble(iInterval);
            double dDay = 86400;


            double dTemp = dInterval / dDay;

            sDays = Math.Ceiling(dTemp).ToString();
            return sDays;
        }


        /// <summary>
        /// 현재 날짜의 끝시간 숫자로
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int EndOfDay(DateTime date)
        {
            DateTime dt = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
            return Set_Time(dt);

        }

        public static int EndOfDay(int iDate)
        {
            DateTime dTemp = clsDateTime.Get_Time(iDate);

            DateTime dt = new DateTime(dTemp.Year, dTemp.Month, dTemp.Day, 23, 59, 59, 999);
            return Set_Time(dt);

        }



        /// <summary>
        /// 현재 날짜의 시작 시간 숫자로
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int StartOfDay(DateTime date)
        {
            return Set_Time(new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0));
        }


        

    }
}

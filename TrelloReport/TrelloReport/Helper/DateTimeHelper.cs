using System;

namespace TrelloReport.Helper
{
    /// <summary>
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Calculate end date with interval
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="interval">Report interval(eg: weekly, daily)</param>
        /// <returns>Calculated end date</returns>
        public static DateTime GetEndDate(DateTime startDate, string interval)
        {
            var endDate = DateTime.Now.Date.AddDays(1).AddMinutes(-1);
            if (interval == "weekly")
            {
                //  need last day 23h59m time  
                endDate = startDate.Date.AddDays(7).AddMinutes(-1);
            }
            else if (interval == "daily")
            {
                endDate = startDate.Date.AddDays(1).AddMinutes(-1);
            }
            else if (interval == "actually")
            {
                // do nothing, is setted
            }

            return endDate;
        }

        /// <summary>
        /// Calculate start date
        /// </summary>
        /// <param name="startdate">input start date</param>
        /// <param name="interval">Report interval</param>
        /// <returns></returns>
        public static DateTime GetStartDate(string startdate, string interval)
        {
            var startDate = Convert.ToDateTime(startdate).Date;
            if (startDate.Year < DateTime.Now.Year - 1)
            {
                startDate = DateTime.Now.Date;
            }
            if (interval == "actually")
            {
                startDate = DateTime.Now.Date;
            }

            return startDate;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tastee.Shared;

namespace Tastee.Application.Utilities
{
    public class Converters
    {
        #region Timespam
        public static TimeSpan? StringToTimeSpan(string str)
        {
            TimeSpan time;
            if (!TimeSpan.TryParse(str, out time))
            {
                return null;
            }
            return time;
        }

        public static string TimeSpanToString(TimeSpan? ts)
        {
            if (ts == null)
                return String.Empty;
            return ts.Value.ToString(Constants.TIMESPAN_FORMAT);
        }
        #endregion

        #region DateTime
        public static DateTime? StringToDateTime(string str, string formatStr = Constants.DATETIME_FORMAT)
        {
            DateTime date;
            if (!DateTime.TryParseExact(str, formatStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return null;
            }
            return date;
        }

        public static string DateTimeToString(DateTime? date, string formatStr = Constants.DATETIME_FORMAT)
        {
            if (date == null)
                return String.Empty;
            return date.Value.ToString(formatStr, CultureInfo.InvariantCulture);
        }

        public static DateTime? UnixTimeStampToDateTime(ulong? unixTimeStamp, bool zeroIsNull = true)
        {
            if (unixTimeStamp == null || (unixTimeStamp == 0 && zeroIsNull))
                return null;
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp.Value).ToLocalTime();
            return dateTime;
        }

        public static ulong? DateTimeToUnixTimeStamp(DateTime? date)
        {
            if (date == null)
                return null;
            return (ulong)((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        #endregion


        #region Number
        public static int? StringToInteger(string str, int? defaultValue = 0)
        {
            return !String.IsNullOrEmpty(str) ? Convert.ToInt32(str) : defaultValue;
        }
        #endregion
    }
}

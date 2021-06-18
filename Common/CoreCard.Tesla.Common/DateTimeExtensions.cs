using System;

namespace CoreCard.Tesla.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime TryToDateTime(this string datetimeStr, DateTime dateTime = default(DateTime))
        {
            DateTime.TryParse(datetimeStr, out dateTime);
            return dateTime;
        }
        public static double ConvertMinutesToMilliseconds(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertToSpecificTimezone(this DateTime originDateTime, string timezoneId)
        {
            if (!TimeZoneInfo.TryFindSystemTimeZoneById(timezoneId, out TimeZoneInfo? targetTimezoneInfo))
                throw new Exception("Invalid Timezone Id");
            var utcDateTime = originDateTime.ToUniversalTime();
            var targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(originDateTime, targetTimezoneInfo);
            return targetDateTime;
        }

        public static DateTime GetCurrentDateInSpecificTimezone(this DateTime originDateTime, string timezoneId)
        {
            var targetDateTime = originDateTime.ConvertToSpecificTimezone(timezoneId);
            return targetDateTime.Date;
        }

        public static double ToUnixTimeStamp(this DateTime datetime)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timeStamp = (datetime.ToUniversalTime() - startTime).TotalMilliseconds;
            return timeStamp;
        }
    }
}

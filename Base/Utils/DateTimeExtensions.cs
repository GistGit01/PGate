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
    }
}

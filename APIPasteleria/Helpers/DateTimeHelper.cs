using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIPasteleria.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime ToMexicoDateTime(this DateTime dt)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt,"Central Standard Time (Mexico)");
        }
    }
}

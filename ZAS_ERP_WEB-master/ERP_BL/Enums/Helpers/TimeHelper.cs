using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Enums
{
    public static class TimeHelper
    {
        public static DateTime ConvertUtcToPakistaniTime(DateTime utcDateTime)
        {
            // Pakistani Standard Time (PST) is UTC +5 hours
            TimeZoneInfo pakistaniTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, pakistaniTimeZone);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Utils
{
    public class DateTimeUtil
    {
        public static DateTime UtcToBrasilia()
        {
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasilia);
        }

        public static DateTime UtcToBrasilia(DateTime date)
        {
            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, brasilia);
        }
    }
}

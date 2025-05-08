using System.Globalization;

namespace Barnamenevisan.Core.Extensions;

public static class DatetimeExtensions
{
    public static string ToShamsi(this DateTime dateTime)
    {
        PersianCalendar calendar = new PersianCalendar();
        return $"{calendar.GetYear(dateTime)}/{calendar.GetMonth(dateTime):00}/{calendar.GetDayOfMonth(dateTime):00}";
    }
}
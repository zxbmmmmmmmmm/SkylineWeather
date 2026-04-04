namespace FluentWeather.Uwp.Helpers;
internal static class TimeZoneHelper
{
    public static IList<TimeZoneInfo> TimeZones { get; } = TimeZoneInfo.GetSystemTimeZones();

    public static TimeZoneInfo GetTimeZoneFromLocation(double longitude)
    {
        var timeZone = 0;

        var quotient = (int)(longitude / 15);
        var remainder = Math.Abs(longitude % 15);
        if (remainder <= 7.5)
        {
            timeZone = quotient;
        }
        else
        {
            timeZone = quotient + (longitude > 0 ? 1 : -1);
        }

        return TimeZones.FirstOrDefault(p => p.BaseUtcOffset == TimeSpan.FromHours(1) * timeZone);
    }
}
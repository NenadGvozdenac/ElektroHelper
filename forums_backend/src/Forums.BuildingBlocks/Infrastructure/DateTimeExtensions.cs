namespace forums_backend.src.Forums.BuildingBlocks.Infrastructure;

public static class DateTimeExtensions
{
    public static string ToNeo4jDateTime(this DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyyTHH:mm:ssZ");
    }

    public static DateTime FromNeo4jDateTime(this string dateTime)
    {
        if (string.IsNullOrEmpty(dateTime))
        {
            return DateTime.MinValue;
        }

        return DateTime.ParseExact(dateTime, "dd.MM.yyyyTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
    }
}
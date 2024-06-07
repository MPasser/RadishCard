using System;

public static class DateTimeUtils {
    public static bool IsSameDateAs(this DateTime dateTime1, DateTime dateTime2) {
        return (dateTime1.Day == dateTime2.Day) &&
                (dateTime1.Month == dateTime2.Month) &&
                (dateTime1.Year == dateTime2.Year);
    }
}
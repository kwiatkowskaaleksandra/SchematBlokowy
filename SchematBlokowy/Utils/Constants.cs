using System;

namespace SchematBlokowy
{
    public static class SystemUsers
    {
        public const string SystemUserName = "System";
        public const string SystemUserEmail = "system@system.pl";

        public const string UnknownUserName = "Nieautoryzowany";
        public const string UnknownUserEmail = "unknown@system.pl";
    }

    public static class DisplayFormats
    {
        public const string PercentValue = "{0} %";
        public const string Decimal92 = "N2";

        public static string ToDecimal92(this decimal? number)
        {
            if (number.HasValue)
                return number.Value.ToString(Decimal92);
            return string.Empty;
        }
        public static string ToDecimal92(this decimal number, string format = "")
        {
            if (format == string.Empty)
            {
                return number.ToString(Decimal92);
            }
            return string.Format(format, number.ToString(Decimal92));
        }
    }
    #region DateTimeFormats
    public static class DateTimeFormats
    {
        public const string DateFormat = "dd.MM.yyyy";
        public const string MonthFormat = "MM.yyyy";
        public const string MonthNameFormat = "MMMM";
        public const string ShortTimeFormat = "HH:mm";
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";
        public const string ShortDateTimeFormat = "dd.MM.yyyy HH:mm";
        public const string ShortDateFormat = "dd.MM.yyyy";
        public const string IsoDateTimeFormat = "o";

        public static string ToDateStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(DateFormat);
            return string.Empty;
        }

        public static string ToDateStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(DateFormat);
            return string.Empty;
        }

        public static string ToShortTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(ShortTimeFormat);
            return string.Empty;
        }

        public static string ToShortTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(ShortTimeFormat);
            return string.Empty;
        }
        public static string ToDateTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(DateTimeFormat);
            return string.Empty;
        }

        public static string ToDateTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(DateTimeFormat);
            return string.Empty;
        }

        public static string ToShortDateTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(ShortDateTimeFormat);
            return string.Empty;
        }

        public static string ToShortDateTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(ShortDateTimeFormat);
            return string.Empty;
        }
    }
    #endregion
}

using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CCM.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string s)
        {
            s = s ?? string.Empty;
            return s.All(Char.IsDigit);
        }

        public static string CsvEscape(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return "\"\"";
            return value.IndexOfAny(new[] { '\r', '\n', '"', CsvSeparator }) >= 0 ? String.Format("\"{0}\"", value.Replace("\"", "\"\"")) : value;
        }

        public static StringBuilder AddCsvSeparator(this StringBuilder sb)
        {
            sb.Append(CsvSeparator);
            return sb;
        }

        public static StringBuilder AddCsvValue(this StringBuilder sb, string value)
        {
            sb.Append(value.CsvEscape());
            return sb;
        }

        public static StringBuilder AddCsvValue(this StringBuilder sb, int value)
        {
            sb.Append(value);
            return sb;
        }

        public static StringBuilder AddCsvValue(this StringBuilder sb, double value, int decimals = 1, CultureInfo culture = null)
        {
            var cultureInfo = culture ?? CultureInfo.InvariantCulture;
            if (decimals < 1)
            {
                sb.AppendFormat(cultureInfo, "{0}", Math.Round(value, MidpointRounding.ToEven));
                return sb;
            }
            var format = "{0:0." + "#".PadRight(decimals, '#') + "}";
            sb.AppendFormat(cultureInfo, format, value);
            return sb;
        }

        private static char CsvSeparator
        {
            get
            {
                return ';';
            }
        }

        /// <summary>
        /// Returnerar början av strängen fram till angiven sträng
        /// </summary>
        public static string LeftOf(this string s, string untilString)
        {
            s = s ?? string.Empty;
            untilString = untilString ?? string.Empty;

            if (string.IsNullOrEmpty(untilString))
            {
                return s;
            }

            var pos = s.IndexOf(untilString, StringComparison.CurrentCulture);
            return pos > -1 ? s.Substring(0, pos) : s;
        }

        public static string Shorten(this string s, int length)
        {
            s = s ?? string.Empty;
            if (s.Length <= length)
            {
                return s;
            }
            return string.Format("{0}...", s.Substring(0, length - 3));
        }

    }
}
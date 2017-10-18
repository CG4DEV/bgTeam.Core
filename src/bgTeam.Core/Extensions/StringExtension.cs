namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class StringExtension
    {
        public static string ToStr(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return str;
        }

        public static int? ToInt(this string str, int? defValue = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defValue;
            }

            return int.Parse(str);
        }

        public static float? ToFloat(this string str, float? defValue = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defValue;
            }

            return float.Parse(str, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return DateTime.Parse(str);
        }

        public static string FirstLetterToUpper(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("string to upper");
            }

            return $"{str[0].ToString().ToUpper()}{string.Join(string.Empty, str.Skip(1))}";
        }
    }
}

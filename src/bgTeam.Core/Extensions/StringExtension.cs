namespace bgTeam.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Расширения для манипуляции строками
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Возвращает входную строку, если она не пустая, иначе null
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <returns>строка</returns>
        public static string ToStr(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return str;
        }

        /// <summary>
        /// Если передана не пустая строка, то парсит строку в целое число, иначе возвращает число по-умолчанию
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="defValue">Число по-умолчанию</param>
        /// <returns>Целое число</returns>
        public static int? ToInt(this string str, int? defValue = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defValue;
            }

            return int.Parse(str);
        }

        /// <summary>
        /// Если передана не пустая строка, то парсит строку в число с плавающей точкой, иначе возвращает число по-умолчанию
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="defValue">Число по-умолчанию</param>
        /// <returns>Число с плавающей точкой</returns>
        public static float? ToFloat(this string str, float? defValue = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defValue;
            }

            return float.Parse(str, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Если передана не пустая строка, то парсит строку в дату и время
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <returns>Дата и время</returns>
        public static DateTime? ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return DateTime.Parse(str);
        }

        /// <summary>
        /// Если передана не пустая строка, то парсит строку в дату и время по шаблону
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="pattern">Шаблон даты</param>
        /// <returns>Дата и время</returns>
        public static DateTime? ToExactDateTime(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return DateTime.ParseExact(str, pattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Делает первую букву переданной строки заглавной
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <exception cref="ArgumentException">Пустая строка</exception>
        /// <returns>Строка</returns>
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

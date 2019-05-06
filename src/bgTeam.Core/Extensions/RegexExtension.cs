namespace bgTeam.Extensions
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Содержит расширения для строк, связанные с регулярными выражениями
    /// </summary>
    public static class RegexExtension
    {
        /// <summary>
        /// Возвращает подстроку из строки на основе регулярного выражения
        /// </summary>
        /// <param name="str">Входная строка</param>
        /// <param name="pattern">Регулярное выражение</param>
        /// <param name="group">Группа</param>
        /// <returns>Подстрока</returns>
        public static string Regex(this string str, string pattern, string group = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            var match = System.Text.RegularExpressions.Regex.Match(str, pattern, RegexOptions.Compiled);

            if (string.IsNullOrEmpty(group))
            {
                return match.Value;
            }

            return match.Groups[group].Value;
        }

        /// <summary>
        /// Возвращает все (объеденёные) подстроки из строки на основе регулярного выражения
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RegexAll(this string str, string pattern)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            var match = System.Text.RegularExpressions.Regex.Matches(str, pattern, RegexOptions.Compiled);

            var sb = new StringBuilder();
            foreach (Match item in match)
            {
                sb.Append(item.Value);
            }

            return sb.ToString();
        }
    }
}

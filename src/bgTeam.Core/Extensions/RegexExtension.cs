namespace bgTeam.Extensions
{
    using System.Text.RegularExpressions;

    public static class RegexExtension
    {
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
    }
}

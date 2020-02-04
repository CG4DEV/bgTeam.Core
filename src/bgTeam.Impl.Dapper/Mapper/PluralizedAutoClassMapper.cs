namespace DapperExtensions.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Automatically maps an entity to a table using a combination of reflection and naming conventions for keys.
    /// Identical to AutoClassMapper, but attempts to pluralize table names automatically.
    /// Example: Person entity maps to People table.
    /// </summary>
    public class PluralizedAutoClassMapper<T> : AutoClassMapper<T>
        where T : class
    {
        public override void Table(string tableName)
        {
            base.Table(Formatting.Pluralize(tableName));
        }

        // Adapted from: http://mattgrande.wordpress.com/2009/10/28/pluralization-helper-for-c/
        public static class Formatting
        {
            private static readonly IList<string> Unpluralizables = new List<string> { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };

            private static readonly IDictionary<string, string> Pluralizations = new Dictionary<string, string>
            {
                { "(p|P)erson$", "$1eople" },
                { "(o|O)x", "$1xen" },
                { "(c|C)hild", "$1hildren" },
                { "(f|F)oot", "$1eet" },
                { "(t|T)ooth", "$1eeth" },
                { "(g|G)oose", "$1eese" },
                { "(.*)fe?$", "$1ves" },
                { "(.*)man$", "$1men" },
                { "(.+[aeiou]y)$", "$1s" },
                { "(.+[^aeiou])y$", "$1ies" },
                { "(.+z)$", "$1zes" },
                { "([m|l])ouse$", "$1ice" },
                { "(.+)(e|i)x$", @"$1ices" },
                { "(octop|vir)us$", "$1i" },
                { "(.+(s|x|sh|ch))$", @"$1es" },
                { "(.+)", @"$1s" },
            };

            public static string Pluralize(string singular)
            {
                if (Unpluralizables.Contains(singular, StringComparer.InvariantCultureIgnoreCase))
                {
                    return singular;
                }

                var plural = string.Empty;

                foreach (var pluralization in Pluralizations)
                {
                    if (Regex.IsMatch(singular, pluralization.Key))
                    {
                        plural = Regex.Replace(singular, pluralization.Key, pluralization.Value);
                        break;
                    }
                }

                return plural;
            }
        }
    }
}
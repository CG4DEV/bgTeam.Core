namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ArgumentsExtension
    {
        public static T CheckNull<T>(this T arg, string message = null)
        {
            if (typeof(T) == typeof(string) && string.IsNullOrWhiteSpace(arg.ToString()))
            {
                throw new ArgumentNullException(message);
            }

            if (arg == null)
            {
                throw new ArgumentNullException(message);
            }

            return arg;
        }

        public static IEnumerable<T> CheckNullOrEmpty<T>(this IEnumerable<T> arg, string message)
        {
            if (arg == null || !arg.Any())
            {
                throw new ArgumentNullException(message);
            }

            return arg;
        }
    }
}

namespace DapperExtensions.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class QueryFunctions
    {
        /// <summary>
        /// For reflection only.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool Like(string pattern, object member)
        {
            throw new InvalidOperationException("For reflection only!");
        }
    }
}

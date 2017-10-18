namespace Trcont.Ris.DataAccess.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class ParamHelper
    {
        private static readonly Regex _regex = new Regex(@"^[\W\d]{0,}(\s|\.|\?)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string TrimParamName(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                return null;
            }

            return _regex.Replace(paramName, string.Empty);
        }

        /// <summary>
        /// Удаляет символы и номера из начала названия параметра <see cref="Trcont.Ris.Domain.Entity.ServiceParam.NameRus"/>
        /// </summary>
        /// <typeparam name="TSource"><see cref="Trcont.Ris.Domain.Entity.ServiceParam"/></typeparam>
        /// <param name="source">Параметры для обработки</param>
        /// <returns>Список отредактированных параметров</returns>
        public static IEnumerable<TSource> TrimNameRus<TSource>(this IEnumerable<TSource> source)
            where TSource : Trcont.Ris.Domain.Entity.ServiceParam
        {
            foreach (TSource element in source)
            {
                element.NameRus = TrimParamName(element.NameRus);
                yield return element;
            }
        }

        /// <summary>
        /// Удаляет символы и номера из начала названия параметра <see cref="Trcont.Ris.Domain.Common.OrderServiceParamsDto.AttribName"/>
        /// </summary>
        /// <typeparam name="TSource"><see cref="Trcont.Ris.Domain.Common.OrderServiceParamsDto"/></typeparam>
        /// <param name="source">Параметры для обработки</param>
        /// <returns>Список отредактированных параметров</returns>
        public static IEnumerable<TSource> TrimAttribName<TSource>(this IEnumerable<TSource> source)
            where TSource : Trcont.Ris.Domain.Common.OrderServiceParamsDto
        {
            foreach (TSource element in source)
            {
                element.AttribName = TrimParamName(element.AttribName);
                yield return element;
            }
        }
    }
}

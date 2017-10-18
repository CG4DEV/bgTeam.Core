namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnumExtension
    {
        public static T GetAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            return enumValue
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute<T>();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            var elements = source.ToArray();
            for (var i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        public static string GetDescription(this Enum enumValue)
        {
            var description = enumValue.GetAttribute<DescriptionAttribute>();
            if (description != null)
            {
                return description.Description;
            }

            return null;
        }
    }
}

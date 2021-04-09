namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Расширения для перечислений
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Возвращает значение атрибута Т у елемента перечисления
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="enumValue">Элемент перечисления</param>
        /// <returns>Объект атрибута T</returns>
        public static T GetAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            return enumValue
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute<T>();
        }

        /// <summary>
        /// Возвращает значение атрибута Description
        /// </summary>
        /// <param name="enumValue">Элемент перечисления</param>
        /// <returns>Строку описания</returns>
        public static string GetDescription(this Enum enumValue)
        {
            var description = enumValue.GetAttribute<DescriptionAttribute>();
            if (description != null)
            {
                return description.Description;
            }

            return null;
        }

        /// <summary>
        /// Возвращает значение перечисления по значению атрибута Description
        /// </summary>
        /// <typeparam name="T">Перечисление</typeparam>
        /// <param name="description">Значение строки описания</param>
        /// <returns>Значение перечисление</returns>
        public static T GetValueFromDescription<T>(string description)
            where T : Enum
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException($"Not found description: {description} on {typeof(T).Name}", nameof(description));
        }

        /// <summary>
        /// Преобразовывает enum в список типа Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Список Dictionary</returns>
        public static Dictionary<object, string> ToDictionary<T>()
            where T : Enum
        {
            var type = typeof(T);
            return Enum.GetNames(type).Select(x =>
            {
                var field = type.GetField(x);

                return new KeyValuePair<object, string>(
                    Convert.ChangeType(field.GetValue(null), Enum.GetUnderlyingType(type)),
                    field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x);
            }).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

namespace bgTeam.Extensions
{
    using System;
    using System.ComponentModel;
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
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

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

            throw new ArgumentException($"Not found description: {description} on {typeof(T).Name}");
        }
    }
}

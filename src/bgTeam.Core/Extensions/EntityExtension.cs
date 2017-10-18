﻿namespace bgTeam.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class EntityExtension
    {
        private static readonly Type[] _systemTypes = new Type[]
        {
            typeof(byte),
            typeof(byte?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(string),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(bool),
            typeof(bool?),
            typeof(decimal),
            typeof(decimal?),
            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(Guid),
            typeof(Guid?)
        };

        public static void SetProperty(this object entity, string propertyName, object propertyValue)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                if (_systemTypes.Contains(propertyInfo.PropertyType))
                {
                    if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                    {
                        propertyInfo.SetValue(entity, DateTime.ParseExact(propertyValue.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(Guid) || propertyInfo.PropertyType == typeof(Guid?))
                    {
                        propertyInfo.SetValue(entity, Guid.Parse(propertyValue.ToString()), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(entity, Convert.ChangeType(propertyValue, propertyInfo.PropertyType, CultureInfo.InvariantCulture), null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(entity, propertyValue);
                }
            }
            else
            {
                propertyInfo = entity.GetType().GetProperty(propertyName + "List", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    return;
                }

                var value = propertyInfo.GetValue(entity);
                if (value == null)
                {
                    value = CreateListInstance(propertyInfo.PropertyType.GetGenericArguments()[0]);
                    propertyInfo.SetValue(entity, value);
                }

                var listValue = (IList)value;
                listValue.Add(propertyValue);
            }
        }

        private static object CreateListInstance(Type propertyType)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(propertyType);

            return Activator.CreateInstance(constructedListType);
        }

        public static Type GetPropertyType(this object entity, string propertyName)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                return null;
            }

            return propertyInfo.PropertyType;
        }
    }
}

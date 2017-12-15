namespace DapperExtensions.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Maps an entity property to its corresponding column in the database.
    /// </summary>
    public interface IPropertyMap
    {
        string Name { get; }

        string ColumnName { get; }

        bool Ignored { get; }

        bool IsReadOnly { get; }

        KeyType KeyType { get; }

        PropertyInfo PropertyInfo { get; }
    }
}
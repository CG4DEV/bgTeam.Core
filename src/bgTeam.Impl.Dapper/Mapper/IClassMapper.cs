namespace DapperExtensions.Mapper
{
    using System;
    using System.Collections.Generic;

    public interface IClassMapper
    {
        string SchemaName { get; }

        string TableName { get; }

        IList<IPropertyMap> Properties { get; }

        Type EntityType { get; }
    }
}
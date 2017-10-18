namespace DapperExtensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MapToAttribute : Attribute
    {
        public string DatabaseColumn { get; private set; }
        public MapToAttribute(string databaseColumn) { DatabaseColumn = databaseColumn; }
    }
}

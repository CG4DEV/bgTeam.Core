namespace DapperExtensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}

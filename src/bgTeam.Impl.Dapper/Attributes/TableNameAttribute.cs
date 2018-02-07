namespace DapperExtensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public TableNameAttribute(string name)
        {
            Name = name;
        }
    }
}

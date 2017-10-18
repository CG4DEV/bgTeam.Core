namespace DapperExtensions.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PrefixForColumnsAttribute : Attribute
    {
        public string Prefix { get; private set; }
        public PrefixForColumnsAttribute(string prefix)
        {
            Prefix = prefix;
        }
    }
}

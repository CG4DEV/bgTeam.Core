namespace bgTeam.DataAccess.Impl.Dapper
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PrefixForColumnsAttribute : Attribute
    {
        public PrefixForColumnsAttribute(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; private set; }
    }
}

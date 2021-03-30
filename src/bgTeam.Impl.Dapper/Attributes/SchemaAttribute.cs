namespace bgTeam.DataAccess.Impl.Dapper
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SchemaAttribute : Attribute
    {
        public SchemaAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}

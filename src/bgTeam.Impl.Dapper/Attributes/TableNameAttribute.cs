namespace bgTeam.DataAccess.Impl.Dapper
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}

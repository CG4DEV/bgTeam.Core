namespace bgTeam.DataAccess.Impl.Dapper
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {
        public IdentityAttribute()
        {
        }
    }
}

namespace DapperExtensions.Attributes
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

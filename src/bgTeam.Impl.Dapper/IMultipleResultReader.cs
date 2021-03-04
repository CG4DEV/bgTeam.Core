namespace DapperExtensions
{
    using System.Collections.Generic;

    public interface IMultipleResultReader
    {
        IEnumerable<T> Read<T>();
    }
}
namespace bgTeam.DataAccess
{
    using System.Collections.Generic;

    public class PaginatedResult<T>
    {
        public int Total { get; set; }

        public IEnumerable<T> Data { get; set; }

        public static PaginatedResult<T> Create(int total, IEnumerable<T> data)
        {
            return new PaginatedResult<T>
            {
                Total = total,
                Data = data,
            };
        }
    }
}

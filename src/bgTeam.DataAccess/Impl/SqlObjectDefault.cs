namespace bgTeam.DataAccess.Impl
{
    using System;

    public class SqlObjectDefault : ISqlObject
    {
        public SqlObjectDefault(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql");
            }

            Sql = sql;
        }

        public SqlObjectDefault(string sql, object queryParams)
            : this(sql)
        {
            QueryParams = queryParams;
        }

        /// <summary>
        /// Gets sql string
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        /// Gets parameter list
        /// </summary>
        public object QueryParams { get; private set; }
    }
}

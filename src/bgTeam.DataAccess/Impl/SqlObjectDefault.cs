namespace bgTeam.DataAccess.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SqlObjectDefault : ISqlObject
    {
        /// <summary>
        /// Gets sql string
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        /// Gets parameter list
        /// </summary>
        public object QueryParams { get; private set; }

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
    }
}

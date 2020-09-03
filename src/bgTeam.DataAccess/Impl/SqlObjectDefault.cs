namespace bgTeam.DataAccess.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sql = new StringBuilder(Sql);
            IEnumerable<KeyValuePair<string, object>> enumerable = QueryParams as IEnumerable<KeyValuePair<string, object>>;
            enumerable.ToList().ForEach(item =>
            {
                sql.Replace(item.Key, item.Value.ToString());
            });
            return sql.ToString();
        }
    }
}

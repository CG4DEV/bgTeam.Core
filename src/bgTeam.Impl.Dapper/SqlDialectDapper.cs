namespace bgTeam.DataAccess.Impl.Dapper
{
    using bgTeam.Impl.Dapper;
    using DapperExtensions;
    using DapperExtensions.Mapper.Sql;
    using System;
    using System.Collections.Generic;

    public class SqlDialectDapper : DataAccess.ISqlDialect
    {
        public ISqlObject GeneratePagingSql(string query, int page, int pageSize, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query");
            }

            if (DapperHelper.SqlDialect == null)
            {
                throw new InvalidOperationException("Sql dialect is not set");
            }

            var parametersDictionary = parameters != null ? parameters.ToDictionary() : new Dictionary<string, object>();
            var sql = DapperHelper.SqlDialect.GetPagingSql(query, page, pageSize, parametersDictionary);
            return new SqlObjectDefault(sql, parametersDictionary);
        }

        public void Init(SqlDialectEnum dialect)
        {
            // TODO : Возможно стоит разместить в другом месте
            //DapperHelper.IdentityColumn = "Id";

            switch (dialect)
            {
                case SqlDialectEnum.MsSql:
                    DapperHelper.SqlDialect = new SqlServerDialect();
                    break;
                case SqlDialectEnum.Oracle:
                    DapperHelper.SqlDialect = new OracleDialect();
                    break;
                case SqlDialectEnum.PostgreSql:
                    DapperHelper.SqlDialect = new PostgreSqlDialect();
                    break;
                case SqlDialectEnum.MySql:
                    DapperHelper.SqlDialect = new MySqlDialect();
                    break;
                case SqlDialectEnum.Sqlite:
                    DapperHelper.SqlDialect = new SqliteDialect();
                    break;
            }
        }
    }
}

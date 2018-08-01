namespace bgTeam.DataAccess.Impl.Dapper
{
    using DapperExtensions;
    using DapperExtensions.Mapper.Sql;
    using System;
    using System.Collections.Generic;

    public class SqlDialectDapper : DataAccess.ISqlDialect
    {
        public string GeneratePagingSql(string query, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query");
            }

            if (DapperHelper.SqlDialect == null)
            {
                throw new InvalidOperationException("Sql dialect is not set");
            }

            return DapperHelper.SqlDialect.GetPagingSql(query, page, pageSize, new Dictionary<string, object>());
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

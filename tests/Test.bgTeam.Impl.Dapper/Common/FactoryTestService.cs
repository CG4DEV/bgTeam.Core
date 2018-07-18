using bgTeam;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using bgTeam.DataAccess.Impl.Sqlite;
using bgTeam.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.bgTeam.Impl.Dapper.Common
{
    internal class FactoryTestService
    {
        public IAppLogger Logger { get; private set; }        

        public IConnectionFactory ConnectionFactory { get; private set; }

        public string ConnectionString { get; } = "Data Source=testSqlite.db;";

        public FactoryTestService()
        {
            Logger = new AppLoggerDefault();

            var dialect = new SqlDialectDapper();
            dialect.Init(SqlDialectEnum.Sqlite);

            ConnectionFactory = new ConnectionFactorySqlite(Logger, ConnectionString, dialect);
        }
    }
}

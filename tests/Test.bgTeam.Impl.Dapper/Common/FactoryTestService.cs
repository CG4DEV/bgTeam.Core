﻿using bgTeam;
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

        public ISqlDialect Dialect { get; private set; }

        public FactoryTestService()
        {
            Logger = new AppLoggerDefault();

            Dialect = new SqlDialectDapper();
            Dialect.Init(SqlDialectEnum.Sqlite);

            ConnectionFactory = new ConnectionFactorySqlite(Logger, ConnectionString, Dialect);
        }
    }
}

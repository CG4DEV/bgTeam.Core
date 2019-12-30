using bgTeam.DataAccess;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Mapper.Sql;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Test.bgTeam.Impl.Dapper.Tests.Mapper.Sql;

namespace Test.bgTeam.Impl.Dapper.Common
{
    public static class SqlHelper
    {
        public static IClassMapper GetMapper<T>() where T : class
        {
            return new AutoClassMapper<T>();
        }

        public static ISqlGenerator GetSqlGenerator()
        {
            return new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqliteDialect(), null));
        }
    }
}

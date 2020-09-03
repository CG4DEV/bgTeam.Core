using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Mapper.Sql;
using System.Collections.Generic;
using System.Reflection;

namespace bgTeam.Core.Tests.Infrastructure
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

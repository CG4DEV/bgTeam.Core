using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using System;

namespace bgTeam.Core.Tests.Infrastructure
{
    public class SqlLiteFixture : IDisposable
    {
        internal FactoryTestService Factory { get; }
        internal ICrudService CrudService { get; }
        internal IRepository Repository { get; }

        public SqlLiteFixture()
        {
            Factory = new FactoryTestService();
            CrudService = new CrudServiceDapper(Factory.ConnectionFactory);
            CrudService.ExecuteAsync(@"CREATE TABLE 'TestEntity'
            (
                [Id] INTEGER  NOT NULL PRIMARY KEY,
                [Name] TEXT  NULL
            )");
            Repository = new RepositoryDapper(Factory.ConnectionFactory);
        }

        public void Dispose()
        {
            CrudService.ExecuteAsync(@"DROP TABLE 'TestEntity'");
        }
    }
}

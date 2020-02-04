using System;
using System.Reflection;
using Xunit.Sdk;

namespace bgTeam.Core.Tests.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class WithRecreatingTable : BeforeAfterTestAttribute
    {
        private static SqlLiteFixture _fixture = new SqlLiteFixture();

        public override void Before(MethodInfo methodUnderTest)
        {
            _fixture.CrudService.ExecuteAsync(@"CREATE TABLE 'TestEntity'
            (
                [Id] INTEGER  NOT NULL PRIMARY KEY,
                [Name] TEXT  NULL
            )");
        }

        public override void After(MethodInfo methodUnderTest)
        {
            _fixture.CrudService.ExecuteAsync(@"DROP TABLE 'TestEntity'");
        }
    }
}

using System;
using System.Reflection;
using Xunit.Sdk;

namespace bgTeam.Core.Tests.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InsertTestEntities : BeforeAfterTestAttribute
    {
        private int _count;

        public InsertTestEntities(int count = 3)
        {
            _count = count;
        }

        private static SqlLiteFixture _fixture = new SqlLiteFixture();

        public override void Before(MethodInfo methodUnderTest)
        {
            for (int i = 0; i < _count; i++)
            {
                _fixture.CrudService.InsertAsync(new TestEntity { Name = (i + 1).ToString() });
                _fixture.CrudService.InsertAsync(new CompositeKeyEntity { Key1 = 1, Key2 = (i + 1), Name = (i + 1).ToString() });
            }
        }
    }
}
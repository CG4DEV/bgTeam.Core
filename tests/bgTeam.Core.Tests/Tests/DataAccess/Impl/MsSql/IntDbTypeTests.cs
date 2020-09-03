using bgTeam.DataAccess.Impl.MsSql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.MsSql
{
    public class IntDbTypeTests
    {
        [Fact]
        public void AddParameter()
        {
            var someGuidType = new SomeIntType(new[] { 21 });
            var command = new SqlCommand("SELECT * FROM dbo.Orders WHERE ID = @Id;");
            someGuidType.AddParameter(command, "Id");
            Assert.Single(command.Parameters);
        }

        [Fact]
        public void GetParameter()
        {
            var someGuidType = new SomeIntType(new[] { 21 });
            var parameter = someGuidType.GetParameter("Id");
            Assert.NotNull(parameter.Value);
            Assert.Equal("IntegerIdList", parameter.TypeName);
            Assert.Equal("Id", parameter.ParameterName);
        }

        private class SomeIntType : IntDbType
        {
            public SomeIntType(IEnumerable<int> ids)
                : base(ids)
            {
            }
        }
    }
}

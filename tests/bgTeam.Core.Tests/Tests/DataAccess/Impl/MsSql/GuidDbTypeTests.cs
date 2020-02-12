using bgTeam.DataAccess.Impl.MsSql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.MsSql
{
    public class GuidDbTypeTests
    {
        [Fact]
        public void AddParameter()
        {
            var someGuidType = new SomeGuidType(new[] { Guid.NewGuid() });
            var command = new SqlCommand("SELECT * FROM dbo.Orders WHERE ID = @Id;");
            someGuidType.AddParameter(command, "Id");
            Assert.Single(command.Parameters);
        }

        [Fact]
        public void GetParameter()
        {
            var someGuidType = new SomeGuidType(new[] { Guid.NewGuid() });
            var parameter = someGuidType.GetParameter("Id");
            Assert.NotNull(parameter.Value);
            Assert.Equal("GuidIdList", parameter.TypeName);
            Assert.Equal("Id", parameter.ParameterName);
        }
        private class SomeGuidType : GuidDbType
        {
            public SomeGuidType(IEnumerable<Guid> ids)
                : base(ids)
            {
            }
        }
    }
}

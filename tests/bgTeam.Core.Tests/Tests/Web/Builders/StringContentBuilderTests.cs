using bgTeam.Web.Builders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Web.Builders
{
    public class StringContentBuilderTests
    {
        [Fact]
        public async Task Build()
        {
            var stringContentBuilder = new StringContentBuilder();
            var form = stringContentBuilder.Build(new { Name = "John", Numbers = new[] { 12, 16 }, Classes = new[] { new TestEntity() }, Class = new TestEntity() });
            var content = await form.ReadAsStringAsync();
            Assert.Equal("{\"Name\":\"John\",\"Numbers\":[12,16],\"Classes\":[{\"Name\":\"Hey\"}],\"Class\":{\"Name\":\"Hey\"}}", content);
        }

        class TestEntity
        {
            public string Name { get; } = "Hey";
        }
    }
}

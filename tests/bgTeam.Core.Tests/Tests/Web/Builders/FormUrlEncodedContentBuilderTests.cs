﻿using bgTeam.Web.Builders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Web.Builders
{
    public class FormUrlEncodedContentBuilderTests
    {
        [Fact]
        public async Task Build()
        {
            var formUrlEncodedContentBuilder = new FormUrlEncodedContentBuilder();
            var form = formUrlEncodedContentBuilder.Build(new { Name = "John", Numbers = new[] { 12, 16 }, Classes = new[] { new TestEntity() }, Class = new TestEntity() });
            var content = await form.ReadAsStringAsync();
            Assert.Equal("name=John&numbers%5B0%5D=12&numbers%5B1%5D=16&classes%5B0%5D%5Bname%5D=Hey&class%5Bname%5D=Hey", content);
        }

        [Fact]
        public void NestedArrayNotSupported()
        {
            var formUrlEncodedContentBuilder = new FormUrlEncodedContentBuilder();
            Assert.Throws<ArgumentException>(() =>
            {
                formUrlEncodedContentBuilder.Build(new { Numbers = new[] { new[] { 12, 16 } } });
            });
        }

        class TestEntity
        {
            public string Name { get; } = "Hey";
        }
    }
}

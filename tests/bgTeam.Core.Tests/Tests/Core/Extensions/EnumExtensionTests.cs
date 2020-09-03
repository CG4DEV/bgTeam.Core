using System;
using System.ComponentModel;
using bgTeam.Extensions;
using Xunit;

namespace bgTeam.Core.Tests.Core.Extensions
{
    public class EnumExtensionTests
    {
        [Fact]
        public void GetDescription()
        {
            Assert.Equal("Hi", Test.Value1.GetDescription());
            Assert.Equal("Bye", Test.Value2.GetDescription());
            Assert.Null(Test.Value3.GetDescription());
        }

        [Fact]
        public void GetAttribute()
        {
            var attr = Test.Value1.GetAttribute<DescriptionAttribute>();
            Assert.Equal("Hi", attr.Description);
        }

        [Fact]
        public void GetValueFromDescriptionAttribute()
        {
            var value = EnumExtension.GetValueFromDescription<Test>("Bye");
            Assert.Equal(Test.Value2, value);
        }

        [Fact]
        public void GetValueFromDescriptionField()
        {
            var value = EnumExtension.GetValueFromDescription<Test>("Value3");
            Assert.Equal(Test.Value3, value);
        }

        [Fact]
        public void GetValueFromDescriptionShouldThrowExceptionIfThisFieldIsNotExists()
        {
            Assert.Throws<ArgumentException>("description", () =>
            {
                EnumExtension.GetValueFromDescription<Test>("Value4");
            });
        }

        public enum Test
        {
            [Description("Hi")]
            Value1,

            [Description("Bye")]
            Value2,

            Value3
        }
    }
}

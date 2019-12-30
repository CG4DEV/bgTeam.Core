using bgTeam.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.bgTeam.Core.Tests.Extensions
{
    public class EntityExtensionTests
    {
        [Fact]
        public void SetIntegerProperty()
        {
            var testInstance = new TestClass();
            testInstance.SetProperty("IntValue", 20);
            Assert.Equal(20, testInstance.IntValue);
        }

        [Fact]
        public void SetDateTimeProperty()
        {
            var testInstance = new TestClass();
            testInstance.SetProperty("Date", "2018-01-13");
            Assert.Equal(2018, testInstance.Date.Year);
            Assert.Equal(1, testInstance.Date.Month);
            Assert.Equal(13, testInstance.Date.Day);
        }

        [Fact]
        public void SetGuidProperty()
        {
            var testInstance = new TestClass();
            testInstance.SetProperty("Id", "00000000-0000-0000-0000-000000000001");
            Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), testInstance.Id);
        }

        [Fact]
        public void SetListProperty()
        {
            var testInstance = new TestClass();
            testInstance.SetProperty("String", "1");
            testInstance.SetProperty("String", "2");
            Assert.Equal(2, testInstance.StringList.Count);
            Assert.Contains("1", testInstance.StringList);
            Assert.Contains("2", testInstance.StringList);
        }

        [Fact]
        public void SetEnumProperty()
        {
            var testInstance = new TestClass();
            testInstance.SetProperty("Type", Type.Good);
            Assert.Equal(Type.Good, testInstance.Type);
        }

        [Fact]
        public void SetUnexistsProperty()
        {
            var testInstance = new TestClass();
            testInstance.SetProperty("UnexistsProperty", 1);
        }

        [Fact]
        public void GetPropertyType()
        {
            var testInstance = new TestClass();
            Assert.Equal(typeof(Type), testInstance.GetPropertyType("Type"));
        }

        [Fact]
        public void GetPropertyTypeOfUnexistsProperty()
        {
            var testInstance = new TestClass();
            Assert.Null(testInstance.GetPropertyType("UnexistsProperty"));
        }

        private enum Type
        {
            Normal,
            Good
        }

        private class TestClass
        {
            public int IntValue { get; set; }
            public DateTime Date { get; set; }
            public Guid Id { get; set; }
            public List<string> StringList { get; set; }
            public Type Type { get; set; }

        }
    }
}

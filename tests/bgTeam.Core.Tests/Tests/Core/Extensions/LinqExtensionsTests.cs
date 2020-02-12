using System;
using System.Collections.Generic;
using System.Text;
using bgTeam.Extensions;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Extensions
{
    public class MaybeExtensionsTests
    {
        [Fact]
        public void IfLess()
        {
            var list = new List<string> { "hey", "bye" };
            var list2 = new List<string> { };
            var list3 = (List<string>)null;

            var goodAssert = list.IfLess(x => x.Count >= 2);
            Assert.Equal(2, goodAssert.Count);

            Assert.Null(list2.IfLess(x => x.Count >= 2));
            Assert.Null(list3.IfLess(x => x.Count >= 2));
        }

        [Fact]
        public void IfUnless()
        {
            var list = new List<string> { "hey", "bye" };
            var list2 = new List<string> { };
            var list3 = (List<string>)null;

            var goodAssert = list.IfUnless(x => x.Count < 2);
            Assert.Equal(2, goodAssert.Count);

            Assert.Null(list2.IfUnless(x => x.Count < 2));
            Assert.Null(list3.IfUnless(x => x.Count < 2));
        }

        [Fact]
        public void With()
        {
            var list = new List<string> { "hey", "bye" };
            var list2 = new List<string> { };
            Assert.True(list.With(x => x.Count == 2));
            Assert.False(list2.With(x => x.Count == 2));
            Assert.False(((List<string>)null).With(x => x.Count == 2));
        }

        [Fact]
        public void WithForStruct()
        {
            int? number1 = 1;
            int? number2 = null;
            Assert.True(number1.With(x => x == 1));
            Assert.False(number2.With(x => x == 1));
        }

        [Fact]
        public void Return()
        {
            string string1 = "Hey";
            string string2 = null;
            Assert.Equal("Hey+", string1.Return((string str) => str + "+", "Bye"));
            Assert.Equal("Bye", string2.Return((string str) => str + "+", "Bye"));
        }

        [Fact]
        public void ReturnForStruct()
        {
            int? number1 = 1;
            int? number2 = null;
            Assert.Equal(2, number1.Return((int num) => num + 1, 0));
            Assert.Equal(0, number2.Return((int num) => num + 1, 0));
        }

        [Fact]
        public void Do()
        {
            string string1 = "Hey";
            string string2 = null;

            Assert.Throws<Exception>(() => string1.Do(x => throw new Exception()));
            string1.Do(x => Assert.Equal("Hey", x));
            string2.Do(x => throw new Exception());
        }

        [Fact]
        public void DoForStruct()
        {
            int? number1 = 1;
            int? number2 = null;

            Assert.Throws<Exception>(() => number1.Do(x => throw new Exception()));
            number1.Do(x => Assert.Equal(1, x));
            number2.Do(x => throw new Exception());
        }

        [Fact]
        public void DoVal()
        {
            int number1 = 1;
            number1.DoVal(x => Assert.Equal(1, x));
        }

        [Fact]
        public void DoForEach()
        {
            var list = new List<string> { "hey", "bye" };
            StringBuilder result = new StringBuilder();
            list.DoForEach(x => result.Append(x));
            Assert.Equal("heybye", result.ToString());
        }

        [Fact]
        public void DoForEachIfValueIsNull()
        {
            var list = (List<string>)null;
            StringBuilder result = new StringBuilder();
            list.DoForEach(x => result.Append(x));
            Assert.Equal(string.Empty, result.ToString());
        }

        [Fact]
        public void AddNotNull()
        {
            var list = new List<string> { "hey", "bye" };
            list.AddNotNull("hi");
            Assert.Equal(3, list.Count);

            list.AddNotNull(null);
            Assert.Equal(3, list.Count);

            var list2 = (List<string>)null;
            list2.AddNotNull("hi");
            Assert.Null(list2);
        }

        [Fact]
        public void AddNotNullForCollectionStruct()
        {
            var list = new List<int> { 1, 2 };
            list.AddNotNull(3);
            Assert.Equal(3, list.Count);

            list.AddNotNull(null);
            Assert.Equal(3, list.Count);

            var list2 = (List<int>)null;
            list2.AddNotNull(3);
            Assert.Null(list2);
        }
    }
}

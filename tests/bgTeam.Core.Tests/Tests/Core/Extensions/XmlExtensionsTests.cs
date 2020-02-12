using System;
using System.Collections.Generic;
using bgTeam.Extensions;
using System.Xml;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Extensions
{
    public class XmlExtensionsTests
    {
        [Fact]
        public void GetAttributeValue()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book genre=""autobiography"" publicationdate=""1981"" ISBN=""1-861003-11-0""></book>");
            Assert.Equal("autobiography", doc.DocumentElement.GetAttributeValue("genre"));
        }

        [Fact]
        public void GetAttributeValueShouldTrhowsExceptionIfNodeHasNotAttributes()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book></book>");
            Assert.Throws<ArgumentException>("attributeName", () =>
            {
                doc.DocumentElement.GetAttributeValue("genre");
            });
        }

        [Fact]
        public void GetAttributeValueShouldTrhowsExceptionIfNodeHasNotNeededAttribute()
        {
            var doc = new XmlDocument();
            doc.LoadXml(@"<book publicationdate=""1981"" ISBN=""1-861003-11-0""></book>");
            Assert.Throws<ArgumentException>("attributeName", () =>
            {
                doc.DocumentElement.GetAttributeValue("genre");
            });
        }
    }
}

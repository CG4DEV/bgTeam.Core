using bgTeam.Core.Helpers;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace bgTeam.Core.Tests.Core.Impl
{
    public class ConfigHelperTests
    {
        [Fact]
        public void Init()
        {
            var configFolderPath = Path.Combine(Environment.CurrentDirectory, "Configurations/WithSameStructure");
            var configurations = ConfigHelper.Init<InsuranceConfiguration>(configFolderPath);
            Assert.NotEmpty(configurations);
        }

        [Fact]
        public void InitWhenDirectoryHasNotJsonsShouldReturnEmptyList()
        {
            var configFolderPath = Path.Combine(Environment.CurrentDirectory, "Assemblies");
            var configurations = ConfigHelper.Init<InsuranceConfiguration>(configFolderPath);
            Assert.Empty(configurations);
        }

        public class InsuranceConfiguration
        {
            public string ConfigName { get; set; }

            public string ContextType { get; set; }

            public string Description { get; set; }

            public string NameQueue { get; set; }

            public string DateFormatStart { get; set; }

            public int? DateChangeOffsetFrom { get; set; }

            public int? DateChangeOffsetTo { get; set; }

            public string[] Sql { get; set; }
        }

    }
}

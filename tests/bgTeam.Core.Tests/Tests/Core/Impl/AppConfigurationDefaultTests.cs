using bgTeam.Core.Impl;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Core.Impl
{
    public class AppConfigurationDefaultTests
    {
        [Fact]
        public void AppSettingsJsonShouldBeUsedAsDefaultConfigurationFile()
        {
            var config = new AppConfigurationDefault();
            Assert.Equal("Test11", config["Param1"]);
            Assert.Equal("Test12", config["Param2"]);
            Assert.Equal("Test14", config["Param3:Param4"]);
            Assert.Equal("Test15", config["Param3:Param5"]);
            Assert.Null(config["Param16"]);
            Assert.Null(config["Param17"]);
            Assert.Equal("Connect10", config.GetConnectionString("MAINDB"));
        }

        [Fact]
        public void ReadingConfigurationFromNotDefaultFileAndDefaultEnv()
        {
            var config = new AppConfigurationDefault(null, "Configurations/appsettings.test");
            Assert.Equal("Test1", config["Param1"]);
            Assert.Equal("Test2", config["Param2"]);
            Assert.Equal("Test4", config["Param3:Param4"]);
            Assert.Equal("Test5", config["Param3:Param5"]);
            Assert.Null(config["Param6"]);
            Assert.Null(config["Param7"]);
            Assert.Equal("Connect", config.GetConnectionString("MAINDB"));
        }

        [Fact]
        public void ReadingConfigurationFromNotDefaultConfigFileAndNotDefaultEnv()
        {
            var config = new AppConfigurationDefault("Debug", "Configurations/appsettings.test");

            Assert.Equal("Test1", config["Param1"]);
            Assert.Equal("Test2", config["Param2"]);
            Assert.Equal("Test4", config["Param3:Param4"]);
            Assert.Equal("Test5", config["Param3:Param5"]);
            Assert.Equal("Test6", config["Param6"]);
            Assert.Equal("Test7", config["Param7"]);
            Assert.Equal("Connect", config.GetConnectionString("MAINDB"));
        }

        [Fact]
        public void NotExistingFileShouldThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new AppConfigurationDefault(null, "appsettings.none"));
        }

        [Fact]
        public void ReadingConfigurationFromArrayFromNotDefaultConfigFileAndNotDefaultEnv()
        {
            var config = new AppConfigurationDefault("Array", "Configurations/appsettings.test");
            Assert.Equal("Test01", config["Param6:0"]);
            Assert.Equal("Test02", config["Param6:1"]);
            Assert.Equal("Test03", config["Param6:2"]);
            Assert.Null(config["Param6:3"]);
        }

        [Fact]
        public void ReadingConfigurationSection()
        {
            var config = new AppConfigurationDefault(null, "Configurations/appsettings.test");
            var section = config.GetSection("Param8");
            Assert.Null(section["Param4"]);
            Assert.Null(section["Param5"]);

            var section2 = config.GetSection("Param3");
            Assert.Equal("Test4", section2["Param4"]);
            Assert.Equal("Test5", section2["Param5"]);
        }

        [Fact]
        public void ReadingWithAdditionalConfigurations()
        {
            var config = new AppConfigurationDefault(null, "Configurations/appsettings.withAdditionalConfigs");
            Assert.Equal("162342", config["Param4815"]);
        }

        [Fact]
        public void ReadingWithEmptyAdditionalConfigurationsShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new AppConfigurationDefault(null, "Configurations/appsettings.withEmptyConfsPath"));
        }
    }
}

using bgTeam.Core.Impl;
using bgTeam.Extensions;
using bgTeam.Impl.Log4Net;
using System;
using System.IO;
using Xunit;

namespace Test.bgTeam.Core
{
    public class Test_AppConfigurationDefault
    {
        public Test_AppConfigurationDefault()
        {
            
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test1()
        {
            Assert.Throws<ArgumentException>(delegate { new AppConfigurationDefault("appsettings.not"); });
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test2()
        {
            var config = new AppConfigurationDefault("appsettings.test");

            Assert.Equal("Test1", config["Param1"]);
            Assert.Equal("Test2", config["Param2"]);
            Assert.Equal("Test4", config["Param3:Param4"]);
            Assert.Equal("Test5", config["Param3:Param5"]);

            Assert.Null(config["Param6"]);
            Assert.Null(config["Param7"]);

            Assert.Equal("Connect", config.GetConnectionString("MAINDB"));
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test3()
        {
            var config = new AppConfigurationDefault("appsettings.test", "Debug");

            Assert.Equal("Test1", config["Param1"]);
            Assert.Equal("Test2", config["Param2"]);
            Assert.Equal("Test4", config["Param3:Param4"]);
            Assert.Equal("Test5", config["Param3:Param5"]);

            Assert.Equal("Test6", config["Param6"]);
            Assert.Equal("Test7", config["Param7"]);

            Assert.Equal("Connect", config.GetConnectionString("MAINDB"));
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test4()
        {
            Assert.Throws<FileNotFoundException>(delegate { new AppConfigurationDefault("appsettings.throw"); });
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test5()
        {
            var config = new AppConfigurationDefault("appsettings.test", "Array");

            Assert.Equal("Test01", config["Param6:0"]);
            Assert.Equal("Test02", config["Param6:1"]);
            Assert.Equal("Test03", config["Param6:2"]);
            Assert.Null(config["Param6:3"]);
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test6()
        {
            var config = new AppConfigurationDefault("appsettings.test");

            var section = config.GetSection("Param8");

            Assert.Null(section["Param4"]);
            Assert.Null(section["Param5"]);
        }

        [Fact]
        public void Test_AppConfigurationDefault_Test7()
        {
            var config = new AppConfigurationDefault("appsettings.test");

            var section = config.GetSection("Param3");

            Assert.Equal("Test4", section["Param4"]);
            Assert.Equal("Test5", section["Param5"]);
        }
    }
}

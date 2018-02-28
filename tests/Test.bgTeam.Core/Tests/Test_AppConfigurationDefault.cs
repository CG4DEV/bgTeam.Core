using bgTeam.Core.Impl;
using bgTeam.Extensions;
using bgTeam.Impl.Log4Net;
using bgTeam.Infrastructure.Logger;
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

            Assert.Equal(null, config["Param6"]);
            Assert.Equal(null, config["Param7"]);

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

        

        //[Fact]
        //public void Test_ArgumentsExtension_StringEmpty()
        //{
        //    string testString = string.Empty;
        //    Assert.Throws<ArgumentNullException>(() => testString.CheckNull(nameof(testString)));
        //}

        //[Fact]
        //public void Test_ArgumentsExtension_StringNotNull()
        //{
        //    string testString = "some string";
        //    var resultString = testString.CheckNull(nameof(testString));

        //    Assert.Equal(testString, resultString);
        //}

        //[Fact]
        //public void Test_Infrastructure_Serilog_Log()
        //{
        //    var logger = new AppLoggerSerilog();

        //    logger.Info("Info message");
        //    logger.Debug("Debug message");
        //    logger.Error("Error message");
        //    logger.Fatal(new Exception("Fatal message"));
        //    logger.Warning("Warning message");

        //    Assert.NotNull(logger);
        //}

        //[Fact]
        //public void Test_Infrastructure_Log4Net_Log()
        //{
        //    var logger = new AppLoggerLog4Net();

        //    logger.Info("Info message");
        //    logger.Debug("Debug message");
        //    logger.Error("Error message");
        //    logger.Fatal(new Exception("Fatal message"));
        //    logger.Warning("Warning message");

        //    Assert.NotNull(logger);
        //}
    }
}

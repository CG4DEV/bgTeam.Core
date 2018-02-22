using bgTeam.Extensions;
using bgTeam.Impl.Log4Net;
using bgTeam.Infrastructure.Logger;
using System;
using Xunit;

namespace Test.bgTeam.Core
{
    public class UnitTest1
    {
        [Fact]
        public void Test_ArgumentsExtension_StringNull()
        {
            string testString = null;
            Assert.Throws<ArgumentNullException>(() => testString.CheckNull(nameof(testString)));
        }

        [Fact]
        public void Test_ArgumentsExtension_StringEmpty()
        {
            string testString = string.Empty;
            Assert.Throws<ArgumentNullException>(() => testString.CheckNull(nameof(testString)));
        }

        [Fact]
        public void Test_ArgumentsExtension_StringNotNull()
        {
            string testString = "some string";
            var resultString = testString.CheckNull(nameof(testString));

            Assert.Equal(testString, resultString);
        }

        [Fact]
        public void Test_Infrastructure_Serilog_Log()
        {
            var logger = new AppLoggerSerilog();

            logger.Info("Info message");
            logger.Debug("Debug message");
            logger.Error("Error message");
            logger.Fatal(new Exception("Fatal message"));
            logger.Warning("Warning message");

            Assert.NotNull(logger);
        }

        [Fact]
        public void Test_Infrastructure_Log4Net_Log()
        {
            var logger = new AppLoggerLog4Net();

            logger.Info("Info message");
            logger.Debug("Debug message");
            logger.Error("Error message");
            logger.Fatal(new Exception("Fatal message"));
            logger.Warning("Warning message");

            Assert.NotNull(logger);
        }
    }
}

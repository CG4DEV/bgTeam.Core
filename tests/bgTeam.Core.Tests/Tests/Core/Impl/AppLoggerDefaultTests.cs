using bgTeam.Impl;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Impl
{
    public class AppLoggerDefaultTests
    {
        [Fact]
        public void Debug()
        {
            var logger = new AppLoggerDefault();
            logger.Debug("hi");
        }

        [Fact]
        public void Info()
        {
            var logger = new AppLoggerDefault();
            logger.Info("hi");
        }

        [Fact]
        public void Warning()
        {
            var logger = new AppLoggerDefault();
            logger.Warning("hi");
        }

        [Fact]
        public void Error()
        {
            var logger = new AppLoggerDefault();
            logger.Error("hi");
        }

        [Fact]
        public void ErrorException()
        {
            var logger = new AppLoggerDefault();
            logger.Error(new Exception());
        }

        [Fact]
        public void ErrorAggregateException()
        {
            var logger = new AppLoggerDefault();
            logger.Error(new AggregateException());
        }

        [Fact]
        public void FatalException()
        {
            var logger = new AppLoggerDefault();
            logger.Fatal(new Exception());
        }

        [Fact]
        public void FatalAggregateException()
        {
            var logger = new AppLoggerDefault();
            logger.Fatal(new AggregateException());
        }
    }
}

using bgTeam.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Impl
{
    public class EmptyAppLoggerTests
    {
        [Fact]
        public void Debug()
        {
            var logger = new EmptyAppLogger();
            logger.Debug("hi");
        }

        [Fact]
        public void Info()
        {
            var logger = new EmptyAppLogger();
            logger.Info("hi");
        }

        [Fact]
        public void Warning()
        {
            var logger = new EmptyAppLogger();
            logger.Warning("hi");
        }

        [Fact]
        public void Error()
        {
            var logger = new EmptyAppLogger();
            logger.Error("hi");
        }

        [Fact]
        public void ErrorException()
        {
            var logger = new EmptyAppLogger();
            logger.Error(new Exception());
        }

        [Fact]
        public void ErrorAggregateException()
        {
            var logger = new EmptyAppLogger();
            logger.Error(new AggregateException());
        }

        [Fact]
        public void FatalException()
        {
            var logger = new EmptyAppLogger();
            logger.Fatal(new Exception());
        }

        [Fact]
        public void FatalAggregateException()
        {
            var logger = new EmptyAppLogger();
            logger.Fatal(new AggregateException());
        }
    }
}

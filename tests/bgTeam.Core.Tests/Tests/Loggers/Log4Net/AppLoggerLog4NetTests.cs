using bgTeam.Impl.Log4Net;
using System;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Loggers.Log4Net
{
    public class AppLoggerLog4NetTests
    {
        private static readonly AppLoggerLog4Net _logger;

        static AppLoggerLog4NetTests()
        {
            _logger = new AppLoggerLog4Net();
        }

        [Fact]
        public void Info()
        {
            _logger.Info("Hi");
        }

        [Fact]
        public void InfoFormat()
        {
            _logger.InfoFormat("Hi, {0}", "Name");
        }

        [Fact]
        public void Debug()
        {
            _logger.Debug("Hi");
        }

        [Fact]
        public void DebugFormat()
        {
            _logger.DebugFormat("Hi, {0}", "Name");
        }

        [Fact]
        public void Error()
        {
            _logger.Error("Hi");
        }

        [Fact]
        public void ErrorException()
        {
            _logger.Error(new Exception("Hi"));
        }

        [Fact]
        public void Warning()
        {
            _logger.Warning("Hi");
        }

        [Fact]
        public void FatalException()
        {
            var ex = new Exception();
            _logger.Fatal(ex);
        }

        [Fact]
        public void FatalAggregateException()
        {
            var ex = new AggregateException();
            _logger.Fatal(ex);
        }

        [Fact]
        public void ErrorAggregateException()
        {
            var ex = new AggregateException("Inner ex", new Exception());
            _logger.Error(ex);
        }

        [Fact]
        public void ErrorFormat()
        {
            _logger.ErrorFormat("Hi, {0}", "Name");
        }

        [Fact]
        public void ErrorFormatException()
        {
            _logger.ErrorFormat(new Exception (), "Hi, {0}", "Name");
        }
    }
}

using Xunit;
using bgTeam.Impl.Serilog;
using Moq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using Serilog;

namespace bgTeam.Core.Tests.Tests.Loggers.Serilog
{
    public class AppLoggerSerilogTests
    {
        private readonly static AppLoggerSerilog _appLoggerSerilog;
        // private readonly static string _logFileName;

        static AppLoggerSerilogTests()
        {
            _appLoggerSerilog = new AppLoggerSerilog(GetLoggerConfig());
        }

        [Fact]
        public void LoggerWithExtensions()
        {
            var logger = new AppLoggerSerilog(GetLoggerConfig("serilogs-with-extensions\\log-{Date}.log"), new[]
            {
                new LoggerExtension()
            });
            logger.Info("Using extensions");
        }

        [Fact]
        public void LoggerDefaultConfig()
        {
            var logger = new AppLoggerSerilog();
            logger.Info("Using default conf");
        }

        [Fact]
        public void Debug()
        {
            _appLoggerSerilog.Debug("Hi");
        }

        [Fact]
        public void Info()
        {
            _appLoggerSerilog.Info("Hi");
        }

        [Fact]
        public void Warning()
        {
            _appLoggerSerilog.Warning("Hi");
        }

        [Fact]
        public void Error()
        {
            _appLoggerSerilog.Error("Hi");
        }

        [Fact]
        public void ErrorException()
        {
            _appLoggerSerilog.Error(new Exception("Hi"));
        }

        [Fact]
        public void ErrorAggregateException()
        {
            _appLoggerSerilog.Error(new AggregateException(new Exception("Hi")));
        }

        [Fact]
        public void FatalException()
        {
            _appLoggerSerilog.Fatal(new Exception("Hi"));
        }

        [Fact]
        public void FatalAggregateException()
        {
            _appLoggerSerilog.Fatal(new AggregateException(new Exception("Hi")));
        }

        private static AppLoggerSerilogConfig GetLoggerConfig(string filepath = "serilogs\\log-{Date}.log")
        {
            var microsoftConfig = new Dictionary<string, string>()
            {
                ["Serilog:MinimumLevel"] = "Debug",
                ["Serilog:Using:0"] = "Serilog.Sinks.ColoredConsole",

                ["Serilog:WriteTo:0:Name"] = "ColoredConsole",
                ["Serilog:WriteTo:0:Args:outputTemplate"] = "{Timestamp:yyyy.MM.dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}",

                ["Serilog:WriteTo:1:Name"] = "RollingFile",
                ["Serilog:WriteTo:1:Args:pathFormat"] = filepath,
                ["Serilog:WriteTo:1:Args:outputTemplate"] = "{Timestamp:yyyy.MM.dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}",
                ["Serilog:WriteTo:1:Args:outputTemplate"] = "{Timestamp:yyyy.MM.dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}",
            };

            var buildedConf = new ConfigurationBuilder()
                .AddInMemoryCollection(microsoftConfig)
                .Build();

            var config = new Mock<IConfigSection>();
            config.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns((string section) => buildedConf.GetSection(section));

            return new AppLoggerSerilogConfig(config.Object);
        }

        private class LoggerExtension : IAppLoggerExtension
        {
            public LoggerConfiguration AddExtension(LoggerConfiguration config)
            {
                return config.MinimumLevel.Debug();
            }
        }
    }
}

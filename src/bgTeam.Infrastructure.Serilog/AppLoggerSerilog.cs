namespace bgTeam.Infrastructure.Logger
{
    using bgTeam.Core;
    using bgTeam.Extensions;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System;
    using System.Collections.Generic;

    public class AppLoggerSerilog : IAppLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IEnumerable<IAppLoggerExtension> _loggerExtensions;

        public AppLoggerSerilog()
        {
            var config = new AppLoggerSerilogConfig();

            _logger = CreateLogger(config);
        }

        public AppLoggerSerilog(IAppLoggerConfig config)
        {
            _logger = CreateLogger(config);
        }

        public AppLoggerSerilog(IAppLoggerConfig config, IEnumerable<IAppLoggerExtension> loggerExtensions)
        {
            _loggerExtensions = loggerExtensions;
            _logger = CreateLogger(config);
        }

        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Warning(string message)
        {
            _logger.LogWarning(message);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Error(Exception exp)
        {
            _logger.LogError(exp, exp.Message);
        }

        public void Error(AggregateException exp)
        {
            _logger.LogError($"Aggregate Exception ---> {exp.Message}");

            foreach (var item in exp.InnerExceptions)
            {
                var ex = item.GetBaseException();
                _logger.LogError(ex, ex.Message);
            }
        }

        public void Fatal(Exception exp)
        {
            _logger.LogCritical(exp, exp.Message);
        }

        public void Fatal(AggregateException exp)
        {
            _logger.LogCritical($"Aggregate Exception ---> {exp.Message}");

            foreach (var item in exp.InnerExceptions)
            {
                var ex = item.GetBaseException();
                _logger.LogCritical(ex, ex.Message);
            }
        }

        private Microsoft.Extensions.Logging.ILogger CreateLogger(IAppLoggerConfig loggerConfig)
        {
            var serilogConf = loggerConfig.GetLoggerConfig();
            var loggerConf = new LoggerConfiguration()
                .ReadFrom.ConfigurationSection(serilogConf);

            if (!_loggerExtensions.NullOrEmpty())
            {
                foreach (var extension in _loggerExtensions)
                {
                    loggerConf = extension.AddExtension(loggerConf);
                }
            }

            var logger = loggerConf.CreateLogger();

            return new LoggerFactory()
                .AddSerilog(logger)
                .CreateLogger(loggerConfig.LoggerName);
        }
    }
}

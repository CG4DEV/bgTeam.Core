namespace bgTeam.Infrastructure
{
    using bgTeam.Core;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System;

    public class AppLoggerSerilog : IAppLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public AppLoggerSerilog()
        {
            var config = new AppLoggerSerilogConfig();

            _logger = CreateLogger(config);
        }

        public AppLoggerSerilog(IAppLoggerConfig config)
        {
            _logger = CreateLogger(config);
        }

        public void Debug(string message)
        {
            _logger.LogDebug(message);
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
            _logger.LogError(string.Format("Aggregate Exception ---> {0}", exp.Message));

            foreach (var item in exp.InnerExceptions)
            {
                var ex = item.GetBaseException();
                _logger.LogError(ex, ex.Message);
            }
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        private Microsoft.Extensions.Logging.ILogger CreateLogger(IAppLoggerConfig loggerConfig)
        {
            var serilogConf = loggerConfig.GetLoggerConfig();
            var logger = new LoggerConfiguration()
                .ReadFrom.ConfigurationSection(serilogConf)
                .CreateLogger();

            return new LoggerFactory()
                .AddSerilog(logger)
                .CreateLogger(loggerConfig.LoggerName);
        }
    }
}

namespace bgTeam.Impl.Log4Net
{
    using log4net;
    using log4net.Config;
    using log4net.Core;
    using System;

    /// <summary>
    /// Реализация IAppLogger через log4net
    /// по умолчанию logger name="App.Main.Logger"
    /// </summary>
    public class AppLoggerLog4Net : IAppLogger
    {
        private readonly ILog _logger;

        public AppLoggerLog4Net()
            : this("App.Main.Logger")
        {
        }

        public AppLoggerLog4Net(string name)
        {
            var repository = LoggerManager.CreateRepository(name, typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(repository);

            // настраиваеться в конфиге
            _logger = LogManager.GetLogger(repository.Name, name);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void InfoFormat(string message, params object[] args)
        {
            _logger.InfoFormat(message, args);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void DebugFormat(string message, params object[] args)
        {
            _logger.DebugFormat(message, args);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception exp)
        {
            ////var expBase = exp.GetBaseException();

            _logger.Error(exp);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Fatal(Exception exp)
        {
            _logger.Fatal(exp);
        }

        public void Fatal(AggregateException exp)
        {
            _logger.Fatal(exp);
        }

        public void Error(AggregateException exp)
        {
            _logger.ErrorFormat("Aggregate Exception ---> {0}", exp.Message);

            foreach (var item in exp.InnerExceptions)
            {
                _logger.Error(item.GetBaseException());
            }
        }

        public void ErrorFormat(string message, params object[] args)
        {
            _logger.ErrorFormat(message, args);
        }

        public void ErrorFormat(Exception exp, string message, params object[] args)
        {
            _logger.ErrorFormat(message, args);
            _logger.Error(exp);
        }
    }
}

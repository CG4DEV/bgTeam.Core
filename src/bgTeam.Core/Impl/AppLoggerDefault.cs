namespace bgTeam.Impl
{
    using System;

    /// <summary>
    /// Реализация логгера по-умолчанию
    /// </summary>
    public class AppLoggerDefault : IAppLogger
    {
        /// <summary>
        /// Логирование с уровнем Debug
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Логирование с уровнем Info
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Логирование с уровнем Warning
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        public void Error(AggregateException exp)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        public void Error(Exception exp)
        {
            Console.WriteLine(exp.Message);
        }

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Логирование с уровнем Fatal
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        public void Fatal(Exception exp)
        {
            Console.WriteLine(exp.Message);
        }

        /// <summary>
        /// Логирование с уровнем Fatal
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        public void Fatal(AggregateException exp)
        {
            // Method intentionally left empty.
        }
    }
}

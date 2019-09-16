namespace bgTeam.Impl
{
    using System;

    /// <summary>
    /// Empty implementation for unit test
    /// </summary>
    public class EmptyAppLogger : IAppLogger
    {
        /// <summary>
        /// Логирование с уровнем Debug
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Debug(string message)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Логирование с уровнем Info
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Info(string message)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Логирование с уровнем Warning
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Warning(string message)
        {
            // Method intentionally left empty.
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
            // Method intentionally left empty.
        }

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        public void Error(string message)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Логирование с уровнем Fatal
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        public void Fatal(Exception exp)
        {
            // Method intentionally left empty.
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

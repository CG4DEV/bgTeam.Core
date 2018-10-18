namespace bgTeam
{
    using System;

    /// <summary>
    /// Интерфейс логгера
    /// </summary>
    public interface IAppLogger
    {
        /// <summary>
        /// Логирование с уровнем Debug
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        void Debug(string message);

        /// <summary>
        /// Логирование с уровнем Info
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        void Info(string message);

        /// <summary>
        /// Логирование с уровнем Warning
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        void Warning(string message);

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        void Error(string message);

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        void Error(Exception exp);

        /// <summary>
        /// Логирование с уровнем Error
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        void Error(AggregateException exp);

        /// <summary>
        /// Логирование с уровнем Fatal
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        void Fatal(Exception exp);

        /// <summary>
        /// Логирование с уровнем Fatal
        /// </summary>
        /// <param name="exp">Сведения об ошибке</param>
        void Fatal(AggregateException exp);
    }
}

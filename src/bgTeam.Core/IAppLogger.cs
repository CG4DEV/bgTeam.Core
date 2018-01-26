namespace bgTeam
{
    using System;

    public interface IAppLogger
    {
        void Debug(string message);

        void Info(string message);

        void Warning(string message);

        void Error(string message);

        void Error(Exception exp);

        void Error(AggregateException exp);

        void Fatal(Exception exp);

        void Fatal(AggregateException exp);
    }
}

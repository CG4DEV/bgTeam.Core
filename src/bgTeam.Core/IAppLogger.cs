namespace bgTeam
{
    using System;

    public interface IAppLogger
    {
        void Info(string message);

        void Debug(string message);

        void Error(string message);

        void Error(Exception exp);

        void Error(AggregateException exp);
    }
}

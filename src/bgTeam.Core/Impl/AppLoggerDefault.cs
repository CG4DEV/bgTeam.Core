namespace bgTeam.Impl
{
    using System;

    public class AppLoggerDefault : IAppLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(AggregateException exp)
        {
            // Method intentionally left empty.
        }

        public void Error(Exception exp)
        {
            Console.WriteLine(exp.Message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Fatal(Exception exp)
        {
            Console.WriteLine(exp.Message);
        }

        public void Fatal(AggregateException exp)
        {
            // Method intentionally left empty.
        }
    }
}

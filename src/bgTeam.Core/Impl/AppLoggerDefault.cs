namespace bgTeam.Impl
{
    using System;

    public class AppLoggerDefault : IAppLogger
    {
        public void Debug(string message)
        {
            // Method intentionally left empty.
            Console.WriteLine(message);
        }

        public void Error(AggregateException exp)
        {
            // Method intentionally left empty.
        }

        public void Error(Exception exp)
        {
            // Method intentionally left empty.
            Console.WriteLine(exp.Message);
        }

        public void Error(string message)
        {
            // Method intentionally left empty.
            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            // Method intentionally left empty.
            Console.WriteLine(message);
        }
    }
}

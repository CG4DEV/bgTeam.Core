namespace bgTeam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAppLogger
    {
        void Debug(string message);

        //void DebugFormat(string message, params object[] args);

        void Error(string message);

        void Error(Exception exp);

        void Error(AggregateException exp);

        //void ErrorFormat(string message, params object[] args);

        //void ErrorFormat(Exception exp, string message, params object[] args);

        void Info(string message);

        //void InfoFormat(string message, params object[] args);
    }
}

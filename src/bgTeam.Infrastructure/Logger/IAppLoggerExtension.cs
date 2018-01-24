namespace bgTeam.Infrastructure.Logger
{
    using Serilog;

    public interface IAppLoggerExtension
    {
        LoggerConfiguration AddExtension(LoggerConfiguration config);
    }
}

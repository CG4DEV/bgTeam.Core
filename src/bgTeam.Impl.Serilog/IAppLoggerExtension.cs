namespace bgTeam.Impl.Serilog
{
    using global::Serilog;

    public interface IAppLoggerExtension
    {
        LoggerConfiguration AddExtension(LoggerConfiguration config);
    }
}

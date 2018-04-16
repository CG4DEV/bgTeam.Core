namespace bgTeam.Impl.Serilog
{
    public interface ICloudLoggerConfig
    {
        string ProjectId { get; set; }

        string ApplicationName { get; set; }

        string AuthFilePath { get; set; }
    }
}

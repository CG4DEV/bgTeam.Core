namespace bgTeam.Infrastructure.Logger
{
    public interface ICloudLoggerConfig
    {
        string ProjectId { get; set; }

        string ApplicationName { get; set; }

        string AuthFilePath { get; set; }
    }
}

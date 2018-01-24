namespace bgTeam.Infrastructure.Logger.Extensions
{
    using System;
    using bgTeam.Extensions;
    using bgTeam.Infrastructure.Logger.GoogleCloudLogging;
    using Serilog;

    public class SerilogGoogleCloudExtension : IAppLoggerExtension
    {
        private readonly ICloudLoggerConfig _cloudSettings;

        private const string GOOGLE_ENV_VAR = "GOOGLE_APPLICATION_CREDENTIALS";

        public SerilogGoogleCloudExtension(ICloudLoggerConfig cloudSettings)
        {
            _cloudSettings = cloudSettings.CheckNull(nameof(cloudSettings));

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(GOOGLE_ENV_VAR, EnvironmentVariableTarget.Process)))
            {
                Environment.SetEnvironmentVariable(GOOGLE_ENV_VAR, cloudSettings.AuthFilePath, EnvironmentVariableTarget.Process);
            }
        }

        public LoggerConfiguration AddExtension(LoggerConfiguration config)
        {
            var options = new GoogleCloudLoggingSinkOptions(_cloudSettings.ProjectId, logName: _cloudSettings.ApplicationName);
            return config.WriteTo.GoogleCloudLogging(options);
        }
    }
}

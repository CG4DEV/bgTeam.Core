namespace bgTeam.Infrastructure.Logger.GoogleCloudLogging
{
    using System;
    using Serilog;
    using Serilog.Configuration;
    using Serilog.Formatting.Display;

    internal static class GoogleCloudLoggingSinkExtensions
    {
        public static LoggerConfiguration GoogleCloudLogging(this LoggerSinkConfiguration loggerConfiguration, GoogleCloudLoggingSinkOptions sinkOptions, int? batchSizeLimit = null, TimeSpan? period = null, string outputTemplate = null)
        {
            var messageTemplateTextFormatter = string.IsNullOrWhiteSpace(outputTemplate) ? null : new MessageTemplateTextFormatter(outputTemplate, null);

            var sink = new GoogleCloudLoggingSink(
                sinkOptions,
                messageTemplateTextFormatter,
                batchSizeLimit ?? 100,
                period ?? TimeSpan.FromSeconds(5)
            );

            return loggerConfiguration.Sink(sink);
        }
    }
}

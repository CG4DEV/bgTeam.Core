using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using bgTeam.Impl.Serilog.Extensions;
using bgTeam.Impl.Serilog;
using Serilog;

namespace bgTeam.Core.Tests.Tests.Loggers.Serilog.Extensions
{
    // TODO - Google logging classes not supports unit testing, only with real credentials
    // Will be better if it will be refactored. 
    // Other loggers too - will be better if it will take in construcor instances of ILoggers
    // Now all loggers creates in constructors
    public class SerilogGoogleCloudExtensionTests
    {
        [Fact]
        public void AddExtension()
        {
            var serilogGoogleCloudExtension = new SerilogGoogleCloudExtension(new CloudLoggerConfig());
            Assert.Throws<InvalidOperationException>(() => serilogGoogleCloudExtension.AddExtension(new LoggerConfiguration()));
        }

        private class CloudLoggerConfig : ICloudLoggerConfig
        {
            public string ProjectId { get; set; } = "id";
            public string ApplicationName { get; set; } = "test";
            public string AuthFilePath { get; set; } = "path";
        }
    }
}

using bgTeam.Core.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace bgTeam.Core.Tests.Core.Helpers
{
    public class CommandLineHelperTests
    {
        [Fact]
        public void ArgsShouldNotBeNull()
        {
            Assert.Throws<ArgumentNullException>("args", () =>
            {
                CommandLineHelper.ParseArgs(null);
            });
        }

        [Fact]
        public void ArgsShouldStartsWithPrefix()
        {
            Assert.Throws<ArgumentException>("keyPrefix", () =>
            {
                CommandLineHelper.ParseArgs(new[] { "-epochs", "16", "-steps" }, "+");
            });
        }

        [Fact]
        public void ParseArgs()
        {
            var agrs = CommandLineHelper.ParseArgs(new[] { "-epochs", "16", "-steps" });
            Assert.Equal(2, agrs.Count);
            Assert.Equal("16", agrs["epochs"]);
            Assert.Null(agrs["steps"]);
        }

        [Fact]
        public void ParseArgs2()
        {
            var agrs = CommandLineHelper.ParseArgs(new[] { "-framework", "netcoreapp3.1", "-project", "TestProject",
                "-is-app", "true", "-is-web", "false" });
            Assert.Equal(4, agrs.Count);
            Assert.Equal("netcoreapp3.1", agrs["framework"]);
            Assert.Equal("TestProject", agrs["project"]);
            Assert.Equal("false", agrs["is-web"]);
            Assert.Equal("true", agrs["is-app"]);
        }

        [Fact]
        public void CreateArgsInstance()
        {
            var instance = CommandLineHelper.CreateArgsInstance<Model>(new Dictionary<string, string>(){ { "epochs", "Epochs" } }, new[] { "epochs", "19" });
            Assert.Equal(19, instance.Epochs);
        }

        private class Model
        { 
            public int Epochs { get; set; }
        }
    }
}

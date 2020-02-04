using bgTeam.Plugins;
using TestLibrary;
using TestLibrary.Base;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Helpers
{
    public class PluginHelpersTests
    {
        [Fact]
        public void LoadExtendingClasses()
        {
            var types = PluginHelpers.Load<SomeClass>("Assemblies");
            var type = Assert.Single(types);
            Assert.NotNull(type);
        }

        [Fact]
        public void LoadImplementingInterfaces()
        {
            var types = PluginHelpers.Load<SomeInterface>("Assemblies");
            var type = Assert.Single(types);
            Assert.NotNull(type);
        }

        [Fact]
        public void LoadExtendingAbstractClasses()
        {
            var types = PluginHelpers.Load<SomeAbsractClass>("Assemblies");
            var type = Assert.Single(types);
            Assert.NotNull(type);
        }

        [Fact]
        public void LoadNonClassesAndInterfacesTypesShouldReturnsEmptyList()
        {
            var types = PluginHelpers.Load<Point>("Assemblies");
            Assert.Empty(types);
        }

        [Fact]
        public void LoadShouldReturnsEmptyListIfDirIsNotExists()
        {
            var types = PluginHelpers.Load<string>("Assemblies/UnexistsDirectory");
            Assert.Empty(types);
        }
    }
}

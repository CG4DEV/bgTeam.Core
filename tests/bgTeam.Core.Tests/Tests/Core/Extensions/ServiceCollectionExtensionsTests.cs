using Xunit;
using bgTeam.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace bgTeam.Core.Tests.Tests.Core.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSettingsFor1Implementation()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, Settings>();
            Assert.Equal(2, collection.Count);
        }

        [Fact]
        public void AddSettingsFor2Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, Settings>();
            Assert.Equal(3, collection.Count);
        }

        [Fact]
        public void AddSettingsFor3Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, Settings>();
            Assert.Equal(4, collection.Count);
        }

        [Fact]
        public void AddSettingsFor4Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, Settings>();
            Assert.Equal(5, collection.Count);
        }

        [Fact]
        public void AddSettingsFor5Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, Settings>();
            Assert.Equal(6, collection.Count);
        }

        [Fact]
        public void AddSettingsFor6Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, Settings>();
            Assert.Equal(7, collection.Count);
        }

        [Fact]
        public void AddSettingsFor7Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, Settings>();
            Assert.Equal(8, collection.Count);
        }

        [Fact]
        public void AddSettingsFor8Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, ISettings8, Settings>();
            Assert.Equal(9, collection.Count);
        }

        [Fact]
        public void AddSettingsFor9Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, ISettings8, ISettings9, Settings>();
            Assert.Equal(10, collection.Count);
        }

        [Fact]
        public void AddSettingsFor10Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, ISettings8, ISettings9, ISettings10, Settings>();
            Assert.Equal(11, collection.Count);
        }

        [Fact]
        public void AddSettingsFor11Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, ISettings8, ISettings9, ISettings10, ISettings11, Settings>();
            Assert.Equal(12, collection.Count);
        }

        [Fact]
        public void AddSettingsFor12Implementations()
        {
            var collection = new ServiceCollection();
            collection.AddSettings<ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, ISettings8, ISettings9, ISettings10, ISettings11, ISettings12, Settings>();
            Assert.Equal(13, collection.Count);
        }
    }

    interface ISettings
    {
        int Setting1 { get; set; }
    }

    interface ISettings2
    {
        int Setting2 { get; set; }
    }

    interface ISettings3
    {
        int Setting3 { get; set; }
    }

    interface ISettings4
    {
        int Setting4 { get; set; }
    }

    interface ISettings5
    {
        int Setting5 { get; set; }
    }

    interface ISettings6
    {
        int Setting6 { get; set; }
    }

    interface ISettings7
    {
        int Setting7 { get; set; }
    }

    interface ISettings8
    {
        int Setting8 { get; set; }
    }

    interface ISettings9
    {
        int Setting9 { get; set; }
    }

    interface ISettings10
    {
        int Setting10 { get; set; }
    }

    interface ISettings11
    {
        int Setting11 { get; set; }
    }

    interface ISettings12
    {
        int Setting12 { get; set; }
    }

    class Settings : ISettings, ISettings2, ISettings3, ISettings4, ISettings5, ISettings6, ISettings7, ISettings8, ISettings9, ISettings10, ISettings11, ISettings12
    {
        public int Setting1 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting2 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting3 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting4 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting5 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting6 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting7 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting8 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting9 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting10 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting11 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Setting12 { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}

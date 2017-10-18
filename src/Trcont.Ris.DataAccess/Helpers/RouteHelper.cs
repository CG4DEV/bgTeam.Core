namespace Trcont.Ris.DataAccess.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using bgTeam.Extensions;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.TransPicture;

    public static class RouteHelper
    {
        private static readonly Guid[] _containerSmall = GetContTypes("Small");
        private static readonly Guid[] _containerBig = GetContTypes("Big");

        private static readonly Guid[] _serviceTrain = GetServiceTypes("Train");
        private static readonly Guid[] _serviceShipping = GetServiceTypes("Shipping");
        private static readonly Guid[] _serviceAuto = GetServiceTypes("Auto");

        public static Guid[] ContainerSmall => _containerSmall;

        public static Guid[] ContainerBig => _containerBig;

        public static Guid[] Containers => _containerBig.Union(_containerSmall).ToArray();

        public static Guid[] ServiceTrain => _serviceTrain;

        public static Guid[] ServiceShipping => _serviceShipping;

        public static Guid[] ServiceAuto => _serviceAuto;

        public static Guid[] Services => _serviceAuto.Union(_serviceShipping).Union(_serviceTrain).ToArray();

        public static IEnumerable<T> GenerateRoutes<T>(IEnumerable<OrderServicesDto> services, Func<OrderServicesDto, T> map)
            where T : class, IBaseRoute
        {
            services.CheckNull(nameof(services));

            return services
                .OrderBy(x => x.ArmIndex)
                .Select(map)
                .DistinctBy(x => x.ArmIndex)
                .ToArray();
        }

        private static Guid[] GetContTypes(string sizeName)
        {
            var t = typeof(CalculateRateContainerTypeEnum);
            return Enum.GetNames(t)
                .Where(x => x.StartsWith(sizeName))
                .Select(x => Guid.Parse(((CalculateRateContainerTypeEnum)Enum.Parse(t, x)).GetDescription()))
                .ToArray();
        }

        private static Guid[] GetServiceTypes(string serviceType)
        {
            var t = typeof(ServicesEnum);
            return Enum.GetNames(t)
                .Where(x => x.StartsWith(serviceType))
                .Select(x => Guid.Parse(((ServicesEnum)Enum.Parse(t, x)).GetDescription()))
                .ToArray();
        }
    }
}

namespace Trcont.Ris.Story.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Ris.DataAccess.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetServiceParamsStory : IStory<GetServiceParamsStoryContext, IEnumerable<ServiceDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;

        public GetServiceParamsStory(
            IAppLogger logger,
            IRepository repository,
            IRepositoryEntity repositoryEn)
        {
            _logger = logger;
            _repository = repository;
            _repositoryEn = repositoryEn;
        }

        public IEnumerable<ServiceDto> Execute(GetServiceParamsStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ServiceDto>> ExecuteAsync(GetServiceParamsStoryContext context)
        {
            CheckInputParams(context);

            var services = await GetServicesIdsAsync(context);

            var paramsSrv = await new GetParamsForServicesCmd(_repository)
                   .ExecuteAsync(new GetParamsForServicesCmdContext() { ServiceIds = services.Select(x => x.Id.Value).Distinct().ToArray() });

            var result = services.Select(x => new ServiceDto()
            {
                ServiceId = x.Id,
                ServiceCode = x.Code,
                Params = paramsSrv.Where(p => p.ServiceId == x.Id.Value).ToArray()
            });

            return result;
        }

        private async Task<IEnumerable<Service>> GetServicesIdsAsync(GetServiceParamsStoryContext context)
        {
            context.ServiceCodes = context.ServiceCodes.Distinct().ToList();
            var services = await _repositoryEn.GetAllAsync<Service>(x => context.ServiceCodes.Contains(x.Code) && x.ServiceGroupId == (int)ServiceGroupEnum.EPU);
            return services.ToArray();
        }

        private static void CheckInputParams(GetServiceParamsStoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ServiceCodes == null || !context.ServiceCodes.Any())
            {
                throw new ArgumentNullException(nameof(context.ServiceCodes));
            }
        }
    }
}

namespace Trcont.IRS.WebApp.Controllers
{
    using bgTeam;
    using bgTeam.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Domain;
    using Trcont.Domain.Entity;
    using Trcont.IRS.Domain.Dto;
    using Trcont.IRS.Domain.Entity;
    using Trcont.IRS.Story.Common;

    [Route("[controller]/[action]")]
    public class InfoController : Controller
    {
        //public IStoryBuilder StoryBuilder { get; set; }

        private readonly IStoryBuilder _storyBuilder;

        public InfoController(IStoryBuilder storyBuilder)
        {
            _storyBuilder = storyBuilder;
        }

        [HttpPost]
        public async Task<IEnumerable<ClientDogovorDto>> GetDogovorsByClientGuid([FromForm] GetDogovorsByClientGuidStoryContext context)
        {
            if (Guid.Empty == context.ClientGuid)
            {
                throw new ArgumentNullException(nameof(context.ClientGuid));
            }

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<ClientDogovorDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<ClientDogovorDto>> GetIRSGuidByNSIGuid([FromForm] GetIRSGuidByNSIGuidStoryContext context)
        {
            if (Guid.Empty == context.NSIGuid)
            {
                throw new ArgumentNullException(nameof(context.NSIGuid));
            }

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<ClientDogovorDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<Reference>> GetReferenceByIRSGuid([FromForm] GetReferenceByIRSGuidListStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<Reference>>();
        }

        [HttpPost]
        public async Task<IEnumerable<FirmInfoDto>> GetFirmInfoGuids([FromForm] GetFirmInfoByGuidsStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<FirmInfoDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<ExternalSource>> GetExternalSource([FromForm] GetExternalSourceStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<ExternalSource>>();
        }

        [HttpPost]
        public async Task<IEnumerable<FactTransportByTeoGuidDto>> GetFactTransportInfoByTeoGuid([FromForm] GetFactTransportInfoByTeoGuidStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<FactTransportByTeoGuidDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<AutoRates>> GetGetAutoRateByParams([FromForm] GetAutoRateByParamsStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<AutoRates>>();
        }

        [HttpPost]
        public async Task<IEnumerable<IrsGuidAndCNSIGuid>> GetIrsGuidByCNSIGuid([FromForm] GetIrsGuidByCNSIGuidStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<IrsGuidAndCNSIGuid>>();
        }

        [HttpPost]
        public async Task<IEnumerable<IrsGuidAndCNSIGuid>> GetCNSIGuidByIrsGuid([FromForm] GetCNSIGuidByIrsGuidStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<IrsGuidAndCNSIGuid>>();
        }

        [HttpPost]
        public async Task<IEnumerable<PlanDatesForTeo>> GetPlanDatesForTeo([FromForm] GetPlanDatesForTeoStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<PlanDatesForTeo>>();
        }

        [HttpPost]
        public async Task<OrderInfoByGuid> GetOrderIdByIrsGuid([FromForm] GetOrderIdByIrsGuidStoryContext context)
        {
            context.CheckNull(nameof(context));

            return await _storyBuilder
                .Build(context)
                .ReturnAsync<OrderInfoByGuid>();
        }
    }
}

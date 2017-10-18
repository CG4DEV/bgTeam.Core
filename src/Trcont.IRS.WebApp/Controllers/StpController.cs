namespace Trcont.IRS.WebApp.Stp
{
    using bgTeam;
    using bgTeam.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.IRS.Common;
    using Trcont.IRS.Domain.Dto;
    using Trcont.IRS.Story.Scripts;
    using Trcont.IRS.WebApp.Contexts;

    [Route("[controller]/[action]")]
    public class StpController : Controller
    {
        private readonly IStoryBuilder _storyBuilder;

        public StpController(IStoryBuilder storyBuilder)
        {
            _storyBuilder = storyBuilder;
        }

        [HttpPost]
        public async Task<SptGetCNTSummDto> SptGetCNTSummByContractGuid([FromForm] SptGetCNTSummContext context)
        {
            if (Guid.Empty == context.ContractGuid)
            {
                throw new ArgumentNullException(nameof(context.ContractGuid));
            }

            var myParams = new Dictionary<string, string>();

            myParams.Add("ContractGUID", $"'{context.ContractGuid}'");
            myParams.Add("ReferenceGUID", "null");
            myParams.Add("LangGUID", "null");

            return await _storyBuilder
                .Build(new SptGetCNTSummStoryContext()
                {
                    ScriptParams = myParams
                }).ReturnAsync<SptGetCNTSummDto>();
        }

        [HttpPost]
        public async Task<SptFSSDataConvertDto> SptFSSDataConvert([FromForm] SptFSSDataConvertContext context)
        {
            context.CheckNull(nameof(context));

            var myParams = new Dictionary<string, string>();

            myParams.Add("ReportDate", $"CONVERT(datetime, '{context.ReportDate.ToString("dd.MM.yyyy")}', 104");
            myParams.Add("CLCurrencyId", context.CLCurrencyId.ToString());
            myParams.Add("FromCountryGUID", $"'{context.FromCountryGUID}'");
            myParams.Add("ToCountryGUID", $"'{context.ToCountryGUID}'");
            myParams.Add("ServiceXML", $"'{context.ServiceXML}'");
            myParams.Add("USLXML", $"'{context.USLXML}'");

            return await _storyBuilder
               .Build(new SptFSSDataConvertStoryContext()
               {
                   ScriptParams = myParams
               }).ReturnAsync<SptFSSDataConvertDto>();
        }

        [HttpPost]
        public async Task<IEnumerable<SptGetFactByTEODto>> SptGetFactByTEO([FromForm] SptGetFactByTEOContext context)
        {
            context.CheckNull(nameof(context));

            var myParams = new Dictionary<string, string>();
            myParams.Add("ReferenceGUID", $"'{context.ReferenceGuid}'");

             return await _storyBuilder
                .Build(new SptGetFactByTEOStoryContext()
                {
                    ScriptParams = myParams
                }).ReturnAsync<IEnumerable<SptGetFactByTEODto>>();
        }
    }
}
